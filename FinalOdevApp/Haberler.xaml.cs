using FinalOdevApp.Model;
namespace FinalOdevApp;

public partial class Haberler : ContentPage
{
    private readonly NewsViewModel _viewModel;

    public Haberler()
    {
        InitializeComponent();
        _viewModel = new NewsViewModel();
        BindingContext = _viewModel;
    }
    private async void OnNewsItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.FirstOrDefault() is RssItem selectedItem)
        {
            // Haber baðlantýsýný varsayýlan tarayýcýda aç
            await Launcher.Default.OpenAsync(selectedItem.Link);
        }

        // Seçimi temizle (isteðe baðlý)
        var collectionView = sender as CollectionView;
        if (collectionView != null)
        {
            collectionView.SelectedItem = null;
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadNewsAsync("Manþet"); // Varsayýlan kategori
    }

    private async void OnCategoryClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await _viewModel.LoadNewsAsync(button.Text);
        }
    }

    private async void OnNewsItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string url)
        {
            try
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Haber açýlamadý: " + ex.Message, "Tamam");
            }
        }
    }
}