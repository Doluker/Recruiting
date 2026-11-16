using Microsoft.AspNetCore.Components;
using MudBlazor;
using Recruiting.Client.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Pages
{
    public partial class Register
    {
        [Inject]
        private IAuthServiceClient AuthClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        private RegisterViewModel registerModel = new();
        private string? errorMessage;
        private bool isLoading = false;

        private async Task HandleRegister()
        {
            errorMessage = null;

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                errorMessage = "Введённые пароли не совпадают";
                return;
            }

            isLoading = true;

            try
            {
                var (success, error) = await AuthClient.RegisterAsync(
                    registerModel.Email,
                    registerModel.UserName,
                    registerModel.Password);

                if (success)
                {
                    Snackbar.Add("Аккаунт создан! Вход выполнен успешно", Severity.Success);
                    Navigation.NavigateTo("/workspace", forceLoad: true);
                }
                else
                {
                    errorMessage = error ?? "Ошибка при регистрации";
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

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Введите имя пользователя")]
            [MinLength(3, ErrorMessage = "Имя должно быть от 3 символов")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите email")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите пароль")]
            [MinLength(6, ErrorMessage = "Минимум 6 символов")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Повторите пароль")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}
