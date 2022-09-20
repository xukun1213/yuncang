
using Wms.ApplicationCore.Interfaces;

namespace Wms.ApplicationCore.Servcies
{
    public class UserService : IUserService
    {
        public Task<IList<string>> GetRoleNamesAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
