using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IBaseRepository<Client> _clientRepository;
        private readonly IBaseRepository<Files> _filesRepository;
        private readonly IExcelService<Client> _excelService;
        public ClientService(IBaseRepository<Client> clientRepository,
                            IExcelService<Client> excelService,
                            IBaseRepository<Files> fileRepository)
        {
            _excelService = excelService;
            _clientRepository = clientRepository;
            _filesRepository = fileRepository;
        }

        public async Task AddUser(Client user)
        {
            await _clientRepository.InsertOneAsync(user);
        }
        public async Task UpdateUser(Client user)
        {
            await _clientRepository.ReplaceOneAsync(user);
        }
        public async Task<Client> GetUser(string id)
        {
            var user = await _clientRepository.FindByIdAsync(id);
            return user;
        }

        public IEnumerable<Client> GetAll()
        {
            var users = _clientRepository.AsQueryable().ToList();
            return users;
        }
        public async Task DeleteUser(string id)
        {
            await _clientRepository.DeleteByIdAsync(id);
        }

        public async Task AddManyClient(IFormFile formFile)
        {
            // Processar e armazenar os dados em segundo plano
            await Task.Run(async () =>
            {
                var clients = await _excelService.LerXls(formFile);

                if (clients is not null)
                {
                     await _clientRepository.InsertManyAsync(clients);
                }

                var file = await _filesRepository.FindOneAsync(f => f.Name == formFile.Name);
                if (file is not null)
                {
                    file.UpdateProcess();
                    await _filesRepository.ReplaceOneAsync(file);
                }
            });
            _filesRepository.InsertOne(new Files().AddFile(formFile));
        }
    }
}
