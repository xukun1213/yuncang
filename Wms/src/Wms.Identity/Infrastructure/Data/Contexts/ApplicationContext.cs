namespace Huayu.Wms.Identity.Infrastructure.Data.Contexts;

public class ApplicationContext : AuditableContext
{
    private readonly ICurrentUserService _currentUserService;
    public ApplicationContext(DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.UserId ?? "";
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId ?? "";
                    break;
            }
        }

        if (_currentUserService.UserId == null)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        else
        {
            return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
        }
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("DECIMAL(18,2)");
        }


        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
        {
            property.SetColumnType("NVARCHAR(60)");
        }

        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleClaimEntityTypeConfiguration());
        builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserClaimEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserLoginEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserTokenEntityTypeConfiguration());
    }
}
