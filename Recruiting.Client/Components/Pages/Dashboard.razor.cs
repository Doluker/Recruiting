using Microsoft.AspNetCore.Components;

namespace Recruiting.Client.Components.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;
    }
}
