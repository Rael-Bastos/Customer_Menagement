using Domain.Entities;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Customer_Menagement_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

        public ClientController(ILogger<ClientController> logger, IClientService clientService)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] Client user)
        {
            try
            {
                await _clientService.AddUser(user);
                _logger.LogInformation($"Usuario {user.Name} adicionado com sucesso");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aconteceu um erro ao adicionar o usuario");
                return BadRequest(ex);
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] Client user)
        {
            try
            {
                await _clientService.UpdateUser(user);
                _logger.LogInformation($"Usuario {user.Name} alterado com sucesso");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aconteceu um erro ao adicionar o usuario");
                return BadRequest(ex);
            }

        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation($"Iniciando consulta de usuario");
                var user = _clientService.GetAll();
                _logger.LogInformation($"Usuario Consultado com sucesso");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Aconteceu um erro ao consultar os usuarios");
                return BadRequest(ex);
            }

        }

        [HttpGet("{id:length(24)}",Name ="GetUser")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                _logger.LogInformation($"Iniciando consulta de usuario");
                var user = await _clientService.GetUser(id);

                _logger.LogInformation($"Usuario Consultado com sucesso");
                
                if (user is null) return NotFound();
                                
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Aconteceu um erro ao consultar o usuario com o Id= {id}");
                return BadRequest(ex);
            }

        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation($"Iniciando delete de usuario");
                await _clientService.DeleteUser(id);
                _logger.LogInformation($"Usuario deletado com sucesso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Aconteceu um erro ao consultar o usuario com o Id= {id}");
                return BadRequest(ex);
            }

        }


        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddManyUsers(IFormFile formFile)
        {
            try
            {
                await _clientService.AddManyClient(formFile);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aconteceu um erro ao adicionar os usuarios");
                return BadRequest(ex);
            }

        }
    }
}
