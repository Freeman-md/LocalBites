using Microsoft.EntityFrameworkCore;
using LocalBites.Models;

namespace LocalBites.Data;

public class LocalBitesContext : DbContext
{
    public LocalBitesContext(DbContextOptions<LocalBitesContext> options) : base(options)
    {

    }

    public DbSet<Restaurant> Restaurants { get; set; }
}