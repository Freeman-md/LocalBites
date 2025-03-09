using System;
using LocalBites.Models;

namespace LocalBites.Interfaces.Repositories;

public interface IRestaurantRepository
{
    public Task<List<Restaurant>> GetAll();
    public Task<Restaurant?> GetById(string id);
    public Task<Restaurant> Add(Restaurant restaurant);
    public Task<List<Restaurant>> AddMany(List<Restaurant> restaurants);

    public Task<Restaurant> Update(string id, Restaurant restaurant);
    public Task<bool> ExistsById(string id);

    public Task Delete(string id);
    public Task<List<Restaurant>> FilterByPreferences(Cuisine cuisine, Location location);
}
