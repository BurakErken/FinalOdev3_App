namespace FinalOdevApp;

public partial class Ayarlar : ContentPage
{
    public bool IsDarkTheme
    {
        get => Preferences.Get("IsDarkTheme", false);
        set => Preferences.Set("IsDarkTheme", value);
    }

    public Ayarlar()
	{
		InitializeComponent();

        ThemeToggle.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {

        if (e.Value)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }

        Preferences.Set("Theme", e.Value);
    }


}