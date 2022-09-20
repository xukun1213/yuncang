namespace Huayu.Wms.Identity.Application.Interfaces.Services;

public interface ITokenService : IService
{
    Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

    Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
}
