using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAndWorkLog.Entities
{
    public class Todo
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime RecordTime { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? DoneDate { get; set; }


        public Project Project { get; set; }
    }
}
