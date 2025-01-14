using FinalOdevApp.Model;

namespace FinalOdevApp;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
	}

    private void GoToRegisterPageAsync(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Register());
    }
}