using LocalBites.Models;

namespace LocalBites.Data;

public static class SeedData {
    public static void Initialize(LocalBitesContext context) {
        if (context.Restaurants.Any()) {
            return; // already seeded restaurants
        }

        var restaurants = new List<Restaurant>{
            new("Bella Italia", Location.London, Cuisine.Italian, "Authentic Italian flavors") { Rating = 4 },
            new("Dragon Palace", Location.NewYork, Cuisine.Chinese, "Traditional dim sum & noodles") { Rating = 5 },
            new("Taco Fiesta", Location.Paris, Cuisine.Mexican, "Mexican street food vibes") { Rating = 3 },
            new("Mama Roma", Location.Rome, Cuisine.Italian, "Rustic Roman dishes") { Rating = 5 },
            new("Chopstick House", Location.London, Cuisine.Chinese, "Hot pot and stir fry") { Rating = 4 },
            new("Burrito Bros", Location.NewYork, Cuisine.Mexican, "Spicy burritos & nachos") { Rating = 2 },
            new("La Piazza", Location.Rome, Cuisine.Italian, "Cozy courtyard dining") { Rating = 5 },
            new("Golden Wok", Location.Paris, Cuisine.Chinese, "Fusion-style cuisine") { Rating = 3 },
            new("Casa Mexicana", Location.London, Cuisine.Mexican, "Margaritas & tacos") { Rating = 4 },
            new("Trattoria Milano", Location.NewYork, Cuisine.Italian, "Northern Italian comfort food") { Rating = 5 },
        };

        context.Restaurants.AddRange(restaurants);
        context.SaveChanges();
    }
}