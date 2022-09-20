namespace Huayu.Oms.Infrastructure.Data.EntityConfigurations;
internal class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants", OmsContext.DEFAULT_SCHEMA);

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .UseMySqlIdentityColumn();

        builder.Property(t => t.Identifier)
            .HasMaxLength(30)
            .IsRequired(true);

        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(t => t.Abbreviation)
            .HasMaxLength(10)
            .IsRequired(true);

        builder.Property(t => t.TaxpayerId)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.OwnsOne(t => t.Address)
            .Property(t => t.Province)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(t => t.Address)
            .Property(t => t.City)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(t => t.Address)
            .Property(t => t.District)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(t => t.Address)
            .Property(t => t.Street)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(t => t.Address)
            .Property(t => t.ZipCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.LogoUrl)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.Industry)
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property<int>("_tenantStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("TenantStatusId")
            .IsRequired();

        builder.Ignore(t => t.DomainEvents);
        builder.Ignore(t => t.TenantStatus);

    }
}


