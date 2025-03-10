using System;
using LocalBites.Interfaces.Repositories;
using LocalBites.Models;
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
    public async Task OnGetAsync_PopulatesThePageModel_WithAListOfMessages()
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
}
