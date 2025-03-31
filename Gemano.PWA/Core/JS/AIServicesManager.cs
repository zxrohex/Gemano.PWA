using Microsoft.JSInterop;

namespace Gemano.PWA.Core.JS
{
    public class AIServicesManager
    {
        IJSRuntime jsRuntime;

        IJSObjectReference jsModule;

        IJSObjectReference aiServicesManager;

        DotNetObjectReference<AIServicesManager> dotNetObjectReference;

        public delegate void DownloadProgress(long loaded, long total);
        public event DownloadProgress OnDownloadProgress;

        public AIServicesManager(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;

            dotNetObjectReference = DotNetObjectReference.Create(this);
        }

        public async Task InitializeAsync()
        {
            jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/ai-api.js");

            aiServicesManager = await jsModule.InvokeAsync<IJSObjectReference>("AIServicesManager.init", dotNetObjectReference);

            string test = await aiServicesManager.InvokeAsync<string>("LLM.isAvailable");

            Console.WriteLine(test);
        }

        [JSInvokable]
        public void OnDownloadProgressEvent(long loaded, long total)
        {
            OnDownloadProgress?.Invoke(loaded, total);
        }

        [JSInvokable]
        public void test()
        {
            Console.WriteLine("test");
        }
    }
}
