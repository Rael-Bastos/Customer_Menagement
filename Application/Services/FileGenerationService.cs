using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class FileGenerationService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileAtapter _fileAtapter;
        private readonly IBaseRepository<Files> _filesRepository;
        public FileGenerationService(IBaseRepository<Files> fileRepository,
                                     IFileAtapter fileAtapter,
                                     IConfiguration configuration)
        {
            _configuration = configuration; 
            _filesRepository = fileRepository;
            _fileAtapter = fileAtapter;
        }
        public async Task GenerateFile()
        {
            // Busca arquivos criados hoje
            var files = _filesRepository.FilterBy(f => f.CreatedAt.Date == DateTime.Today);

            if (files != null && files.Any())
            {
                // Definir caminho do arquivo
                string caminhoArquivo = Path.Combine(_configuration.GetSection("PathsFiles:DailyArchive").Value,$"Resumo_{DateTime.Now.ToString("dd-MM-yyyy")}.csv" ); ; // Substitua pelo caminho real do arquivo

                // Chamar o método para gerar o arquivo CSV
                await _fileAtapter.GenerateFileCSV<Files>(files.ToList(), caminhoArquivo, nameof(Files.Content));
            }
        }
        
    }
}
