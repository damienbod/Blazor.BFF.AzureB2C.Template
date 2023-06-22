# Blazor.BFF.AzureB2C.Template

[![.NET](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml) [![NuGet Status](http://img.shields.io/nuget/v/Blazor.BFF.AzureB2C.Template.svg?style=flat-square)](https://www.nuget.org/packages/Blazor.BFF.AzureB2C.Template/) [Change log](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/blob/main/Changelog.md)

This template can be used to create a Blazor WASM application hosted in an ASP.NET Core Web app using Azure B2C and Microsoft.Identity.Web to authenticate using the BFF security architecture. (server authentication) This removes the tokens from the browser and uses cookies with each HTTP request, response. The template also adds the required security headers as best it can for a Blazor application.

## Features

- WASM hosted in ASP.NET Core 7
- BFF with Azure B2C using Microsoft.Identity.Web
- OAuth2 and OpenID Connect OIDC
- No tokens in the browser
- Azure AD Continuous Access Evaluation CAE support

## Using the template

### install

```
dotnet new install Blazor.BFF.AzureB2C.Template
```

### run

```
dotnet new blazorbffb2c -n YourCompany.Bff
```

Use the `-n` or `--name` parameter to change the name of the output created. This string is also used to substitute the namespace name in the .cs file for the project.

## Setup after installation

Add the Azure B2C App registration settings

```
"AzureB2C": {
	"Instance": "https://--your-domain--.b2clogin.com",
	"Domain": "[Enter the domain of your tenant, e.g. contoso.onmicrosoft.com]",
	"TenantId": "[Enter 'common', or 'organizations' or the Tenant Id (Obtained from the Azure portal. Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]",
	"ClientId": "[Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
	"ClientSecret": "[Copy the client secret added to the app from the Azure portal]",
	"ClientCertificates": [
	],
	// the following is required to handle Continuous Access Evaluation challenges
	"ClientCapabilities": [ "cp1" ],
	"CallbackPath": "/signin-oidc"
	// Add your policy here
	"SignUpSignInPolicyId": "B2C_1_signup_signin", 
	"SignedOutCallbackPath ": "/signout-callback-oidc"
},

```

Add the permissions for Microsoft Graph if required, application scopes are used due to Azure B2C

```
"GraphApi": {
	// Add the required Graph permissions to the Azure App registration
	"TenantId": "[Enter 'common', or 'organizations' or the Tenant Id (Obtained from the Azure portal. Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]",
	"ClientId": "[Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
	"Scopes": ".default"
	//"ClientSecret": "--in-user-secrets--"
},
```

### Use Continuous Access Evaluation CAE with a downstream API (access_token)

#### Azure app registration manifest

```json
"optionalClaims": {
	"idToken": [],
	"accessToken": [
		{
			"name": "xms_cc",
			"source": null,
			"essential": false,
			"additionalProperties": []
		}
	],
	"saml2Token": []
},
```

Any API call for the Blazor WASM could be implemented like this:

```
[HttpGet]
public async Task<IActionResult> Get()
{
  try
  {
	// Do logic which calls an API and throws claims challenge 
	// WebApiMsalUiRequiredException. The WWW-Authenticate header is set
	// using the OpenID Connect standards and Signals spec.
  }
  catch (WebApiMsalUiRequiredException hex)
  {
	var claimChallenge = WwwAuthenticateParameters
		.GetClaimChallengeFromResponseHeaders(hex.Headers);
		
	return Unauthorized(claimChallenge);
  }
}
```

The downstream API call could be implemented something like this:

```
public async Task<T> CallApiAsync(string url)
{
	var client = _clientFactory.CreateClient();

	// ... add bearer token
	
	var response = await client.GetAsync(url);
	if (response.IsSuccessStatusCode)
	{
		var stream = await response.Content.ReadAsStreamAsync();
		var payload = await JsonSerializer.DeserializeAsync<T>(stream);

		return payload;
	}

	// You can check the WWW-Authenticate header first, if it is a CAE challenge
	
	throw new WebApiMsalUiRequiredException($"Error: {response.StatusCode}.", response);
}
```

### Use Continuous Access Evaluation CAE in a standalone app (id_token)

#### Azure app registration manifest

```json
"optionalClaims": {
	"idToken": [
		{
			"name": "xms_cc",
			"source": null,
			"essential": false,
			"additionalProperties": []
		}
	],
	"accessToken": [],
	"saml2Token": []
},
```
If using a CAE Authcontext in a standalone project, you only need to challenge against the claims in the application.

```
private readonly CaeClaimsChallengeService _caeClaimsChallengeService;

public AdminApiCallsController(CaeClaimsChallengeService caeClaimsChallengeService)
{
  _caeClaimsChallengeService = caeClaimsChallengeService;
}

[HttpGet]
public IActionResult Get()
{
  // if CAE claim missing in id token, the required claims challenge is returned
  var claimsChallenge = _caeClaimsChallengeService
	.CheckForRequiredAuthContextIdToken(AuthContextId.C1, HttpContext);

  if (claimsChallenge != null)
  {
	return Unauthorized(claimsChallenge);
  }
```

### uninstall

```
dotnet new uninstall Blazor.BFF.AzureB2C.Template
```

### Troubleshooting 
 
If running the app in a service such as Web App for Containers or Azure Container apps then you may experience issues with Azure terminating the SSL connection and passing the requests on as HTTP. 
 
The first area affected will be the AntiForgery cookie, which will need the SecurePolicy changing as shown below: 
 
``` 
services.AddAntiforgery(options => 
{ 
    options.HeaderName = "X-XSRF-TOKEN"; 
    options.Cookie.Name = "__Host-X-XSRF-TOKEN"; 
    options.Cookie.SameSite = SameSiteMode.Strict; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
}); 
``` 
 
The second area affected will be the login process itself, which will fail with a 'Correlation failed' error. Inspecting the event logs will show errors referring to 'cookie not found'. To remedy this, modify the code in the two areas below: 
 
``` 
builder.Services.Configure<ForwardedHeadersOptions>(options => 
{ 
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto; 
}); 
 
services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureB2C") 
    .EnableTokenAcquisitionToCallDownstreamApi(Array.Empty<string>()) 
    .AddInMemoryTokenCaches(); 
``` 
and this 
 
``` 
app.UseForwardedHeaders(); 
 
if (env.IsDevelopment()) 
{ 
    app.UseDeveloperExceptionPage(); 
``` 
 
Further details may be found here [Configure ASP.NET Core to work with proxy servers and load balancers](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1#nginx-configuration) 
 
Please note, adding the 'XForwardedFor' enum as shown in the Microsoft document above did not work and needed to be removed so only the XForwardedProto remains. 

## Credits, Used NuGet packages + ASP.NET Core 7.0 standard packages

- NetEscapades.AspNetCore.SecurityHeaders

## Links

https://github.com/AzureAD/microsoft-identity-web
