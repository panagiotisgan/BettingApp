using Betting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Betting.DataAccess.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.MatchDate)
                .HasColumnType("date")
                .IsRequired();
            builder.Property(p => p.MatchTime)
               .HasColumnType("time")
               .IsRequired()
               .HasConversion(v => v.TimeOfDay, v => DateTime.Now.Date.Add(v));
            builder.Property(p => p.Sport)
                .IsRequired();
            builder.Property(p => p.TeamA)
                .HasMaxLength(36)
                .IsRequired();
            builder.Property(p => p.TeamB)
                .HasMaxLength(36)
                .IsRequired();
            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(70);

        }
    }
}
