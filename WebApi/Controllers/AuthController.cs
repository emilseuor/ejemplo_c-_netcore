using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApi.Models.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private IUserService _userService;
        public AuthController(IUserService ius) {
            _userService = ius;
        }

        [HttpPost("Admin/CreateRole")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody]RoleRequest request)
        {
            ResponseModel resp = await _userService.CreateRole(request);
            return Json(resp);
        }

        [HttpPost("Admin/Rolelist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Rolelist()
        {
            try
            {
                var list = await _userService.GetRoles();
                return Json(list);
            }
            catch (Exception ex) {
                return Json(new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("Admin/UpdateUserRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoles(RoleUpdateRequest req)
        {
            ResponseModel resp = await _userService.UpdateUserRoles(req);
            return Json(resp);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserRequestModel req) {

            if (ModelState.IsValid) {
                var result = await _userService.RegisterUserAsync(req);
                if (result.IsSuccess) {
                    return Json(result);
                }
                //THERE MAYBE MORE THAN ONE PROBLEM HERE
                return Json(result.ErrorList);
            }

            return BadRequest("Invalid request");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserRequestModel r)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(r);

                if (result.IsSuccess)
                {
                    return Json(result);
                }
                return Json(result.ErrorList);
            }

            return BadRequest("Invalid request");
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email){
            if(email != null && email != "") { 
                ResponseModel res = await _userService.ForgetPassword(email);
                return Json(res);
            }
            return Json(new ResponseModel { 
                IsSuccess = false,
                Message = "Empty email"
            });            
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ChangePassword req)
        {
            ResponseModel result = await _userService.ResetPassword(req);
            return Json(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            ResponseModel result = await _userService.ConfirmEmail(userid, token);
            return Json(result);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            //AuthenticationToken authenticationToken = User.Claims.Select(x => x.)
            //User.;
            ResponseModel mod = await _userService.Logout(User, accessToken);
            return Json(mod);
        }
    }
}
