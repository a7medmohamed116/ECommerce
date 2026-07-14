using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public record Error(string Code, string Description , ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string Code = "General.Failure", string Description = "Failure Error Has Occured")
            => new Error(Code, Description, ErrorType.Failure);
        public static Error NotFound(string Code = "General.NotFound", string Description = "NotFound Error Has Occured")
            => new Error(Code, Description, ErrorType.NotFound);
        public static Error Forbidden(string Code = "General.Forbidden", string Description = "Forbidden Error Has Occured")
            => new Error(Code, Description, ErrorType.Forbidden);
        public static Error Unauthorized(string Code = "General.Unauthorized", string Description = "Unauthorized Error Has Occured")
           => new Error(Code, Description, ErrorType.Unauthorized);
        public static Error Conflict(string Code = "General.Conflict", string Description = "Conflict Error Has Occured")
          => new Error(Code, Description, ErrorType.Conflict);
        public static Error Validation(string Code = "General.Validation", string Description = "Validation Error Has Occured")
          => new Error(Code, Description, ErrorType.Validation);
        public static Error InvalidCredentials(string Code = "General.InvalidCredentials", string Description = "InvalidCredentials Error Has Occured")
          => new Error(Code, Description, ErrorType.InvalidCredentials);




    }

    [JsonConverter(typeof(JsonStringEnumConverter))] // to convert enum to string in json in response
    public enum ErrorType
    {
       Failure = 0,
       NotFound,
       Forbidden,
       Unauthorized,
       Conflict,
       Validation,
       InvalidCredentials
    }
}
