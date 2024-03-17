using Domain.DTO;
using Domain.Entities;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Customer_Menagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AccountController(ILogger<ClientController> logger, 
                                    IUserService userService,
                                    ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }
        [HttpPost(Name = "Login")]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                var user = await _userService.GetUser(login);
                if (user is not null)
                {
                    var tokenDto = _tokenService.GerarToken(user);
                    return Ok(tokenDto);
                }
                else
                    return Unauthorized("Usuario ou senha não encontrados");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Usuario ou senha não encontrados");
                return Unauthorized(ex);
            }
        }
        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                await _userService.AddUser(user);
                _logger.LogInformation($"Usuario {user.Username} adicionado com sucesso");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aconteceu um erro ao adicionar o usuario");
                return BadRequest(ex);
            }

        }
    }
}
