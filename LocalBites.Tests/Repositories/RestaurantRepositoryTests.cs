using System;
using System.Threading.Tasks;
using LocalBites.Data;
using LocalBites.Interfaces.Repositories;
using LocalBites.Repositories;
using LocalBites.Tests.Builders;
using Microsoft.EntityFrameworkCore;
using LocalBites.Models;
using Moq;

namespace LocalBites.Tests.Repositories;

public class RestaurantRepositoryTests
{
    private readonly LocalBitesContext _dbContext;
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantRepositoryTests()
    {
        DbContextOptions<LocalBitesContext> options = new DbContextOptionsBuilder<LocalBitesContext>()
                                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                                .Options;

        _dbContext = new LocalBitesContext(options);

        _restaurantRepository = new RestaurantRepository(_dbContext);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllRestaurants_WhenDataExists()
    {
        #region Arrange
        const int NUMBER_OF_RESTAURANTS = 4;

        var restaurants = RestaurantBuilder.BuildMany(NUMBER_OF_RESTAURANTS).ToList();

        await _restaurantRepository.AddMany(restaurants);
        #endregion

        #region Act
        var result = await _restaurantRepository.GetAll();
        #endregion

        #region Assert
        Assert.NotNull(result);
        Assert.Equal(NUMBER_OF_RESTAURANTS, restaurants.Count);
        Assert.All(result, r => Assert.Contains(restaurants, seeded => seeded.Name == r.Name));

        var trackingStatus = _dbContext.Entry(result[0]).State;
        Assert.Equal(EntityState.Detached, trackingStatus);
        #endregion
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoRestaurantsExist()
    {
        #region Act
        var restaurants = await _restaurantRepository.GetAll();
        #endregion

        #region Assert
        Assert.NotNull(restaurants);
        Assert.Empty(restaurants);
        #endregion
    }

    [Fact]
    public async Task GetById_ShouldReturnRestaurant_WhenIdExists()
    {
        #region Arrange
        Restaurant restaurant = new RestaurantBuilder().Build();

        await _restaurantRepository.Add(restaurant);
        #endregion

        #region Act
        Restaurant? retrievedRestaurant = await _restaurantRepository.GetById(restaurant.Id);
        #endregion

        #region Assert
        Assert.NotNull(retrievedRestaurant);
        Assert.True(retrievedRestaurant.PropertiesAreEqual(restaurant));
        #endregion
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        #region Act
        Restaurant? retrievedRestaurant = await _restaurantRepository.GetById(Guid.NewGuid().ToString());
        #endregion

        #region Assert
        Assert.Null(retrievedRestaurant);
        #endregion
    }

    [Fact]
    public async Task Add_ShouldAddRestaurant_WhenValidDataProvided()
    {
        #region Arrange
        Restaurant unsavedRestaurant = new RestaurantBuilder().Build();
        #endregion

        #region Act
        Restaurant savedRestaurant = await _restaurantRepository.Add(unsavedRestaurant);
        Restaurant? retrievedRestaurant = await _restaurantRepository.GetById(savedRestaurant.Id);
        #endregion

        #region Assert
        Assert.NotNull(retrievedRestaurant);
        Assert.True(savedRestaurant.PropertiesAreEqual(retrievedRestaurant));
        #endregion
    }

    [Fact]
    public async Task Update_ShouldUpdateRestaurant_WhenValidIdProvided()
    {
        #region Arrange
        const string UPDATED_NAME = "Newly Updated Restaurant Name";
        Restaurant unsavedRestaurant = new RestaurantBuilder().Build();
        Restaurant savedRestaurant = await _restaurantRepository.Add(unsavedRestaurant);

        savedRestaurant.Name = UPDATED_NAME;
        #endregion

        #region Act
        Restaurant? updatedRestaurant = await _restaurantRepository.Update(savedRestaurant.Id, savedRestaurant);
        #endregion

        #region Assert
        Assert.NotNull(updatedRestaurant);
        Assert.Equal(UPDATED_NAME, updatedRestaurant.Name);
        Assert.True(updatedRestaurant.PropertiesAreEqual(savedRestaurant));
        #endregion
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenIdDoesNotExist()
    {
        #region Act && Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _restaurantRepository.Update(Guid.NewGuid().ToString(), new RestaurantBuilder().Build()));
        #endregion
    }

    [Fact]
    public async Task Delete_ShouldRemoveRestaurant_WhenValidIdProvided()
    {
        #region Arrange
        Restaurant unsavedRestaurant = new RestaurantBuilder().Build();
        Restaurant savedRestaurant = await _restaurantRepository.Add(unsavedRestaurant);
        #endregion

        #region Act
        await _restaurantRepository.Delete(savedRestaurant.Id);
        var result = await _restaurantRepository.GetById(savedRestaurant.Id);
        #endregion

        #region Assert
        Assert.Null(result);
        #endregion
    }

    [Fact]
    public async Task Delete_ShouldThrowException_WhenIdDoesNotExist()
    {
        #region Act && Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _restaurantRepository.Delete(Guid.NewGuid().ToString()));
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
    [InlineData(Cuisine.Italian, (Location)(-1))]
    [InlineData(Cuisine.Chinese, (Location)(-1))]
    [InlineData(Cuisine.Mexican, (Location)(-1))]
    [InlineData((Cuisine)(-1), Location.NewYork)]
    [InlineData((Cuisine)(-1), Location.London)]
    [InlineData((Cuisine)(-1), Location.Paris)]
    [InlineData((Cuisine)(-1), Location.Rome)]
    public async Task FilterByPreferences_ShouldReturnMatchingRestaurants_WhenCriteriaMatch(Cuisine? cuisine, Location? location)
    {
        #region Arrange
        var restaurants = new List<Restaurant>{
            new RestaurantBuilder().WithCuisine(Cuisine.Italian).WithLocation(Location.NewYork).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Chinese).WithLocation(Location.London).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Mexican).WithLocation(Location.Paris).Build(),
            new RestaurantBuilder().WithCuisine(Cuisine.Italian).WithLocation(Location.Rome).Build(),
        };

        await _restaurantRepository.AddMany(restaurants);
        #endregion

        #region Act
        var result = await _restaurantRepository.FilterByPreferences(cuisine, location);
        #endregion

        #region Assert
        Assert.NotNull(result);
        Assert.All(result, (restaurant) =>
        {

            if (cuisine.HasValue)
            {
                Assert.Equal(cuisine.Value, restaurant.Cuisine);
            }

            if (location.HasValue)
            {
                Assert.Equal(location.Value, restaurant.Location);
            }

        });
        #endregion
    }

    [Fact]
    public async Task FilterByPreferences_ShouldReturnEmptyList_WhenNoMatchesFound()
    {
        #region Arrange
        var restaurants = new List<Restaurant>{
        new RestaurantBuilder().WithCuisine(Cuisine.Italian).WithLocation(Location.NewYork).Build(),
        new RestaurantBuilder().WithCuisine(Cuisine.Chinese).WithLocation(Location.London).Build()
    };

        await _restaurantRepository.AddMany(restaurants);
        #endregion

        #region Act
        var result = await _restaurantRepository.FilterByPreferences(Cuisine.Mexican, Location.Paris); // No matching restaurant
        #endregion

        #region Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        #endregion
    }


    private void Dispose()
    {
        _dbContext.Dispose();
    }
}
