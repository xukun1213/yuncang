
#nullable disable

namespace Huayu.Oms.Domain.AgregatesModel.TenantAggregate
{
    public class Tenant : AuditableEntity
    {
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public string TaxpayerId { get; private set; }
        public Address Address { get; private set; }
        public string LogoUrl { get; private set; }
        public string Industry { get; private set; }
        public TenantStatus TenantStatus { get; private set; }
        private int _tenantStatusId;

        public Tenant()
        {

        }
    }
}
