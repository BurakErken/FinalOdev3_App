using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FinalOdevApp.Model
{
    public class FirebaseService
    {
        private readonly string _firebaseApiKey;

        public FirebaseService(string firebaseApiKey)
        {
            _firebaseApiKey = firebaseApiKey;
        }

        public async Task<string> RegisterUserAsync(string username, string email, string password)
        {
            var client = new HttpClient();
            var requestData = new
            {
                username = username,
                email = email,
                password = password,
                returnSecureToken = true
            };

            var response = await client.PostAsJsonAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_firebaseApiKey}",
                requestData
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                return result.IdToken; // Kullanıcı giriş yaparken kullanabileceğiniz token
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error.Error.Message);
            }
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var client = new HttpClient();
            var requestData = new
            {   
                email = email,
                password = password,
                returnSecureToken = true
            };

            var response = await client.PostAsJsonAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}",
                requestData
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result.IdToken; // Kullanıcı giriş yaparken kullanabileceğiniz token
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error.Error.Message);
            }
        }
    }

    public class RegisterResponse
    {
        public string IdToken { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }

    public class LoginResponse
    {
        public string IdToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ErrorResponse
    {
        public FirebaseError Error { get; set; }
    }

    public class FirebaseError
    {
        public string Message { get; set; }
    }

}
