namespace Huayu.Wms.Identity.Infrastructure.Services;

public class IdentityService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppConfiguration _appConfig;


    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        AppConfiguration appConfig)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appConfig = appConfig;
    }

    public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return await Result<TokenResponse>.FailAsync("未找到此用户.");
        }
        if (!user.IsActive)
        {
            return await Result<TokenResponse>.FailAsync("用户未激活，请联系管理员.");
        }
        if (!user.EmailConfirmed)
        {
            return await Result<TokenResponse>.FailAsync("电子邮箱未验证.");
        }
        var passvalid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passvalid)
        {
            return await Result<TokenResponse>.FailAsync("无效的凭证.");
        }

        user.RefreshToken = 
        throw new NotImplementedException();
    }


    public Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
    {
        ApplicationUser user = new ApplicationUser();


        throw new NotImplementedException();
    }

    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        var token = GetClaimsIdentityFromExpiredTokenAsync(user.RefreshToken);
    }


    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token=new JsonWebToken
        {
            Claims=claims,
        }
    }

    private async Task<ClaimsIdentity> GetClaimsIdentityFromExpiredTokenAsync(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        var tokenHandler = new JsonWebTokenHandler();
       
        var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        if (tokenValidationResult.SecurityToken is not JsonWebToken jsonWebToken || !jsonWebToken.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("无效的token");
        }
        return tokenValidationResult.ClaimsIdentity;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
