using Microsoft.EntityFrameworkCore;
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

        public AppService(AppDbContext db)
        {
            _db = db;
        }

        public WorkItem[] GetWorkItems()
        {
            return _db.WorkItem
               .AsNoTracking()
               .ToArray()
               .Where(item => item.ParentId == null)
               .ToArray();
        }

        //public Project InsertPost(Project model)
        //{
        //    model.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        //    model.RecordTime = DateTime.Now;
        //    _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        //    _db.SaveChanges();
        //    return model;
        //}

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
