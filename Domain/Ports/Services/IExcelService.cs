using Microsoft.AspNetCore.Http;

namespace Domain.Ports.Services
{
    public interface IExcelService<TEntity>
    {
        MemoryStream LerStream(IFormFile formFile);
        Task<List<TEntity>> LerXls(IFormFile formFile);
    }
}
