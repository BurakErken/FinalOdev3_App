using FinalOdevApp.Model;
using System.Collections.ObjectModel;

namespace FinalOdevApp;

public partial class Todo : ContentPage
{
    private readonly FirestoreService _firebaseService;
    public ObservableCollection<TodoItem> Todos { get; set; }
    public Command<TodoItem> DeleteCommand { get; set; }

    public Todo()
    {
        InitializeComponent();
        _firebaseService = new FirestoreService();
        Todos = new ObservableCollection<TodoItem>();
        DeleteCommand = new Command<TodoItem>(async (todo) => await DeleteTodo(todo));
        BindingContext = this;
        LoadTodos();
    }

    private async void LoadTodos()
    {
        var todos = await _firebaseService.GetTodosAsync();
        Todos.Clear();
        foreach (var todo in todos)
        {
            Todos.Add(todo);
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        AddForm.IsVisible = !AddForm.IsVisible;
        if (string.IsNullOrWhiteSpace(TodoEntry.Text))
            return;

        var todo = new TodoItem
        {
            Title = TodoEntry.Text,
            Description = DescriptionEntry.Text,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _firebaseService.AddTodoAsync(todo);
        TodoEntry.Text = string.Empty;
        DescriptionEntry.Text = string.Empty;
        LoadTodos();
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is TodoItem todo)
        {
            await Navigation.PushAsync(new AddPage(todo, _firebaseService));
        }
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is TodoItem todo)
        {
            bool answer = await DisplayAlert("Silme Onayý", "Bu görevi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (answer)
            {
                await DeleteTodo(todo);
                LoadTodos();
            }
        }
    }
    private async void OnTodoCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoItem todo)
        {
            todo.IsCompleted = e.Value;
            await _firebaseService.UpdateTodoAsync(todo);
        }
    }

    private async Task DeleteTodo(TodoItem todo)
    {
        await _firebaseService.DeleteTodoAsync(todo.Id);
        Todos.Remove(todo);
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        try
        {
            LoadTodos();
            // Ýsteðe baðlý: Yenileme baþarýlý mesajý
            await DisplayAlert("Baþarýlý", "Liste yenilendi", "Tamam");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Liste yenilenirken bir hata oluþtu", "Tamam");
        }
    }
}