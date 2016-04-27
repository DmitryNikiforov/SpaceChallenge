using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SpaceHerders.Models;
using SpaceHerders.Services;


namespace SpaceHerders.Web.Controllers
{
    [Route("api/[controller]")]
    public class CrowdsourcedPlaceController : Controller
    {
        private readonly ICrowdsourcedPlacesService _crowdsourcedPlacesService;
        private readonly IUsersLocationService _usersLocationService;

        public CrowdsourcedPlaceController(ICrowdsourcedPlacesService crowdsourcedPlacesService, IUsersLocationService usersLocationService)
        {
            _crowdsourcedPlacesService = crowdsourcedPlacesService;
            _usersLocationService = usersLocationService;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<CrowdsourcedPlace>> Get(Guid userId)
        {
            var lastPosition = await _usersLocationService.GetLastUserPosition(userId);
            return await _crowdsourcedPlacesService.GetClosePlaces(lastPosition, 0);
        }

        [HttpPost]
        public async Task<IActionResult> PostPlace([FromBody]CrowdsourcedPlace place)
        {
            if (place == null) return new BadRequestObjectResult(ModelState);

            await _crowdsourcedPlacesService.CreateCrowdsourcedPoint(place);
            return Ok();
        }
    }
}
