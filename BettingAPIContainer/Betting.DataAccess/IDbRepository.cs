using Betting.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Betting.DataAccess
{
    public interface IDbRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
    }
}
