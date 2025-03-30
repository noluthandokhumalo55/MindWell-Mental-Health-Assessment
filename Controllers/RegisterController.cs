using Microsoft.AspNetCore.Mvc;
using Mindwell.Models;
using System;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace Mindwell.Controllers
{
    public class RegisterController : Controller
    {
        private readonly string _connectionString = "Data Source=noluthando\\sqlexpress01;Initial Catalog=Mindwell;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        // GET: Register
        public IActionResult Index()
        {
            return View("~/Views/Login&R/Register.cshtml");
        }

        // POST: Register
        [HttpPost]
        public IActionResult Index(UserModel model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ViewBag.ErrorMessage = "All fields are required.";
                return View("~/Views/Login&R/Register.cshtml");
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View("~/Views/Login&R/Register.cshtml");
            }

            string hashedPassword = HashPassword(model.Password);

            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        // Ensure the Users table exists
                        string tableCheckQuery = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                            CREATE TABLE Users (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                FullName NVARCHAR(255) NOT NULL,
                                Email NVARCHAR(255) UNIQUE NOT NULL,
                                PasswordHash NVARCHAR(255) NOT NULL
                            )";

                        using (var checkCommand = new SqlCommand(tableCheckQuery, connection))
                        {
                            checkCommand.ExecuteNonQuery();
                        }

                        // Insert user into Users table
                        string query = "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@FullName, @Email, @PasswordHash)";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FullName", model.FullName);
                            command.Parameters.AddWithValue("@Email", model.Email);
                            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                ViewBag.SuccessMessage = "Successfully Registered!";
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "An error occurred while registering.";
                            }
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation (duplicate email)
                {
                    ViewBag.ErrorMessage = "This email is already registered. Please use a different email.";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error: " + ex.Message;
                }
            }

            return View("~/Views/Login&R/Register.cshtml");
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
