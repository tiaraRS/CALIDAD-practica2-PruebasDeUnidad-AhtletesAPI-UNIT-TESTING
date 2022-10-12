﻿using AthletesRestAPI.Data.Entity;
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
        //GetDisciplinesAsync
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
        //GetDisciplineAsync
        //tc1
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
        //tc2
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

        //DeleteDisciplineAsync
        //tc1
        [Fact]
        public async Task DeleteDisciplineAsync_ValidId_ReuturnsDBException()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 100,
                Name = "100M"
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            
            repositoryMock.Setup(r => r.DeleteDisciplineAsync(100));
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(false);
            repositoryMock.Setup(r => r.GetDisciplineAsync(100, false)).ReturnsAsync(disciplineEntity100M);
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);            

            var exception = Assert.ThrowsAsync<Exception>(async () => await disciplinesService.DeleteDisciplineAsync(100));
            Assert.Equal("Database Error", exception.Result.Message);
        }
        //tc2
        [Fact]
        public async Task DeleteDisciplineAsync_ValidId_DeletesDisicpline()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = 1,
                Name = "100M"
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.DeleteDisciplineAsync(1));
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetDisciplineAsync(1, false)).ReturnsAsync(disciplineEntity100M);
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            await disciplinesService.DeleteDisciplineAsync(1);                    
        }

        //CreateDisciplineAsync
        //tc1
        [Fact]
        public async Task CreateDisciplineAsync_ValidId_ReuturnsDBException()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var longJumpDisciplineEntity = new DisciplineEntity()
            {
                Name = "Long Jump"
            };
            var longJumpDisciplineModel = new DisciplineModel()
            {
                
                Name = "Long Jump"
            };
            var repositoryMock = new Mock<IAthleteRepository>();           
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(false);
            repositoryMock.Setup(r => r.CreateDiscipline(longJumpDisciplineEntity));
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var exception = Assert.ThrowsAsync<Exception>(async () => await disciplinesService.CreateDisciplineAsync(longJumpDisciplineModel));
            Assert.Equal("Database Error", exception.Result.Message);
        }
        //tc2
        [Fact]
        public async Task CreateDisciplineAsync_ValidId_CreatesDiscipline()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var longJumpDisciplineEntity = new DisciplineEntity()
            {
                Name = "Long Jump"
            };
            var longJumpDisciplineModel = new DisciplineModel()
            {

                Name = "Long Jump"
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.CreateDiscipline(longJumpDisciplineEntity));
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            await disciplinesService.CreateDisciplineAsync(longJumpDisciplineModel);
        }

        //CheckPersonalBest
        //tc1
        [Fact]
        public void CheckPersonalBest_NoPersonalBest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id=1,Nationality="USA", Name="Sydney Maclaughlin", Gender=Gender.F, Points=1000, PersonalBest=52.75m
            };
            var mark = 53.05m;
            string discipline = "400MH";
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckPersonalBest(sydney,mark, discipline);
            Assert.False(result);            
        }

        //tc2
        [Fact]
        public void CheckPersonalBest_PersonalBest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m
            };
            var mark = 51.79m;
            string discipline = "400MH";
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckPersonalBest(sydney, mark, discipline);
            Assert.True(result);
        }

        //Mark
        //tc1
        [Fact]
        public void Mark_ReturnsAthleteMark_SeasonBestIncluded()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = 52.75m
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.Mark(sydney);
            Assert.InRange(result, 52.55m, 52.94m);// Random.Next(inclusivo, exclusivo) - InRange(
        }

        //tc2
        [Fact]
        public void Mark_ReturnsAthleteMark_NoSeasonBest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = null
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.Mark(sydney);
            Assert.InRange(result, 52.55m, 53.24m);// Random.Next(inclusivo, exclusivo) - InRange(
        }


        //CheckSeasonBest
        //tc1
        [Fact]
        public void CheckSesasonBest_SeasonBestNull_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = null
            };
            var disciplineName = "400MH";
            var mark = 51.76m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney,mark,disciplineName);
            Assert.True(result);
        }
        //tc2
        [Fact]
        public void CheckSesasonBest_SeasonBestImproved_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52m,
                SeasonBest = 51.79m
            };
            var disciplineName = "400MH";
            var mark = 51.76m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney, mark, disciplineName);
            Assert.True(result);
        }

        //tc3
        [Fact]
        public void CheckSesasonBest_SeasonBestNotImproved_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var sydney = new AthleteModel()
            {
                Id = 1,
                Nationality = "USA",
                Name = "Sydney Maclaughlin",
                Gender = Gender.F,
                Points = 1000,
                PersonalBest = 52.75m,
                SeasonBest = 52m
            };
            var disciplineName = "400MH";
            var mark = 52.79m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney, mark, disciplineName);
            Assert.False(result);
        }

    }
}
