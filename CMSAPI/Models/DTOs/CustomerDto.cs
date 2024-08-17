namespace CMSAPI.Models.DTOs
{
    public record CustomerDto(
        Guid CustomerId,
        string FirstName,
        string LastName,
        string? Email,
        string? PhoneNumber,
        string? Address);
   
}
