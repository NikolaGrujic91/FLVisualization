using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private ITeamRepo Repo { get; set; }

        public TeamController(ITeamRepo repo)
        {
            this.Repo = repo;
        }

        // GET: api/team
        [HttpGet]
        public IActionResult Get()
        {
            return Json(Repo.GetAll());
        }

        // GET api/team/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.Find(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }

        // POST api/team
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/team/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/team/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
