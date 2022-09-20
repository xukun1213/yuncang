using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huayu.Oms.Domain.AgregatesModel.TenantAggregate
{
    public class TenantStatus : Enumeration
    {
        public static TenantStatus Initial = new TenantStatus(1, nameof(Initial).ToLowerInvariant());
        public static TenantStatus Active = new TenantStatus(1, nameof(Active).ToLowerInvariant());
        public TenantStatus(int id, string name) : base(id, name)
        {

        }


        public static IEnumerable<TenantStatus> List() => new[] { Initial, Active };

        public TenantStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new TenantDomianException($"TenantStatus 可选的值：{string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TenantStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);


            if (state == null)
            {
                throw new TenantDomianException($"TenantStatus 可选的值：{string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
