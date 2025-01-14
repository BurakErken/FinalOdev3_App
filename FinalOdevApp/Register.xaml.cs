using FinalOdevApp.Model;
namespace FinalOdevApp;

public partial class Register : ContentPage
{
    public RegisterViewModel _viewModel;
	public Register()
	{
		InitializeComponent();
        _viewModel = new RegisterViewModel();
        BindingContext = _viewModel;
    }


    private void GoToLoginPageAsync(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Login());
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        var result = _viewModel.RegisterAsync();
        if (result != null) 
        {
            await Navigation.PushAsync(new Login());
        }
    }
}