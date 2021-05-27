using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Models.Helpers
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public string Value { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> ErrorList { get; set; }
    }
}
