using Huayu.Wms.Identity.Infrastructure.Data.Contexts;

namespace Huayu.Wms.Identity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IExcelService _excelService;
    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ICurrentUserService currentUserService,
        IExcelService excelService)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
        var user = await _userManager.Users.ToListAsync();

        return await Result<List<UserResponse>>.SuccessAsync();
    }

    public async Task<Shared.Wrapper.IResult> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            return await Result.FailAsync(String.Format("用户名 {0} 已被占用。", request.UserName));
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.ActivateUser,
            EmailConfirmed = request.AutoConfirmEmail
        };

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return await Result.FailAsync(string.Format("手机号码 {0} 已经被注册", request.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.BasicRole);
                if (!request.AutoConfirmEmail)
                {
                    var verificationUri = await SendVerificationEmail(user, origin);
                    var mailRequest = new MailRequest
                    {
                        From = "xukun@ztt.com",
                        To = user.Email,
                        Body = String.Format("请确认你的账户通过 <a href='{0}'> 点击这里 </a>。", verificationUri),
                        Subject = "注册确认"
                    };

                    //todo: 发送邮件 _mailService.SendAsync(mailRequest);

                    return await Result<string>.SuccessAsync(user.Id, string.Format("用户 {0} 已注册。请检查你的邮箱确认！", user.UserName));
                }
                return await Result<string>.SuccessAsync(user.Id, string.Format("用户 {0} 已注册。", user.UserName));
            }
            else
            {
                return await Result.FailAsync(result.Errors.Select(a => a.Description).ToList());
            }
        }
        else
        {
            return await Result.FailAsync(string.Format("此邮箱 {0} 已经被注册！", request.Email));
        }
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/identity/user/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

        return await Result<UserResponse>.SuccessAsync(new UserResponse());
    }

    public async Task<Shared.Wrapper.IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
        var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
        if (isAdmin)
        {
            return await Result.FailAsync("管理员信息状态无法切换");
        }
        if (user != null)
        {
            user.IsActive = request.ActivateUser;
            var identityRequest = await _userManager.UpdateAsync(user);
        }
        return await Result.SuccessAsync();
    }


    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
    {
        var userRoles = new List<UserRoleModel>();
        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _roleManager.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var userRole = new UserRoleModel
            {
                RoleName = role.Name,
                RoleDescription = role.Description
            };

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRole.Selected = true;
            }
            else
            {
                userRole.Selected = false;
            }

            userRoles.Add(userRole);
        }
        var result = new UserRolesResponse { UserRoles = userRoles };
        return await Result<UserRolesResponse>.SuccessAsync(result);
    }


    public async Task<Shared.Wrapper.IResult> UpadateRolesAsync(UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if ("xukun@ztt.com".Equals(user.Email))
        {
            return await Result.FailAsync("保留邮箱，无法修改");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
        {
            var tryToAddAdministratorRole = selectedRoles
                .Any(x => x.RoleName == RoleConstants.AdministratorRole);

            var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);

            if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
            {
                return await Result.FailAsync("不允许添加或删除管理员角色如果你没有这个角色");
            }
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
        return await Result.SuccessAsync("角色更新");
    }

    public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return await Result<string>.SuccessAsync(user.Id, string.Format("账户已确认，邮箱 {0}。你现在可以通过 /api/identity/token endpoint 来创建 JWT。", user.Email));
        }
        else
        {
            throw new ApiException(string.Format("确认邮件 {0} 时出现错误"), user.Email);
        }
    }


    public async Task<Shared.Wrapper.IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return await Result.FailAsync("出现错误");
        }

        //For more information on how to enable account confirmation and password reset
        //please visir https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body = String.Format("<a href='{0}'>点击这里</a> 重置你的密码。", HtmlEncoder.Default.Encode(passwordResetURL)),
            Subject = "重置密码",
            To = request.Email
        };

        //todo: _mailService.SendAsync(mailRequest);
        return await Result.SuccessAsync("密码重置邮件已经发送到你的认证邮箱。");
    }

    public async Task<Shared.Wrapper.IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return await Result.FailAsync("出现错误");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded)
        {
            return await Result.SuccessAsync("密码重置成功!");
        }
        else
        {
            return await Result.FailAsync("出现错误！");
        }
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _userManager.Users.CountAsync();
        return count;
    }

    public async Task<string> ExportToExcelAsync(string searchString = "")
    {
        var userSpec = new UserFilterSpecification(searchString);
        var users = await _userManager.Users
            .Specify(userSpec)
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();

        var result = await _excelService.ExportAsync(users, sheetName: "用户集合",
            mappers: new Dictionary<string, Func<ApplicationUser, object>>
            {
                {"Id",item=>item.Id },
                {"FirstName",item=>item.FirstName },
                {"LastName",item=>item.LastName },
                {"UserName",item=>item.UserName },
                {"Email",item=>item.Email },
                {"EmailConfirmed",item=>item.EmailConfirmed },
                {"PhoneNumber",item=>item.PhoneNumber },
                {"PhoneNumberConfirmed",item=>item.PhoneNumberConfirmed },
                {"IsActive",item=>item.IsActive },
                {"CreatedOn(local)",item=>DateTime.SpecifyKind( item.CreatedOn,DateTimeKind.Utc).ToLocalTime().ToString("G",CultureInfo.CurrentCulture) },
                {"CreatedOn(UTC)",item=>item.CreatedOn.ToString("G",CultureInfo.CurrentCulture) }
            });

        return result;
    }









}
