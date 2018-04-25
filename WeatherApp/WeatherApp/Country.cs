using System.Collections.ObjectModel; // wat?

namespace WeatherApp
{
    public class Country
    {
        public string CountryCode;
        public string CountryName;

        public override string ToString() => $"{CountryName} ({CountryCode})";

        public static ObservableCollection<Country> Init()
        {
            ObservableCollection<Country> CountryList = new ObservableCollection<Country>
            {
                new Country() { CountryCode = "ID", CountryName = "Indonesia" },
                new Country() { CountryCode = "US", CountryName = "America" },
                new Country() { CountryCode = "BE", CountryName = "Belgium" },
                new Country() { CountryCode = "MK", CountryName = "Macedonia" }
            };

            return CountryList;

        }
    }
}
