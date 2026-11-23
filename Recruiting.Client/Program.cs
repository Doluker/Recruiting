using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Recruiting.Client.Interfaces;
using Recruiting.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthServiceClient,AuthServiceClient>();
builder.Services.AddScoped<IVacancyServiceClient, VacancyServiceClient>();
builder.Services.AddScoped<IApplicationServiceClient, ApplicationServiceClient>();
builder.Services.AddScoped<ICandidateServiceClient, CandidateServiceClient>();
builder.Services.AddScoped<IDictionaryServiceClient, DictionaryServiceClient>();
builder.Services.AddScoped<IResumeServiceClient, ResumeServiceClient>();

await builder.Build().RunAsync();
