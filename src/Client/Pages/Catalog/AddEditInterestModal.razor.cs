using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Interest;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Interest;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;
using Microsoft.AspNetCore.Components.Forms;
using BlazorHero.CleanArchitecture.Application.Requests;
using System.IO;
using System;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditInterestModal
    {
        [Inject] private IInterestManager InterestManager { get; set; }

        [Parameter] public AddEditInterestCommand AddEditInterestModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await InterestManager.SaveAsync(AddEditInterestModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await Task.CompletedTask;
        }
        #region Image
        private void DeleteAsync()
        {
            AddEditInterestModel.PictureDataUrl = null;
            AddEditInterestModel.UploadRequest = new UploadRequest();
        }

        private IBrowserFile _file;
        private async Task LoadImageAsync()
        {
            var data = await  InterestManager.GetProductImageAsync(AddEditInterestModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditInterestModel.PictureDataUrl = imageData;
                }
            }
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditInterestModel.PictureDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditInterestModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }

        #endregion
    }
}