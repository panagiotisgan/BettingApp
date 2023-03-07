using Betting.DataAccess.Configurations;
using Betting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Betting.DataAccess
{
    public class BettingApiDbContext : DbContext
    {
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<MatchOdds> MatchesOdds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Match>(new MatchConfiguration());
            modelBuilder.ApplyConfiguration<MatchOdds>(new MatchOddsConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("AppConnString");
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Betting.DataAccess"));
            }
        }
    }
}
