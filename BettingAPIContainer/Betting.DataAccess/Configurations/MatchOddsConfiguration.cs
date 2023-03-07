using Betting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betting.DataAccess
{
    public class MatchOddsConfiguration : IEntityTypeConfiguration<MatchOdds>
    {
        public void Configure(EntityTypeBuilder<MatchOdds> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne<Match>()
                .WithMany(x => x.MatchOdds)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.Odd)
                .HasPrecision(7, 3)
                .IsRequired();
            builder.Property(p => p.Specifier)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}
