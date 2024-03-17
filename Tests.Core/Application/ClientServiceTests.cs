using Application.Services;
using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Ports.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Core.Application
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task AddUser_ShouldInsertUser()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var user = new Client();

            // Act
            await service.AddUser(user);

            // Assert
            clientRepositoryMock.Verify(repo => repo.InsertOneAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldReplaceUser()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var user = new Client();

            // Act
            await service.UpdateUser(user);

            // Assert
            clientRepositoryMock.Verify(repo => repo.ReplaceOneAsync(user), Times.Once);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var userId = "123";
            var expectedUser = new Client();
            clientRepositoryMock.Setup(repo => repo.FindByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await service.GetUser(userId);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var expectedUsers = new List<Client> { new Client(), new Client(), new Client() };
            clientRepositoryMock.Setup(repo => repo.AsQueryable().ToList()).Returns(expectedUsers);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.Equal(expectedUsers, result);
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUser()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var userId = "123";

            // Act
            await service.DeleteUser(userId);

            // Assert
            clientRepositoryMock.Verify(repo => repo.DeleteByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task AddManyClient_ShouldInsertManyClients()
        {
            // Arrange
            var clientRepositoryMock = new Mock<IBaseRepository<Client>>();
            var excelServiceMock = new Mock<IExcelService<Client>>();
            var fileRepositoryMock = new Mock<IBaseRepository<Files>>();
            var service = new ClientService(clientRepositoryMock.Object, excelServiceMock.Object, fileRepositoryMock.Object);
            var formFileMock = new Mock<IFormFile>();
            var clients = new List<Client>();

            // Set up mocks for excel service and file repository
            excelServiceMock.Setup(service => service.LerXls(It.IsAny<IFormFile>())).ReturnsAsync(clients);
            fileRepositoryMock.Setup(repo => repo.FindOneAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Files, bool>>>()))
                             .ReturnsAsync((Files)null);

            // Act
            await service.AddManyClient(formFileMock.Object);

            // Assert
            clientRepositoryMock.Verify(repo => repo.InsertManyAsync(clients), Times.Once);
        }
    }
}