using Wms.ApplicationCore.SeedWork;

namespace Wms.ApplicationCore.Entities.UserAggregate
{
    public class User : BaseEntity
    {
        public string? Name { get; private set; }
    }
}
