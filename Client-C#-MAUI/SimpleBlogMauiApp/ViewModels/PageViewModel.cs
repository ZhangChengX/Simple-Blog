using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleBlogMauiApp.Models;
using SimpleBlogMauiApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Page = SimpleBlogMauiApp.Models.Page;

namespace SimpleBlogMauiApp.ViewModels
{
	[QueryProperty(nameof(Page), nameof(Page))]
	internal partial class PageViewModel : ObservableObject
	{
		private readonly string api = "http://localhost:8080";

		[ObservableProperty]
		private bool isBusy;

		[ObservableProperty]
		private bool isAddNew = true;

		[ObservableProperty]
		private bool isEdit = false;

		[ObservableProperty]
		private string pageAction = "Add New";

		private Page _page = new();
		public Page Page
		{
			get => _page;
			set 
			{
				SetPageAction(_page);
				SetProperty(ref _page, value);
			}
		}

		public ObservableCollection<Page> Pages { get; set; } = new();

		private readonly HttpClient httpClient;

		public PageViewModel()
		{
			httpClient = new HttpClient();
			SetPageAction(Page);
			_ = GetPageList();
		}

		//[ObservableProperty]
		//private User user;

		//public void ApplyQueryAttributes(IDictionary<string, object> query)
		//{
		//	//throw new NotImplementedException();
		//	user = query[nameof(user)] as User;
		//	OnPropertyChanged(nameof(user));
		//}

		private static async void DisplayAlert(string title, string message)
		{
			Debug.WriteLine(title, message);
			await Application.Current.MainPage.DisplayAlert(title, message, "OK");
		}

		private void SetPageAction(Page Page)
		{
			if (Page is null || Page.Title is null)
			{
				PageAction = "Add New";
				IsAddNew = true;
				IsEdit = false;
			}
			else
			{
				PageAction = "Edit";
				IsAddNew = false;
				IsEdit = true;
			}
			OnPropertyChanged(nameof(PageAction));
			OnPropertyChanged(nameof(IsAddNew));
			OnPropertyChanged(nameof(IsEdit));
		}

		//private DateTime TimestampToDatetime(long timestamp)
		//{
		//	return new DateTime(timestamp);
		//}

		[RelayCommand]
		public async Task GetPageList()
		{
			if (IsBusy)
			{
				return;
			}
			if (User.Token == null)
			{
				DisplayAlert("Error", "Token is empty. Please login.");
				return;
			}
			var url = api + "/api/page?id=all&token=" + User.Token;
			try
			{
				IsBusy = true;
				var response = await httpClient.GetAsync(url);
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					var pages = JsonSerializer.Deserialize<ObservableCollection<Page>>(responseMessage.Content.ToString());
					Pages = pages;
					OnPropertyChanged(nameof(Pages));
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
			IsBusy = false;
		}

		[RelayCommand]
		private async Task GetPage(int pageId)
		{
			if (IsBusy)
			{
				return;
			}
			var url = $"{api}/api/page?id={pageId}&token={User.Token}";
			try
			{
				IsBusy = true;
				var response = await httpClient.GetAsync(url);
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					var pages = JsonSerializer.Deserialize<Collection<Page>>(responseMessage.Content.ToString());
					if (pages.Count == 1)
					{
						Page = pages[0];
						await Shell.Current.GoToAsync($"//{nameof(Views.EditPage)}", true,
									new Dictionary<string, object> {
										{ nameof(Page), Page },
										//{ nameof(User), User }
									});
					}
					else
					{
						DisplayAlert("Error", "No page found by Page ID: " + pageId.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
			IsBusy = false;
		}

		[RelayCommand]
		private async Task EditPage(int pageId)
		{
			if (IsBusy)
			{
				return;
			}
			var url = $"{api}/api/page/?id={pageId}&token={User.Token}";
			try
			{
				IsBusy = true;
				Page.DateModified = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
				var jsonStr = JsonSerializer.Serialize<Page>(Page);
				var jsonStrContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
				var response = await httpClient.PutAsync(url, jsonStrContent);
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					DisplayAlert(responseMessage.Type, responseMessage.Content.ToString());
					await Shell.Current.GoToAsync($"//{nameof(ManagePage)}");
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
			IsBusy = false;
			await GetPageList();
		}

		[RelayCommand]
		private async Task AddPage()
		{
			if (IsBusy)
			{
				return;
			}
			if (Page.Title is "" || Page.Url is "")
			{
				DisplayAlert("Info", "Title and URL can't be empty.");
				return;
			}
			var url = $"{api}/api/page/?token={User.Token}";
			try
			{
				IsBusy = true;
				Page.UserId = User.Id;
				Page.DatePublished = Page.DateModified = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
				var jsonStr = JsonSerializer.Serialize<Page>(Page);
				var jsonStrContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
				var response = await httpClient.PostAsync(url, jsonStrContent);
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					DisplayAlert(responseMessage.Type, responseMessage.Content.ToString());
					await Shell.Current.GoToAsync($"//{nameof(ManagePage)}");
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
			IsBusy = false;
			await GetPageList();
		}

		[RelayCommand]
		private async Task DeletePage(int pageId)
		{
			if (IsBusy)
			{
				return;
			}
			var url = $"{api}/api/page/?id={pageId}&token={User.Token}";
			try
			{
				IsBusy = true;
				var response = await httpClient.DeleteAsync(url);
				var responseMessage = await response.Content.ReadFromJsonAsync<Message>();
				if (responseMessage.Type is "success")
				{
					DisplayAlert(responseMessage.Type, responseMessage.Content.ToString());
					await Shell.Current.GoToAsync($"//{nameof(ManagePage)}");
				}
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", ex.Message);
			}
			IsBusy = false;
			await GetPageList();
		}
	}
}
