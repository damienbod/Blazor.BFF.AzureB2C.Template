# Blazor.BFF.AzureB2C.Template

[![.NET](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/actions/workflows/dotnet.yml) [![NuGet Status](http://img.shields.io/nuget/v/Blazor.BFF.AzureB2C.Template.svg?style=flat-square)](https://www.nuget.org/packages/Blazor.BFF.AzureB2C.Template/) [Change log](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/blob/main/Changelog.md)

This template can be used to create a Blazor WASM application hosted in an ASP.NET Core Web app using Azure B2C and Microsoft.Identity.Web to authenticate using the BFF security architecture. (server authentication) This removes the tokens form the browser and uses cookies with each HTTP request, response. The template also adds the required security headers as best it can for a Blazor application.

![Blazor BFF Azure B2C](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/blob/main/images/blazorBFFAzureB2C.png)

## Features

- WASM hosted in ASP.NET Core 6
- BFF with Azure B2C using Microsoft.Identity.Web
- OAuth2 and OpenID Connect OIDC
- No tokens in the browser

## Other templates

[Blazor BFF Azure AD](https://github.com/damienbod/Blazor.BFF.AzureAD.Template)

[Blazor BFF Azure OpenID Connect](https://github.com/damienbod/Blazor.BFF.OpenIDConnect.Template)

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
	"Domain": "[Enter the domain of your B2C tenant, e.g. contoso.onmicrosoft.com]",
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
	"SignedOutCallbackPath": "/signout-callback-oidc"
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

### uninstall

```
dotnet new -u Blazor.BFF.AzureB2C.Template
```

## Development

### build

https://docs.microsoft.com/en-us/dotnet/core/tutorials/create-custom-template

```
nuget pack content/Blazor.BFF.AzureB2C.Template.nuspec
```

### install developement

Locally built nupkg:

```
dotnet new -i Blazor.BFF.AzureB2C.Template.1.0.7.nupkg
```

Local folder:

```
dotnet new -i <PATH>
```

Where `<PATH>` is the path to the folder containing .template.config.

## Azure App registrations documentation

https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-register-applications

https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-register-applications?tabs=app-reg-ga

## Credits, Used NuGet packages + ASP.NET Core 6.0 standard packages

- NetEscapades.AspNetCore.SecurityHeaders
- IdentityModel.AspNetCore

## Links

https://github.com/AzureAD/microsoft-identity-web
