using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoAndWorkLog.Entities;

namespace TodoAndWorkLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProjectController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return _db.Project.ToArray();
        }

        [HttpPost]
        public ActionResult<Project> Post([FromBody] Project model)
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
        public ActionResult<Project> Patch(string id, [FromBody] Project model)
        {
            if (id != model.Id)
                return BadRequest();
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return model;
        }
    }
}
