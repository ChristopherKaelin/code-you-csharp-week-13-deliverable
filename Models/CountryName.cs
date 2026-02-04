using System.Collections.Generic;

namespace World_Countries;

public class CountryName
{
    public string common { get; set; } = "N/A";
    public string official { get; set; } = "N/A";
    public Dictionary<string, NativeName> nativeName { get; set; } = new Dictionary<string, NativeName>();
}