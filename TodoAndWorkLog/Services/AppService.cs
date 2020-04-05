using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AppService(ILogger<AppService> logger,IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public WorkItem[] GetWorkItems()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var allItems = db.WorkItem.ToArray();
            return allItems.Where(item=>item.ParentId == null).ToArray();
        }

        public WorkItem AddWorkItem(WorkItem model)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var oldParentIdAndName = db.WorkItem.Select(item=>new
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
            db.Attach(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
            return model;
        }

        public WorkItem UpdateWorkItem(WorkItem model)
        {
            model.Parent = null;

            using var scope = _serviceScopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            model.RecordTime = DateTime.Now;
            db.Update(model);
            db.SaveChanges();
            return model;
        }

        public void DeleteWorkItem(WorkItem item){
            
            using var scope = _serviceScopeFactory.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Remove(item);
            db.SaveChanges();
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
