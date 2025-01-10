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


        [Parameter]
        public bool Visible { get; set; }

        private bool isVisible;

        protected override async Task OnParametersSetAsync()
        {
            isVisible = Visible;
        }

        public void Show()
        {
            isVisible = true;
            StateHasChanged();
        }

        public void Hide()
        {
            isVisible = false;
            StateHasChanged();
        }
    }
}
