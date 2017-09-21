using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class PlayerHistoryController : Controller
    {
        private IPlayerHistoryRepo Repo { get; set; }

        public PlayerHistoryController(IPlayerHistoryRepo repo)
        {
            this.Repo = repo;
        }

        // GET: api/playerhistory
        [HttpGet]
        public IActionResult Get()
        {
            return Json(Repo.GetAll());
        }

        // GET api/playerhistory/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.FindPlayerHistory(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }

        // POST api/playerhistory
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/playerhistory/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/playerhistory/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
