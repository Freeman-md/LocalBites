namespace Models.Restaurant;

public class Restaurant {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string Cuisine { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public double Rating { get; set; }

    public Restaurant(string name, string location, string cuisine, string description) {
        Name = name;
        Location = location;
        Cuisine = cuisine;
        Description = description;
    }
}