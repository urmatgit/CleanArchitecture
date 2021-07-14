using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.GameType;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog.GameType
{
    public partial class GameTypes
    {
        [Inject] private IGameTypeManager GameTypeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllGameTypesResponse> _gametypeList = new();
        private GetAllGameTypesResponse _gametype = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateGameTypes;
        private bool _canEditGameTypes;
        private bool _canDeleteGameTypes;
        private bool _canExportGameTypes;
        private bool _canSearchGameTypes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateGameTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.GameTypes.Create)).Succeeded;
            _canEditGameTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.GameTypes.Edit)).Succeeded;
            _canDeleteGameTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.GameTypes.Delete)).Succeeded;
            _canExportGameTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.GameTypes.Export)).Succeeded;
            _canSearchGameTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.GameTypes.Search)).Succeeded;

            await GetGameTypesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetGameTypesAsync()
        {
            var response = await GameTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _gametypeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await GameTypeManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await GameTypeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(GameTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["GameTypes exported"]
                    : _localizer["Filtered GameTypes exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _gametype = _gametypeList.FirstOrDefault(c => c.Id == id);
                if (_gametype != null)
                {
                    parameters.Add(nameof(AddEditGameTypeModal.AddEditGameTypeModel), new AddEditGameTypeCommand
                    {
                        Id = _gametype.Id,
                        Name = _gametype.Name,
                        Description = _gametype.Description
                        
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditGameTypeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _gametype = new GetAllGameTypesResponse();
            await GetGameTypesAsync();
        }

        private bool Search(GetAllGameTypesResponse gametype)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (gametype.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (gametype.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}