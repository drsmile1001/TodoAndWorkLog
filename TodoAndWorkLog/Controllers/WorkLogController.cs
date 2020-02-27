using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoAndWorkLog.Entities;

namespace TodoAndWorkLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkLogController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WorkLogController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<WorkLog> Get()
        {
            return _db.WorkLog.ToArray();
        }

        [HttpPost]
        public ActionResult<WorkLog> Post([FromBody] WorkLog model)
        {
            model.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            model.RecordTime = DateTime.Now;
            try
            {
                _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return model;
        }

        [HttpPut("{id}")]
        public ActionResult<WorkLog> Put(string id, [FromBody] WorkLog model)
        {
            if (id != model.Id)
                return BadRequest();
            model.RecordTime = DateTime.Now;
            try
            {
                _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return model;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var model = new WorkLog
            {
                Id = id
            };
            try
            {
                _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return Ok();
        }
    }
}
