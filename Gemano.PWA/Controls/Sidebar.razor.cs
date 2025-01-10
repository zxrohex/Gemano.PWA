using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gemano.PWA.Core.JS;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace Gemano.PWA.Controls
{
    public partial class Sidebar : ComponentBase
    {
        [CascadingParameter(Name = "GemanoInterfaceManager")]
        public GemanoInterfaceManager GemanoInterfaceManager { get; set; }

        public List<GemanoChatConversation> Chats;


        public bool IsVisible = false;

        protected override async Task OnParametersSetAsync()
        {
            Chats = await GemanoInterfaceManager.ChatInterface.GetAllChats();
        }

        public void Toggle()
        {
            IsVisible = !IsVisible;

            StateHasChanged();
        }

        async Task NewChat()
        {
            GemanoInterfaceManager.NavigationManager.NavigateTo($"/Chat");



            Toggle();
        }

        async Task LoadChat(string id)
        {
            GemanoInterfaceManager.NavigationManager.NavigateTo($"/Chat/View/{id}");

            Toggle();
        }
    }
}
