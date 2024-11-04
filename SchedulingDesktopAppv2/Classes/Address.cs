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
    public class Address
    {
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public static List<Address> GetAllAddresses()
        {
            List<Address> addresses = new List<Address>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        @"SELECT addressId, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy FROM address";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Address address = new Address
                                {
                                    AddressId = reader.GetInt32("addressId"),
                                    Address1 = reader.GetString("address"),
                                    Address2 = reader.GetString("address2"),
                                    CityId = reader.GetInt32("cityId"),
                                    PostalCode = reader.GetString("postalCode"),
                                    Phone = reader.GetString("phone"),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };

                                addresses.Add(address);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving addresses: {ex.Message}");
            }

            return addresses;
        }
    }
}
