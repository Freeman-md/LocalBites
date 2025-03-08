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

    private void Dispose()
    {
        _dbContext.Dispose();
    }
}
