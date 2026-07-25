using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase // make a whole one response for all application
    {
        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.data);
            }
            else
            {
                //Errors
                return ToProblem(result.Errors);
            }
        }

        public static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
            {
                return new OkResult();
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }


        protected static ObjectResult ToProblem(IReadOnlyList<Error>errors)
        {
            var firsterror = errors[0];
            var statuscode = firsterror.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError

            };
            var problem= new ProblemDetails()
            {
                Status = statuscode,
                Title = firsterror.Code,
                Detail = firsterror.Description,
                Extensions = { ["Errors"] = errors}

            };

            return new ObjectResult(problem) { StatusCode = statuscode };
        }
        protected string? GetUserEmail()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.Email);
             
        } 

        

    }
}
