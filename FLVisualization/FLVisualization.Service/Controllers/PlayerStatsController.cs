using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class PlayerStatsController : Controller
    {
        private IPlayerHistoryRepo Repo { get; set; }

        public PlayerStatsController(IPlayerHistoryRepo repo)
        {
            this.Repo = repo;
        }

        // GET api/playerstats/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.FindPlayerHistory(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }
    }
}
