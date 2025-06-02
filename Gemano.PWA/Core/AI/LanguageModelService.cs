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

        public async Task<LanguageModelSession> Create(List<LanguageModelConversation.LanguageModelMessage> initialPrompts)
        {
            var jsObject = await jsRuntime.InvokeAsync<IJSObjectReference>("LanguageModel.create", new { 
                initialPrompts = initialPrompts.Select(mp => new { role = mp.Role, content = mp.Content }).ToList()
            });

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
            return await jsObject.GetValueAsync<int>("inputQuota");
        }

        public async Task<int> GetInputUsage()
        {
            return await jsObject.GetValueAsync<int>("inputUsage");
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
