﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorBffAzureB2C.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace BlazorBffAzureB2C.Server.Controllers;

[ValidateAntiForgeryToken]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[AuthorizeForScopes(Scopes = new string[] { "User.ReadBasic.All user.read" })]
[ApiController]
[Route("api/[controller]")]
public class GraphApiCallsController : ControllerBase
{
    private readonly MsGraphService _msGraphtService;

    public GraphApiCallsController(MsGraphService msGraphtService)
    {
        _msGraphtService = msGraphtService;
    }

    [HttpGet]
    public async Task<IEnumerable<string>> Get()
    {
        var userId = User.GetNameIdentifierId();
        if (userId != null)
        {
            var userData = await _msGraphtService.GetGraphApiUser(userId);
            if (userData == null)
                return new List<string> { "no user data" };

            return new List<string> { $"DisplayName: {userData.DisplayName}",
            $"GivenName: {userData.GivenName}", $"AboutMe: {userData.AboutMe}" };
        }

        return Array.Empty<string>(); 
    }
}