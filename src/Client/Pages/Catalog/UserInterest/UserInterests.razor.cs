using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Extensions.Controls;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.UserInterest;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog.UserInterest
{
    public partial class UserInterests
    {
        [Inject] private IUserInterestManager UserInterestManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllInterestsCheckedResponse> _interestList = new();

        private bool IsChanged { get; set; }

        MudChip[] selected;
        protected override async Task OnInitializedAsync()
        {
            await GetInterestsAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);

            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task GetInterestsAsync()
        {
            var response = await UserInterestManager.GetCheckedAll();
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
        private async Task MudChipCliked(object par)
        {
            IsChanged = true;
        }
        private async Task DoSave()
        {
           
            if (selected!=null && selected.Length > 0)
            {
                 
                List<int> selectedInterests = new List<int>();

                foreach(MudChip mudChip in selected)
                {
                    selectedInterests.Add((mudChip as MudChipEx).Id);
                }
                var response = await UserInterestManager.EditMassAsync(new AddMassInterestCommand() { InterestIds = selectedInterests });
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                    await GetInterestsAsync();
                }
            }
            IsChanged = false;
        }
        private async Task DoInterestClick(MudChip[] mudChipIces)
        {
          //  selected = mudChipIces;
            Console.WriteLine((mudChipIces[0] as MudChipEx).Id);
        }
    }
}
