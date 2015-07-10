using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EeVeeCee1._0
{
    /// <summary>
    /// Class to represent a query returned by the JSON query
    /// </summary>
    public class Rootobject
    {
        public Fuel_Stations[] fuel_stations { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int? offset { get; set; }
        public Precision precision { get; set; }
        public string station_locator_url { get; set; }
        public int? total_results { get; set; }
    }
    /// <summary>
    /// Class to represent the precision of the query
    /// </summary>
    public class Precision
    {
        public string name { get; set; }
        public string[] types { get; set; }
        public int? value { get; set; }
    }
    /// <summary>
    /// Class to represent a fuel station
    /// </summary>
    public class Fuel_Stations
    {
        public string access_days_time { get; set; }
        public object bd_blends { get; set; }
        public string cards_accepted { get; set; }
        public string city { get; set; }
        public string date_last_confirmed { get; set; }
        public object expected_date { get; set; }
        public string fuel_type_code { get; set; }
        public string geocode_status { get; set; }
        public string groups_with_access_code { get; set; }
        public object hy_status_link { get; set; }
        public string intersection_directions { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string open_date { get; set; }
        public string owner_type_code { get; set; }
        public object plus4 { get; set; }
        public string station_name { get; set; }
        public string station_phone { get; set; }
        public string status_code { get; set; }
        public string street_address { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public object ng_fill_type_code { get; set; }
        public object ng_psi { get; set; }
        public object ng_vehicle_class { get; set; }
        public object e85_blender_pump { get; set; }
        public int? ev_level1_evse_num { get; set; }
        public int? ev_level2_evse_num { get; set; }
        public int? ev_dc_fast_num { get; set; }
        public string ev_other_evse { get; set; }
        public string ev_network { get; set; }
        public string ev_network_web { get; set; }
        public object lpg_primary { get; set; }
        public int id { get; set; }
        public DateTime updated_at { get; set; }
        public float? distance { get; set; }
        public Federal_Agency federal_agency { get; set; }
    }
    /// <summary>
    /// Class to represent a federal agency
    /// </summary>
    public class Federal_Agency
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
