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

    public Task Delete(int id)
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

    public Task<Restaurant> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Restaurant> Update(int id, Restaurant restaurant)
    {
        throw new NotImplementedException();
    }
}
