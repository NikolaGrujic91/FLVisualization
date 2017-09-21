using Microsoft.AspNetCore.Mvc;
using FLVisualization.DAL.Repos.Interfaces;

namespace FLVisualization.Service.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private IPositionRepo Repo { get; set; }

        public PositionController(IPositionRepo repo)
        {
            this.Repo = repo;
        }

        // GET: api/position
        [HttpGet]
        public IActionResult Get()
        {
            return Json(Repo.GetAll());
        }

        // GET api/position/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Repo.Find(id);

            if (item == null)
                return NotFound();

            return Json(item);
        }

        // POST api/position
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/position/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/position/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
