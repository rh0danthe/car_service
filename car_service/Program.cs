using System.Reflection;
using car_service.Interface;
using car_service.Middleware;
using car_service.Repository;
using Dapper;
using DbUp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

//builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

using var scoped = app.Services.CreateScope();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Migrate(app);
//app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void Migrate(IHost host)
{
    using var scope = host.Services.CreateScope();

    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Migrating PostgreSQL database...");

    string connection = configuration.GetConnectionString("DefaultConnection")!;

    EnsureDatabase.For.PostgresqlDatabase(connection);

    var upgrader = DeployChanges.To
        .PostgresqlDatabase(connection)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        logger.LogError(result.Error, "An error occurred while migrating the PostgreSQL database");
        return;
    }

    logger.LogInformation("PostgreSQL database migration has been completed");
}