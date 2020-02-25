using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoAndWorkLog.Entities;

namespace TodoAndWorkLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TodoController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            return _db.Todo.ToArray();
        }

        [HttpPost]
        public ActionResult<Todo> Post([FromBody] Todo model)
        {
            model.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return model;
        }

        [HttpPut("{id}")]
        public ActionResult<Todo> Put(string id, [FromBody] Todo model)
        {
            if (id != model.Id)
                return BadRequest();
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return model;
        }
    }
}
