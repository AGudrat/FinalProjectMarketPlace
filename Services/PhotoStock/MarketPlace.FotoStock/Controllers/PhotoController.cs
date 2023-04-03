using MarketPlace.PhotoStock.Dtos;
using MarketPlace.Shared.ControllerBases;
using MarketPlace.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoPost(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo is not null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);


                var returnPath = photo.FileName;

                PhotoDto photoDto = new()
                {
                    Url = returnPath
                };
                return CreateActionResult(Response<PhotoDto>.Success(photoDto, 200));

            }
            return CreateActionResult(Response<PhotoDto>.Failed("photo is empty", 400));

        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
                return CreateActionResult(Response<NoContent>.Failed("photo not found", 404));

            System.IO.File.Delete(path);
            return CreateActionResult(Response<NoContent>.Success(204));
        }
    }
}
