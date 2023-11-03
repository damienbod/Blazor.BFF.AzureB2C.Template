# Blazor.BFF.AzureB2C.Template

[Readme](https://github.com/damienbod/Blazor.BFF.AzureB2C.Template/blob/main/README.md) 

**2023-11-03** 2.2.0
- Fix XSS block security header
- Updated packages

**2023-06-21** 2.1.0
- Switched to Graph SDK 5
- Updated packages

**2023-03-12** 2.0.2
- Fix security headers, use on app requests including API calls
- Updated packages

**2023-01-15** 2.0.1
- Updated packages
- Improved Microsoft Graph client 

**2022-12-03** 2.0.0
- Updated packages to .NET 7

**2022-09-23** 1.2.4
- Updated packages

**2022-08-12** 1.2.3
- Http port generator added
- Updated packages

**2022-08-07** 1.2.2
- Improve template
- Updated packages

**2022-07-09** 1.2.1
- Improved User controller, 
- Fix Graph dependency breaking changes
- Update nuget packages
- Improved docs regarding Azure Container support

**2022-03-22** 1.2.0
- use new top-level statements and remove
- enable ImplicitUsings
- add IAntiforgeryHttpClientFactory/AntiforgeryHttpClientFactory
- Replace of IdentityModel with System.Security.Claims and remove IdentityModel nuget package
- Add support for Azure AD Continuous Access Evaluation CAE
- Add 404, 401, response handling
- Update nuget packages

**2022-03-20** 1.1.0
- Updated nuget packages
- Using nullable enabled

**2022-03-05** 1.0.11
- bugfix downstream APIs support

**2022-03-04** 1.0.10
- Cache claims after authentication

**2022-02-11** 1.0.9
- Update namespaces
- Update nuget packages

**2022-01-23** 1.0.8
- Using the object identifier to request the MS graph data

**2022-01-23** 1.0.7
- Remove PWA items, default template uses anti-forgery cookies and no PWA support
  (Will consider supporting this later, requires switching to CORS preflight CSRF protection)
- Using the AuthorizedHandler for protected requests

**2022-01-21** 1.0.6
- Update readme and appsettings by precising description of domain
- Remove unused LoginDisplay razor page

**2022-01-18** 1.0.5
- fix name project generation

**2022-01-17** 1.0.4
- removed launchsettings.json from WASM client
- removed unused client files

**2022-01-09** 1.0.3
- small fixes
- removed unused configurations and packages

**2022-01-04** 1.0.2
- Small fixes
- Updated packages

**2021-12-09** 1.0.1
- Remove unused static host file
- fix text in template description


**2021-12-03** 1.0.0
- Initial release 
- Azure B2C authentication using Microsoft.Identity.Web
- Microsoft Graph
- ASP.NET Core 6


