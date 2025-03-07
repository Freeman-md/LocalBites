using System;
using System.Collections.Generic;
using Models.Restaurant;

namespace LocalBites.Tests.Builders;

public class RestaurantBuilder
{
    private Restaurant _restaurant;

    public RestaurantBuilder()
    {
        _restaurant = new Restaurant(
            name: "CocoCure",
            location: "Bromwich",
            cuisine: Cuisine.Italian,
            description: "Best of the best"
        );
    }

    public RestaurantBuilder WithName(string name)
    {
        _restaurant.Name = name;
        return this;
    }

    public RestaurantBuilder WithLocation(string location)
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

    public IEnumerable<Restaurant> BuildMany(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new RestaurantBuilder()
                .WithName($"{_restaurant.Name} {i}")
                .WithLocation(_restaurant.Location)
                .WithCuisine(_restaurant.Cuisine)
                .WithDescription(_restaurant.Description)
                .Build();
        }
    }
}
