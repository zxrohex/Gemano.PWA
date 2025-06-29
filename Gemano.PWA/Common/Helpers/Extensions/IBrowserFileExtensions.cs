using Microsoft.AspNetCore.Components.Forms;

namespace Gemano.PWA.Common.Helpers.Extensions
{
    public static class IBrowserFileExtensions
    {
        public static async Task<byte[]> ReadAllAsBytes(this IBrowserFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File cannot be null.");
            }

            using var stream = file.OpenReadStream(long.MaxValue);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
