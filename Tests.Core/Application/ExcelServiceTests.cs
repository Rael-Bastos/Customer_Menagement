using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using OfficeOpenXml;

namespace Tests.Core.Application
{
    public class ExcelServiceTests
    {
        [Fact]
        public async Task LerXls_ShouldReadExcelFile()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            var excelPackageMock = new Mock<ExcelPackage>();
            var excelWorksheetMock = new Mock<ExcelWorksheet>();
            var stream = new MemoryStream();

            excelWorksheetMock.SetupGet(ws => ws.Dimension).Returns(new ExcelAddressBase(3,3,3,3));
            excelPackageMock.Setup(ep => ep.Workbook.Worksheets[0]).Returns(excelWorksheetMock.Object);
            excelPackageMock.Setup(ep => ep.Dispose());
            var excelService = new ExcelService<Client>();

            // Act
            var result = await excelService.LerXls(formFileMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Client>>(result);
            Assert.Empty(result); // Assuming the Excel file is empty
        }

        [Fact]
        public void LerStream_ShouldReturnMemoryStream()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var excelService = new ExcelService<Client>();

            // Act
            var result = excelService.LerStream(formFileMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MemoryStream>(result);
            Assert.Equal(stream, result);
        }
    }
}