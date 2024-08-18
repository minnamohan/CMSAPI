using System.ComponentModel.DataAnnotations;

namespace CMSAPI.Models.DTOs
{
    public record CustomerDto(
        Guid? CustomerId,
        [Required]
        [StringLength(50)]
        string? FirstName,

        [Required]
        [StringLength(50)]
        string? LastName,

        [EmailAddress]
        string? Email,

        [Phone]
        string? PhoneNumber,

        [StringLength(100)]
        string? Address);

}
