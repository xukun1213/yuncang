namespace Huayu.Wms.Identity.Infrastructure.Data.EntityConfigurations;

public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("UserClaims", "Identity");
    }
}
