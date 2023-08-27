using System.Net;

namespace Services
{
    public static class StripeServices
    {
        /// <summary>
        /// Maps Stripe-related services to provided endpoint routes.
        /// </summary>
        /// <param name="routes">The endpoint route builder.</param>
        /// <returns>The endpoint route builder.</returns>
        public static IEndpointRouteBuilder MapStripeServices(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/balance", (ISingletonRetrievable<Balance> balanceService, IConfiguration config) => GetBalance(balanceService, config));

            routes.MapGet("/balanceTransactions", (IListable<BalanceTransaction, BalanceTransactionListOptions> service, IConfiguration config) => GetBalanceTransactions(service, config));

            return routes;
        }

        internal static async Task<Balance> GetBalance(ISingletonRetrievable<Balance> balanceService, IConfiguration config)
        {
            try
            {
                StripeConfiguration.ApiKey = config["Stripe:ApiKey"];
                Balance balance = await balanceService.GetAsync();

                return balance;
            }
            catch (StripeException)
            {
                throw new WebException("Server error.");
            }
        }

        internal static async Task<StripeList<BalanceTransaction>> GetBalanceTransactions(IListable<BalanceTransaction, BalanceTransactionListOptions> service, IConfiguration config)
        {
            try
            {
                StripeConfiguration.ApiKey = config["Stripe:ApiKey"];
                var options = new BalanceTransactionListOptions { Currency = "usd" };
                StripeList<BalanceTransaction> balancetransactions = await service.ListAsync(options);

                return balancetransactions;
            }
            catch (StripeException)
            {
                throw new WebException("Server error.");
            }
        }
    }
}