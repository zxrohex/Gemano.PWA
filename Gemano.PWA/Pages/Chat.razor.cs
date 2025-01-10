using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components;

namespace Gemano.PWA.Pages
{
    public partial class Chat : ComponentBase
    {
        protected override async Task OnParametersSetAsync()
        {
            await GemanoInterfaceManager.ChatInterface.GetAllChats();

            GemanoInterfaceManager.ChatInterface.OnSessionChanged += () =>
            {
                StateHasChanged();
            };
        }
    }
}
