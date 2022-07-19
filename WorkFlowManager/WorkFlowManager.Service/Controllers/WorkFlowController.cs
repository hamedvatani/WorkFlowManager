using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Service.Core;
using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkFlowManager.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowController : ControllerBase
    {
        private readonly IWorkFlowManagerCore _core;
        private readonly IWorkFlowRepository _repository;

        public WorkFlowController(IWorkFlowManagerCore core, IWorkFlowRepository repository)
        {
            _core = core;
            _repository = repository;
        }

        [HttpPost("StartWorkFlow")]
        public ActionResult StartWorkFlow([FromBody] string jsonEntity, string workFlowName)
        {
            var entity = jsonEntity.CreateEntity();
            if (entity == null)
                return Problem("Invalid jsonEntity");

            var wf = _repository.GetWorkFlow(workFlowName);
            if (wf == null)
                return Problem("WorkFlow not found!");

            var result = _core.StartWorkFlow(entity, wf);
            if (!result.IsSuccess)
                return Problem(result.Message);

            return Ok();
        }




















        // GET: api/<WorkFlowController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WorkFlowController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WorkFlowController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<WorkFlowController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WorkFlowController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
