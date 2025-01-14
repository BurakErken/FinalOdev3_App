namespace FinalOdevApp;
using FinalOdevApp.Model;
public partial class Hava : ContentPage
{
    private readonly WeatherService weatherService;

    public Hava()
    {
        InitializeComponent();
        weatherService = new WeatherService();
        BindingContext = weatherService;
    }
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        // Silme i�lemine t�klanan nesneyi al
        var button = sender as ImageButton;
        var weatherToDelete = button?.CommandParameter as WeatherModel;

        if (weatherToDelete != null)
        {
            // Listeden kald�r
            weatherService.RemoveWeather(weatherToDelete);

            // CollectionView'i yenile
            weatherCollection.ItemsSource = null;
            weatherCollection.ItemsSource = weatherService.WeatherList;
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // Kullan�c�dan �ehir ad� al
        string city = await DisplayPromptAsync("�ehir Ekle", "Hava durumu i�in bir �ehir ad� girin:",
                                               "Ekle", "�ptal", "�rn: Ankara");

        // Bo� ya da ge�ersiz giri� kontrol�
        if (!string.IsNullOrWhiteSpace(city))
        {
            weatherService.AddWeather(city);

            // CollectionView'i g�ncelle
            weatherCollection.ItemsSource = null;
            weatherCollection.ItemsSource = weatherService.WeatherList;
        }
        else if (city != null) // Kullan�c� "�ptal"e basmad�ysa
        {
            await DisplayAlert("Hata", "Ge�erli bir �ehir ad� girin.", "Tamam");
        }
    }
}