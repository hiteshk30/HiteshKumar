using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Net.Mail;

namespace RegistrationForm1.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage="";
        public string successMessage="";
        public void OnGet()
        {
           
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
            userInfo.Name = Request.Form["name"];
            userInfo.Email = Request.Form["email"];
            userInfo.Phone = Request.Form["phone"];
            userInfo.Address = Request.Form["address"];
            userInfo.State = Request.Form["state"];
            userInfo.City = Request.Form["city"];
            if(userInfo.Name.Length  == 0 || userInfo.Email.Length == 0)
            {
                errorMessage = "Please Enter Required Fields..";
                return;
            }
            if (userInfo.State is null || userInfo.City is null)
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
                    String sql = "Exec USER_I @name, @email, @phone, @address, @state,@city";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
            catch(Exception ex)
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
