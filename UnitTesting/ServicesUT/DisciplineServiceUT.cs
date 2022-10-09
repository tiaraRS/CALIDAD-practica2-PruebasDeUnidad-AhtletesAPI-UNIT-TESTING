using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
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
    }
}
