namespace Application.Owners.DTOs;
public class OwnerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public Guid LastModifiedBy { get; set; }
}

