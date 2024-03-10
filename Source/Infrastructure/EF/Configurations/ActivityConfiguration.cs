using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Infrastructure.EF.Configurations;

internal class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.Property(x => x.StartDateLocal)
            .HasColumnType("timestamp without time zone");
    }
}
