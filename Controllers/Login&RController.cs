using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;

namespace Mindwell.Controllers
{
    public class Login_RController : Controller
    {
        private readonly string _connectionString = "Data Source=noluthando\\sqlexpress01;Initial Catalog=Mindwell;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public IActionResult Index()
        {
            return View("~/Views/Login&R/L&R.cshtml");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Please enter both email and password.";
                return RedirectToAction("Index");
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PasswordHash FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string storedPasswordHash = result.ToString();

                        // Verify the hashed password
                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                        {
                            // Store user session
                            HttpContext.Session.SetString("UserEmail", email);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            TempData["Error"] = "Invalid email or password.";
            return RedirectToAction("Index");
        }
    }
}
