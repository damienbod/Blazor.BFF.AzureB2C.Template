# Blazor.BFF.AzureB2C.Template

[![.NET](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml) [![NuGet Status](http://img.shields.io/nuget/v/Blazor.BFF.AzureB2C.Template.svg?style=flat-square)](https://www.nuget.org/packages/Blazor.BFF.AzureB2C.Template/) [Change log](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/blob/main/Changelog.md)

This template can be used to create a Blazor WASM application hosted in an ASP.NET Core Web app using Azure B2C and Microsoft.Identity.Web to authenticate using the BFF security architecture. (server authentication) This removes the tokens form the browser and uses cookies with each HTTP request, response. The template also adds the required security headers as best it can for a Blazor application.

## Features

- WASM hosted in ASP.NET Core 6
- BFF with Azure B2C using Microsoft.Identity.Web
- OAuth2 and OpenID Connect OIDC
- No tokens in the browser

## Using the template

### install

```
dotnet new -i Blazor.BFF.AzureB2C.Template
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
	//"ClientSecret": "--in-user-settings--"
},

```

Add the permissions for Microsoft Graph if required, application scopes are used due to Azure B2C

```
"GraphApi": {
	// Add the required Graph permissions to the API
	"TenantId": "[Enter 'common', or 'organizations' or the Tenant Id (Obtained from the Azure portal. Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]",
	"ClientId": "[Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
	"Scopes": ".default"
	//"ClientSecret": "--in-user-settings--"
},
```

### uninstall

```
dotnet new -u Blazor.BFF.AzureB2C.Template
```


## Credits, Used NuGet packages + ASP.NET Core 6.0 standard packages

- NetEscapades.AspNetCore.SecurityHeaders

## Links

https://github.com/AzureAD/microsoft-identity-web
