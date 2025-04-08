using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover validation scenarios.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid sale data")]
    public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "", // Invalid: required
            SaleDate = DateTime.UtcNow.AddDays(1), // Invalid: future date
            Customer = "", // Invalid: required
            Branch = "", // Invalid: required
            Product = "", // Invalid: required
            Quantity = 0, // Invalid: must be between 1-20
            UnitPrice = 0, // Invalid: must be > 0
            Discount = -5, // Invalid: must be >= 0
            Total = 0, // Invalid: must be > 0
            IsCancelled = false
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}