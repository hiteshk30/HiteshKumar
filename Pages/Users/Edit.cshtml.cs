using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Mail;

namespace RegistrationForm1.Pages.Users
{
    public class EditModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";
        string id = "";
        string state = "";
        string city = "";

        public void OnGet()
        {
             id = Request.Query["Id"];
            try
            {
                String ConnectionString = "Data Source=(localdb)\\local;Initial Catalog=MyLocalDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    String sql = "Exec USER_S1 @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userInfo.Id = "" + reader.GetInt32(0);
                                userInfo.Name = reader.GetString(1);
                                userInfo.Email = reader.GetString(2);
                                userInfo.Phone = reader.GetString(3);
                                userInfo.Address = reader.GetString(4);
                                userInfo.State = reader.GetString(5);
                                userInfo.City = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        private static bool emailValidation(string email)
        {
            var res = true;
            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                res = false;
            }
            return res;
        }

        public void OnPost()
        {
            userInfo.Id = Request.Form["Id"];
            userInfo.Name = Request.Form["name"];
            userInfo.Email = Request.Form["email"];
            userInfo.Phone = Request.Form["phone"];
            userInfo.Address = Request.Form["address"];
            userInfo.State = Request.Form["state"];
            userInfo.City = Request.Form["city"];
            
            

            if (userInfo.Name.Length == 0 || userInfo.Email.Length == 0)
            {
                errorMessage = "Please Enter Required Fields..";
                return;
            }
            if(userInfo.State is null || userInfo.City is null)
            {
                errorMessage = "Please Enter Dropdown Fields..";
                return;
            }
            if (userInfo.Phone.Length > 0)
            {
                if (userInfo.Phone.Length != 10)
                {
                    errorMessage = "Please Enter a valid Phone Number..";
                    return;
                }
            }
            if (!emailValidation(userInfo.Email))
            {
                errorMessage = "Please Enter a valid Email Address..";
                return;

            }

            try
            {
                String ConnectionString = "Data Source=(localdb)\\local;Initial Catalog=MyLocalDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    String sql = "Exec USER_U @Id, @name, @email, @phone, @address, @state,@city";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userInfo.Id);
                        command.Parameters.AddWithValue("@name", userInfo.Name);
                        command.Parameters.AddWithValue("@email", userInfo.Email);
                        command.Parameters.AddWithValue("@phone", userInfo.Phone);
                        command.Parameters.AddWithValue("@address", userInfo.Address);
                        command.Parameters.AddWithValue("@state", userInfo.State);
                        command.Parameters.AddWithValue("@city", userInfo.City);

                        command.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userInfo.Name = "";
            userInfo.Email = "";
            userInfo.Phone = "";
            userInfo.Address = "";
            userInfo.State = "";
            userInfo.City = "";
            successMessage = "New User Registered...";

            Response.Redirect("/Users/Index");

        }
    }
}
