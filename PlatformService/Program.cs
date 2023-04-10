using PlatformService.Configuration;
using PlatformService.Services.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGrpcService<GrpcPlatformService>();
});


app.Run();