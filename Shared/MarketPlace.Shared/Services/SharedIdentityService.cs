using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MarketPlace.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _htppContextAccessor;

        public SharedIdentityService(IHttpContextAccessor contextAccessor)
        {
            _htppContextAccessor = contextAccessor;
        }

        public string GetUserId => _htppContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
    }

}