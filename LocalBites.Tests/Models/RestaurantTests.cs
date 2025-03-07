using System;
using System.ComponentModel.DataAnnotations;
using LocalBites.Tests.Builders;
using Models.Restaurant;

namespace LocalBites.Tests.Models;

public class RestaurantTests
{
    [Fact]
    public void TestValidRestaurant_ShouldPassValidation() {
        #region Arrange
            Restaurant restaurant = new RestaurantBuilder().Build();
            ValidationContext validationContext = new ValidationContext(restaurant);
            List<ValidationResult> validationResults = new List<ValidationResult>();
        #endregion

        #region Act
            bool isValid = Validator.TryValidateObject(restaurant, validationContext, validationResults, true);
        #endregion

        #region Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
            Assert.NotNull(restaurant);
        #endregion
    }

    [Theory]
    [InlineData("", "London", Cuisine.Chinese, "This is a description", 3)]
    [InlineData(null, "London", Cuisine.Chinese, "This is a description", 3)]
    [InlineData("*********************************************************", "London", Cuisine.Chinese, "This is a description", 3)]
    [InlineData("CocoCure", "", Cuisine.Chinese, "This is a description", 3)]
    [InlineData("CocoCure", null, Cuisine.Chinese, "This is a description", 3)]
    [InlineData("CocoCure", "*********************************************************", Cuisine.Chinese, "This is a description", 3)]
    [InlineData("CocoCure", "London", (Cuisine)(-1), "This is a description", 3)]
    [InlineData("CocoCure", "London", Cuisine.Chinese, "", 3)]
    [InlineData("CocoCure", "London", Cuisine.Chinese, null, 8)]
    public void TestMissingProperties_ShouldFailValidation(
        string name, 
        string location,
        Cuisine cuisine,
        string description,
        int rating
    ) {
        #region Arrange 
            Restaurant restaurant = new RestaurantBuilder()
                                        .WithName(name)
                                        .WithLocation(location)
                                        .WithCuisine(cuisine)
                                        .WithDescription(description)
                                        .WithRating(rating)
                                        .Build();

            ValidationContext validationContext = new ValidationContext(restaurant);
            List<ValidationResult> validationResults = new List<ValidationResult>();
        #endregion

        #region Act
            bool isValid = Validator.TryValidateObject(restaurant, validationContext, validationResults, true);
        #endregion

        #region Assert
            Assert.False(isValid);
            Assert.NotNull(validationResults);
        #endregion
    }
}
