using System;
using LocalBites.Models;

namespace LocalBites.Interfaces.Repositories;

public interface IRestaurantRepository
{
    public Task<List<Restaurant>> GetAll();
    public Task<Restaurant?> GetById(string id);
    public Task<Restaurant> Add(Restaurant restaurant);
    public Task<Restaurant> Update(string id, Restaurant restaurant);
    public Task Delete(string id);
    public Task<List<Restaurant>> FilterByPreferences(Cuisine cuisine, Location location);
}
