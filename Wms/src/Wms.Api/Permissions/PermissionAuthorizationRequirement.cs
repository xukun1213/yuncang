using System.Security.Claims;

using Wms.ApplicationCore.Entities.UserAggregate;
using Wms.ApplicationCore.Interfaces;

namespace Wms.Api.Permissions
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string name)
        {
            Name = name;
        }


        public string Name { get; set; }
    }


    public class PermissionAuthorizationRequirementHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly ILoginService<User> _loginService;
        public PermissionAuthorizationRequirementHandler(ILoginService<User> loginService)
        {
            _loginService = loginService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        //todo  : 判断当前用户是否有权限
                        if (_loginService.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
