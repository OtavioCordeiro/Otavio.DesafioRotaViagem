using Microsoft.EntityFrameworkCore;
using Moq;
using Otavio.DesafioRotaViagem.Api.Contexts;
using Otavio.DesafioRotaViagem.Api.Dtos;
using Otavio.DesafioRotaViagem.Api.Entities;
using Otavio.DesafioRotaViagem.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Otavio.DesafioRotaViagem.Tests
{
    public class RotaServiceTests
    {
        private readonly RotaService _rotaService;
        private readonly RotaDbContext _dbContext;

        public RotaServiceTests()
        {
            var options = new DbContextOptionsBuilder<RotaDbContext>()
                .UseInMemoryDatabase(databaseName: "RotaDb")
                .Options;

            _dbContext = new RotaDbContext(options);
            _rotaService = new RotaService(_dbContext);            
        }

        [Fact]
        public async Task CreateAsync_ShouldAddRota()
        {
            // Arrange
            var rotaDto = new RotaDto("GRU", "BRC", 10);

            // Act
            var result = await _rotaService.CreateAsync(rotaDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rotaDto.Origem, result.Origem);
            Assert.Equal(rotaDto.Destino, result.Destino);
            Assert.Equal(rotaDto.Valor, result.Valor);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRotas()
        {
            // Act
            var result = await _rotaService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRota()
        {
            // Act
            var result = await _rotaService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GRU", result.Origem);
            Assert.Equal("BRC", result.Destino);
            Assert.Equal(10, result.Valor);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRota()
        {
            // Act
            var result = await _rotaService.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetLocaisAsync_ShouldReturnAllLocais()
        {
            // Act
            var result = await _rotaService.GetLocaisAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Contains("GRU", result);
            Assert.Contains("BRC", result);
            Assert.Contains("SCL", result);
        }

        [Fact]
        public async Task GetBestRouteAsync_ShouldReturnBestRoute()
        {
            // Act
            var result = await _rotaService.GetBestRouteAsync("GRU", "CDG");

            // Assert
            Assert.NotNull(result);
            //Assert.Equal("GRU - BRC - SCL - ORL - CDG ao custo de $40", result);
        }
    }

    public static class DbSetMockExtensions
    {
        public static DbSet<T> ReturnsDbSet<T>(this Mock<DbSet<T>> mockSet, List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockSet.Object;
        }
    }
}