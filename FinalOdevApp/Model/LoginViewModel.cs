using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalOdevApp.Model
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly FirebaseService _firebaseService;
        private string _email;
        private string _password;

        public LoginViewModel()
        {
            _firebaseService = new FirebaseService("AIzaSyCxGi-DnSzoViGMj2RLXAcFy6SW6OqhkUQ"); // Key'i gizli tut!
            LoginCommand = new Command(async () => await LoginAsync());
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public Command LoginCommand { get; }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Geçersiz bilgiler.", "Tamam");
                return;
            }

            try
            {
                var token = await _firebaseService.LoginUserAsync(Email, Password);
                await Application.Current.MainPage.DisplayAlert("Başarılı", "Giriş başarılı! " , "Tamam");
                // Ana sayfaya yönlendirme
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
    
}
