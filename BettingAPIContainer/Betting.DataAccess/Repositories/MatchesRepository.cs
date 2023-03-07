using Betting.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Betting.DataAccess.Repositories
{
    public class MatchesRepository : GenericRepository<Match, BettingApiDbContext>, IMatchRepository
    {
        public MatchesRepository(BettingApiDbContext dbContext) : base(dbContext)
        {

        }
    }
}
