using Domain.DTO;
using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        public TokenDTO GerarToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Settings._securityKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name.ToString()),
                        new Claim(ClaimTypes.Role, user.Profile.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new TokenDTO()
                {
                    Usuario = user.Username,
                    DataExpiracao = tokenDescriptor.Expires.Value,
                    Token = tokenHandler.WriteToken(token),
                    NomeCompleto = user.Name,
                    Perfil = user.Profile,
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
