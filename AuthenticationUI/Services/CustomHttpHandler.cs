using Blazored.LocalStorage;

namespace AuthenticationUI.Services
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        public CustomHttpHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
