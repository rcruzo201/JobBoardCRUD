using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        IEnumerable<Job> GetOrderedJobs();
    }
}
