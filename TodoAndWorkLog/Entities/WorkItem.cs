using System;
using System.Collections.Generic;

namespace TodoAndWorkLog.Entities
{
    public class WorkItem
    {
        public string Id { get; set; }

        public string? ParentId { get; set; }

        public DateTime RecordTime { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = "";

        public DateTime? DueDate { get; set; }

        public DateTime? DoneDate { get; set; }


        public WorkItem Parent{ get; set; }

        public List<WorkItem> Children { get; set; }

        public List<WorkLog> WorkLogs { get; set; }
    }
}
