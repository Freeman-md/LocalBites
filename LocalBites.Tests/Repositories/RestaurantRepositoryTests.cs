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
        await _dbContext.Restaurants.AddRangeAsync(restaurants);
        await _dbContext.SaveChangesAsync();
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
    public async Task GetById_ShouldReturnRestaurant_WhenIdExists() {
        #region Arrange
            Restaurant restaurant = new RestaurantBuilder().Build();

            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
        #endregion

        #region Act
            Restaurant? retrievedRestaurant = await _restaurantRepository.GetById(restaurant.Id);
        #endregion

        #region Assert
            Assert.NotNull(retrievedRestaurant);
            Assert.Equal(restaurant.Name, retrievedRestaurant.Name);
        #endregion
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist() {
        #region Act
            Restaurant? retrievedRestaurant = await _restaurantRepository.GetById(Guid.NewGuid().ToString());
        #endregion

        #region Assert
            Assert.Null(retrievedRestaurant);
        #endregion
    }

    [Fact]
    public async Task Add_ShouldAddRestaurant_WhenValidDataProvided() {
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

    private void Dispose()
    {
        _dbContext.Dispose();
    }
}
