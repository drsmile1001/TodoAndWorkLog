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
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return model;
        }

        [HttpPut("{id}")]
        public ActionResult<Project> Patch(string id, [FromBody] Project model)
        {
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return model;
        }
    }
}
