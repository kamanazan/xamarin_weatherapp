using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WeatherApp
{
    public class Core : INotifyPropertyChanged
    {
        //private string _ZipCode;
        private Weather weather = new Weather();

        public string ErrorMessage { get; set; }
        public string ZipCode { get; set;  }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // Called everytime we set value of Weather properties (Title, Temperature,..etc)
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Country _SelectedCountry;
        public Country SelectedCountry
        {
            get { return _SelectedCountry;  }
            set
            {
                if (_SelectedCountry != value)
                {
                    _SelectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        public ObservableCollection<Country> Countries = Country.Init();

        private static async Task<Weather> GetWeather(string pZipCode, string pCountryCode)
        {
            string key = "d67b8e1df3446927be734bfa2d81674e";
            string queryString = $"http://api.openweathermap.org/data/2.5/weather?zip={pZipCode},{pCountryCode}&appid={key}&units=imperial";
            if (key != "d67b8e1df3446927be734bfa2d81674e")
            {
                throw new ArgumentException("You need api key from http://openweathermap.org/appid");
            }

            dynamic results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

            if (results["weather"] != null)
            {
                DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                return new Weather
                {
                    Title = (string)results["name"],
                    Temperature =  $"{(string)results["main"]["temp"]} F",
                    Wind =  $"{(string)results["wind"]["speed"]} mph",
                    Visibility = (string)results["visibility"],
                    Humidity =  $"{(string)results["main"]["humidity"]} %",
                    Sunrise = $"{time.AddSeconds((double)results["sys"]["sunrise"]).ToString()} UTC",
                    Sunset = $"{time.AddSeconds((double)results["sys"]["sunset"]).ToString()} UTC",
                };
            }
            else if(results["cod"] != null)
            {
                return new Weather
                {
                    Title = $"{(string)results["cod"]} {(string)results["message"]}"
                };
            }
            else
            {
                return null;
            }
        }

        public async void GetWeather()
        {
            weather = await GetWeather(ZipCode, SelectedCountry?.CountryCode);

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Temperature));
            OnPropertyChanged(nameof(Wind));
            OnPropertyChanged(nameof(Humidity));
            OnPropertyChanged(nameof(Visibility));
            OnPropertyChanged(nameof(Sunrise));
            OnPropertyChanged(nameof(Sunset));
        }

        #region Binding Fields
        public string Title
        {
            get
            {
                return weather.Title;
            }
            set
            {
                if (weather.Title != value)
                { 
                    weather.Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public string Temperature
        {
            get
            {
                return weather.Temperature;
            }
            set
            {
                weather.Temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        public string Humidity
        {
            get
            {
                return weather.Humidity;
            }
            set
            {
                weather.Humidity = value;
                OnPropertyChanged("Humidity");
            }
        }

        public string Wind
        {
            get
            {
                return weather.Wind;
            }
            set
            {
                weather.Wind = value;
                OnPropertyChanged("Wind");
            }
        }
        
        public string Visibility
        {
            get
            {
                return weather.Visibility;
            }
            set
            {
                weather.Visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        public string Sunrise
        {
            get
            {
                return weather.Sunrise;
            }
            set
            {
                weather.Sunrise = value;
                OnPropertyChanged("Sunrise");
            }
        }

        public string Sunset
        {
            get
            {
                return weather.Sunset;
            }
            set
            {
                weather.Sunset = value;
                OnPropertyChanged("Sunset");
            }
        }
        #endregion Binding Fields
    }
}
