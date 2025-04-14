using AuthenticationUI;
using AuthenticationUI.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

// register a named HttpClient for LibraryService
//builder.Services.AddHttpClient<LibraryService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7213"); // Book Service API
//});

//////this  for Authentication API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:44353")
});

// Autentication Provider register
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthService>();
builder.Services.AddScoped<CustomAuthService>();
builder.Services.AddScoped<LibraryService>();
await builder.Build().RunAsync();
