using BaseArchitecture.Api.Controllers;
using BaseArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BaseArchitectureDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddMediator();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var controllers = typeof(Program)
    .Assembly.GetTypes()
    .Where(x => x.IsAssignableTo(typeof(IMinimalApiController)) && !x.IsAbstract && !x.IsInterface)
    .Select(Activator.CreateInstance)
    .Cast<IMinimalApiController>();

foreach (var controller in controllers)
{
    controller.MapEndpoints(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
