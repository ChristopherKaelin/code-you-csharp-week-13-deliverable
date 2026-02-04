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
            Console.WriteLine("\n\n");

            Console.WriteLine("  World Regions  ");
            Console.WriteLine("=================");
            Console.WriteLine("1. Africa ");
            Console.WriteLine("2. Americas ");
            Console.WriteLine("3. Antarctic ");
            Console.WriteLine("4. Asia ");
            Console.WriteLine("5. Europe ");
            Console.WriteLine("6. Oceania ");
            Console.WriteLine("0. Exit Program ");

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
                for (int i = 0; i < allCountries.Count; i += 2)
                {
                    Console.Write($"{i + 1}. {allCountries[i].name.common.PadRight(30)} ");
                    if ((i + 1) < allCountries.Count)
                        Console.WriteLine($"\t{i + 2}. {allCountries[i + 1].name.common} ");
                    else
                        Console.WriteLine();
                }
                Console.WriteLine("0. Return to main menu ");

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

                        string capitalName = selectedCountry.capital?.Count > 0 ? selectedCountry.capital[0] : "N/A";
                        Console.WriteLine($" Capital: {capitalName}");

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
        string url = $"https://restcountries.com/v3.1/name/{countryName}?fields=name,capital,tld,subregion,area,flag,population,fifa,timezones,flags,coatOfArms,startOfWeek";

        Console.Write(url);
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonResponse);

        return countries[0]; // API returns array, we want first item
    }

}
