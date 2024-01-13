﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Base
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        bool Commit();
        void Rollback();

    }
}
