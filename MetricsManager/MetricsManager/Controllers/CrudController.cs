﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MetricsManager.Controllers
{
    [Route("api")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly ValuesHolder holder;

        public CrudController(ValuesHolder holder)
        {
            this.holder = holder;
        }

        [HttpPost("set")]
        public IActionResult Create([FromQuery] DateTime? date, [FromQuery] int? temperature)
        {
            DataAndTemp dt = new DataAndTemp();

            if(date != null)
            {
                dt.Date = (DateTime)date;
            }
            else
            {
                dt.Date = DateTime.Now;
            }

            if (temperature != null)
            {
                dt.Temperature = (int)temperature;
                holder.Values.Add(dt); // without temp is nothing to save
            }

            return Ok();
        }

        [HttpGet("get")]
        public IActionResult Read([FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd)
        {
            if (dateEnd == null)
            {
                dateEnd = DateTime.MaxValue;
            }
                
            if (dateStart == null)
            {
                dateStart = DateTime.MinValue;
            }
                
            var results = holder.Values.Where(x => x.Date >= (DateTime)dateStart && x.Date <= (DateTime)dateEnd);

            if (results.Count<DataAndTemp>() > 0)
                return Ok(results);
            else
                return NoContent();

            //if (dateStart != null)
            //{
            //    System.Collections.Generic.IEnumerable<DataAndTemp> result = null;
            //    if (dateEnd != null) // with dateEnd = return range
            //    {
            //        result = holder.Values.Where(x => x.Date >= (DateTime)dateStart && x.Date <= (DateTime)dateEnd);
            //    }
            //    else // only dateStart sended = return exact value
            //    {
            //        result = holder.Values.Where(x => x.Date == (DateTime)dateStart);
            //    }

            //    if (result.Count<DataAndTemp>() > 0)
            //        return Ok(result);
            //    else
            //        return NoContent();
            //}
            //else // no data sended = return all data
            //{
            //    return Ok(holder.Values);
            //}
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime? date, [FromQuery] int? newValue)
        {
            if (date != null && newValue != null)
            {
                bool founded = false;

                foreach (DataAndTemp data in holder.Values)
                {
                    if (data.Date == date)
                    {
                        data.Temperature = (int)newValue;
                        founded = true;
                    }
                }

                if (!founded)
                    return BadRequest();

                return Ok();
            }
            else // no data for update = error
            {
                return BadRequest();
            }            
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd)
        {
            if (dateEnd == null)
            {
                dateEnd = DateTime.MaxValue;
            }

            if (dateStart == null)
            {
                dateStart = DateTime.MinValue;
            }

            bool founded = false;
            // !!! backward direction only!
            for (int i = holder.Values.Count - 1; i >= 0; i--)
            {
                if (holder.Values[i].Date >= dateStart && holder.Values[i].Date <= dateEnd)
                {
                    holder.Values.RemoveAt(i);
                    founded = true;
                }
            }

            if (!founded)
                return BadRequest();

            return Ok();

            //if (dateStart != null)
            //{
            //    if (dateEnd != null) // with dateEnd = delete range
            //    {
            //        for (int i = 0; i < holder.Values.Count; i++)
            //        {
            //            if (holder.Values[i].Date >= dateStart && holder.Values[i].Date <= dateEnd)
            //            {
            //                holder.Values.RemoveAt(i);
            //            }
            //        }
            //        return Ok();
            //    }
            //    else // only dateStart sended = exact dataTime to delete
            //    {
            //        for (int i = 0; i < holder.Values.Count; i++)
            //        {
            //            if (holder.Values[i].Date == dateStart)
            //            {
            //                holder.Values.RemoveAt(i);
            //            }
            //        }
            //        return Ok();
            //    }               
            //}
            //else // no data sended = error
            //{
            //    return BadRequest();
            //}
        }
    }
}
