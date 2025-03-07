using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Restaurant;

public enum Cuisine
{
    Italian,
    Chinese,
    Mexican
}

[Index(nameof(Name), IsUnique = true)]
public class Restaurant
{
    public string Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Location { get; set; }

    [Required]
    [EnumDataType(typeof(Cuisine))]
    public Cuisine Cuisine { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    [Range(0, 5)]
    public int Rating { get; set; }

    public Restaurant(string name, string location, Cuisine cuisine, string description)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Location = location;
        Cuisine = cuisine;
        Description = description;
    }
}