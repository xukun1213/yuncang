namespace Huayu.Wms.Identity.Infrastructure.Data.EntityConfigurations;

public class RoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.ToTable("RoleClaims", "Identity");

        builder.HasOne(c => c.Role)
            .WithMany(r => r.RoleClaims)
            .HasForeignKey(c => c.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
