using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingLogger.Infrastructure.Strava.Models;

namespace TrainingLogger.Infrastructure.EF.Configurations;

internal class ApiAccessTokenConfiguration : IEntityTypeConfiguration<ApiAccessToken>
{
    public void Configure(EntityTypeBuilder<ApiAccessToken> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
