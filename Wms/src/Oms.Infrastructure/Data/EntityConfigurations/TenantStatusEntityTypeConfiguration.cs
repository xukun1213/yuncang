namespace Huayu.Oms.Infrastructure.Data.EntityConfigurations;

internal class TenantStatusEntityTypeConfiguration : IEntityTypeConfiguration<TenantStatus>
{
    public void Configure(EntityTypeBuilder<TenantStatus> builder)
    {
        builder.ToTable("tenantstatus", OmsContext.DEFAULT_SCHEMA);

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
