using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Helpers;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<ResponseModel> RegisterUserAsync(UserRequestModel model);
        Task<ResponseModel> ConfirmEmail(string userid, string token);
        Task<ResponseModel> LoginUserAsync(UserRequestModel model);
        Task<ResponseModel> Logout(ClaimsPrincipal User, string str);
        Task<ResponseModel> UpdateUserRoles(RoleUpdateRequest req);
        Task<ResponseModel> CreateRole(RoleRequest request);
        Task<ResponseModel> ForgetPassword(string email);
        Task<ResponseModel> ResetPassword(ChangePassword model);
        Task<IEnumerable<IdentityRole>> GetRoles();
    }
    public class UserService : IUserService {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _config;
        private RoleManager<IdentityRole> _roleManager;
        private IMailService _mailService;

        public UserService(UserManager<IdentityUser> userm, IConfiguration conf, RoleManager<IdentityRole> rol, IHttpContextAccessor httpContextAccessor, IMailService mail) {
            _userManager = userm;
            _roleManager = rol;
            _config = conf;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mail;
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            var list = await _roleManager.Roles.ToListAsync();
            return list;
        }
        public async Task<ResponseModel> CreateRole(RoleRequest request) {
            IdentityResult roleResult;

            try
            {
                var roleCheck = await _roleManager.RoleExistsAsync(request.Name);
                if (!roleCheck)
                {
                    IdentityRole newrole = new IdentityRole(request.Name);
                    roleResult = await _roleManager.CreateAsync(newrole);
                    foreach (string au in request.Permissions) {
                        _roleManager.AddClaimAsync(newrole, new Claim(ClaimTypes.AuthorizationDecision, au)).Wait();
                    }
                    return new ResponseModel
                    {
                        Message = "New role created",
                        Value = newrole.Id,
                        IsSuccess = true
                    };
                }
                return new ResponseModel
                {
                    Message = "Already exists",
                    IsSuccess = false
                };
            }
            catch (Exception e) {
                return new ResponseModel
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ResponseModel> UpdateUserRoles(RoleUpdateRequest req)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(req.RoleId);
                var usercheck = _userManager.GetUsersInRoleAsync(role.Name).Result.Where(x => x.Id == req.UserId).FirstOrDefault();

                if (usercheck != null)
                {
                    //Exist, remove
                    var res = await _userManager.RemoveFromRoleAsync(usercheck, role.Name);
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Value = "UserID:" + usercheck.Id + " Role: " + role.Name, 
                        Message = "Unassigned"
                    };
                }
                //Doesn't exist, add
                var user = await _userManager.FindByIdAsync(req.UserId);
                await _userManager.AddToRoleAsync(user, role.Name);

                return new ResponseModel
                {
                    IsSuccess = true,
                    Value = "UserID:" + user.Id + " Role: " + role.Name,
                    Message = "Assigned"
                };
            }
            catch (Exception exc) {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = exc.Message
                };
            }
        }

        public async Task<ResponseModel> LoginUserAsync(UserRequestModel model)
        {
            try { 
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null) {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Incorrect Username"
                    };
                }
                else {

                    var r = await _userManager.CheckPasswordAsync(user, model.Password);
                
                    if (!r) {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Incorrect Password"
                        };
                    }
                
                    var claims = new List<Claim>
                    {
                        new Claim("Email", model.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };
                
                    IList<string> roles = _userManager.GetRolesAsync(user).Result;

                    foreach (string role in roles) {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    double.TryParse(_config["Jwt:Expiration_Hours"], out double exphs);
                
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                    var tok = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Issuer"],
                        claims,
                        expires: DateTime.Now.AddHours(exphs),
                        signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                    );

                    string token = new JwtSecurityTokenHandler().WriteToken(tok);

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = token,
                        Value = user.Id
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel> Logout(ClaimsPrincipal User, string token) {
            
            try {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken tok = handler.ReadToken(token);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var newtok = new JwtSecurityToken(
                        issuer: tok.Issuer,
                        audience: tok.Issuer,
                        User.Claims,
                        expires: DateTime.Now,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                string newtoken = new JwtSecurityTokenHandler().WriteToken(newtok);

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Logged out"
                };

            }
            catch (Exception ex) {
                return new ResponseModel { 
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel> RegisterUserAsync(UserRequestModel model) {
            if (model.ConfirmPassword == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Confirm Password field is required"
                };
            }

            if (model.Password != model.ConfirmPassword) { 
                return new ResponseModel
                { 
                    IsSuccess = false,
                    Message = "The password field and the confirm password field are different."
                };
            }

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            try { 
                var result = await _userManager.CreateAsync(identityUser, model.Password);
                if (model.Roles != null)
                {
                    foreach (string roleid in model.Roles)
                    {
                        var role = await _roleManager.FindByIdAsync(roleid);
                        if (role == null)
                        {
                            return new ResponseModel
                            {
                                IsSuccess = false,
                                Message = "Role doesn't exist"
                            };
                        }
                        await _userManager.AddToRoleAsync(identityUser, role.Name);
                    }
                }
                else 
                {
                    var role = await _roleManager.FindByNameAsync("Client");
                    await _userManager.AddToRoleAsync(identityUser, role.Name);
                }
                if (result.Succeeded)
                {
                    var conftoken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    string tok = EncodeToken(conftoken);
                    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                    string dir = $"https://{ host }/ConfirmEmail?userid={identityUser.Id}&token={tok}";

                    await _mailService.SendMail(new MailModel { 
                        fromemail = "login@no-reply.com", 
                        toemail = identityUser.Email, 
                        content = $"{ dir }", 
                        subject = "Confirm your email: Paste the URL below in any browser" });

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Value = identityUser.Id,
                        Message = "New user registered"
                    };
                }

                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Can't register new user",
                    ErrorList = result.Errors.Select(x => x.Description.ToString())
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel { 
                IsSuccess = false,
                Message = ex.Message
                };

            }
        }
        public async Task<ResponseModel> ConfirmEmail(string userid, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Fail"
                    };
                }


                var decoded = DecodeToken(token);
                var result = await _userManager.ConfirmEmailAsync(user, decoded);
                if (result.Succeeded)
                {
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = "Mail confirmed"
                    };
                }
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Fail"
                };
            }
            catch (Exception e) {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
        public async Task<ResponseModel> ForgetPassword(string email) {
            try { 
         
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "User does not exist"
                    };
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = EncodeToken(token);

                string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                string dir = $"https://{ host }/ResetPassword";
            
                await _mailService.SendMail(new MailModel
                {
                    fromemail = "login@no-reply.com",
                    toemail = email,
                    content = $"{ dir } \r\n Method: Post \r\n Body: Token, UserId, NewPassword, ConfirmPassword \r\n UserId: {user.Id} \r\n Token: {encodedToken} ",
                    subject = "Reset your password"
                });

                return new ResponseModel { 
                    IsSuccess = true,
                    Message = "Email sent"
                };
            }
            catch (Exception e)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseModel> ResetPassword(ChangePassword model) {
            try { 
                var user = await _userManager.FindByIdAsync(model.UserId);
                string decodedtoken = DecodeToken(model.Token);
                if(model.NewPassword == model.ConfirmPassword) {
                    var res = await _userManager.ResetPasswordAsync(user, decodedtoken, model.NewPassword);
                    if (res.Succeeded) {
                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "Please login with your new password"
                        };
                    }
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Error"
                    };
                }
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Passwords does not match"
                };
            }
            catch (Exception e)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
        private string EncodeToken(string token) {
            var bites = Encoding.UTF8.GetBytes(token);
            string encoded_token = WebEncoders.Base64UrlEncode(bites);
            return encoded_token;
        }
        private string DecodeToken(string token)
        {
            var tokenbites = WebEncoders.Base64UrlDecode(token);
            string decoded_token = Encoding.UTF8.GetString(tokenbites);
            return decoded_token;
        }
    }
}