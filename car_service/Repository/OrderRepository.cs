using car_service.Entities;
using car_service.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace car_service.Repository;

public class OrderRepository : IOrderRepository
{
    private string connectionString;
    
    public OrderRepository(IConfiguration config) 
    { 
        connectionString = config.GetConnectionString("DefaultConnection");
    }
    
    public async Task<Order> CreateAsync(Order order)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "INSERT INTO \"Orders\" (\"CarId\", \"ClientId\", \"AcceptanceTime\", \"WorkDescription\", \"Status\") VALUES(@CarId, @ClientId, @AcceptanceTime, @WorkDescription, @Status) RETURNING *";
            var parameters = new { CarId = order.Car.Id, ClientId = order.Client.Id, AcceptanceTime = order.AcceptanceTime, WorkDescription = order.WorkDescription, Status = order.Status };
            return await db.QueryFirstOrDefaultAsync<Order>(query, parameters);
        }
    }

    public async Task<Order> GetByIdAsync(int orderId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            string query = "SELECT * FROM \"Orders\" WHERE \"Id\" = @id";
            var parameters = new { id = orderId };
            return await db.QueryFirstOrDefaultAsync<Order>(query, parameters);
        }
    }

    public async Task<ICollection<Order>> GetAllAsync()
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();

            var query = @"
            SELECT o.*, c.*, cl.*
            FROM ""Orders"" o
            LEFT JOIN ""Cars"" c ON o.""CarId"" = c.""Id""
            LEFT JOIN ""Clients"" cl ON o.""ClientId"" = cl.""Id""";

            var orders = await db.QueryAsync<Order, Car, Client, Order>(
                query,
                (order, car, client) =>
                {
                    order.Car = car;
                    order.Client = client;
                    return order;
                },
                splitOn: "Id,Id"
            );

            return orders.ToList();
        }
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query =
                "UPDATE \"Orders\" SET \"CarId\" = @CarId, \"ClientId\" = @ClientId, \"AcceptanceTime\" = @AcceptanceTime, \"WorkDescription\" = @WorkDescription, \"Status\" = @Status WHERE \"Id\" = @Id RETURNING *";
            var parameters = new { CarId = order.Car.Id, ClientId = order.Client.Id, AcceptanceTime = order.AcceptanceTime, WorkDescription = order.WorkDescription, Status = order.Status };
            return await db.QueryFirstOrDefaultAsync<Order>(query, parameters);
        }
    }

    public async Task<bool> DeleteAsync(int orderId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "DELETE FROM \"Orders\" WHERE \"Id\" = @id";
            var res = await db.ExecuteAsync(query, new { id = orderId });
            return res > 0;
        }
    }
}