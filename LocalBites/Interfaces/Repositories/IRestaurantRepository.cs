using System;
using LocalBites.Models;

namespace LocalBites.Interfaces.Repositories;

public interface IRestaurantRepository
{
    public Task<List<Restaurant>> GetAll();
    public Task<Restaurant> GetById(int id);
    public Task<Restaurant> Add(Restaurant restaurant);
    public Task<Restaurant> Update(int id, Restaurant restaurant);
    public Task Delete(int id);
    public Task<List<Restaurant>> FilterByPreferences(Cuisine cuisine, Location location);
}
