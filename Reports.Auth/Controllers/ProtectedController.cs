using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reports.Auth.Controllers
{
    [ApiController]
    public class ProtectedController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("/api/employeeinterface")]
        public IActionResult GetProtectedData()
        {
            return Ok("Hello world from protected controller.");
        }

        [HttpGet]
        [Authorize(Roles = "TeamLead")]
        [Route("/api/teamleadinterface")]
        public IActionResult GetProtectedDataForTeamLeader()
        {
            return Ok("Hello team lead!");
        }
    }
}