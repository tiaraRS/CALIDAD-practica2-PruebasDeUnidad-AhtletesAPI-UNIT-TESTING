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
