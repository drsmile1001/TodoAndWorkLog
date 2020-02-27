using System;
using System.Collections.Generic;

namespace TodoAndWorkLog.Entities
{
    public class Project
    {
        public string Id { get; set; }

        public DateTime RecordTime { get; set; }

        public string Name { get; set; }

        
        public List<Todo> Todos { get; set; }
    }
}
