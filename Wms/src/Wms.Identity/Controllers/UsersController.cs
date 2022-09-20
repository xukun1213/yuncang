

namespace Huayu.Wms.Identity.Controllers;

[Route("api/identity/user")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userService.GetAsync(id);
        return Ok(user);
    }


    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet("roles/{id}")]
    public async Task<IActionResult> GetRolesAsync(string id)
    {
        var userRoles = await _userService.GetRolesAsync(id);
        return Ok(userRoles);
    }


    [Authorize(Policy = Permissions.Users.Edit)]
    [HttpPut("roles/{id}")]
    public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        return Ok(await _userService.UpadateRolesAsync(request));
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _userService.RegisterAsync(request, origin));
    }

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return Ok(await _userService.ConfirmEmailAsync(userId, code));
    }

    [HttpPost("toggle-status")]
    public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        return Ok(await _userService.ToggleUserStatusAsync(request));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _userService.ForgotPasswordAsync(request, origin));
    }


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return Ok(await _userService.ResetPasswordAsync(request));
    }

    [Authorize(Policy = Permissions.Users.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "")
    {
        var data = await _userService.ExportToExcelAsync(searchString);
        return Ok(data);
    }
}


