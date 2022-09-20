namespace Huayu.Wms.Identity.Application.Requests.Identity;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public string UserId { get; set; }
}
