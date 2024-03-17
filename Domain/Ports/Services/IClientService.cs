using Domain.Entities;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Services
{
    public interface IClientService
    {
        Task AddUser(Client user);
        Task UpdateUser(Client user);
        Task<Client> GetUser(string id);
        IEnumerable<Client> GetAll();
        Task DeleteUser(string id);
        Task AddManyClient(IFormFile formFile);
    }
}
