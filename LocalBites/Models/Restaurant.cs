using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.Restaurant;

public enum Cuisine
{
    Italian,
    Chinese,
    Mexican
}

public enum Location 
{
    NewYork,
    London,
    Rome,
    Paris
}

[Index(nameof(Name), IsUnique = true)]
public class Restaurant
{
    public string Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(Location))]
    public Location Location { get; set; }

    [Required]
    [EnumDataType(typeof(Cuisine))]
    public Cuisine Cuisine { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    [Range(0, 5)]
    public int Rating { get; set; }

    public Restaurant(string name, Location location, Cuisine cuisine, string description)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Location = location;
        Cuisine = cuisine;
        Description = description;
    }
}