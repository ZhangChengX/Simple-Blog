using SimpleBlogMauiApp.ViewModels;

namespace SimpleBlogMauiApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new UserViewModel();
	}
}