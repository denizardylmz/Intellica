using System.Collections.Generic;

namespace NoteAPI.API.DataContracts.ExternalAPIContracts;

public class WeatherResponse
{
    public Location Location { get; set; }
    public Current Current { get; set; }
    public Forecast forecast { get; set; }
}

public class Location
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Tz_Id { get; set; }
    public long Localtime_Epoch { get; set; }
    public string Localtime { get; set; }
}

public class Current
{
    public long Last_Updated_Epoch { get; set; }
    public string Last_Updated { get; set; }
    public double Temp_C { get; set; }
    public double Temp_F { get; set; }
    public int Is_Day { get; set; }
    public Condition Condition { get; set; }
    public double Wind_Mph { get; set; }
    public double Wind_Kph { get; set; }
    public int Wind_Degree { get; set; }
    public string Wind_Dir { get; set; }
    public double Pressure_Mb { get; set; }
    public double Pressure_In { get; set; }
    public double Precip_Mm { get; set; }
    public double Precip_In { get; set; }
    public int Humidity { get; set; }
    public int Cloud { get; set; }
    public double Feelslike_C { get; set; }
    public double Feelslike_F { get; set; }
    public double Windchill_C { get; set; }
    public double Windchill_F { get; set; }
    public double Heatindex_C { get; set; }
    public double Heatindex_F { get; set; }
    public double Dewpoint_C { get; set; }
    public double Dewpoint_F { get; set; }
    public double Vis_Km { get; set; }
    public double Vis_Miles { get; set; }
    public double Uv { get; set; }
    public double Gust_Mph { get; set; }
    public double Gust_Kph { get; set; }
}

public class Condition
{
    public string Text { get; set; }
    public string Icon { get; set; }
    public int Code { get; set; }
}

public class Forecast
{
    public List<ForecastDay> forecastday { get; set; }
}

public class ForecastDay
{
    public string date { get; set; }
    public long date_epoch { get; set; }
    public Day day { get; set; }
    public Astro astro { get; set; }
    public List<Hour> hour { get; set; }
}

public class Day
{
    public double maxtemp_c { get; set; }
    public double maxtemp_f { get; set; }
    public double mintemp_c { get; set; }
    public double mintemp_f { get; set; }
    public double avgtemp_c { get; set; }
    public double avgtemp_f { get; set; }
    public double maxwind_mph { get; set; }
    public double maxwind_kph { get; set; }
    public double totalprecip_mm { get; set; }
    public double totalprecip_in { get; set; }
    public double totalsnow_cm { get; set; }
    public double avgvis_km { get; set; }
    public double avgvis_miles { get; set; }
    public int avghumidity { get; set; }
    public int daily_will_it_rain { get; set; }
    public int daily_chance_of_rain { get; set; }
    public int daily_will_it_snow { get; set; }
    public int daily_chance_of_snow { get; set; }
    public Condition condition { get; set; }
    public double uv { get; set; }
}

public class Astro
{
    public string sunrise { get; set; }
    public string sunset { get; set; }
    public string moonrise { get; set; }
    public string moonset { get; set; }
    public string moon_phase { get; set; }
    public int moon_illumination { get; set; }
    public int is_moon_up { get; set; }
    public int is_sun_up { get; set; }
}

public class Hour
{
    public long time_epoch { get; set; }
    public string time { get; set; }
    public double temp_c { get; set; }
    public double temp_f { get; set; }
    public int is_day { get; set; }
    public Condition condition { get; set; }
    public double wind_mph { get; set; }
    public double wind_kph { get; set; }
    public int wind_degree { get; set; }
    public string wind_dir { get; set; }
    public double pressure_mb { get; set; }
    public double pressure_in { get; set; }
    public double precip_mm { get; set; }
    public double precip_in { get; set; }
    public double snow_cm { get; set; }
    public int humidity { get; set; }
    public int cloud { get; set; }
    public double feelslike_c { get; set; }
    public double feelslike_f { get; set; }
    public double windchill_c { get; set; }
    public double windchill_f { get; set; }
    public double heatindex_c { get; set; }
    public double heatindex_f { get; set; }
    public double dewpoint_c { get; set; }
    public double dewpoint_f { get; set; }
    public int will_it_rain { get; set; }
    public int chance_of_rain { get; set; }
    public int will_it_snow { get; set; }
    public int chance_of_snow { get; set; }
    public double vis_km { get; set; }
    public double vis_miles { get; set; }
    public double gust_mph { get; set; }
    public double gust_kph { get; set; }
    public double uv { get; set; }
}

