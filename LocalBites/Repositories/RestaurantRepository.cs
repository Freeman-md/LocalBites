using System;
using LocalBites.Data;
using LocalBites.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using LocalBites.Models;

namespace LocalBites.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly LocalBitesContext _dbContext;

    public RestaurantRepository(LocalBitesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Restaurant> Add(Restaurant restaurant)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Restaurant>> FilterByPreferences(Cuisine cuisine, Location location)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Restaurant>> GetAll()
    {
        return await _dbContext.Restaurants.AsNoTracking().ToListAsync();
    }

    public async Task<Restaurant?> GetById(string id)
    {
        return await _dbContext.Restaurants.FindAsync(id);
    }

    public Task<Restaurant> Update(string id, Restaurant restaurant)
    {
        throw new NotImplementedException();
    }
}
