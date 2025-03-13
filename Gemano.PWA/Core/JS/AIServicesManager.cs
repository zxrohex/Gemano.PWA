using Microsoft.JSInterop;

namespace Gemano.PWA.Core.JS
{
    public class AIServicesManager
    {
        IJSRuntime jsRuntime;

        IJSObjectReference jsModule;

        IJSObjectReference aiServicesManager;

        DotNetObjectReference<AIServicesManager> dotNetObjectReference;

        public AIServicesManager(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;

            dotNetObjectReference = DotNetObjectReference.Create(this);
        }

        public async Task InitializeAsync()
        {
            jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/ai-api.js");

            aiServicesManager = await jsModule.InvokeAsync<IJSObjectReference>("AIServicesManager.init", dotNetObjectReference);

            string test = await aiServicesManager.InvokeAsync<string>("LanguageModel.isAvailable");

            Console.WriteLine(test);
        }

        [JSInvokable]
        public void test()
        {
            Console.WriteLine("test");
        }
    }
}
