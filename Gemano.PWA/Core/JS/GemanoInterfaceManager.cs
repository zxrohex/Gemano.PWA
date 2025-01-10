using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gemano.PWA.Core.JS
{
    public class GemanoInterfaceManager
    {
        public IJSRuntime JSRuntime { get; set; }

        public IJSObjectReference JSModule { get; set; }

        public GemanoChatInterface ChatInterface { get; set; }

        public NavigationManager NavigationManager { get; set; }

        public GemanoInterfaceManager(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            JSRuntime = jsRuntime;

            NavigationManager = navigationManager;
        }

        public async Task<bool> LoadAsync()
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/ai-api.js");

            ChatInterface = new GemanoChatInterface(JSModule);

            return true;
        }
    }
}
