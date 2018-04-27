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
            // We use BindingContext whenever we want to put data on the UI
            // Since BindingContext type is object, we have to cast it to target class, in this case Core()
            cbxCountry.ItemsSource = ((Core)BindingContext).Countries;

            ((Core)BindingContext).SelectCountryByISOCode();

            edtZipCode.Completed += (s, e) => btnGetWeather_Click(s, e); // What is this mean
		}

        protected override void OnAppearing()
        {
            //base.OnAppearing();

            if (cbxCountry.SelectedIndex > -1)
                edtZipCode.Focus();
            else
                cbxCountry.Focus();

            base.OnAppearing();
        }

        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(edtZipCode.Text))
            {
                ((Core)BindingContext).GetWeather();
                
            }
        }
	}
}
