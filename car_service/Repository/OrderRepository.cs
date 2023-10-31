using car_service.Entities;
using car_service.Interface;
using Dapper;
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
            var joinQuery = @"WITH inserted AS (
  INSERT INTO ""Orders"" (""CarId"", ""ClientId"", ""AcceptanceTime"", ""WorkDescription"", ""Status"") VALUES(@CarId, @ClientId, @AcceptanceTime, @WorkDescription, @Status) RETURNING *
) SELECT * FROM inserted JOIN ""Cars"" ON inserted.""CarId"" = ""Cars"".""Id"" JOIN ""Clients"" ON inserted.""ClientId"" = ""Clients"".""Id"" ";
            var parameters = new { CarId = order.Car.Id, ClientId = order.Client.Id, AcceptanceTime = order.AcceptanceTime, WorkDescription = order.WorkDescription, Status = order.Status};
            var result = await db.QueryAsync<Order, Car, Client, Order>(joinQuery, (order, car, client) =>
            {
                order.Car = car;
                order.Client = client;
                return order;
            },parameters);
            return result.FirstOrDefault();
        }
    }

    public async Task<Order> GetByIdAsync(int orderId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = @"
            SELECT o.*, c.*, cl.*
            FROM ""Orders"" o
            JOIN ""Cars"" c ON o.""CarId"" = c.""Id""
            JOIN ""Clients"" cl ON o.""ClientId"" = cl.""Id"" WHERE o.""Id"" = @OrderId";
            var result = await db.QueryAsync<Order, Car, Client, Order>(query,  (order, car, client) =>
            {
                order.Car = car;
                order.Client = client;
                return order;
            },new {OrderId = orderId});
            return result.FirstOrDefault();
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
            JOIN ""Cars"" c ON o.""CarId"" = c.""Id""
            JOIN ""Clients"" cl ON o.""ClientId"" = cl.""Id""";

            var orders = await db.QueryAsync<Order, Car, Client, Order>(
                query,
                (order, car, client) =>
                {
                    order.Car = car;
                    order.Client = client;
                    return order;
                }
            );

            return orders.ToList();
        }
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var joinQuery = @"WITH updated AS (
  UPDATE ""Orders"" SET ""CarId"" = @CarId, ""ClientId"" = @ClientId, ""AcceptanceTime"" = @AcceptanceTime, ""WorkDescription"" = @WorkDescription, ""Status"" = @Status WHERE ""Id"" = @Id RETURNING *
) SELECT * FROM updated JOIN ""Cars"" ON updated.""CarId"" = ""Cars"".""Id"" JOIN ""Clients"" ON updated.""ClientId"" = ""Clients"".""Id""";
            var parameters = new {Id = order.Id, CarId = order.Car.Id, ClientId = order.Client.Id, AcceptanceTime = order.AcceptanceTime, WorkDescription = order.WorkDescription, Status = order.Status };
            var result = await db.QueryAsync<Order, Car, Client, Order>(joinQuery, (order, car, client) =>
            {
                order.Car = car;
                order.Client = client;
                return order;
            },parameters);
            return result.FirstOrDefault();
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