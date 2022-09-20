
using Wms.ApplicationCore.Entities.UserAggregate;
using Wms.ApplicationCore.Interfaces;

namespace Wms.ApplicationCore.Servcies
{
    public class LoginService : ILoginService<User>
    {
        public bool CheckPermission(int userId, string permissionName)
        {
            // user-permission
            // user-role-permission

            throw new NotImplementedException();
        }

        public Task<User> FindByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SignIn(User user)
        {
            throw new NotImplementedException();
        }

        public Task SignOut(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCredentials(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
