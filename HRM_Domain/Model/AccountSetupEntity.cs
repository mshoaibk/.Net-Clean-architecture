using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Domain.Model
{
    public class ApplicationSettings
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
    }
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
    }
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Company = "Company";
        public const string Employee = "Employee";
    }
    public class LoginModel
    {
        public string Email { get; set; } 
        public string Password { get; set; }
        public string Role { get; set; }
    }
    public class ChangeUserStatus
    {
        public string userID { get; set; }
        public bool isActivated { get; set; }
    }
    public class ChangePasswordModel
    {
        public string userId { get; set; }
        public string currentPasswordInput { get; set; }
        public string newPasswordInput { get; set; }
    }
}
