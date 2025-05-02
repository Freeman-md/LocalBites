using System.ComponentModel.DataAnnotations;
using LocalBites.Models.Enums;

namespace LocalBites.Models.DTOs;

public class CreateRestaurantDto {
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(Location))]
    public Location Location { get; set;}

    [Required]
    [EnumDataType(typeof(Cuisine))]
    public Cuisine Cuisine { get; set;}

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    public IFormFile Image { get; set; }
}