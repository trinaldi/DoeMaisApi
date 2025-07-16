namespace DoeMais.DTO.User;

public class UpdateUserDto
{
    public string? AvatarUrl { get; set; }
    public string? Name { get; set; } = "";
    public string? Phone { get; set; } = "";
    public string? Address { get; set; } = "";
    public string? Complement { get; set; }
    public string? Neighborhood { get; set; } = "";
    public string? City { get; set; } = "";
    public string? State { get; set; } = "";
    public string? ZipCode { get; set; } = "";
}