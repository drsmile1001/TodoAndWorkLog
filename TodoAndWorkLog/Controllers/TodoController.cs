﻿using System;
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
        public ActionResult<Todo> Put(string id, [FromBody] Todo model)
        {
            if (id != model.Id)
                return BadRequest();
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return model;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var model = new Todo
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
