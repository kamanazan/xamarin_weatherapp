using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeatherApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            // Moved the binding in the xaml file
            //btnGetWeather.Clicked += btnGetWeather_Click;
            BindingContext = new Core(); // Property of BindableObject class
            // Since BindingContext type is object, we have to cast it to target class, in this case Core()
            cbxCountry.ItemsSource = ((Core)BindingContext).Countries;
		}

        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(edtZipCode.Text))
            {
                ((Core)BindingContext).GetWeather();
                //Weather weather = await Core.GetWeather(edtZipCode.Text);

                //if (weather != null)
                //{
                //    txtLocation.Text = weather.Title;
                //    txtTemperature.Text = weather.Temperature;
                //    txtWind.Text = weather.Wind;
                //    txtVisibility.Text = weather.Visibility;
                //    txtHumidity.Text = weather.Humidity;
                //    txtSunrise.Text = weather.Sunrise;
                //    txtSunset.Text = weather.Sunset;

                //    btnGetWeather.Text = "Search Again";
                //}
            }
        }
	}
}
