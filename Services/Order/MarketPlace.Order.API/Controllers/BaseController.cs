using MarketPlace.Shared.ControllerBases;
using MarketPlace.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : CustomBaseController
    {
        private readonly IMediator? _mediator;
        private readonly ISharedIdentityService? _sharedIdentityService;
        protected IMediator Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<IMediator>();
        protected ISharedIdentityService SharedIdentity => _sharedIdentityService ?? HttpContext.RequestServices.GetRequiredService<ISharedIdentityService>();
    }
}
