namespace Huayu.Wms.Identity.Application.Responses.Identity;

public class UserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfilePictureDataUrl { get; set; }
}
