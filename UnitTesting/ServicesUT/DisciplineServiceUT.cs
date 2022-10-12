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
        public void GetDisciplineAsync_InvalidId_ThrowsNotFoundElementException()
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
            repositoryMock.Setup(r => r.DeleteDisciplineAsync(1)).ReturnsAsync(true);
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetDisciplineAsync(1, false)).ReturnsAsync(disciplineEntity100M);
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var result = await disciplinesService.DeleteDisciplineAsync(1);      
            Assert.True(result);
        }

        //CreateDisciplineAsync
        //tc1
        [Fact]
        public void CreateDisciplineAsync_ValidId_ReuturnsDBException()
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
                Id=1,
                Name = "Long Jump"
            };
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.CreateDiscipline(longJumpDisciplineEntity));
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var result = await disciplinesService.CreateDisciplineAsync(longJumpDisciplineModel);
            Assert.NotNull(result);
            Assert.Equal("Long Jump",result.Name);
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
                DisciplineId = 1,
                BirthDate = new DateTime(),
                IsActive = true,
                SeasonBest = 52m
            };
            var disciplineName = "400MH";
            var mark = 52.79m;
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = disciplinesService.CheckSeasonBest(sydney, mark, disciplineName);
            Assert.False(result);
        }

        //UpdateWorldRecord
        //tc1
        [Fact]
        public async Task UpdateWorldRecord_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineId = 1;
            var worldRecord = 51.7m;
            var gender = "f";
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = await disciplinesService.updateWorldRecord(disciplineId, worldRecord, gender);
            Assert.False(result);
        }

        //tc2
        [Fact]
        public async Task UpdateWorldRecord_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineId = 1;
            var worldRecord = 51.7m;
            var gender = "m";
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var result = await disciplinesService.updateWorldRecord(disciplineId, worldRecord, gender);
            Assert.False(result);            
        }

        //GetWorldRankingAsync
        //tc1
        [Fact]
        public void GetWorldRankingAsync_InvalidGender_ReturnsInvalidElementOperationException()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineId = 1;
            var gender = "k";
            var repositoryMock = new Mock<IAthleteRepository>();
            //repositoryMock.Setup(d=>d.GetDisciplineAsync(1,true)).ReturnsAsync()
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var exception = Assert.ThrowsAsync<InvalidElementOperationException>(async () => await disciplinesService.GetWorldRankingsAsync(disciplineId, gender));
            Assert.Equal("invalid gender value : k. The allowed values for param are: f,m,all", exception.Result.Message);
        }

        //tc2
        [Fact]
        public async Task GetWorldRankingAsync_ReturnsDisciplineWorldRankings()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineId = 1;
            var gender = "f";
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(d => d.GetDisciplineAsync(1, true)).ReturnsAsync(
                new DisciplineEntity()
                {
                    Id = 1,
                    Name = "400M",
                    FemaleWorldRecord = 57.05m,
                    Athletes = new List<AthleteEntity>(){
                        new AthleteEntity(){ Id = 1,Nationality = "USA", Name = "Sydney Maclaughlin", Gender = Gender.F, Points = 1000},
                        new AthleteEntity(){ Id = 2,Nationality = "USA", Name = "Allyson Felix", Gender = Gender.F, Points = 1500},
                        new AthleteEntity(){ Id = 3,Nationality = "Jamaica", Name = "Usain Bolt", Gender = Gender.M}
                    }
                });
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            
            var worldRankings = await disciplinesService.GetWorldRankingsAsync(disciplineId, gender);
            Assert.NotNull(worldRankings);
            Assert.NotEmpty(worldRankings);
            Assert.Equal(2,worldRankings.Count());
            Assert.Contains(worldRankings, athlete => athlete.Id == 1);
            Assert.Contains(worldRankings, athlete => athlete.Id == 2);
            Assert.DoesNotContain(worldRankings, athlete => athlete.Id == 3);
            Assert.All(worldRankings, athlete => Assert.Equal(Gender.F,athlete.Gender));
            
        }
        //tc3
        [Fact]
        public async Task GetWorldRankingAsync_GenderAll_ReturnsDisciplineWorldRankings()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var disciplineId = 1;
            var gender = "all";
            var repositoryMock = new Mock<IAthleteRepository>();
            repositoryMock.Setup(d => d.GetDisciplineAsync(1, true)).ReturnsAsync(
                new DisciplineEntity()
                {
                    Id = 1,
                    Name = "400M",
                    FemaleWorldRecord = 57.05m,
                    Athletes = new List<AthleteEntity>(){
                        new AthleteEntity(){ Id = 1,Nationality = "USA", Name = "Sydney Maclaughlin", Gender = Gender.F, Points = 1000},
                        new AthleteEntity(){ Id = 2,Nationality = "USA", Name = "Allyson Felix", Gender = Gender.F, Points = 1500},
                        new AthleteEntity(){ Id = 3,Nationality = "Jamaica", Name = "Usain Bolt", Gender = Gender.M, Points = 2000}
                    }
                });
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);

            var worldRankings = await disciplinesService.GetWorldRankingsAsync(disciplineId, gender);
            Assert.NotNull(worldRankings);
            Assert.NotEmpty(worldRankings);
            Assert.Equal(3, worldRankings.Count());
            Assert.Contains(worldRankings, athlete => athlete.Id == 1);
            Assert.Contains(worldRankings, athlete => athlete.Id == 2);
            Assert.Contains(worldRankings, athlete => athlete.Id == 3);
            Assert.Equal("Usain Bolt",worldRankings.First().Name);
            Assert.Equal("Sydney Maclaughlin", worldRankings.Last().Name);            
        }
        //CheckWorldRecord
        //tc1
        [Fact]
        public void CheckWorldRecord_400MHW_ReturnsTrue()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var discipline = new DisciplineModel() { Id = 1, Name = "400MH", FemaleWorldRecord = 51.9m };
            var gender = "f";
            var competingResults = new RaceInfoModel(new List<RaceAthleteModel>(){
                        new RaceAthleteModel(){ Id = 1,Country = "USA", Name = "Sydney Maclaughlin", Mark=51.8m, PB=true, SB=true},
                        new RaceAthleteModel(){ Id = 2,Country = "USA", Name = "Allyson Felix", Mark=52.98m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 3,Country = "Netherlands", Name = "Femke Bol", Mark=53.8m, PB=false, SB=false} }
                        );
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var worldRecord = -1m;
            var result = disciplinesService.checkWorldRecord(gender, discipline, competingResults, out worldRecord);

            Assert.True(result);
            Assert.NotEqual(-1, worldRecord);
            Assert.Equal(51.8m, worldRecord);            
        }

        //tc2
        [Fact]
        public void CheckWorldRecord_100MM_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var discipline = new DisciplineModel() { Id = 1, Name = "100M", MaleWorldRecord = 9.19m };
            var gender = "m";
            var competingResults = new RaceInfoModel(new List<RaceAthleteModel>(){
                        new RaceAthleteModel(){ Id = 1,Country = "Jamaica", Name = "Usain Bolt", Mark=9.5m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 2,Country = "Jamaica", Name = "Johann Blake", Mark=10.2m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 3,Country = "Italy", Name = "Lamont Marcell Jacobs", Mark=9.9m, PB=true, SB=false}
                    });
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var bestMark = -1m;
            var result = disciplinesService.checkWorldRecord(gender, discipline, competingResults, out bestMark);

            Assert.False(result);
            Assert.NotEqual(-1, bestMark);
            Assert.Equal(9.5m, bestMark);
        }

        //tc3
        [Fact]
        public void CheckWorldRecord_LongJumpW_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var discipline = new DisciplineModel() { Id = 1, Name = "Long Jump", FemaleWorldRecord = 7.52m };
            var gender = "f";
            var competingResults = new RaceInfoModel(new List<RaceAthleteModel>(){
                        new RaceAthleteModel(){ Id = 1,Country = "USA", Name = "Tara Davis", Mark=7.44m, PB=true, SB=true},
                        new RaceAthleteModel(){ Id = 2,Country = "USA", Name = "Brittney Reese", Mark=7.35m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 3,Country = "Netherlands", Name = "Jackie Joyner-Kersee", Mark=7.28m, PB=false, SB=false}
                    });
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var bestMark = -1m;
            var result = disciplinesService.checkWorldRecord(gender, discipline, competingResults, out bestMark);

            Assert.False(result);
            Assert.NotEqual(-1, bestMark);
            Assert.Equal(7.44m, bestMark);
        }

        //tc4
        [Fact]
        public void CheckWorldRecord_LongJumpM_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var discipline = new DisciplineModel() { Id = 1, Name = "Long Jump", MaleWorldRecord = 8.95m };
            var gender = "m";
            var competingResults = new RaceInfoModel(new List<RaceAthleteModel>(){
                        new RaceAthleteModel(){ Id = 1,Country = "Greece", Name = "Tentoglou Miltiadis", Mark=8.98m, PB=true, SB=true},
                        new RaceAthleteModel(){ Id = 2,Country = "Cuba", Name = "Juan Miguel Echevarria", Mark=8.76m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 3,Country = "Spain", Name = "Eusebio Cáceres", Mark=8.46m, PB=false, SB=false}
                    });
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var bestMark = -1m;
            var result = disciplinesService.checkWorldRecord(gender, discipline, competingResults, out bestMark);

            Assert.True(result);
            Assert.NotEqual(-1, bestMark);
            Assert.Equal(8.98m, bestMark);
        }

        //tc4
        [Fact]
        public void CheckWorldRecord_InvalidGender_ReturnsFalse()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = config.CreateMapper();
            var discipline = new DisciplineModel() { Id = 1, Name = "Long Jump", MaleWorldRecord = 8.95m };
            var gender = "all";
            var competingResults = new RaceInfoModel(new List<RaceAthleteModel>(){
                        new RaceAthleteModel(){ Id = 1,Country = "Greece", Name = "Tentoglou Miltiadis", Mark=8.98m, PB=true, SB=true},
                        new RaceAthleteModel(){ Id = 2,Country = "Cuba", Name = "Juan Miguel Echevarria", Mark=8.76m, PB=false, SB=true},
                        new RaceAthleteModel(){ Id = 3,Country = "Spain", Name = "Eusebio Cáceres", Mark=8.46m, PB=false, SB=false}
                    });
            var repositoryMock = new Mock<IAthleteRepository>();
            var disciplinesService = new DisciplineService(repositoryMock.Object, mapper);
            var bestMark = -1m;
            var result = disciplinesService.checkWorldRecord(gender, discipline, competingResults, out bestMark);

            Assert.False(result);            
        }
    }
}
