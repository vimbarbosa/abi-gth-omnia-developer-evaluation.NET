using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    /// <summary>
    /// Contains unit tests for the <see cref="SaleValidator"/> class.
    /// Covers validation of product name, quantity, unit price, and sale date.
    /// </summary>
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        [Fact(DisplayName = "Valid sale should pass all validation rules")]
        public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory(DisplayName = "Invalid product names should fail validation")]
        [InlineData("")]
        [InlineData(null)]
        public void Given_InvalidProduct_When_Validated_Then_ShouldHaveError(string product)
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.Product = product;

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Product);
        }

        [Fact(DisplayName = "Product name exceeding 100 characters should fail validation")]
        public void Given_ProductTooLong_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.Product = new string('A', 101);

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Product);
        }

        [Theory(DisplayName = "Invalid quantities should fail validation")]
        [InlineData(0)]
        [InlineData(-5)]
        public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError(int quantity)
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.Quantity = quantity;

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Quantity);
        }

        [Theory(DisplayName = "Invalid unit prices should fail validation")]
        [InlineData(0)]
        [InlineData(-1)]
        public void Given_InvalidUnitPrice_When_Validated_Then_ShouldHaveError(decimal unitPrice)
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.UnitPrice = unitPrice;

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.UnitPrice);
        }

        [Fact(DisplayName = "Future sale date should fail validation")]
        public void Given_FutureSaleDate_When_Validated_Then_ShouldHaveError()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleDate = DateTime.UtcNow.AddDays(1);

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.SaleDate);
        }
    }
}
