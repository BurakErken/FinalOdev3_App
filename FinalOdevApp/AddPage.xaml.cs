namespace FinalOdevApp;
using FinalOdevApp.Model;
public partial class AddPage : ContentPage
{
    private TodoItem _todo;
    private readonly FirestoreService _firebaseService;

    public AddPage(TodoItem todo, FirestoreService firebaseService)
	{
		InitializeComponent();
        _todo = todo;
        _firebaseService = firebaseService;

        // Mevcut deðerleri forma yükle
        TitleEntry.Text = todo.Title;
        DescriptionEditor.Text = todo.Description;
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Uyarý", "Baþlýk boþ olamaz", "Tamam");
            return;
        }

        try
        {
            _todo.Title = TitleEntry.Text;
            _todo.Description = DescriptionEditor.Text;

            await _firebaseService.UpdateTodoAsync(_todo);
            await DisplayAlert("Baþarýlý", "Not baþarýyla güncellendi", "Tamam");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Güncelleme sýrasýnda bir hata oluþtu", "Tamam");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}