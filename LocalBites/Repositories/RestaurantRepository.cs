using System;
using LocalBites.Data;
using LocalBites.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using LocalBites.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LocalBites.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly LocalBitesContext _dbContext;

    public RestaurantRepository(LocalBitesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Restaurant> Add(Restaurant restaurant)
    {
        _dbContext.Add(restaurant);
        await _dbContext.SaveChangesAsync();
        return restaurant;
    }

    public async Task<List<Restaurant>> AddMany(List<Restaurant> restaurants)
    {
        if (restaurants == null || restaurants.Count == 0)
            return new List<Restaurant>();

        await _dbContext.Restaurants.AddRangeAsync(restaurants);
        await _dbContext.SaveChangesAsync();

        return restaurants;
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

    public async Task<Restaurant> Update(string id, Restaurant restaurant)
    {
        var existingRestaurant = await _dbContext.Restaurants.FindAsync(id);
        if (existingRestaurant == null)
            throw new KeyNotFoundException($"Restaurant with ID '{id}' not found");

        _dbContext.Entry(existingRestaurant).CurrentValues.SetValues(restaurant);

        await _dbContext.SaveChangesAsync();

        return existingRestaurant;
    }

    public async Task<bool> ExistsById(string id)
    {
        return await _dbContext.Restaurants.AnyAsync(r => r.Id == id);
    }
}
