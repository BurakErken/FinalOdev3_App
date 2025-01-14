using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalOdevApp.Model
{

    public class RegisterViewModel : BaseViewModel
    {
        private readonly FirebaseService _firebaseService;
        private string _email;
        private string _password;
        private string _username;
        public RegisterViewModel()
        {
            _firebaseService = new FirebaseService("AIzaSyCxGi-DnSzoViGMj2RLXAcFy6SW6OqhkUQ"); // Key'i gizli tut
        }
        public string UserName
        {
            get => _username;
            set => SetProperty(ref _username, value);
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

        public Command RegisterCommand { get; }

        public async Task RegisterAsync()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Hata", "Geçersiz bilgiler.", "Tamam");
                return;
            }

            try
            {
                var token = await _firebaseService.RegisterUserAsync(UserName, Email, Password);
                await Application.Current.MainPage.DisplayAlert("Başarılı", "Kayıt başarılı! " , "Tamam");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }

}
