using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tokens.Configuration;

namespace Tokens.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IDocumentDBRepository<Token> _repo;
        private readonly ILogger<TokenController> _logger;
        private readonly IOptions<DB> _settings;

        public TokenController(IOptions<DB> settings, ILogger<TokenController> logger, IDocumentDBRepository<Token> repo)
        {
            _logger = logger;
            _repo = repo;
            _settings = settings;
            _logger.LogInformation(_settings.Value.Endpoint);

        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody]Token token)
        {
            _logger.LogInformation("TokenController::CreateAsync");
            await _repo.CreateItemAsync(token);
            return Ok();
        }

        [HttpGet]
        public ActionResult GetHomeAsync()
        {
            _logger.LogInformation("TokenController::GetHomeAsync");
            return Ok($"Using: {_settings.Value.Endpoint}/{_settings.Value.Database}/{_settings.Value.Collection}");
        }

        [HttpGet]
        [Route("api/[Controller]/{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            _logger.LogInformation("TokenController::GetAsync");
            var result = await _repo.GetItemAsync(id);
            return Ok(result);
        }

		[HttpGet]
		[Route("api/[Controller]/health/result")]
		public ActionResult GetHealth(){
			return Ok();
		}

    }
}