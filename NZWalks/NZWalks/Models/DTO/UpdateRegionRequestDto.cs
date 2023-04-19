using System.ComponentModel.DataAnnotations;
namespace NZWalks.Models.DTO;

public class UpdateRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code has to be min of 3 characters")]
    [MaxLength(3, ErrorMessage = "Code has to be max of 3 characters")]
    public string Code { get; set; }
    
    [Required]
    [MaxLength(100, ErrorMessage = "Code has to be max of 100 characters")]
    public string Name { get; set; }

    public string? RegionImageUrl  { get; set; }
}