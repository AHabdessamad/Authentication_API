using AuthenticationUI.Models;
using AuthenticationUI.Results;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace AuthenticationUI.Services
{
    public class CustomAuthService : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        private readonly ClaimsPrincipal Unauthenticated =
            new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("accessToken");
                if(string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(Unauthenticated);
                }

                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking authentication", ex);

            }
        }
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task<FormResult> LoginAsync(string username, string password)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44354/");

            try
            {
                var res = await _httpClient.PostAsJsonAsync(
                    "api/Auth/login",
                    new { 
                        username, 
                        password 
                    });
                if (res.IsSuccessStatusCode)
                {
                    var tokenResponse = await res.Content.ReadAsStringAsync();
                    var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(tokenResponse);
                    await _localStorageService.SetItemAsync("accessToken", tokenInfo.AccessToken);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new FormResult(true, "Login successful");
                }
            }
            catch(Exception ex)
            {
                return new FormResult(false, ex.Message);
            }

            return new FormResult(false, "Login failed");
        }
    }
}