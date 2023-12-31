// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TradingBot.Backend.Services.Identity.Api.Quickstart.Consent;

namespace TradingBot.Backend.Services.Identity.Api.Quickstart.Device;

[Authorize]
[SecurityHeaders]
public class DeviceController : Controller
{
    private readonly IDeviceFlowInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IOptions<IdentityServerOptions> _options;

    public DeviceController(
        IDeviceFlowInteractionService interaction,
        IEventService eventService,
        IOptions<IdentityServerOptions> options)
    {
        _interaction = interaction;
        _events = eventService;
        _options = options;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userCodeParamName = _options.Value.UserInteraction.DeviceVerificationUserCodeParameter;
#pragma warning disable CS8600
        string userCode = Request.Query[userCodeParamName];
#pragma warning restore CS8600
        if (string.IsNullOrWhiteSpace(userCode)) return View("UserCodeCapture");

        var vm = await BuildViewModelAsync(userCode);
        if (vm == null) return View("Error");

        vm.ConfirmUserCode = true;
        return View("UserCodeConfirmation", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserCodeCapture(string userCode)
    {
        var vm = await BuildViewModelAsync(userCode);
        return vm == null ? View("Error") : View("UserCodeConfirmation", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Callback(DeviceAuthorizationInputModel? model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var result = await ProcessConsent(model);
        return View(result.HasValidationError ? "Error" : "Success");
    }

    private async Task<ProcessConsentResult> ProcessConsent(DeviceAuthorizationInputModel? model)
    {
        var result = new ProcessConsentResult();

        var request = await _interaction.GetAuthorizationContextAsync(model?.UserCode);
        if (request == null) return result;

        ConsentResponse? grantedConsent = null;

        switch (model?.Button)
        {
            // user clicked 'no' - send back the standard 'access_denied' response
            case "no":
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
                break;
            // user clicked 'yes' - validate the data
            // if the user consented to some scope, build the response model
            case "yes" when model.ScopesConsented != null && model.ScopesConsented.Any():
            {
                var scopes = model.ScopesConsented;
                if (ConsentOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = model.Description
                };

                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                break;
            }
            case "yes":
                result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                break;
            default:
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
                break;
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identityserver
            await _interaction.HandleRequestAsync(model?.UserCode, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            result.RedirectUri = model?.ReturnUrl;
            result.Client = request.Client;
        }
        else
        {
            // we need to redisplay the consent UI
            result.ViewModel = await BuildViewModelAsync(model?.UserCode??"", model);
        }

        return result;
    }

    private async Task<DeviceAuthorizationViewModel?> BuildViewModelAsync(string userCode, DeviceAuthorizationInputModel? model = null)
    {
        var request = await _interaction.GetAuthorizationContextAsync(userCode);
        return request != null ? CreateConsentViewModel(userCode, model, request) : null;
    }

    private DeviceAuthorizationViewModel CreateConsentViewModel(string userCode, DeviceAuthorizationInputModel? model, DeviceFlowAuthorizationRequest request)
    {
        var vm = new DeviceAuthorizationViewModel
        {
            UserCode = userCode,
            Description = model?.Description,

            RememberConsent = model?.RememberConsent ?? true,
            ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

        var apiScopes = (from parsedScope in request.ValidatedResources.ParsedScopes let apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName) where apiScope != null select CreateScopeViewModel(parsedScope, apiScope, vm.ScopesConsented.Contains(parsedScope.RawValue) || model == null)).ToList();
        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
        }
        vm.ApiScopes = apiScopes;

        return vm;
    }

    private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        return new ScopeViewModel
        {
            Value = parsedScopeValue.RawValue,
            // todo: use the parsed scope value in the display?
            DisplayName = apiScope.DisplayName ?? apiScope.Name,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }
    private ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }
}