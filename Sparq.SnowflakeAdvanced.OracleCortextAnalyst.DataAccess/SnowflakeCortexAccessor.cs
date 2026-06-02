using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Snowflake.Data.Client;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;

namespace Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataAccess
{
    /// <summary>
    /// Provides access to the SNOWFLAKE.CORTEX.COMPLETE function for AI completions.
    /// </summary>
    /// <seealso cref="DataAccessor" />
    public class SnowflakeCortexAccessor : DataAccessor
    {
        private const string DefaultModel = "mistral-large2";

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeCortexAccessor"/> class.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="password">The password, usually the programmatic access token (PAT).</param>
        public SnowflakeCortexAccessor(string userAccount, string password) : base(userAccount, password) { }

        /// <summary>
        /// Sends a prompt (with optional history) to SNOWFLAKE.CORTEX.COMPLETE and returns the completion.
        /// </summary>
        /// <param name="request">The chat request containing the prompt, history, and target model.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An <see cref="AskResponse"/> with the model's reply and timing metadata.</returns>
        public async Task<AskResponse> CompleteAsync(AskRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Prompt)) throw new ArgumentException("Prompt is required.", nameof(request));

            var model = string.IsNullOrWhiteSpace(request.Model) ? DefaultModel : request.Model;

            // Build a JSON messages array: [{"role":"system|user|assistant","content":"..."}]
            var messages = new List<ChatMessage>(request.History ?? new List<ChatMessage>());
            messages.Add(new ChatMessage { Role = "user", Content = request.Prompt });

            var messagesJson = JsonSerializer.Serialize(
                messages.Select(m => new { role = m.Role, content = m.Content }));

            var stopwatch = Stopwatch.StartNew();

            using (var connection = new SnowflakeDbConnection(ConnectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                using (var command = connection.CreateCommand())
                {
                    // CORTEX.COMPLETE supports either (model, prompt) or (model, messages_array, options).
                    // Using PARSE_JSON so the messages array is treated as ARRAY/OBJECT input.
                    command.CommandText = Queries.CortexComplete;

                    var modelParam = command.CreateParameter();
                    modelParam.ParameterName = "1";
                    modelParam.DbType = DbType.String;
                    modelParam.Value = model;
                    command.Parameters.Add(modelParam);

                    var msgParam = command.CreateParameter();
                    msgParam.ParameterName = "2";
                    msgParam.DbType = DbType.String;
                    msgParam.Value = messagesJson;
                    command.Parameters.Add(msgParam);

                    var raw = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                    stopwatch.Stop();

                    return new AskResponse
                    {
                        Reply = ExtractReply(raw),
                        Model = model,
                        ElapsedMs = stopwatch.ElapsedMilliseconds
                    };
                }
            }
        }

        /// <summary>
        /// Cortex returns either a plain string or a JSON object with "choices[0].messages".
        /// Normalize both shapes into a plain string.
        /// </summary>
        private static string ExtractReply(object raw)
        {
            if (raw == null || raw is DBNull) return string.Empty;
            var text = raw.ToString() ?? string.Empty;

            var trimmed = text.TrimStart();
            if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
            {
                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("choices", out var choices) &&
                        choices.ValueKind == JsonValueKind.Array &&
                        choices.GetArrayLength() > 0)
                    {
                        var first = choices[0];
                        if (first.TryGetProperty("messages", out var messagesProp))
                        {
                            return messagesProp.GetString() ?? text;
                        }
                        if (first.TryGetProperty("message", out var message) &&
                            message.TryGetProperty("content", out var content))
                        {
                            return content.GetString() ?? text;
                        }
                    }
                }
                catch
                {
                    // Fall through and return the raw text.
                }
            }

            return text;
        }
    }
}