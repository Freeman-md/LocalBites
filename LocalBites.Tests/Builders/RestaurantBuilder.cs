using System;
using System.Collections.Generic;
using LocalBites.Models;

namespace LocalBites.Tests.Builders;

public class RestaurantBuilder
{
    private Restaurant _restaurant;

    public RestaurantBuilder()
    {
        _restaurant = new Restaurant(
            name: "CocoCure",
            location: Location.NewYork,
            cuisine: Cuisine.Italian,
            description: "Best of the best"
        );
    }

    public RestaurantBuilder WithName(string name)
    {
        _restaurant.Name = name;
        return this;
    }

    public RestaurantBuilder WithLocation(Location location)
    {
        _restaurant.Location = location;
        return this;
    }

    public RestaurantBuilder WithCuisine(Cuisine cuisine)
    {
        _restaurant.Cuisine = cuisine;
        return this;
    }

    public RestaurantBuilder WithDescription(string description)
    {
        _restaurant.Description = description;
        return this;
    }

    public RestaurantBuilder WithRating(int rating)
    {
        _restaurant.Rating = rating;
        return this;
    }

    public Restaurant Build()
    {
        return new Restaurant(
            name: _restaurant.Name,
            location: _restaurant.Location,
            cuisine: _restaurant.Cuisine,
            description: _restaurant.Description
        )
        {
            ImageUrl = _restaurant.ImageUrl,
            Rating = _restaurant.Rating
        };
    }

    public static IEnumerable<Restaurant> BuildMany(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new RestaurantBuilder()
                .WithName($"{Guid.NewGuid()} {i}")
                .WithLocation(Location.NewYork)
                .WithCuisine(Cuisine.Chinese)
                .WithDescription(Guid.NewGuid().ToString())
                .Build();
        }
    }
}
