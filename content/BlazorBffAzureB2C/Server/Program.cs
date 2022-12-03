using BlazorBffAzureB2C.Server;
using BlazorBffAzureB2C.Server.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;
IServiceProvider? applicationServices = null;

services.AddScoped<MsGraphService>();
services.AddScoped<MsGraphClaimsTransformation>();
services.AddScoped<CaeClaimsChallengeService>();

services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-X-XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.AddHttpClient();
services.AddOptions();

//var scopes = Configuration.GetValue<string>("DownstreamApi:Scopes");
//string[] initialScopes = scopes.Split(' ');

services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureB2C")
    .EnableTokenAcquisitionToCallDownstreamApi(Array.Empty<string>())
    .AddInMemoryTokenCaches();

services.Configure<MicrosoftIdentityOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Events.OnTokenValidated = async context =>
    {
        if (applicationServices != null && context.Principal != null)
        {
            using var scope = applicationServices.CreateScope();
            context.Principal = await scope.ServiceProvider
                .GetRequiredService<MsGraphClaimsTransformation>()
                .TransformAsync(context.Principal);
        }
    };
});

services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

services.AddRazorPages().AddMvcOptions(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //    .RequireAuthenticatedUser()
    //    .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

var app = builder.Build();
applicationServices = app.Services;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseSecurityHeaders(
        SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment(),
            configuration["AzureB2C:Instance"]));

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseNoUnauthorizedRedirect("/api");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapNotFound("/api/{**segment}");
app.MapFallbackToPage("/_Host");

app.Run();
