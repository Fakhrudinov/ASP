using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MetricsManager.Controllers
{
    [Route("api/crud")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly ValuesHolder holder;

        public CrudController(ValuesHolder holder)
        {
            this.holder = holder;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime? date, [FromQuery] int? temperature)
        {
            WeatherForecast setWFC = new WeatherForecast();

            if(date != null)
            {
                setWFC.Date = (DateTime)date;
            }
            else
            {
                setWFC.Date = DateTime.Now;
            }

            if (temperature != null)
            {
                setWFC.TemperatureC = (int)temperature;
                holder.Values.Add(setWFC); // without temp is nothing to save
            }

            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read()
        {
            return Ok(holder.Values);
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime? date, [FromQuery] int newValue)
        {
            //for (int i = 0; i < holder.Values.Count; i++)
            //{
            //    if (holder.Values[i] == stringsToUpdate)
            //        holder.Values[i] = newValue;
            //}

            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string stringsToDelete)
        {
            //holder.Values = holder.Values.Where(w => w != stringsToDelete).ToList();
            return Ok();
        }
    }
}
