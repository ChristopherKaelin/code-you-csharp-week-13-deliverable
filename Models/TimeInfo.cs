namespace World_Countries;

public class DateTimeInfo
{
    public string date_time { get; set; } = "";
    public string date { get; set; } = "";
    public string time { get; set; } = "";
    public string day_of_week { get; set; } = "";
    public bool dst_active { get; set; }
    public string timezone { get; set; } = "";
    public int utc_offset_seconds { get; set; }
}