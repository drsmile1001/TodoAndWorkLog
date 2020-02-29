using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAndWorkLog.Entities;

namespace TodoAndWorkLog.Services
{
    public class AppService
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public AppService(AppDbContext db,ILogger<AppService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public WorkItem[] GetWorkItems()
        {
            return _db.WorkItem
               .ToArray()
               .Where(item => item.ParentId == null)
               .ToArray();
        }

        public WorkItem AddWorkItem(WorkItem model)
        {
            _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(model));
            model.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return model;
        }

        //[HttpPut("{id}")]
        //public ActionResult<Project> Patch(string id, [FromBody] Project model)
        //{
        //    if (id != model.Id)
        //        return BadRequest();
        //    model.RecordTime = DateTime.Now;
        //    _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    _db.SaveChanges();
        //    return model;
        //}
    }
}
