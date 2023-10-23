using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
	public class AuthenticationService
	{
		private readonly IClient _httpClient;
		private readonly ILocalStorageService _localStorageService;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public AuthenticationService(IClient httpClient, ILocalStorageService localStorageService,
			AuthenticationStateProvider authenticationStateProvider)
		{
			_httpClient = httpClient;
			_localStorageService = localStorageService;
			_authenticationStateProvider = authenticationStateProvider;
		}
		public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
		{
			var authResponse = await _httpClient.LoginAsync(loginModel);
			//store token
			await _localStorageService.SetItemAsync("accessToken", authResponse.Token);

			//change auth state of app
			await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
				.LoggedIn();

			return true;
		}

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .LoggedOut();
        }
    }
}
