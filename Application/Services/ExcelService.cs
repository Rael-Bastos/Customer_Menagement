using Domain.DTO;
using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Reflection;

namespace Application.Services
{
    public class ExcelService<TEntity> : IExcelService<TEntity> where TEntity : class
    {
        public ExcelService()
        {

        }


        public async Task<List<TEntity>> LerXls(IFormFile formFile)
        {
            try
            {
                return await Task.Run(() => {
                    var response = new List<TEntity>();

                    var stream = LerStream(formFile);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage excel = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = excel.Workbook.Worksheets[0];
                        int numRow = worksheet.Dimension.End.Row;
                        int numColumn = worksheet.Dimension.End.Column;
                        string[] nameColumns = new string[numColumn];

                        if (numRow > 1) // Verifica se há pelo menos duas linhas (incluindo cabeçalho e pelo menos uma linha de dados)
                        {

                            // Lê os nomes das colunas (cabeçalho)
                            for (int column = 1; column <= numColumn; column++)
                            {
                                nameColumns[column - 1] = worksheet.Cells[1, column].Value?.ToString();
                            }

                            // Remova colunas vazias do array nameColumns
                            nameColumns = nameColumns.Where(column => !string.IsNullOrEmpty(column)).ToArray();
                            numColumn = nameColumns.Length;
                            // Lê os dados das linhas
                            for (int row = 2; row <= numRow; row++) // Começa da segunda linha, assumindo que a primeira é o cabeçalho
                            {
                                var entity = Activator.CreateInstance<TEntity>(); // Cria uma nova instância da entidade genérica

                                for (int column = 1; column <= numColumn; column++)
                                {
                                    // Atribui os valores das células às propriedades da entidade
                                    PropertyInfo property = typeof(TEntity).GetProperty(nameColumns[column - 1]);
                                    if (property != null)
                                    {
                                        property.SetValue(entity, worksheet.Cells[row, column].Value?.ToString());
                                    }
                                }

                                response.Add(entity); // Adiciona a entidade à lista de resposta
                            }

                        }
                    }

                    return response;
                });
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public MemoryStream LerStream(IFormFile formFile)
        {

            using (var stream = new MemoryStream())
            {
                formFile?.CopyTo(stream);
                var listBytes = stream.ToArray();
                return new MemoryStream(listBytes);
            }

        }
    }
}
