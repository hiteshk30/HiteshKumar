using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RegistrationForm1.Pages.Users
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>();
        public void OnGet()
        {
            try
            {
                String ConnectionString = "Data Source=(localdb)\\local;Initial Catalog=MyLocalDB;Integrated Security=True";
                using(SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    String sql = "Exec USER_S";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var userInfo = new UserInfo();
                                userInfo.Id = "" +reader.GetInt32(0);
                                userInfo.Name = reader.GetString(1);
                                userInfo.Email = reader.GetString(2);
                                userInfo.Phone = reader.GetString(3);
                                userInfo.Address = reader.GetString(4);
                                userInfo.State = reader.GetString(5);
                                userInfo.City = reader.GetString(6);

                                listUsers.Add(userInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnPostDeleteUser(int userId)
        {
            try
            {
                String ConnectionString = "Data Source=(localdb)\\local;Initial Catalog=MyLocalDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    String sql = "Exec USER_D @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId);
                        
                        command.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }

    

    public class UserInfo
    {
        public string Id;
        public string Name;
        public string Email;
        public string Phone;
        public string Address;
        public string State;
        public string City;

    }
}
