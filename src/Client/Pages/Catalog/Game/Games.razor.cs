using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll;
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
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Game;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog.Game
{
    public partial class Games
    {
        [Inject] private IGameManager GameManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllGamesResponse> _gameList = new();
        private GetAllGamesResponse _game = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateGames;
        private bool _canEditGames;
        private bool _canDeleteGames;
        private bool _canExportGames;
        private bool _canSearchGames;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateGames = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Games.Create)).Succeeded;
            _canEditGames = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Games.Edit)).Succeeded;
            _canDeleteGames = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Games.Delete)).Succeeded;
            _canExportGames = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Games.Export)).Succeeded;
            _canSearchGames = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Games.Search)).Succeeded;

            await GetGamesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetGamesAsync()
        {
            var response = await GameManager.GetAllAsync();
            if (response.Succeeded)
            {
                _gameList = response.Data.ToList();
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
                var response = await GameManager.DeleteAsync(id);
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
            var response = await GameManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Games).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Games exported"]
                    : _localizer["Filtered Games exported"], Severity.Success);
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
                _game = _gameList.FirstOrDefault(c => c.Id == id);
                if (_game != null)
                {
                    parameters.Add(nameof(AddEditGameModal.AddEditGameModel), new AddEditGameCommand
                    {
                        Id = _game.Id,
                        Name = _game.Name,
                        Description = _game.Description,
                        PlayerCount=_game.PlayerCount,
                        UserId=_game.UserId,
                        InterestId=_game.InterestId,
                        GameTypeId=_game.GameTypeId,
                        Archive=_game.Archive,
                        Publish=_game.Publish

                        
                        
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditGameModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _game = new GetAllGamesResponse();
            await GetGamesAsync();
        }

        private bool Search(GetAllGamesResponse game)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (game.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (game.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}