using CommunicationModel;
using GameComponents.Model;
using Microsoft.AspNetCore.Mvc;
using Windows.Web.Http;

namespace InspectorGoeServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {

        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post(Player model)
        {
            //todo: add player to database

            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        private object Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}