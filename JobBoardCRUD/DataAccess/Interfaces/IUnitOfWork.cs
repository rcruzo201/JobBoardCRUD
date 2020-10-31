using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IJobRepository Jobs { get; }
        int Complete();
    }
}
