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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PhotoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> PhotoPost(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo is not null && photo.Length > 0)
            {
                var photosPath = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
                if (!Directory.Exists(photosPath))
                {
                    Directory.CreateDirectory(photosPath);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var path = Path.Combine(photosPath, fileName);

                using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 100);
                await photo.CopyToAsync(stream, cancellationToken);

                var returnPath = fileName;

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
