using System;

namespace TodoAndWorkLog.Entities
{
    public class WorkLog
    {
        public string Id { get; set; }

        public string TodoId { get; set; }

        public DateTime RecordTime { get; set; }

        public DateTime Date { get; set; }

        public double Hours { get; set; }


        public Todo Todo { get; set; }
    }
}
