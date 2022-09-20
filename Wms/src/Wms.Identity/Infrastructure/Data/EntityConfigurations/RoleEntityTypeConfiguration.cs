namespace Huayu.Wms.Identity.Infrastructure.Data.EntityConfigurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles", "Identity");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

    }
}
