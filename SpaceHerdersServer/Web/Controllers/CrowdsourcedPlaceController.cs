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
        private readonly ICrowdsourcedPointsService _crowdsourcedPointsService;
        private readonly IUsersLocationService _usersLocationService;

        public CrowdsourcedPlaceController(ICrowdsourcedPointsService crowdsourcedPointsService, IUsersLocationService usersLocationService)
        {
            _crowdsourcedPointsService = crowdsourcedPointsService;
            _usersLocationService = usersLocationService;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<Place>> Get(Guid userId)
        {
            var lastPosition = await _usersLocationService.GetLastUserPosition(userId);
            return await _crowdsourcedPointsService.GetClosePoint(lastPosition, 0);
        }

        [HttpPost]
        public async void PostPlace([FromBody]Place place)
        {
            await _crowdsourcedPointsService.CreateCrowdsourcedPoint(place);
        }
    }
}
