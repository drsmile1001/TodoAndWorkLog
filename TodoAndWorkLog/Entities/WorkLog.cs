using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAndWorkLog.Entities
{
    public class WorkLog
    {
        public string Id { get; set; }

        public string TodoId { get; set; }

        public DateTime RecordTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
