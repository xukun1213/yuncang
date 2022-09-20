namespace Huayu.Oms.Domain.Contracts
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }
}
