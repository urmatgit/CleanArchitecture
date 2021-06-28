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
        private async Task DoSave()
        {
            if (selected!=null && selected.Length > 0)
            {
                foreach(MudChip mudChip in selected)
                {
                    MudChipEx mudChipEx = mudChip as MudChipEx;
                    GetAllInterestsCheckedResponse existInterest = _interestList.FirstOrDefault(i => i.UserInterestId == mudChipEx.Id);
                    if (existInterest!=null)
                    {
                        //Update
                        

                    }
                    else
                    {
                        //Add interest
                    }
                }     
            }
        }
        private async Task DoInterestClick(MudChip[] mudChipIces)
        {
          //  selected = mudChipIces;
            Console.WriteLine((mudChipIces[0] as MudChipEx).Id);
        }
    }
}
