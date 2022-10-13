using AthletesRestAPI.Controllers;
using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
using AthletesRestAPI.Exceptionss;
using AthletesRestAPI.Models;
using AthletesRestAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllersUT
{
    public class AthletesControllerUT
    {
        //CreateAthleteAsync
        //tc1
        [Fact]
        public async Task CreateAthleteAsync_ReturnsStatusCode500()
        {
            int disciplineId = 1;
            var serviceMock = new Mock<IAthleteService>();
            var athlete = new AthleteModel()
            {
                Id = 1,
                BirthDate = DateTime.Now,
                DisciplineId = disciplineId,
                Gender = Gender.M,
                Name = "Pepe",
                NumberOfCompetitions = 1,
                PersonalBest = 125,
                SeasonBest = 150,
                Nationality = "Boliviano"
            };

            serviceMock.Setup(serv => serv.CreateAthleteAsync(It.IsAny<AthleteModel>(), It.IsAny<int>()))
                .Throws(new Exception("Something happened."));

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object, fileService);

            var response = await athletesController.CreateAthleteAsync(athlete,disciplineId);
            var athleteActual = ((ObjectResult)response.Result).Value;
            var actualStatusCode = ((ObjectResult) response.Result).StatusCode;

            Assert.Equal(500, actualStatusCode);
            Assert.Equal("Something happened.", athleteActual);
        }

        //tc2
        [Fact]
        public async Task CreateAthleteAsync_ReturnsStatusCode404()
        {
            int disciplineId = 87;
            var serviceMock = new Mock<IAthleteService>();
            var athlete = new AthleteModel()
            {
                Id = 1,
                BirthDate = DateTime.Now,
                DisciplineId = disciplineId,
                Gender = Gender.M,
                Name = "Pepe",
                NumberOfCompetitions = 1,
                PersonalBest = 125,
                SeasonBest = 150,
                Nationality = "Boliviano"
            };

            serviceMock.Setup(serv => serv.CreateAthleteAsync(It.IsAny<AthleteModel>(), It.IsAny<int>()))
                .Throws(new NotFoundElementException($"Athlete with id {athlete.Id} does not exist in discipline {disciplineId}"));

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object, fileService);

            var response = await athletesController.CreateAthleteAsync(athlete, disciplineId);
            var athleteActual = ((ObjectResult)response.Result).Value;
            var actualStatusCode = ((ObjectResult)response.Result).StatusCode;
            
            Assert.Equal(404, actualStatusCode);
            Assert.Equal("Athlete with id 1 does not exist in discipline 87", athleteActual);
        }
        //tc3
        [Fact]
        public async Task CreateAthleteAsync_ReturnsStatusCode201()
        {
            int disciplineId = 1;
            var serviceMock = new Mock<IAthleteService>();
            var athlete = new AthleteModel()
            {
                Id = 1,
                BirthDate = DateTime.Now,
                DisciplineId = disciplineId,
                Gender = Gender.M,
                Name = "Pepe",
                NumberOfCompetitions = 1,
                PersonalBest = 125,
                SeasonBest = 150,
                Nationality = "Boliviano"
            };

            serviceMock.Setup(serv => serv.CreateAthleteAsync(It.IsAny<AthleteModel>(), It.IsAny<int>())).ReturnsAsync(athlete);

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object, fileService);

            var response = await athletesController.CreateAthleteAsync(athlete, disciplineId);
            var athleteActual = ((ObjectResult)response.Result).Value;
            var actualStatusCode = ((ObjectResult)response.Result).StatusCode;

            Assert.Equal(201, actualStatusCode);
            Assert.Equal(athlete, athleteActual);
        }
        //tc4
        //Failing, BUG Identified Model.IsValid not working
        //TO DO: try to fix the bug 
        //[Fact]
        //public async Task CreateAthleteAsync_ReturnsStatusCode400()
        //{
        //    int disciplineId = 1;
        //    var serviceMock = new Mock<IAthleteService>();
        //    var athlete = new AthleteModel()//no name set
        //    {
        //        Name = null,
        //        BirthDate = DateTime.Now,
        //        NumberOfCompetitions = 1,
        //        PersonalBest = 125,
        //        SeasonBest = 150,
        //        Nationality = "Boliviano"
        //    };

        //    serviceMock.Setup(serv => serv.CreateAthleteAsync(It.IsAny<AthleteModel>(), It.IsAny<int>())).ReturnsAsync(athlete);

        //    var fileService = new FileService();
        //    var athletesController = new AthletesController(serviceMock.Object, fileService);

        //    var response = await athletesController.CreateAthleteAsync(athlete, disciplineId);
        //    var athleteActual = ((ObjectResult)response.Result).Value;
        //    var actualStatusCode = ((ObjectResult)response.Result).StatusCode;

        //    Assert.Equal(400, actualStatusCode);
        //    Assert.IsType<BadRequestObjectResult>(response.Result);
        //}



        //GetAthletesAsync
        //tc1
        [Fact]
        public async Task GetAthletesAsync_ReturnsStatusCode500()
        {
            int disciplineId = 87;
            var serviceMock = new Mock<IAthleteService>();

            serviceMock.Setup(serv => serv.GetAthletesAsync(disciplineId))
                .Throws(new Exception("Something happened"));

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object, fileService);

            var response = await athletesController.GetAthletesAsync(disciplineId);
            var athletesList = response.Value as List<ShortAthleteModel>;
            var actualStatusCode = ((ObjectResult)response.Result).StatusCode;

            Assert.Equal(500, actualStatusCode);
        }
        //tc2
        [Fact]
        public async Task GetAthletesAsync_ReturnsStatusCode404()
        {
            int athleteId = 95;
            int disciplineId = 87;
            var serviceMock = new Mock<IAthleteService>();

            serviceMock.Setup(serv => serv.GetAthletesAsync(disciplineId))
                .Throws(new NotFoundElementException($"Athlete with id {athleteId} does not exist in discipline {disciplineId}"));

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object, fileService);

            var response = await athletesController.GetAthletesAsync(disciplineId);
            var athletesList = response.Value as List<ShortAthleteModel>;
            var actualStatusCode = ((NotFoundObjectResult) response.Result).StatusCode;

            Assert.Equal(404, actualStatusCode);
        }
        //tc3
        [Fact]
        public async Task GetAthletesAsync_ReturnsListOfAthletes()
        {
            int disciplinedId = 1;
            int statusCodeExpected = 200;
            var athleteModel = new ShortAthleteModel()
            {
                Id = 95,
                Name = "Juan",
                Nationality = "Boliviano",
                NumberOfCompetitions = 1,
                DisciplineId = disciplinedId,
                Gender = Gender.M,
                PersonalBest = 125,
                SeasonBest = 125,
            };
            var athletesEnumerable = new List<ShortAthleteModel>() { athleteModel } as IEnumerable<ShortAthleteModel>;
            var serviceMock = new Mock<IAthleteService>();

            serviceMock.Setup(serv => serv.GetAthletesAsync(disciplinedId)).ReturnsAsync(athletesEnumerable);

            var fileService = new FileService();
            var athletesController = new AthletesController(serviceMock.Object,fileService);

            var response = await athletesController.GetAthletesAsync(disciplinedId);
            var athletesList = response.Value as List<ShortAthleteModel>;
            var actualStatusCode = ((OkObjectResult) response.Result).StatusCode;

            Assert.Equal(statusCodeExpected,actualStatusCode);
        }
    }
}
