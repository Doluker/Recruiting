using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Recruiting.Client.Pages;
using Recruiting.Components;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Поиск .env вверх по дереву папок
Env.TraversePath().Load();

// Строка подключения к PostgreSQL
var connectionString = new Npgsql.NpgsqlConnectionStringBuilder
{
    Host = Env.GetString("DB_HOST", "localhost"),
    Port = int.TryParse(Env.GetString("DB_PORT"), out var port) ? port : 5432,
    Database = Env.GetString("DB_NAME"),
    Username = Env.GetString("DB_USER"),
    Password = Env.GetString("DB_PASSWORD")
}.ConnectionString;
builder.Services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(connectionString));

// Сервисы ASP.NET Core и Blazor
builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// 4. Регистрация сервисов приложения
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 5.Настройка Cookie - аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.Cookie.Name = "Recruiting.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.SlidingExpiration = true;
        // Важно для API: отдавать 401 и 403 вместо редиректа на страницу входа
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Конфигурация HTTP конвейера
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

// Статика Blazor
app.UseStaticFiles();
app.MapStaticAssets();

// Маппинг контроллеров API(/api/auth/...)
app.MapControllers();

// Blazor WebAssembly хостинг
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Recruiting.Client._Imports).Assembly);

app.Run();
