using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using World_Countries;

//  https://restcountries.com/#api-endpoints-using-this-project
//  https://restcountries.com/v3.1/region/europe?fields=name
//  https://restcountries.com/v3.1/name/Germany


class Program
{
    static async Task Main()
    {
        bool keepGoing = true;

        while (keepGoing)
        {
            // Regions menu
            string[] regions = { "Africa", "Americas", "Antarctic", "Asia", "Europe", "Oceania" };
            Utilities.DisplayMenu("World Regions", regions);

            Console.Write("Select your region: ");
            string regionChoice = Console.ReadLine() ?? "";

            if (!int.TryParse(regionChoice, out int regionNumber) || regionNumber < 0 || regionNumber > 6)
            {
                Console.WriteLine($"{regionChoice} is an invalid choice, try again.");
            }
            else if (regionNumber == 0)
            {
                keepGoing = false;
            }
            else
            {
                string regionName = regionNumber switch
                {
                    1 => "africa",
                    2 => "americas",
                    3 => "antarctic",
                    4 => "asia",
                    5 => "europe",
                    6 => "oceania",
                    _ => ""
                };

                Console.WriteLine($"\n Here are the countries with the {regionName.ToUpper()} region");

                List<Country> allCountries = await FetchCountryNamesByRegion(regionName);

                Console.WriteLine($"Number of countries: {allCountries.Count}");
                string[] countries = allCountries.Select(c => c.name.common).ToArray();
                Utilities.DisplayMenu("Countries", countries, "Return to main menu");

                bool validStateSelected = false;
                while (!validStateSelected)
                {
                    Console.Write("Select your country: ");
                    string countryChoice = Console.ReadLine() ?? "";

                    if (!int.TryParse(countryChoice, out int countryNumber)
                        || countryNumber < 0 || countryNumber > allCountries.Count)
                    {
                        Console.WriteLine($"{countryChoice} is an invalid choice, try again.");
                    }
                    else if (countryNumber == 0)
                    {
                        validStateSelected = true;
                    }
                    else
                    {
                        Country selectedCountry = await FetchCountryByName(allCountries[countryNumber - 1].name.common);

                        string countryName = selectedCountry.name.common ?? "N/A";
                        string countryOfficial = selectedCountry.name.official ?? "N/A";
                        Console.WriteLine($"\n Country");
                        Console.WriteLine($"\t Official: {countryOfficial}");
                        Console.WriteLine($"\t Common: {countryName}");

                        string subRegion = selectedCountry.subregion ?? "N/A";
                        Console.WriteLine($" Subregion: {subRegion}");

                        string population = selectedCountry.population.ToString() ?? "UNK";
                        Console.WriteLine($" Population: {population}");

                        string countryFlag = selectedCountry.flags.png ?? "N/A";
                        string flagDescription = selectedCountry.flags.alt ?? "N/A";
                        Console.WriteLine($" Flag");
                        Console.WriteLine($"\t Link: {countryFlag}");
                        Console.WriteLine($"\t Description: {flagDescription}");

                        string coatOfArms = selectedCountry.coatOfArms.png ?? "N/A";
                        Console.WriteLine($" Coat of Arms link: {coatOfArms}");

                        string capitalName = selectedCountry.capital?.Count > 0 ? selectedCountry.capital[0] : "N/A";
                        Console.WriteLine($" Capital: {capitalName}");

                        // Display current local time
                        DateOnly localDate = DateOnly.FromDateTime(DateTime.Now);
                        TimeOnly localTime = TimeOnly.FromDateTime(DateTime.Now);
                        Console.WriteLine($" Current Local Date/Time: {localDate} @ {localTime} ");

                        TimeOnly capitalTime = TimeOnly.MinValue;
                        DateOnly capitalDate = DateOnly.MinValue;
                        if (selectedCountry.capitalInfo.latlng.Count > 0)
                        {
                            string capitalLat = selectedCountry.capitalInfo.latlng[0].ToString();
                            string capitalLng = selectedCountry.capitalInfo.latlng[1].ToString();
                            DateTimeInfo capitalDateTimeInfo = await FetchCapitalTime(capitalLat, capitalLng);
                            capitalTime = TimeOnly.Parse(capitalDateTimeInfo.time);
                            capitalDate = DateOnly.Parse(capitalDateTimeInfo.date);
                        }
                        Console.WriteLine($" Current Capital Date/Time:{capitalDate} @ {capitalTime.ToString() ?? "UNK"}");

                        // Calculate time difference
                        DateTime localDateTime = localDate.ToDateTime(localTime);
                        DateTime capitalDateTime = capitalDate.ToDateTime(capitalTime);

                        TimeSpan timeDifference = capitalDateTime - localDateTime;
                        int hoursDifference = (int)Math.Round(timeDifference.TotalHours);

                        Console.WriteLine($"Time Difference: {hoursDifference} hours");

                        validStateSelected = true;
                        Console.Write("\n Press Enter to return to main menu");
                        Console.ReadLine();
                    }
                }
            }

        }

        Console.WriteLine("\n\n Have a great day!\n");
    }

    static async Task<List<Country>> FetchCountryNamesByRegion(string regionName)
    {
        string url = $"https://restcountries.com/v3.1/region/{regionName}?fields=name";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonResponse);

        return countries;
    }

    static async Task<Country> FetchCountryByName(string countryName)
    {
        string url = $"https://restcountries.com/v3.1/name/{countryName}?fullText=true&fields=name,capital,tld,subregion,area,flag,population,fifa,timezones,flags,coatOfArms,startOfWeek,capitalInfo";

        // Console.Write(url);
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonResponse) ?? new List<Country>();

        return countries[0]; // API returns array, we want first item
    }

    static async Task<DateTimeInfo> FetchCapitalTime(string latitude, string longitude)
    {
        string url = $"https://timeapi.io/api/v1/time/current/coordinate?latitude={latitude}&longitude={longitude}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        DateTimeInfo timeInfo = JsonSerializer.Deserialize<DateTimeInfo>(jsonResponse);
        return timeInfo;
    }

}
