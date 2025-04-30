using Microsoft.JSInterop;

namespace Gemano.PWA.Core.AI
{
    public class LanguageModelService
    {
        IJSRuntime jsRuntime;

        public LanguageModelService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task<LanguageModelSession> Create()
        {
            var jsObject = await jsRuntime.InvokeAsync<IJSObjectReference>("LanguageModel.create");

            return new LanguageModelSession(jsObject);
        }
    }

    public class LanguageModelSession
    {
        IJSObjectReference jsObject;

        public LanguageModelSession(IJSObjectReference jsObject)
        {
            this.jsObject = jsObject;
        }

        public async Task<string> Prompt(string prompt)
        {
            return await jsObject.InvokeAsync<string>("prompt", prompt);
        }

        public async Task<int> GetInputQuota()
        {
            return await jsObject.InvokeAsync<int>("getInputQuota");
        }

        public async Task<int> GetInputUsage()
        {
            return await jsObject.InvokeAsync<int>("getInputUsage");
        }

        public async Task<int> MeasureInputUsage(string prompt)
        {
            return await jsObject.InvokeAsync<int>("measureInputUsage", prompt);
        }

        public async Task<bool> Destroy() 
        {
            await jsObject.InvokeVoidAsync("destroy");

            return true;
        }
    }
}
