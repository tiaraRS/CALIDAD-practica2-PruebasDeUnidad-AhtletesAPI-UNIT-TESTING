using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
using AthletesRestAPI.Exceptionss;
using AthletesRestAPI.Models;
using AthletesRestAPI.Services;
using AutoMapper;
using Moq;
using RestaurantRestAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ServicesUT
{
    public class DisciplineServiceUT
    {
        [Fact]
        public async Task GetDisciplinesAsync_ReturnsListOfDisciplines()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"              
            };
            var disciplineEntity200M = new DisciplineEntity()
            {
                Id = 1,
                Name = "200M"               
            };
            var disciplinesEnumerable = new List<DisciplineEntity>() { disciplineEntity100M, disciplineEntity200M } as IEnumerable<DisciplineEntity>;
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplinesAsync()).ReturnsAsync(disciplinesEnumerable);

            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var disciplinesList = await disciplinesService.GetDisciplinesAsync();
            Assert.NotEmpty(disciplinesList);
            Assert.Equal(2,disciplinesList.Count());
        }


        [Fact]
        public async Task GetDisciplineAsync_ValidId_ReturnsDisciplineCorrespondingToId()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"
            };           
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplineAsync(1, false)).ReturnsAsync(disciplineEntity100M);
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var disciplineFromDB = await disciplinesService.GetDisciplineAsync(1);

            Assert.NotNull(disciplineFromDB);
            Assert.True("100M" == disciplineFromDB.Name);
            Assert.IsType<DisciplineModel>(disciplineFromDB);
        }
        //24.5%
        [Fact]
        public async Task GetDisciplineAsync_InvalidId_ThrowsNotFoundElementException()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplineAsync(100, false)).ReturnsAsync((DisciplineEntity)null);
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var exception = Assert.ThrowsAsync<NotFoundElementException>(async () => await disciplinesService.GetDisciplineAsync(100));
            Assert.Equal("discipline with id 100 does not exist", exception.Result.Message);

        }
    }
}
