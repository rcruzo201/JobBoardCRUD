using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(ApplicationContext context) : base(context)
        {
        }
        public IEnumerable<Job> GetOrderedJobs()
        {
            return _context.Jobs.OrderByDescending(d => d.CreatedAt).ToList();
        }
    }
}
