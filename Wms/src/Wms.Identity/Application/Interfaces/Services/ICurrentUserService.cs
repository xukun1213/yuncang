namespace Huayu.Wms.Identity.Application.Interfaces.Services;

public interface ICurrentUserService : IService
{
    string? UserId { get; }
}
