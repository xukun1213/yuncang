

namespace Huayu.Oms.Domain.Contracts
{
    public class AuditableEntity//<long> : IEntity<long>
    {
        int? _requestedHashCode;
        List<INotification>? _domainEvents;

        public int Id { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? LastModified { get; private set; }
        public DateTime LastModifiedOn { get; private set; }

        public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();


        public void AddDomain(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }


        public void RemoveDomainEvent(INotification eventItem) => _domainEvents?.Remove(eventItem);

        public void ClearDomainEvents() => _domainEvents?.Clear();

        public bool IsTransient() => this.Id == default;

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not AuditableEntity) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (this.GetType() != obj.GetType()) return false;

            AuditableEntity item = (AuditableEntity)obj;

            if (item.IsTransient() || this.IsTransient()) return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (this.IsTransient()) return base.GetHashCode();

            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31;


            return _requestedHashCode.Value;
        }


        public static bool operator ==(AuditableEntity left, AuditableEntity right)
        {
            if (Object.Equals(left, null)) return Object.Equals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(AuditableEntity left, AuditableEntity right) => !(left == right);
    }
}
