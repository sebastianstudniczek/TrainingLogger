using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingLogger.Core.Models;

namespace TrainingLogger.Infrastructure.EF.Configurations;

internal class RefreshTokenConfigurations : IEntityTypeConfiguration<ApiAccessToken>
{
    public void Configure(EntityTypeBuilder<ApiAccessToken> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
