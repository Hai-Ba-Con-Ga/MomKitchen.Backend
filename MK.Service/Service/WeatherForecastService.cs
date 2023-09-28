

using MK.API.Application.Repository;
using MK.Application.Repository;
using MK.Domain.Common;
using System.CodeDom;

namespace MK.Service.Service
{
    public class WeatherForecastService : BaseService, IWeatherForecastService
    {

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var weatherForecastList = new List<WeatherForecast>();

            var weatherForecast = _mapper.Map<WeatherForecast>(new
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });

            if (weatherForecast != null)
            {
                weatherForecastList.Add(weatherForecast);
            }

            return weatherForecastList;
        }
    }
}
