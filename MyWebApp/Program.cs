using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Middleware to serve static files from wwwroot
app.UseStaticFiles();

// Middleware to handle /end route and terminate the chain
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/end"))
    {
        await context.Response.WriteAsync("Chain terminated.");
        return; // Terminate the middleware chain
    }

    await next.Invoke();
});

// Middleware to handle /hello route, display hello1 and move to the next middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/hello"))
    {
        await context.Response.WriteAsync("Hello1 ");
        await next.Invoke();
    }
    else
    {
        await next.Invoke();
    }
});

// Another middleware to handle /hello route, display hello2
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/hello"))
    {
        await context.Response.WriteAsync("Hello2");
        // Continue the chain
        await next.Invoke();
    }
    else
    {
        await next.Invoke();
    }
});

// Final middleware
app.Run(async (context) =>
{
    await context.Response.WriteAsync("Default response from middleware.");
});

app.Run();
