using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNet.Mvc;
using SpaceHerders.Services;

namespace SpaceHerders.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersLocationController : Controller
    {
        private readonly IUsersLocationService _usersLocationService;

        public UsersLocationController(IUsersLocationService usersLocationService)
        {
            _usersLocationService = usersLocationService;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<Point>> Get(Guid userId)
        {
            var lastPosition = await _usersLocationService.GetLastUserPosition(userId);
            return await _usersLocationService.GetCloseUsers(lastPosition, 0);
        }

        [HttpPost("{userId}")]
        public async void PostLocation(Guid userId, [FromBody]Point coordinates)
        {
            await _usersLocationService.UpdateUserPosition(userId, coordinates);
        }
    }
}
