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
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        //public static void AddUser(User user)
        //{
        //    try
        //    {
        //        using (var conn = DBConnection.conn)
        //        {
        //            if (conn.State == System.Data.ConnectionState.Closed)
        //            {
        //                conn.Open();
        //            }

        //            string query = @"INSERT INTO user
        //                            (userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy)
        //                            VALUES
        //                            (@userName, @password, @active, NOW(), @createdBy, NOW(), @lastUpdateBy)";

        //            using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@userName", user.UserName);
        //                cmd.Parameters.AddWithValue("@password", user.Password);
        //                cmd.Parameters.AddWithValue("@active", user.Active);
        //                cmd.Parameters.AddWithValue("@createDate", user.CreateDate);
        //                cmd.Parameters.AddWithValue("@createdBy", user.CreatedBy);
        //                cmd.Parameters.AddWithValue("@lastUpdate", user.LastUpdate);
        //                cmd.Parameters.AddWithValue("@lastUpdateBy", user.LastUpdateBy);

        //                cmd.ExecuteNonQuery();
        //                MessageBox.Show($"User added successfully");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error adding user: {ex.Message}");
        //    }
        //}

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query =
                        @"SELECT userId, userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy
                                    FROM user";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32("userId"),
                                    UserName = reader.GetString("userName"),
                                    Password = reader.GetString("password"),
                                    Active = reader.GetBoolean("active"),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving users: {ex.Message}");
            }

            return users;
        }
    }
}
