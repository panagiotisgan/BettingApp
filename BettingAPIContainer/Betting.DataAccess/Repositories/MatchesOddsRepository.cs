using Betting.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Betting.DataAccess.Repositories
{
    public class MatchesOddsRepository : GenericRepository<MatchOdds, BettingApiDbContext>, IMatchOddsRepository
    {
        public MatchesOddsRepository(BettingApiDbContext dbContext) : base(dbContext)
        {

        }
    }
}
