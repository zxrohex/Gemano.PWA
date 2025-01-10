using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace Gemano.PWA.Controls
{
    public partial class BaseDialog : ComponentBase
    {
        public BaseDialog()
        {
            IsVisible = false;
        }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public async Task OnVisibleChanged(bool isVisible)
        {
            IsVisible = isVisible;

            await IsVisibleChanged.InvokeAsync(IsVisible);
        }

        public async Task Show()
        {
            await OnVisibleChanged(true);
        }

        public async Task Hide()
        {
            await OnVisibleChanged(false);
        }
    }
}
