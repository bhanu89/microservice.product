using microservice.product.Models;
using Microsoft.AspNetCore.Mvc;

namespace microservice.product.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, bool isGetCall = false)
        {
            if (result == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (result.Value == null && result.Success == true)
            {
                return isGetCall ? new NotFoundResult() : new AcceptedResult();
            }

            if (result.Success == false && string.IsNullOrEmpty(result.ErrorMessage) == false)
            {
                return new ObjectResult(result.ErrorMessage) { StatusCode = StatusCodes.Status400BadRequest };
            }

            return new OkObjectResult(result.Value);
        }
    }
}
