using Stripe;

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

    private static async Task<IResult> GetBalance(ISingletonRetrievable<Balance> balanceService, IConfiguration config)
    {
        try
        {
            StripeConfiguration.ApiKey = config["Stripe:ApiKey"];
            Balance balance = await balanceService.GetAsync();

            return Results.Ok(balance);
        }
        catch (Exception)
        {
            throw new Exception("Server error.");
        }
    }

    private static async Task<IResult> GetBalanceTransactions(IListable<BalanceTransaction, BalanceTransactionListOptions> service, IConfiguration config)
    {
        try
        {
            StripeConfiguration.ApiKey = config["Stripe:ApiKey"];
            var options = new BalanceTransactionListOptions { Currency = "usd" };
            StripeList<BalanceTransaction> balancetransactions = await service.ListAsync(options);

            return Results.Ok(balancetransactions);
        }
        catch (Exception)
        {
            throw new Exception("Server error.");
        }
    }

}

