using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Extensions.Controls
{
    public class MudChipEx: MudChip
    {
        [Parameter]
        public int Id { get; set; }
    }
}
