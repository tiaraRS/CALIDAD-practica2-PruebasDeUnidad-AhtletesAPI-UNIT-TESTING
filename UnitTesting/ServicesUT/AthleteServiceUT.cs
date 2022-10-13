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
    public class AthleteServiceUT
    {

        //GetAthletesAsync
        //tc1
        [Fact]
        public async Task GetAthletesAsync_DisciplineIdValidAndExistAthletes_ReturnsListOfAthletes()
        {
            int disciplineId = 1;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var athleteModel = new AthleteEntity()
            {
                Id = 95,
                Name = "Juan",
                Nationality = "Boliviano",
                NumberOfCompetitions = 1,
                Gender = Gender.M,
                PersonalBest = 125,
                SeasonBest = 125,
            };
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"
            };

            var athletesEntityEnumerable = new List<AthleteEntity>() { athleteModel } as IEnumerable<AthleteEntity>;
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplineAsync(disciplineId, false)).ReturnsAsync(disciplineEntity100M);
            repositoryMock.Setup(r => r.GetAthletesAsync(disciplineId)).ReturnsAsync(athletesEntityEnumerable);

            var athleteService = new AthleteService(repositoryMock.Object, mapper);
            var athletesList = await athleteService.GetAthletesAsync(disciplineId);

            int expectedListSize = 1;

            Assert.Equal(expectedListSize, athletesList.Count());
        }

        //tc2
        [Fact]
        public async Task GetAthletesAsync_DisciplineIdValidAndNotExistAthletes_ReturnsEmptyList()
        {
            int disciplineId = 2;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"
            };
            var athletesEntityEnumerable = new List<AthleteEntity>() as IEnumerable<AthleteEntity>;
            
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplineAsync(disciplineId, false)).ReturnsAsync(disciplineEntity100M);
            repositoryMock.Setup(r => r.GetAthletesAsync(disciplineId)).ReturnsAsync(athletesEntityEnumerable);

            var athleteService = new AthleteService(repositoryMock.Object, mapper);
            var athletesList = await athleteService.GetAthletesAsync(disciplineId);

            Assert.Empty(athletesList);
        }

        //tc3
        [Fact]
        public async Task GetAthletesAsync_DisciplineIdNotExist_ThrowsNotFoundElementException()
        {
            int disciplineId = 87;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"
            };
            var athletesEntityEnumerable = new List<AthleteEntity>() as IEnumerable<AthleteEntity>;

            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.GetDisciplineAsync(disciplineId, false)).ThrowsAsync(new NotFoundElementException($"Discipline with id {disciplineId} was not found"));
            repositoryMock.Setup(r => r.GetAthletesAsync(disciplineId)).ReturnsAsync(athletesEntityEnumerable);

            var athleteService = new AthleteService(repositoryMock.Object, mapper);


            NotFoundElementException exception = await Assert.ThrowsAsync<NotFoundElementException>(() => athleteService.GetAthletesAsync(disciplineId));
            Assert.Equal("Discipline with id 87 was not found", exception.Message);
        }
    }
}
