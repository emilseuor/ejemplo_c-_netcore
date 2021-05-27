using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("Operation/")]
    [ApiController, Authorize]
    public class OperationsController : Controller
    {
        private IOperationService _payService;

        public OperationsController(IOperationService pay)
        {
            _payService = pay;
        }


        [HttpPost("List")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List([FromBody] OperationListRequest req)
        {
            try
            {
                var list = await _payService.GetFilteredList(req);
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

        [HttpPost("MyList")]
        [Authorize]
        public async Task<IActionResult> MyList([FromBody] OperationListRequest req)
        {
            try
            {
                if (req.UserId == null) {
                    return Json(new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Your userid is required"
                    });
                }
                var list = await _payService.GetFilteredList(req);
                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
        [HttpGet("Details/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Not Found");
            }

            int def_id = id ?? 0;
            try
            {
                var operation = await _payService.GetDetailById(def_id);

                return Json(operation);
            }
            catch (Exception ex) {
                return Json(new ResponseModel { 
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("Add")]
        [Authorize]
        public async Task<ResponseModel> Create(OperationRequest pay)
        {
            if (ModelState.IsValid)
            {
                string cashierid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var result = await _payService.AddOperation(cashierid, pay);
                return result;
            }

            return new ResponseModel { 
                IsSuccess = false,
                Message = "Invalid Model"
            };
        }

        [HttpPost("Edit")]
        [Authorize]
        public async Task<ResponseModel> Edit(OperationRequest pay)
        {
            if (ModelState.IsValid && pay.Id > 0)
            {
                string cashierid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var result = await _payService.UpdateOperation(cashierid, pay);
                return result;
            }

            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Invalid Model"
            };
        }

        [HttpPost("Admin/UnreturnedList")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnreturnedList()
        {
            try
            {
                var result = await _payService.UserUnreturnedMovies();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("Unreturned")]
        [Authorize]
        public async Task<IActionResult> Unreturned()
        {
            try
            {
                string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _payService.UserUnreturnedMovies(userid);

                return Json(result);
            }
            catch (Exception ex) { 
                return Json(new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
    }
}