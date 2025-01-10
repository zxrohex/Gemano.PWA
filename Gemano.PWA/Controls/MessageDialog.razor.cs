using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components;

namespace Gemano.PWA.Controls
{
    public partial class MessageDialog : ComponentBase
    {
        [Parameter]
        public RenderFragment HeaderText { get; set; }

        [Parameter]
        public RenderFragment BodyText { get; set; }

       

        public bool IsVisible { get; set; }

        public void Show()
        {
            IsVisible = true;
            StateHasChanged();
        }

        public void Hide()
        {
            IsVisible = false;
            StateHasChanged();
        }
    }
}
