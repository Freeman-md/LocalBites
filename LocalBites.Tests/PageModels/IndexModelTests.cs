using System;
using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
using LocalBites.Models.Enums;
using LocalBites.Pages;
using LocalBites.Repositories;
using LocalBites.Tests.Builders;
using Microsoft.Extensions.Logging;
using Moq;

namespace LocalBites.Tests.PageModels;

public class IndexModelTests
{
    private readonly Mock<ILogger<IndexModel>> _logger;
    private readonly Mock<IRestaurantRepository> _restaurantRepository;
    private readonly IndexModel _indexModel;

    public IndexModelTests()
    {
        _logger = new Mock<ILogger<IndexModel>>();
        _restaurantRepository = new Mock<IRestaurantRepository>();

        _indexModel = new IndexModel(_logger.Object, _restaurantRepository.Object);
    }

    [Fact]
    public async Task OnGetAsync_PopulatesPageModel_WithAListOfRestaurants()
    {
        #region Arrange
        var restaurants = RestaurantBuilder.BuildMany(4).ToList();
        _restaurantRepository.Setup(repo => repo.GetAll()).ReturnsAsync(restaurants);
        #endregion

        #region Act
        await _indexModel.OnGet();
        #endregion

        #region Assert
        var retrievedRestaurants = Assert.IsAssignableFrom<List<Restaurant>>(_indexModel.Restaurants);
        Assert.Equal(
            restaurants.OrderBy(restaurant => restaurant.Id).Select(restaurant => restaurant.Name),
            retrievedRestaurants.OrderBy(restaurant => restaurant.Id).Select(restaurant => restaurant.Name)
        );
        #endregion
    }

    [Theory]
    [InlineData(Cuisine.Italian, Location.NewYork)]
    [InlineData(Cuisine.Mexican, Location.NewYork)]
    [InlineData(Cuisine.Chinese, Location.NewYork)]
    [InlineData(Cuisine.Italian, Location.London)]
    [InlineData(Cuisine.Mexican, Location.London)]
    [InlineData(Cuisine.Chinese, Location.London)]
    [InlineData(Cuisine.Italian, Location.Paris)]
    [InlineData(Cuisine.Mexican, Location.Paris)]
    [InlineData(Cuisine.Chinese, Location.Paris)]
    [InlineData(Cuisine.Italian, Location.Rome)]
    [InlineData(Cuisine.Mexican, Location.Rome)]
    [InlineData(Cuisine.Chinese, Location.Rome)]
    public async Task OnGetFilterByPreferences_PopulatesPageModel_WithFilteredRestaurants(Cuisine cuisine, Location location)
    {
        #region Arrange
        var restaurants = new List<Restaurant>{
            new RestaurantBuilder().WithCuisine(Cuisine.Italian).WithLocation(Location.NewYork).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Chinese).WithLocation(Location.London).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Mexican).WithLocation(Location.Paris).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Italian).WithLocation(Location.Rome).Build(),
        };

        _restaurantRepository.Setup(repo => repo.FilterByPreferences(cuisine, location)).ReturnsAsync(
            restaurants
                .AsQueryable()
                .Where(restaurant => restaurant.Cuisine == cuisine)
                .Where(restaurant => restaurant.Location == location)
                .ToList()
            );

        _indexModel.Filter = new FilterCriteria { Cuisine = cuisine, Location = location };
        #endregion

        #region Act
        await _indexModel.OnGet();
        #endregion

        #region Assert
        Assert.NotNull(_indexModel.Restaurants);
        Assert.True(_indexModel.ModelState.IsValid);

        Assert.All(_indexModel.Restaurants, (restaurant) =>
        {
            Assert.Equal(cuisine, restaurant.Cuisine);
            Assert.Equal(location, restaurant.Location);
        });
        #endregion
    }

    [Fact]
    public async Task OnGetFilterByPreferences_ShouldReturnEmptyList_WhenNoMatchesFound()
    {
        #region Arrange
        _restaurantRepository
            .Setup(repo => repo.FilterByPreferences(It.IsAny<Cuisine>(), It.IsAny<Location>()))
            .ReturnsAsync(new List<Restaurant>());

        _indexModel.Filter = new FilterCriteria { Cuisine = Cuisine.Italian, Location = Location.NewYork };
        #endregion

        #region Act
        await _indexModel.OnGet();
        #endregion

        #region Assert
        Assert.NotNull(_indexModel.Restaurants);
        Assert.Empty(_indexModel.Restaurants);
        Assert.True(_indexModel.ModelState.IsValid);
        #endregion
    }

    [Theory]
    [InlineData(Cuisine.Italian, (Location)(-1))]
    [InlineData(Cuisine.Chinese, (Location)(-1))]
    [InlineData(Cuisine.Mexican, (Location)(-1))]
    [InlineData((Cuisine)(-1), Location.NewYork)]
    [InlineData((Cuisine)(-1), Location.London)]
    [InlineData((Cuisine)(-1), Location.Paris)]
    [InlineData((Cuisine)(-1), Location.Rome)]
    public async Task OnGetFilterByPreferences_ShouldHandleInvalidInputs_Gracefully(Cuisine cuisine, Location location)
    {
        #region Arrange
        _indexModel.Filter = new FilterCriteria { Cuisine = cuisine, Location = location };
        #endregion

        #region Act
        await _indexModel.OnGet();
        #endregion

        #region Assert
        Assert.False(_indexModel.ModelState.IsValid);
        #endregion
    }

}
