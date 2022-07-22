using AutenticaB.Entities;
using Microsoft.AspNetCore.Mvc;
using TMan.Entities.LocationEntity;
using TMan.Entities.User.Commands;

namespace AutenticaB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : BaseController
    {

        [HttpGet("[action]")]
        public async Task<ExtremeLocationsDto> GetAsync()
        {
            return await Mediator.Send(new GetExtremeLocations.Command());
        }
    }
}