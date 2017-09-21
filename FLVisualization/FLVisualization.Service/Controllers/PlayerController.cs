using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private IPlayerRepo Repo { get; set; }

        public PlayerController(IPlayerRepo repo)
        {
            this.Repo = repo;
        }

        // GET: api/player
        [HttpGet]
        public IActionResult Get()
        {
            return Json(Repo.GetAll());
        }

        // GET api/player/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.Find(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }

        // POST api/player
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/player/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/player/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
