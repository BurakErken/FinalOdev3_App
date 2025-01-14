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
        // Silme iþlemine týklanan nesneyi al
        var button = sender as ImageButton;
        var weatherToDelete = button?.CommandParameter as WeatherModel;

        if (weatherToDelete != null)
        {
            // Listeden kaldýr
            weatherService.RemoveWeather(weatherToDelete);

            // CollectionView'i yenile
            weatherCollection.ItemsSource = null;
            weatherCollection.ItemsSource = weatherService.WeatherList;
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // Kullanýcýdan þehir adý al
        string city = await DisplayPromptAsync("Þehir Ekle", "Hava durumu için bir þehir adý girin:",
                                               "Ekle", "Ýptal", "Örn: Ankara");

        // Boþ ya da geçersiz giriþ kontrolü
        if (!string.IsNullOrWhiteSpace(city))
        {
            weatherService.AddWeather(city);

            // CollectionView'i güncelle
            weatherCollection.ItemsSource = null;
            weatherCollection.ItemsSource = weatherService.WeatherList;
        }
        else if (city != null) // Kullanýcý "Ýptal"e basmadýysa
        {
            await DisplayAlert("Hata", "Geçerli bir þehir adý girin.", "Tamam");
        }
    }
}