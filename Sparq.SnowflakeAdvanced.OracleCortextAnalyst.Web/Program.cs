using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Radzen;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.DataModels;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web.Authentication;
using Sparq.SnowflakeAdvanced.OracleCortextAnalyst.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddOutputCache();

// --- Authentication / Authorization -------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = "OracleCortexAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy; // require auth by default
});

builder.Services.AddScoped<ISnowflakeCredentialsProvider, ClaimsSnowflakeCredentialsProvider>();
builder.Services.AddTransient<CredentialsForwardingHandler>();
// ------------------------------------------------------------------------

//builder.Services.AddHttpClient<WeatherApiClient>(client =>
//{
//    client.BaseAddress = new("https+http://apiservice");
//});

builder.Services.AddHttpClient<SalesApiClient>(client =>
{
    client.BaseAddress = new("https+http://salesapi");
    client.Timeout = TimeSpan.FromMinutes(2);
})
.AddHttpMessageHandler<CredentialsForwardingHandler>();

builder.Services.AddRadzenComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

// Logout endpoint
app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).AllowAnonymous();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
