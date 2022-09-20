
/*
 * dotnet ef migrations add InitialCreate --output-dir Data/Migrations -s ..\Oms.Web\Oms.Web.csproj
 * dotnet ef database update
 * dotnet ef migrations list
 * dotnet ef migrations remove
 */

#nullable disable

namespace Huayu.Oms.Infrastructure.Data;

public class OmsContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "oms";
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantStatus> TenantStatus { get; set; }

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;



    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;



    public OmsContext(DbContextOptions<OmsContext> options) : base(options) { }
    public OmsContext(DbContextOptions<OmsContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        System.Diagnostics.Debug.WriteLineIf(true, "OmsContext::ctor ->" + this.GetHashCode());
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TenantStatusEntityTypeConfiguration());
    }


    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {

        await _mediator.DispatchDomainEventsAsync(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;
        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }


    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");


        try
        {
            await base.SaveChangesAsync();

            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}