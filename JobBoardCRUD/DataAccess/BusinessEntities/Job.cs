using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class Job
    {
        public long JobId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
