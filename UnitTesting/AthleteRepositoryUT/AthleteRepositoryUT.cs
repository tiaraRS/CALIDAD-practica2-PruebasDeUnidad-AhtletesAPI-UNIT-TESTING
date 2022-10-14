using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
using AthletesRestAPI.Models;
using AthletesRestAPI.Services;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.AthleteRepositoryUT
{
    public class AthleteRepositoryUT : BaseTest
    {
        //GetDisciplineAsync
        //tc1
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task GetDisciplineAsync_DisciplineExist_ReturnsDiscipline(bool showAthletes)
        {
            int disciplineId = 1;
            var disciplineEntity100M = new DisciplineEntity()
            {
                Name = "100M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var repository = new AthleteRepository(ctx);
            repository.CreateDiscipline(disciplineEntity100M);
            var result = await repository.SaveChangesAsync();

            var athleteEntity = new AthleteEntity()
            {
                Name = "Juan",
                Nationality = "Boliviano",
                NumberOfCompetitions = 1,
                Gender = Gender.M,
                PersonalBest = 125,
                SeasonBest = 125,
                Discipline = disciplineEntity100M,
                BirthDate = DateTime.Now,
                ImagePath = " ",
                IsActive = false,
                Points = 100
            };

            repository.CreateAthlete(athleteEntity, disciplineId);
            result = await repository.SaveChangesAsync();

            var discipline = await repository.GetDisciplineAsync(disciplineId,showAthletes);

            Assert.Equal(disciplineId, discipline.Id);
            if(showAthletes)
                Assert.NotEmpty(discipline.Athletes);
            if(!showAthletes)
                Assert.Null(discipline.Athletes);
        }


        //GetDisciplinesAsync
        //tc1
        [Fact]
        public async Task GetDisciplinesAsync_DisciplinesExist_ReturnsDisciplinesList()
        {
            int disciplineId = 1;
            var disciplineEntity100M = new DisciplineEntity()
            {
                Name = "100M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var disciplineEntity400M = new DisciplineEntity()
            {
                Name = "400M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var repository = new AthleteRepository(ctx);
            repository.CreateDiscipline(disciplineEntity100M);
            var result = await repository.SaveChangesAsync();
            repository.CreateDiscipline(disciplineEntity400M);
            result = await repository.SaveChangesAsync();

            var disciplines = await repository.GetDisciplinesAsync();

            Assert.Equal(2, disciplines.Count());
        }


        //DeleteDisciplineAsync
        //tc1
        [Fact]
        public async Task DeleteDisciplineAsync_AthletesNotEmpty_ReturnsTrue()
        {
            int disciplineId = 1;
            var disciplineEntity100M = new DisciplineEntity()
            {
                Name = "100M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };

            var athleteEntity = new AthleteEntity()
            {
                Name = "Juan",
                Nationality = "Boliviano",
                NumberOfCompetitions = 1,
                Gender = Gender.M,
                PersonalBest = 125,
                SeasonBest = 125,
                Discipline = disciplineEntity100M,
                BirthDate = DateTime.Now,
                ImagePath = " ",
                IsActive = false,
                Points = 100
            };


            var repository = new AthleteRepository(ctx);
            repository.CreateAthlete(athleteEntity, disciplineId);
            var result = await repository.SaveChangesAsync();

            var disciplineEntity400M = new DisciplineEntity()
            {
                Name = "400M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            
            repository.CreateDiscipline(disciplineEntity100M);
            result = await repository.SaveChangesAsync();
            repository.CreateDiscipline(disciplineEntity400M);
            result = await repository.SaveChangesAsync();


            var deleted = await repository.DeleteDisciplineAsync(disciplineEntity100M.Id);
            result = await repository.SaveChangesAsync();

            var disciplines = await repository.GetDisciplinesAsync();
            var athletes = await repository.GetAthletesAsync(disciplineId);

            Assert.True(deleted);
            Assert.Single(disciplines);
            Assert.Empty(athletes);
        }

        //tc2
        [Fact]
        public async Task DeleteDisciplineAsync_AthletesEmpty_ReturnsTrue()
        {
            int disciplineId = 1;
            var disciplineEntity100M = new DisciplineEntity()
            {
                Name = "100M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var disciplineEntity400M = new DisciplineEntity()
            {
                Name = "400M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var repository = new AthleteRepository(ctx);
            repository.CreateDiscipline(disciplineEntity100M);
            var result = await repository.SaveChangesAsync();
            repository.CreateDiscipline(disciplineEntity400M);
            result = await repository.SaveChangesAsync();


            var deleted = await repository.DeleteDisciplineAsync(disciplineEntity100M.Id);
            result = await repository.SaveChangesAsync();

            var disciplines = await repository.GetDisciplinesAsync();

            Assert.True(deleted);
            Assert.Single(disciplines);
        }



        //GetAthleteAsync
        //tc1
        [Fact]
        public async Task GetAthleteAsync_DisciplineAndAthleteExist_ReturnsAthlete()
        {
            int disciplineId = 1;
            int athleteId = 1;
            var disciplineEntity100M = new DisciplineEntity()
            {
                Id = disciplineId,
                Name = "100M",
                CreationDate = DateTime.Now,
                Rules = "fafasdfasf",
                MaleWorldRecord = 125,
                FemaleWorldRecord = 130,
                ImagePath = "Faafdasfsd"
            };
            var repository = new AthleteRepository(ctx);
            repository.CreateDiscipline(disciplineEntity100M);
            var result = await repository.SaveChangesAsync();


            var athleteEntity = new AthleteEntity()
            {
                Name = "Juan",
                Nationality = "Boliviano",
                NumberOfCompetitions = 1,
                Gender = Gender.M,
                PersonalBest = 125,
                SeasonBest = 125,
                Discipline = disciplineEntity100M,
                BirthDate = DateTime.Now,
                ImagePath = " ",
                IsActive = false,
                Points = 100
            };
            
            repository.CreateAthlete(athleteEntity,disciplineId);
            result = await repository.SaveChangesAsync();

            
            var athlete = await repository.GetAthleteAsync(athleteId, disciplineId);

            Assert.Equal(athleteId, athlete.Id);
        }

    }
}
