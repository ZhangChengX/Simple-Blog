using SimpleBlogMauiApp.ViewModels;

namespace SimpleBlogMauiApp.Views;

public partial class ManagePage : ContentPage
{
	public ManagePage()
	{
		InitializeComponent();
		BindingContext = new PageViewModel();
    }
}