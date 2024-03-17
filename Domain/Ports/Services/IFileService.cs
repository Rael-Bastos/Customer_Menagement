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
    public interface IFileService
    {
        IEnumerable<Files> GetAll();
    }
}
