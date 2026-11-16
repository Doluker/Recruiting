using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Layout
{
    public partial class MainLayout
    {
        private bool _isDarkMode = false;
        private bool _profileMenuOpen = false;

        private void ToggleProfileMenu() => _profileMenuOpen = !_profileMenuOpen;
        private void CloseProfileMenu() => _profileMenuOpen = false;

        private async Task HandleLogout()
        {
            _profileMenuOpen = false;
            await AuthClient.LogoutAsync();
            Snackbar.Add("Вы вышли из системы", Severity.Info);
            Navigation.NavigateTo("/", forceLoad: true);
        }

        private readonly MudTheme _theme = new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1E293B",
                Secondary = "#5A7569",
                Tertiary = "#C28163",
                Background = "#EEF2F6",
                Surface = "#FFFFFF",
                AppbarBackground = "#FFFFFF",
                AppbarText = "#1E293B",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#1E293B",
                TextPrimary = "#1E293B",
                TextSecondary = "#64748B",
                LinesDefault = "#D8DFE7",
                TableLines = "#D8DFE7",
                Divider = "#D8DFE7",
                Success = "#10B981",
                Warning = "#F59E0B",
                Error = "#EF4444",
                Info = "#1E293B"
            },
            PaletteDark = new PaletteDark
            {
                Primary = "#F1F5F9",
                Secondary = "#749283",
                Tertiary = "#D89577",
                Background = "#0B111A",
                Surface = "#16202D",
                AppbarBackground = "#16202D",
                AppbarText = "#F1F5F9",
                DrawerBackground = "#16202D",
                DrawerText = "#F1F5F9",
                TextPrimary = "#F1F5F9",
                TextSecondary = "#94A3B8",
                LinesDefault = "#2C3C50",
                TableLines = "#2C3C50",
                Divider = "#2C3C50",
                Success = "#34D399",
                Warning = "#FBBF24",
                Error = "#F87171",
                Info = "#38BDF8"
            },
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "8px"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Inter", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "Roboto", "sans-serif" }
                }
            }
        };
    }
}
