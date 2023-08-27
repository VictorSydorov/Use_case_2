using Microsoft.AspNetCore.Diagnostics;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Add(new ServiceDescriptor(typeof(ISingletonRetrievable<Balance>), new BalanceService()));
builder.Services.Add(new ServiceDescriptor(typeof(IListable<BalanceTransaction, BalanceTransactionListOptions>), new BalanceTransactionService()));


var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var result = new
        {
            error = new
            {
                message = "Internal Server Error",
                details = exception?.Message
            }
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

// Configure the HTTP request pipeline.

app.MapStripeServices();

app.Run();
