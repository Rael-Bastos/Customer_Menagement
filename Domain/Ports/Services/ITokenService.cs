using Domain.DTO;
using Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Services
{
    public interface ITokenService
    {
        TokenDTO GerarToken(User user);
    }
}
