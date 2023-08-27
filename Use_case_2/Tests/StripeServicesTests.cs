using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Stripe;

namespace Tests
{
    public class StripeServicesTests
    {
        [Fact]
        public async Task GetBalance_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockBalanceService = new Mock<ISingletonRetrievable<Balance>>();
            var mockConfig = new Mock<IConfiguration>();
            var configSection = new Mock<IConfigurationSection>();
            configSection.Setup(c => c.Value).Returns("mykey");
            mockConfig.Setup(c => c.GetSection("Stripe:ApiKey")).Returns(configSection.Object);

            mockBalanceService.Setup(bs => bs.GetAsync(null, CancellationToken.None)).ReturnsAsync(new Balance());

            // Act
            var result = await StripeServices.GetBalance(mockBalanceService.Object, mockConfig.Object);

            // Assert
            Assert.IsType<Balance>(result);
        }

        [Fact]
        public async Task GetBalanceTransactions_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockBalanceTransactionService = new Mock<IListable<BalanceTransaction, BalanceTransactionListOptions>>();
            var mockConfig = new Mock<IConfiguration>();
            var configSection = new Mock<IConfigurationSection>();
            configSection.Setup(c => c.Value).Returns("your_api_key");
            mockConfig.Setup(c => c.GetSection("Stripe:ApiKey")).Returns(configSection.Object);

            var mockStripeList = new StripeList<BalanceTransaction>
            {
                Data = new List<BalanceTransaction> { new BalanceTransaction() }
            };
            mockBalanceTransactionService.Setup(bs => bs.ListAsync(It.IsAny<BalanceTransactionListOptions>(), null, CancellationToken.None)).ReturnsAsync(mockStripeList);

            // Act
            var result = await StripeServices.GetBalanceTransactions(mockBalanceTransactionService.Object, mockConfig.Object);

            // Assert
            Assert.IsType<StripeList<BalanceTransaction>>(result);
        }
    }
}