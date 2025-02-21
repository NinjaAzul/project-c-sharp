namespace Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

public class UserBasicInfoResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}