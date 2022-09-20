
using Wms.ApplicationCore.Entities.UserAggregate;

namespace Wms.ApplicationCore.Interfaces
{
    public interface ITokenClaimsService
    {
        public Task<string> GetTokenAsync(User user);
    }
}
