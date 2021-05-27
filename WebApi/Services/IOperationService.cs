using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Helpers;

namespace WebApi.Services
{
    public interface IOperationService
    {
        public Task<IEnumerable<Operation>> GetFilteredList(OperationListRequest req);
        public Task<Operation> GetDetailById(int payid);
        public Task<IEnumerable<Operation>> UserUnreturnedMovies(string userid = "");
        public Task<ResponseModel> AddOperation(string operatorid, OperationRequest pay);
        public Task<ResponseModel> UpdateOperation(string operatorid, OperationRequest pay);
        public Task<ResponseModel> Update(Operation pay);

    }

    public class OperationService : IOperationService {

        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _config;

        public OperationService(ApplicationDbContext context, UserManager<IdentityUser> userm, IConfiguration conf)
        {
            _userManager = userm;
            _config = conf;
            _context = context;

        }

        public async Task<IEnumerable<Operation>> GetFilteredList(OperationListRequest req) {

            IEnumerable<Operation> list;

            var query = _context.operation.Select(x => x);

            if (req.UserId != null)
            {
                query = query.Where(x => x.UserId == req.UserId);
            }
            if (req.MovieId > 0)
            {
                query = query.Where(x => x.Movie.Id == req.MovieId);
            }

            if (req.Type != null)
            {
                query = query.Where(x => x.Type == req.Type);
            }

            if (req.Status != null)
            {
                query = query.Where(x => x.Status == req.Status);
            }

            if (req.ToDate != DateTime.MinValue && req.FromDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date > req.FromDate && x.Date < req.ToDate);
            }

            list = await query.ToListAsync();

            return list;
        }

        public async Task<Operation> GetDetailById(int payid) {

            Operation p = new Operation(); 

            p = await _context.operation
                .Where(x => x.Id == payid)
                .FirstAsync();

            return p;

        }
        public async Task<IEnumerable<Operation>> UserUnreturnedMovies(string userid) {
            IEnumerable<Operation> r = null;
            var query = _context.operation.Select(x => x);
            if (userid != "")
            {
                query = query.Where(x => x.UserId == userid);
            }
            r = await query.Where(x => x.Status != "RETURNED")
                .Where(x => x.Status != "CANCELLED")
                .Where(x => x.Status != "FAIL")
                .Where(x => x.Status != "PENALTY_PAID")
                .ToListAsync();

            return r;
        }
        public async Task<ResponseModel> AddOperation(string operatorid, OperationRequest pay) {

            try
            {
                var mov = await _context.movie
                    .Where(x => x.Id == pay.MovieId)
                    .FirstAsync();
                
                var user = await _userManager.FindByIdAsync(pay.UserId);

                if (mov == null || user == null) {
                    return new ResponseModel { 
                        IsSuccess = false,
                        Message = "Movieid or userid wrong"
                    };
                }

                Operation operation = new Operation();
                
                operation.Movie = mov;
                operation.UserId = user.Id;
                operation.OperatorUserId = operatorid;
                operation.Date = DateTime.Now;
                operation.Type = pay.Type;
                operation.Details = pay.Details;

                if (pay.Price == 0)
                {
                    if (pay.Type == "RENT")
                    {
                        operation.Price = mov.RentalPrice;
                    }
                    else
                    {
                        operation.Price = mov.SalePrice;
                    }
                }
                else {
                    operation.Price = pay.Price;
                }

                if (pay.Status == "PAID")
                {
                    operation.Status = "PAID";
                    mov.Stock--;
                    _context.movie.Update(mov);
                }
                else {
                    operation.Status = pay.Status;
                }

                if (pay.DueDate == DateTime.MinValue)
                {
                    operation.DueDate = DateTime.Now.AddDays(pay.DaysBeforeDueDate);
                }
                else {
                    operation.DueDate = pay.DueDate;
                }

                if (pay.Type == "RENT")
                {
                    operation.PenaltyPrice = mov.RentalPrice + double.Parse(_config["PenaltyValue"]);
                }

                _context.operation.Add(operation);
                var p = await _context.SaveChangesAsync();
                
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "operation saved"
                };

            } catch (Exception ex) {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> UpdateOperation(string operatorid, OperationRequest pay)
        {
            try
            {
                var operation = await _context.operation
                    .Include(x => x.Movie)
                    .Where(x => x.Id == pay.Id)
                    .FirstAsync();

                if (operation == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Operationid wrong"
                    };
                }

                operation.OperatorUserId = operatorid;
                operation.Details = pay.Details;

                if (pay.Status != operation.Status)
                {
                    if (pay.Status == "RETURNED") {
                        var mov = operation.Movie;
                        mov.Stock++;
                        _context.movie.Update(mov);
                    }
                    if (pay.Status == "PAID")
                    {
                        var mov = operation.Movie;
                        mov.Stock--;
                        _context.movie.Update(mov);
                    }
                    operation.Status = pay.Status;

                }

                if (pay.DueDate != DateTime.MinValue && operation.DueDate != pay.DueDate)
                {
                    operation.DueDate = pay.DueDate;
                }

                if (operation.PenaltyPrice != pay.PenaltyPrice)
                {
                    operation.PenaltyPrice = pay.PenaltyPrice;
                }

                _context.operation.Update(operation);
                
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "operation saved"
                };

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
        public async Task<ResponseModel> Update(Operation operation)
        {
            try
            {
                _context.operation.Update(operation);

                var p = await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Operation saved"
                };

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

    }
}
