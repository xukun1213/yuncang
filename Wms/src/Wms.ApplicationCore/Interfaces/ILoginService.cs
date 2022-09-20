namespace Wms.ApplicationCore.Interfaces
{
    public interface ILoginService<T>
    {
        bool CheckPermission(int userId, string permissionName);

        Task<bool> ValidateCredentials(T user, string password);

        Task<T> FindByUserId(string userId);

        Task SignIn(T user);

        Task SignOut(T user);
    }
}
