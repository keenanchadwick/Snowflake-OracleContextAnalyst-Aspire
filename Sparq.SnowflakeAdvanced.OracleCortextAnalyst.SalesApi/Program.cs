using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.SalesApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISnowflakeCredentialsProvider, HeaderSnowflakeCredentialsProvider>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
