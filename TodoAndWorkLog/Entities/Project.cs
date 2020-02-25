using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAndWorkLog.Entities
{
    public class Project
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime RecordTime { get; set; }


        public List<Todo> Todos { get; set; }
    }
}
