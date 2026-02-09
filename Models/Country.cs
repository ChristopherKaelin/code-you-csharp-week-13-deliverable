using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace World_Countries;

public class Country
{
    public CountryName name { get; set; } = new CountryName();
    public List<string> capital { get; set; } = new List<string>();
    public List<string> tld { get; set; } = new List<string>();
    public string subregion { get; set; } = "N/A";
    public double area { get; set; }
    public string flag { get; set; } = "N/A";
    public int population { get; set; }
    public string fifa { get; set; } = "N/A";
    public List<string>? timezones { get; set; }
    public Flags flags { get; set; } = new Flags();
    public CoatOfArms coatOfArms { get; set; } = new CoatOfArms();
    public string startOfWeek { get; set; } = "UNK";
    public CapitalInfo capitalInfo { get; set; } = new CapitalInfo();

}