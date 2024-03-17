using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IBaseRepository<Files> _filesRepository;
        public FileService(
                            IBaseRepository<Files> fileRepository)
        {
            _filesRepository = fileRepository;
        }

        public IEnumerable<Files> GetAll()
        {
            var files = _filesRepository.AsQueryable().ToList();
            return files;
        }
       
    }
}
