using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SchedulingDesktopApp.Database;

namespace SchedulingDesktopApp.Classes
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Address Address { get; set; }
        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public static void AddCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerName) || string.IsNullOrWhiteSpace(customer.Address.Phone))
            {
                MessageBox.Show("Customer name and phone number cannot be empty.");
                return;
            }

            if (!Regex.IsMatch(customer.Address.Phone, @"^[0-9\-]+$"))
            {
                MessageBox.Show("Phone number must contain only digits and dashes.");
                return;
            }
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    {
                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        string query = @"INSERT INTO customer
                                        (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                        VALUES
                                        (@customerName, @addressId, @active, NOW(), @createdBy, NOW(), @lastUpdateBy)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                            cmd.Parameters.AddWithValue("@addressId", customer.Address.AddressId);
                            cmd.Parameters.AddWithValue("@active", customer.Active);
                            cmd.Parameters.AddWithValue("@createdBy", customer.CreatedBy);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", customer.LastUpdateBy);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Customer added successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }

        public static List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        @"SELECT customerId, customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy
                                    FROM customer";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Address address = new Address { AddressId = reader.GetInt32("addressId") };

                                Customer customer = new Customer
                                {
                                    CustomerId = reader.GetInt32("customerId"),
                                    CustomerName = reader.GetString("customerName"),
                                    Address = address,
                                    Active = reader.GetBoolean("active"),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving customers: {ex.Message}");
            }

            return customers;
        }

        public static void UpdateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerName) || string.IsNullOrWhiteSpace(customer.Address.Phone))
            {
                MessageBox.Show("Customer name and phone number cannot be empty.");
                return;
            }

            if (!Regex.IsMatch(customer.Address.Phone, @"^[0-9\-]+$"))
            {
                MessageBox.Show("Phone number must contain only digits and dashes.");
                return;
            }
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"UPDATE customer
                                    SET customerName = @customerName,
                                        addressId = @addressId,
                                        active = @active,
                                        lastUpdate = NOW(),
                                        lastUpdateBy = @lastUpdateBy
                                    WHERE customerId = @customerId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("@addressId", customer.Address.AddressId);
                        cmd.Parameters.AddWithValue("@active", customer.Active);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", customer.LastUpdateBy);
                        cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }

        public static void DeleteCustomer(int customerId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"DELETE FROM customer WHERE customerId = @customerId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Customer deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
