using FinalOdevApp.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
namespace FinalOdevApp;

public partial class Kurlar : ContentPage
{
    public ObservableCollection<CurrencyItem> Currencies { get; set; }
    public Kurlar()
    {
        InitializeComponent();
        Currencies = new ObservableCollection<CurrencyItem>();
        CurrencyCollectionView.ItemsSource = Currencies;
        LoadCurrencies();
    }

    private async void LoadCurrencies()
    {
        try
        {
            string apiUrl = "https://finans.truncgil.com/today.json";
            HttpClient client = new HttpClient();
            var jsonResponse = await client.GetStringAsync(apiUrl);

            var response = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonResponse);

            if (response == null)
            {
                await DisplayAlert("Hata", "Veri çekilemedi.", "Tamam");
                return;
            }

            Currencies.Clear();
            foreach (var item in response)
            {
                if (item.Key != "Update_Date" && item.Value is JsonElement element && element.TryGetProperty("Satýþ", out _))
                {
                    string change = element.TryGetProperty("Deðiþim", out var changeProperty) ? changeProperty.GetString() : "0";
                    change = change.Replace("%", "").Replace(",", "."); // "%" iþaretini ve virgülü noktaya çeviriyoruz
                    double.TryParse(change, NumberStyles.Any, CultureInfo.InvariantCulture, out double changeValue);

                    Currencies.Add(new CurrencyItem
                    {
                        CurrencyType = item.Key,
                        Buy = element.GetProperty("Alýþ").GetString(),
                        Sell = element.GetProperty("Satýþ").GetString(),
                        Change = change,
                        Image = changeValue < 0 ? "decrease2.png" : "increase.png", // Deðiþime göre resim atanýyor
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Döviz kurlarý yüklenemedi: {ex.Message}", "Tamam");
        }
    }

    private void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadCurrencies();
    }
}

public class CurrencyItem
{
    public string CurrencyType { get; set; }
    public string Buy { get; set; }
    public string Sell { get; set; }
    public string Change { get; set; }
    public string Image { get; set; }
}
    

