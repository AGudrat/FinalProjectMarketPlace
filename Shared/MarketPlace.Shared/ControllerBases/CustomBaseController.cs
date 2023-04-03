using MarketPlace.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResult<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}

