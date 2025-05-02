using System;
using LocalBites.Data;
using LocalBites.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using LocalBites.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LocalBites.Models.Enums;

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
        _dbContext.Restaurants.Add(restaurant);
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


    public async Task Delete(string id)
    {
        var existingRestaurant = await GetById(id);
        if (existingRestaurant == null)
            throw new KeyNotFoundException($"Restaurant with ID '{id}' not found");

        _dbContext.Restaurants.Remove(existingRestaurant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Restaurant>> FilterByPreferences(Cuisine? cuisine = null, Location? location = null)
    {
        IQueryable<Restaurant> query = _dbContext.Restaurants.AsNoTracking();

        if (cuisine.HasValue) query = query.Where(restaurant => restaurant.Cuisine == cuisine.Value);

        if (location.HasValue) query = query.Where(restaurant => restaurant.Location == location.Value);

        return await query.ToListAsync();
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
        var existingRestaurant = await GetById(id);
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
