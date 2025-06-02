using Microsoft.JSInterop;

using Newtonsoft.Json;

namespace Gemano.PWA.Core.Storage
{
    public class LocalStorageService
    {
        IJSRuntime jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task SetItem<T>(string key, T value)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonConvert.SerializeObject(value));
        }

        public async Task<T> GetItem<T>(string key)
        {
            var value = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task RemoveItem(string key)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task Clear()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
}
