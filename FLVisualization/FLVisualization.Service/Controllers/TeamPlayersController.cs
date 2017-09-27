using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class TeamPlayersController : Controller
    {
        private IPlayerRepo Repo { get; set; }

        public TeamPlayersController(IPlayerRepo repo)
        {
            this.Repo = repo;
        }

        // GET api/teamplayers/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.FindPlayersByTeam(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }
    }
}
