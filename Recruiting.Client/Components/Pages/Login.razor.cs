using Microsoft.AspNetCore.Components;
using MudBlazor;
using Recruiting.Client.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Pages
{
    public partial class Login
    {
        [Inject]
        private IAuthApiClient AuthClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        private LoginViewModel loginModel = new();
        private string? errorMessage;
        private bool isLoading = false;

        private async Task HandleLogin()
        {
            errorMessage = null;
            isLoading = true;

            try
            {
                var (success, error) = await AuthClient.LoginAsync(loginModel.Email, loginModel.Password);
                if (success)
                {
                    Snackbar.Add("Добро пожаловать в систему!", Severity.Success);
                    Navigation.NavigateTo("/workspace", forceLoad: true);
                }
                else
                {
                    errorMessage = error ?? "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Ошибка подключения: " + ex.Message;
            }
            finally
            {
                isLoading = false;
            }
        }

        public class LoginViewModel
        {
            [Required(ErrorMessage = "Введите email")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите пароль")]
            public string Password { get; set; } = string.Empty;
        }
    }
}
