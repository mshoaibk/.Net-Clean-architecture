using HRM_Common;
using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Core_WebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ApplicationUsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly HRMContexts dbContextHRM;

        public ApplicationUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, HRMContexts context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            dbContextHRM = context;
        }

        [HttpGet]
        [Route("RegisterCompany/{EncodedModel}")]
        public async Task<IActionResult> RegisterCompany(string EncodedModel)
        {
            try
            {

                RegisterModel model = Methods.GetDecodedModel<RegisterModel>(EncodedModel);
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return Ok(new Response { Status = "Error", Message = "Email address already exists!" });

                var existingCompanies = dbContextHRM.tblCompanyDetail.Where(c =>
                c.CompanyName == model.CompanyName ||
                c.PhoneNumber == model.PhoneNumber ||
                c.EmailAddress == model.Email).ToList();

                if (existingCompanies.Any(c => c.CompanyName == model.CompanyName))
                {
                    return Ok(new Response { Status = "Error", Message = "Company name already exists." });
                }
                else if (existingCompanies.Any(c => c.PhoneNumber == model.PhoneNumber))
                {
                    return Ok(new Response { Status = "Error", Message = "Phone number already exists." });
                }
                else if (existingCompanies.Any(c => c.EmailAddress == model.Email))
                {
                    return Ok(new Response { Status = "Error", Message = "Email address already exists." });
                }

                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email
                };
                // Hash the password and set the PasswordHash property of the user object
                var passwordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = passwordHash;

                var result = await userManager.CreateAsync(user);

                if (!result.Succeeded)
                    return Ok(new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Company))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Company));

                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));

                if (await roleManager.RoleExistsAsync(UserRoles.Company))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Company);
                }
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "RegisterCompany Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("UpdateCompanyRegisteration")]
        public async Task<IActionResult> UpdateCompanyRegisteration(UpdateUserModel model)
        {
            try
            {
                var userExists = await userManager.FindByIdAsync(model.id);
                if (userExists != null)
                {
                    userExists.Email = model.email;
                    userExists.NormalizedEmail = model.email.ToUpper();
                    userExists.UserName = model.email;
                    userExists.NormalizedUserName = model.email.ToUpper();

                    if (!string.IsNullOrEmpty(model.password))
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(userExists);
                        var results = await userManager.ResetPasswordAsync(userExists, token, model.password);

                        if (!results.Succeeded)
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password update failed! Please check user details and try again." });
                    }

                    var result = await userManager.UpdateAsync(userExists);
                    if (result.Succeeded)
                        return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User update failed! Please check user details and try again." });
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdateCompanyRegisteration Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpGet]
        [Route("RegisterEmployee/{EncodedModel}")]
        public async Task<IActionResult> RegisterEmployee(string EncodedModel)
        {
            try
            {
                RegisterModel model = Methods.GetDecodedModel<RegisterModel>(EncodedModel);
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email
                };
                // Hash the password and set the PasswordHash property of the user object
                var passwordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = passwordHash;

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Company))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Company));

                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));

                if (await roleManager.RoleExistsAsync(UserRoles.Employee))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Employee);
                }
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "RegisterEmployee Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email
                };
                // Hash the password and set the PasswordHash property of the user object
                var passwordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = passwordHash;

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Company))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Company));
                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "RegisterAdmin Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login"), Route("Throw")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.LockoutEnabled)
                {
                    return Ok(new
                    {
                        message = "Your account has been blocked!",
                        Status = false
                    });
                }
                else if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), };
                    if (!userRoles.Contains(model.Role))
                    {
                        return Ok(new
                        {
                            message = "You are not authorized to login!",
                            Status = false
                        });
                    }
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    string roleName = userRoles.Select(x => x).FirstOrDefault();
                    long companyID = 0;
                    string companyName = "";
                    string companyLogo = "";
                    long employeeId = 0;
                    string employeeName = "";
                    string signalRId = "";
                    List<int> roles = new List<int>();
                    if (roleName.ToLower() == "company")
                    {
                        var companyDetail = dbContextHRM.tblCompanyDetail.Where(x => x.UserID == user.Id).
                        Select(x => new { x.CompanyID, x.CompanyName, x.CompanyLogo }).FirstOrDefault();
                        companyID = companyDetail.CompanyID;
                        companyName = companyDetail.CompanyName;
                        companyLogo = companyDetail.CompanyLogo;
                        signalRId = dbContextHRM.TblSignalR_User.Where(x=>x.ActualUserID== companyID && x.Type== "company").FirstOrDefault().SignalRUserID.ToString();
                    }
                    if (roleName.ToLower() == "employee")
                    {
                        var employeeData = dbContextHRM.tblEmployee.
                            Where(x => x.UserID == user.Id && !x.IsDeleted && x.IsActivated == true).
                            Select(x => new { x.CompanyID, x.EmployeeID,x.FullName, x.Position, x.Office, x.Department, x.Team }).
                            FirstOrDefault();
                        signalRId = dbContextHRM.TblSignalR_User.Where(x => x.ActualUserID == employeeData.EmployeeID && x.Type == "employee").FirstOrDefault().SignalRUserID.ToString();

                        companyID = employeeData != null ? employeeData.CompanyID : 0;

                        var companyDetail = dbContextHRM.tblCompanyDetail.
                            Where(x => x.CompanyID == companyID && !x.IsDeleted && x.IsActivated == true).
                            Select(x => new { x.CompanyID, x.CompanyName, x.CompanyLogo }).
                            FirstOrDefault();

                        if (employeeData == null)
                        {
                            return Ok(new
                            {
                                message = "Employee data not exist in our record.",
                                Status = false
                            });
                        }
                        if (companyDetail == null)
                        {
                            return Ok(new
                            {
                                message = "Company data not exist in our record.",
                                Status = false
                            });
                        }
                        companyName = companyDetail.CompanyName;
                        companyLogo = companyDetail.CompanyLogo;
                        employeeId = employeeData.EmployeeID;
                        employeeName = employeeData.FullName;
                        if (!string.IsNullOrEmpty(employeeData.Position) 
                            && !string.IsNullOrEmpty(employeeData.Team)
                            && !string.IsNullOrEmpty(employeeData.Office)
                            && !string.IsNullOrEmpty(employeeData.Department)) {
                            long? positionId = Convert.ToInt64(employeeData.Position);
                            long? teamId = Convert.ToInt64(employeeData.Team);
                            long? officeId = Convert.ToInt64(employeeData.Office);
                            long? deptId = Convert.ToInt64(employeeData.Department);
                            roles = dbContextHRM.
                                TblEmployeeRole.
                                Where(x => x.CompanyId==companyID
                                && x.PositionId == positionId
                                && x.OfficeId==officeId
                                && x.DepartmentId==deptId
                                && x.TeamId==teamId 
                                && (x.IsRead == true || x.IsWrite == true)).
                                Select(x => x.ModuleId).
                                ToList();
                        }
                    }
                    HttpContext.Session.SetInt32("companyID",Convert.ToInt32(companyID));
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        message = "success",
                        Status = true,
                        userRole = userRoles.Select(x => x).FirstOrDefault(),
                        companyID = companyID,
                        companyName = companyName,
                        companyLogo = companyLogo,
                        user = user,
                        employeeId = employeeId,
                        employeeName = employeeName,
                        signalRId = signalRId,
                        roles = roles
                    });

                }
                return Ok(new
                {
                    message = "Incorrect user name or password!",
                    Status = false
                });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "Login Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ShowUserInfo/{userID}")]
        public async Task<IActionResult> ShowUserInfo(string userID)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userID);
                return Ok(new
                {
                    Status = true,
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ShowUserInfo Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// delete User Info
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteUserInfo/{userID}")]
        public async Task<IActionResult> DeleteUserInfo(string userID)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userID);
                if (user != null) {
                    await userManager.DeleteAsync(user);
                }
                return Ok(new {
                    Status = true
                });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "DeleteUserInfo Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// acivate / deactivate User Info
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangeUserInfoStatus")]
        public async Task<IActionResult> ChangeUserInfoStatus(ChangeUserStatus model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(model.userID);
                await userManager.SetLockoutEnabledAsync(user, model.isActivated);
                await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
                return Ok(new
                {
                    Status = true
                });
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "ChangeUserInfoStatus Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// change password
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdatePassword/{EncodedModel}")]
        public async Task<IActionResult> UpdatePassword(string encodedModel)
        {
            try
            {

                ChangePasswordModel model = Methods.GetDecodedModel<ChangePasswordModel>(encodedModel);
                IdentityUser user = await userManager.FindByIdAsync(model.userId);

                // Check if the current password is correct
                bool isPasswordValid = await userManager.CheckPasswordAsync(user, model.currentPasswordInput);
                if (isPasswordValid)
                {
                    // Current password is correct, proceed with updating the password
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.newPasswordInput);

                    if (result.Succeeded)
                    {
                        return Ok(new
                        {
                            Status = true,message= "Password has been updated."
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Status = false,
                            message = "Password reset failed due to some error occured."
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        Status = false,
                        message = "Current password is incorrect."
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and send the email
                LogAndSendException(ex, "UpdatePassword Exception");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet, Route("Throw")]
        public void LogAndSendException(Exception ex,string msg)
        {
            // Log the exception
            Log.Error(ex, msg);
            // Send the exception email
            //_IEmailServices.SendExceptionEmail("athariqbal294@gmail.com", msg, ex.ToString());
        }
    }
}
