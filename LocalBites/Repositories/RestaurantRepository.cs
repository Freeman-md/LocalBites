using System;
using LocalBites.Interfaces.Repositories;
using Models.Restaurant;

namespace LocalBites.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
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

    public Task<List<Restaurant>> GetAll()
    {
        throw new NotImplementedException();
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
