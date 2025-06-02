using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;


namespace Gemano.PWA.Controls
{
    public partial class Sidebar : ComponentBase
    {
        public bool IsVisible = false;


        public void Toggle()
        {
            IsVisible = !IsVisible;

            StateHasChanged();
        }
    }
}
