using System;

namespace TodoAndWorkLog.Entities
{
    public class WorkLog
    {
        public string Id { get; set; }

        public string WorkItemId { get; set; }

        public DateTime RecordTime { get; set; }

        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public double Hours { get; set; }


        public WorkItem WorkItem { get; set; }
    }
}
