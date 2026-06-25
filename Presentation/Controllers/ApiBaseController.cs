using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        // Handle result without value
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();

            else
                return HandleProblem(result.Errors);
        }

        // Handle result with value
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            else
                return HandleProblem(result.Errors);
        }

        #region Helper

        protected ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                    title: "An Unexpected Error Occured");
            if (errors.All(e => e.Type == ErrorType.validation))
                return HandleValidationProblem(errors);

            return HandleSingleErrorProblem(errors[0]);
        }

        private ActionResult HandleSingleErrorProblem(Error error)
        {

            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type)
                );
        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.validation => StatusCodes.Status400BadRequest,
            ErrorType.InvalidCrendentials => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError

        };

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modleStata = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modleStata.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modleStata);
        }

        #endregion
    }
}
