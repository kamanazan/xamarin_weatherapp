using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

using Xamarin.Forms;

namespace WeatherApp
{
    public class Core : INotifyPropertyChanged
    {
        #region property event handler
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
        #endregion property event handler

        #region private declaration
        private string _ZipCode;
        private Weather weather = new Weather();
        private bool UseMetric;
        private Country _SelectedCountry;
        private bool _CanGetWeather = true;

        private static async Task<Weather> RetrieveWeather(string pZipCode, string pCountryCode, bool pUseMetric)
        {
            string unit_type = pUseMetric ? "metric" : "imperial";
            string temperature_utype = pUseMetric ? "C" : "F";
            string windspeed_utype = pUseMetric ? "kph" : "mph";

            string key = "d67b8e1df3446927be734bfa2d81674e";

            string queryString = $"http://api.openweathermap.org/data/2.5/weather?zip={pZipCode},{pCountryCode}&appid={key}&units={unit_type}";

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
                    Temperature = $"{(string)results["main"]["temp"]} {temperature_utype}",
                    Wind = $"{(string)results["wind"]["speed"]} {windspeed_utype}",
                    Visibility = (string)results["visibility"],
                    Humidity = $"{(string)results["main"]["humidity"]} %",
                    Sunrise = $"{time.AddSeconds((double)results["sys"]["sunrise"]).ToString()} UTC",
                    Sunset = $"{time.AddSeconds((double)results["sys"]["sunset"]).ToString()} UTC",
                };
            }
            else if (results["cod"] != null)
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
        #endregion private declaration

        public string ErrorMessage { get; set; }
        public string ZipCode
        {
            get
            { return _ZipCode; }
            set
            {
                _ZipCode = value;
                OnPropertyChanged(nameof(ZipCode));
            }
        }

        public void SelectCountryByISOCode()
        {
            foreach (Country item in Countries)
                if (item.CountryCode == RegionInfo.CurrentRegion.TwoLetterISORegionName)
                    SelectedCountry = item;
        }

        public Country SelectedCountry
        {
            get { return _SelectedCountry;  }
            set
            {
                if (_SelectedCountry != value)
                {
                    _SelectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));

                    UseMetric = new RegionInfo(SelectedCountry?.CountryCode).IsMetric;

                    ZipCode = "";
                }
            }
        }

        public ObservableCollection<Country> Countries = Country.Init();

        public string ButtonContent { get { return "Get Weather"; } }

        public bool CanGetWeather
        {
            get { return _CanGetWeather; }
            set
            {
                if (_CanGetWeather != value)
                {
                    _CanGetWeather = value;
                    OnPropertyChanged(nameof(CanGetWeather));
                }
            }
        }

        public ICommand GetWeather { get; private set; }

        public Core()
        {
            GetWeather = new Command(async () => await _GetWeather(), () => CanGetWeather);
        }

        public async Task _GetWeather()
        {
            weather = await RetrieveWeather(ZipCode, SelectedCountry?.CountryCode, UseMetric);

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
