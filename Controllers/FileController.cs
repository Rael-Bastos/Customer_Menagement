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
    public class FileController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IFileService _fileService;

        public FileController(ILogger<ClientController> logger, IFileService fileService)
        {
            _fileService = fileService;
            _logger = logger;
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation($"Iniciando consulta de usuario");
                var user = _fileService.GetAll();
                _logger.LogInformation($"Files Consultado com sucesso");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Aconteceu um erro ao consultar os usuarios");
                return BadRequest(ex);
            }
        }

        
    }
}
