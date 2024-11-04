using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopApp.Classes
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public static List<City> GetAllCities()
        {
            List<City> cities = new List<City>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        @"SELECT cityId, city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy FROM city";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                City city = new City
                                {
                                    CityId = reader.GetInt32("cityId"),
                                    CityName = reader.GetString("city"),
                                    CountryId = reader.GetInt32("countryId"),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };

                                cities.Add(city);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving cities: {ex.Message}");
            }

            return cities;
        }
    }
}
