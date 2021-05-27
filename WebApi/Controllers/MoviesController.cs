using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Helpers;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("Movie/")]
    [ApiController]
    public class MoviesController : Controller
    {
        private IMovieService _movieManager;

        public MoviesController(IMovieService movieManager)
        {
            _movieManager = movieManager;
        }
        
        [HttpGet("List/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}")]
        public async Task<IActionResult> Index(int pagesize = 2, int pagenumber = 0, int sortby = 0)
        {
            var mlist = await _movieManager.GetFilteredList(1, sortby, pagesize, pagenumber);
            var totalitems = await _movieManager.GetTotalCount(1);

            return Json(new
            {
                list = mlist,
                totalitems = totalitems
            });
        }

        [HttpGet("Admin/List/{availability:int?}/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminList(int availability = 2, int sortby = 0, int pagesize = 0, int pagenumber = 0)
        {
            var mlist = await _movieManager.GetFilteredList(availability, sortby, pagesize, pagenumber);
            var totalitems = await _movieManager.GetTotalCount(availability);

            return Json(new
            {
                list = mlist,
                totalitems = totalitems
            });
        }

        [HttpGet("Search/{q}/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}")]
        public async Task<IActionResult> Search(string q, int sortby = 0, int pagesize = 0, int pagenumber = 0)
        {
            var mlist = await _movieManager.GetFilteredList(1, sortby, pagesize, pagenumber, q);
            var totalitems = await _movieManager.GetTotalCount(1, q);

            return Json(new
            {
                list = mlist,
                totalitems = totalitems
            });
        }

        [HttpGet("Admin/Search/{q}/{available:int?}/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminSearch(string q, int availability = 2, int sortby = 0, int pagesize = 0, int pagenumber = 0)
        {
            var mlist = await _movieManager.GetFilteredList(availability, sortby, pagesize, pagenumber, q);
            var totalitems = await _movieManager.GetTotalCount(1, q);

            return Json(new
            {
                list = mlist,
                totalitems = totalitems
            });
        }


        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieManager.GetDetailsById(id);
            if (movie == null)
            {
                return BadRequest("Not Found");
            }

            return Json(movie);
        }

        [HttpGet("Like/{id}")]
        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var resp = await _movieManager.LikeUnlikeMovie(userid, id);
            return Json(resp);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Movie m)
        {
            if (ModelState.IsValid)
            {
                var resp = await _movieManager.CreateMovie(m);
                return Json(resp);
            }
            return BadRequest("Invalid request");

        }

        [HttpPost("Edit")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                var resp = await _movieManager.UpdateMovie(movie);
                return Json(resp);
            }
            return BadRequest("Invalid request");
        }
        
        [Route("Delete/{id}"), Authorize(Roles = "Admin")]
        [HttpPut, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _movieManager.DeleteMovie(id);
            return Json(resp);
        }
    }
}
