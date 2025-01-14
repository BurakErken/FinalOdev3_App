using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalOdevApp.Model
{
   public class WeatherModel
   {
        public string? City { get; set; }
        public string? WeatherImageUrl { get; set; }
   }
   public class WeatherService
   {
        public List<WeatherModel> WeatherList { get; private set; } = new List<WeatherModel>();
        private const string API_URL = "http://www.mgm.gov.tr/sunum/tahmin-show-2.aspx";

        public void AddWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return;

            city = city.Replace('Ç', 'C');
            city = city.Replace('Ğ', 'G');
            city = city.Replace('İ', 'I');
            city = city.Replace('Ö', 'O');
            city = city.Replace('Ü', 'U');
            city = city.Replace('Ş', 'S');

            var weatherModel = new WeatherModel
            {

                City = city.ToUpper(),
                WeatherImageUrl = $"{API_URL}?m={city}&basla=1&bitir=5&rC=111&rZ=fff"
            };

            WeatherList.Add(weatherModel);
        }
        public void RemoveWeather(WeatherModel weather)
        {
            WeatherList.Remove(weather);
        }
    }
}
