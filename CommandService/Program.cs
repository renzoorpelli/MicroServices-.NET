using CommandService.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Environment, builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();