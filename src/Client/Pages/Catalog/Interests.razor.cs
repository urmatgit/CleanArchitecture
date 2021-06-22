using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
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
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Interest;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using System.Diagnostics;


namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class Interests
    {
        [Inject] private IInterestManager InterestManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllInterestsResponse> _interestList = new();
        private GetAllInterestsResponse _interest = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        GetAllInterestsResponse _selectedItem = null;
        private GetAllInterestsResponse selectedItem {
            get {
                return _selectedItem;
            } 
            set
            {
                _selectedItem = value;
                InterestId = _selectedItem!=null? selectedItem.Id: 0;
                Console.WriteLine($"Changed a interestid={InterestId}");
                
            }
        }
        
        private ClaimsPrincipal _currentUser;
        private bool _canCreateInterests;
        private bool _canEditInterests;
        private bool _canDeleteInterests;

        [Parameter]
        public bool HideId { get; set; } = true;
        int _interestId = 0;
        [Parameter]
        public int InterestId
        {
            get { return _interestId; }
            set
            {
                if (value == _interestId) return;
                _interestId = value;
                if (OnInterestIdChanged.HasDelegate)
                {
                    OnInterestIdChanged.InvokeAsync(_interestId);
                }
            }
        }
        [Parameter]
        public EventCallback<int> OnInterestIdChanged { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateInterests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Interests.Create)).Succeeded;
            _canEditInterests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Interests.Edit)).Succeeded;
            _canDeleteInterests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Interests.Delete)).Succeeded;

            await GetInterestsAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetInterestsAsync()
        {
            var response = await InterestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _interestList = response.Data.ToList();
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
                var response = await InterestManager.DeleteAsync(id);
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
            var base64 = await InterestManager.ExportToExcelAsync(_searchString);
            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"{nameof(Interests).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });
            _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                ? _localizer["Interests exported"]
                : _localizer["Filtered Interests exported"], Severity.Success);
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _interest = _interestList.FirstOrDefault(c => c.Id == id);
                if (_interest != null)
                {
                    parameters.Add(nameof(AddEditInterestModal.AddEditInterestModel), new AddEditInterestCommand
                    {
                        Id = _interest.Id,
                        Name = _interest.Name,
                        Description = _interest.Description,
                        PictureDataUrl = _interest.PictureDataUrl
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditInterestModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _interest = new GetAllInterestsResponse();
            await GetInterestsAsync();
        }

        private bool Search(GetAllInterestsResponse interest)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (interest.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (interest.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}