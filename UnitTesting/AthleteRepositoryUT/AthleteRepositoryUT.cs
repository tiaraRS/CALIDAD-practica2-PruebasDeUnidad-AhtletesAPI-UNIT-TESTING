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
        //GetAthleteAsync
        //tc1
        [Fact]
        public async Task GetAthleteAsync_DisciplineIdValidAndExistAthletes_ReturnsListOfAthletes()
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

            var athleteEntity = new AthleteEntity()
            {
                Id = athleteId,
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
            var result = await repository.SaveChangesAsync();

            
            var athlete = await repository.GetAthleteAsync(athleteId, disciplineId);

            Assert.Equal(athleteId, athlete.Id);
        }

    }
}
