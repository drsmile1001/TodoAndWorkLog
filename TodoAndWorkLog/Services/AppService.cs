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
            var oldParentIdAndName = _db.WorkItem.Select(item=>new
            {
                item.ParentId,
                item.Name
            }).ToHashSet();
            var conflict = oldParentIdAndName.Contains(new
            {
                ParentId = model.ParentId,
                Name = model.Name
            });
            if(conflict)
                throw new ArgumentException("已有相同名稱工作項目");
            model.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            model.RecordTime = DateTime.Now;
            _db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return model;
        }

        public WorkItem UpdateWorkItem(WorkItem model)
        {
            model.RecordTime = DateTime.Now;
            _db.Update(model);
            _db.SaveChanges();
            return model;
        }

        public void DeleteWorkItem(WorkItem item){
            _db.Remove(item);
            _db.SaveChanges();
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
