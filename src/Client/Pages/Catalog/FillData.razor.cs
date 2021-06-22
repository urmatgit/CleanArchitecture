using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.FillData;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do;
using Blazored.FluentValidation;
using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class FillData
    {
        [Inject] private IFillDataManager FillDataManager { get; set; }
        private ClaimsPrincipal _currentUser;
        

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public DoCommand doCommand { get; set; } = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); }) && !StartingFill;

        private bool StartingFill { get; set; }
        private bool _canFillDo;

        protected override async Task OnInitializedAsync()
        {

            _currentUser = await _authenticationManager.CurrentUser();
            _canFillDo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Fill.Do)).Succeeded;
            HubConnection = HubConnection.TryInitialize(_navigationManager);

            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            //await base.OnInitializedAsync();
        }
        private async Task DoAsync()
        {
            _snackBar.Add(_localizer["Fill data started"], MudBlazor.Severity.Info);

            StartingFill = true;
            var response = await FillDataManager.Do(doCommand);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                StartingFill = false;
            }
        }
    }
}
