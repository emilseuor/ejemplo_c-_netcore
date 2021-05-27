using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Helpers;

namespace WebApi.Services
{
    public interface IMovieService
    {
        public Task<IEnumerable<MovieObj>> GetFilteredList(int availability = 2, int sortby = 0, int pagesize = 0, int pagenumber = 0, string searchq = "");
        public Task<int> GetTotalCount(int availability = 2, string searchq = "");
        public Task<MovieObj> GetDetailsById(int id);
        public Task<ResponseModel> CreateMovie(Movie movie);
        public Task<ResponseModel> UpdateMovie(Movie movie);
        public Task<ResponseModel> DeleteMovie(int movieid);
        public Task<ResponseModel> LikeUnlikeMovie(string userid, int movieid);
    }
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IMovieService> _logger;

        public MovieService(ApplicationDbContext context, ILogger<IMovieService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> GetTotalCount(int availability, string searchq) { 

            var q = _context.movie.Select(x => x);

            switch(availability){
                case 0:
                    q = q.Where(x => x.Availability == false);
                    break;
                case 1:
                    q = q.Where(x => x.Availability == true);
                    break;
                default:
                    break;
            }
            if (searchq != "") {
                q = q.Where(x => x.Title.Contains(searchq));
            }
            var items = await q.CountAsync();
            return items;
        }
        public async Task<ResponseModel> CreateMovie(Movie movie) {
            try
            {
                _context.Add(movie);
                var m = await _context.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "New movie saved"
                };

            }
            catch (Exception ex) {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };

            }

        }
        public async Task<ResponseModel> UpdateMovie(Movie movie)
        {
            if (!MovieExists(movie.Id))
            {
                return new ResponseModel { 
                    IsSuccess = false,
                    Message = "The movie does not exist"
                };
            }
            else
            {
                Movie oldm = await _context.movie.FirstOrDefaultAsync(m => m.Id == movie.Id);
                _context.Entry(oldm).State = EntityState.Detached;
                
                //TITLE
                if (movie.Title != oldm.Title)
                {
                    _logger.LogInformation("TITLE MODIFIED", movie.Title);
                }

                //RENTAL PRICE
                if (movie.RentalPrice != oldm.RentalPrice)
                {
                    _logger.LogInformation("RENTAL PRICE MODIFIED", movie.RentalPrice);
                }

                //SALE PRICE
                if (movie.SalePrice != oldm.SalePrice)
                {
                    _logger.LogInformation("SALE PRICE MODIFIED", movie.SalePrice);
                }

                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException dbexc)
                {
                    _logger.Log(LogLevel.Error, dbexc.Message, dbexc.Data);
                    return new ResponseModel
                    {
                    IsSuccess = false,
                    Message = dbexc.Message
                    };
                }
                
            }
            return new ResponseModel { 
                IsSuccess = true,
                Message = "Changes saved"
            };
        }

        public async Task<MovieObj> GetDetailsById(int id) {
        MovieObj mobj = await _context.movie
            .Select(st => new MovieObj
            {
                Id = st.Id,
                Title = st.Title,
                Description = st.Description,
                RentalPrice = st.RentalPrice,
                SalePrice = st.SalePrice,
                Availability = st.Availability,
                Img = st.Img,
                Stock = st.Stock,
                CountLikes = st.likes.Count()
            }).Where(x => x.Id == id).FirstOrDefaultAsync();

            return mobj;
        }

        public async Task<IEnumerable<MovieObj>> GetFilteredList(int availability, int sortby, int pagesize, int pagenumber, string searchq)
        {

            var mlistquery = _context.movie
                    .Select(st => new MovieObj {
                        Id = st.Id,
                        Title = st.Title,
                        Description = st.Description,
                        RentalPrice = st.RentalPrice,
                        SalePrice = st.SalePrice,
                        Availability = st.Availability,
                        Img = st.Img,
                        CountLikes = st.likes.Count()
                    });
            switch (availability)
            {
                case 0:
                    mlistquery = mlistquery.Where(x => x.Availability == false);
                    break;
                case 1:
                    mlistquery = mlistquery.Where(x => x.Availability == true);
                    break;
                default:
                    break;
            }

            if (searchq != "")
            {
                mlistquery = mlistquery.Where(m => m.Title.Contains(searchq));
            }
            if (sortby > 0)
            {
                mlistquery = mlistquery.OrderByDescending(x => x.CountLikes);
            }
            else
            {
                mlistquery = mlistquery.OrderBy(x => x.Title);
            }

            if (pagesize > 0)
            {
                mlistquery = mlistquery.Skip(pagenumber * pagesize);
            }

            if (mlistquery.Count() > 0 && pagesize > 0)
            {
                mlistquery = mlistquery.Take(pagesize);
            }

            IEnumerable<MovieObj> movies = await mlistquery.ToListAsync();
            return movies;
        }

        public async Task<ResponseModel> DeleteMovie(int id) {
            try
            {
                var movie = await _context.movie.FindAsync(id);

                _context.movie.Remove(movie);
                
                await _context.SaveChangesAsync();
                
                return new ResponseModel {
                    IsSuccess = true,
                    Message = "deleted id:" + id.ToString()
                };
            }
            catch (DbUpdateConcurrencyException dbexc)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message =  dbexc.Message
                };
            }
        }
        public async Task<ResponseModel> LikeUnlikeMovie(string userid, int movieid) {
            
            MovieLikes li = LikeExists(userid, movieid);
            if (MovieExists(movieid)) { 
                if (li == null)
                {
                    //Like doesn't exists, like movie
                    li = new MovieLikes();
                    li.UserId = userid;
                    li.Movie = _context.movie.FirstOrDefault(x => x.Id == movieid);

                    _context.Add(li);

                    await _context.SaveChangesAsync();

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Value = li.Id.ToString(),
                        Message = "Like added"
                    };
                }
                else
                {
                    //Like exists, unlike movie
                    _context.movieLikes.Remove(li);
                    await _context.SaveChangesAsync();
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Value = li.Id.ToString(),
                        Message = "Like removed"
                    };
                }
            }

            return new ResponseModel { 
                IsSuccess = false,
                Message = "Movie doesn't exist"
            };
        }
        private bool MovieExists(int id)
        {
            return _context.movie.Any(e => e.Id == id);
        }
        private MovieLikes LikeExists(string userid, int movieid)
        {
            return _context.movieLikes.FirstOrDefault(e => e.UserId == userid && e.Movie.Id == movieid);
        }
    }
}