using System.Diagnostics;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleBlogMauiApp.Models;
using SimpleBlogMauiApp.Views;

namespace SimpleBlogMauiApp.ViewModels
{
	public partial class UserViewModel: ObservableObject
	{
		private readonly string api = "http://localhost:8080";

		private string _username;
		private string _password;

		public string Username
		{
			get => _username;
			set => SetProperty(ref _username, value);
		}

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		private readonly HttpClient httpClient;

		public UserViewModel() {
			httpClient = new HttpClient();
		}

		private static async void DisplayAlert(string title, string message)
		{
			Debug.WriteLine(title, message);
			await Application.Current.MainPage.DisplayAlert(title, message, "OK");
		}

		[RelayCommand]
		private async Task Login()
		{
			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
			{
				DisplayAlert("Error", "Please enter username and password");
				return;
			}

			//var url = api + "/api/user/login/?username=user1&password=password1";
			var url = $"{api}/api/user/login/?username={Username}&password={Password}";
			try
			{
				var response = await httpClient.GetAsync(url);
				//if (response.IsSuccessStatusCode)
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					//await Shell.Current.GoToAsync($"//{nameof(ManagePage)}", true,
					//	new Dictionary<string, object> {
					//		{ nameof(User), User }
					//	});
					User.Username = Username;
					User.Id = 1;
					User.Token = responseMessage.Content.ToString();
					await Shell.Current.GoToAsync($"//{nameof(ManagePage)}");
				}
				else
				{
					DisplayAlert("Error", responseMessage.Content.ToString());
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
		}

	}
}
