using AuthenticationUI.Models;
using AuthenticationUI.Results;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace AuthenticationUI.Services
{
    public class CustomAuthService : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private NavigationManager _navigationManager;
        private readonly ClaimsPrincipal Unauthenticated =
            new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthService(HttpClient httpClient, ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var decodedPayload = System.Text.Encoding.UTF8.GetString(jsonBytes);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = new List<Claim>();

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key == "role")
                {
                    // If the role is an array, deserialize it and add each role as a claim
                    if (kvp.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var role in jsonElement.EnumerateArray())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.GetString()));
                        }
                    }
                    else
                    {
                        // If the role is a single value, add it directly
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()));
                    }
                }
                else
                {
                    // Add other claims
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }
            

            return claims;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorageService.GetItemAsync<string>("accessToken");

                if (string.IsNullOrEmpty(token))
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

                    var tokenObject = JsonSerializer.Deserialize<Dictionary<string, string>>(tokenResponse);

                    if (tokenObject != null && tokenObject.TryGetValue("token", out var token))
                    {
                        // Store the token in local storage
                        await _localStorageService.SetItemAsync("accessToken", token);

                        // Notify the authentication state has changed
                        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                        return new FormResult(true, "Login successful");
                    }
                } else
                {
                    // Read error message from response
                    var errorContent = await res.Content.ReadAsStringAsync();
                    return new FormResult(false, errorContent);
                }
            }
            catch(Exception ex)
            {
                return new FormResult(false, ex.Message);
            }

            return new FormResult(false, "Login failed");
        }

        public async Task<FormResult> RegisterAsync(RegisterDTO registerDTO)
        {

            try
            {
                var res = await _httpClient.PostAsJsonAsync(
                    "/api/Auth/Register", registerDTO);
                Console.WriteLine(res);
                if (res.IsSuccessStatusCode)
                {
                    return new FormResult(true, "User Created Successfully");
                }
            }
            catch (Exception ex)
            {
                return new FormResult(false, ex.Message);
            }

            return new FormResult(false, "Register failed");
        }
        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("accessToken");
            NotifyAuthenticationStateChanged(Task.FromResult( new AuthenticationState(Unauthenticated)));
            _navigationManager.NavigateTo("/login", forceLoad: true);
        }

        public async Task<List<User>> GetAllUserAsync()
        {
                // Get role from JWT token
                var token = await _localStorageService.GetItemAsync<string>("accessToken");

                if (!string.IsNullOrEmpty(token))
                {
                    // add token to headers
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    return null;

                }

                    var claims = ParseClaimsFromJwt(token);
                var userRole = claims.FirstOrDefault( c => c.Type == ClaimTypes.Role)?.Value;

                if (userRole != "Admin")
                {
                    return null;
                }

                //proceed with the API call
                return  await _httpClient.GetFromJsonAsync<List<User>>("api/Auth/user");
        
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<bool>($"api/Auth/user/{id}");
        }

    }
}