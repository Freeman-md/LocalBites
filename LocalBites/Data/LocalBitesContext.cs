using LocalBites.Models;
using Microsoft.EntityFrameworkCore;
using Models.Restaurant;

namespace LocalBites.Data;

public class LocalBitesContext : DbContext {
    public LocalBitesContext(DbContextOptions<LocalBitesContext> options) : base(options) {

    }

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
}