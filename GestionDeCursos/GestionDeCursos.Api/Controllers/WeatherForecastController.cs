using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeCursos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IUnitOfWork unitOfWork,
            ILogger<WeatherForecastController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var courses = await _unitOfWork.CourseRepository.GetAll();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //[HttpGet(Name = "GetCourses")]
        //public async Task<IEnumerable<Course>> GetCourses()
        //{
        //    var courses = await _unitOfWork.CourseRepository.GetAll();

        //    return courses;
        //}
    }
}
