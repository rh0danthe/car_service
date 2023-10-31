using car_service.Entities;
using car_service.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace car_service.Repository;

public class ClientRepository : IClientRepository
{
    private string connectionString;
    
    public ClientRepository(IConfiguration config) 
    { 
        connectionString = config.GetConnectionString("DefaultConnection");
    }
    
    public async Task<Client> CreateAsync(Client client)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "INSERT INTO \"Clients\" (\"Name\", \"Surname\", \"Adress\", \"PhoneNumber\") VALUES(@Name, @Surname, @Adress, @PhoneNumber) RETURNING *";
            return await db.QueryFirstOrDefaultAsync<Client>(query, client);
        }
    }

    public async Task<Client> GetByIdAsync(int clientId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            string query = "SELECT * FROM \"Clients\" WHERE \"Id\" = @id";
            var parameters = new { id = clientId };
            return await db.QueryFirstOrDefaultAsync<Client>(query, parameters);
        }
    }

    public async Task<ICollection<Client>> GetAllAsync()
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var clients = await db.QueryAsync<Client>("SELECT * FROM \"Clients\"");
            return clients.ToList();
        }
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query =
                "UPDATE \"Clients\" SET \"Name\" = @Name, \"Surname\" = @Surname, \"Adress\" = @Adress, \"PhoneNumber\" = @PhoneNumber WHERE \"Id\" = @Id RETURNING *";
            return await db.QueryFirstOrDefaultAsync<Client>(query, client);
        }
    }

    public async Task<bool> DeleteAsync(int clientId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "DELETE FROM \"Clients\" WHERE \"Id\" = @id";
            var res = await db.ExecuteAsync(query, new { id = clientId });
            return res > 0;
        }
    }

    public async Task<ICollection<Car>> GetCarsAsync(int clientId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query =
                "SELECT \"Cars\".* FROM \"Orders\" JOIN \"Cars\" ON \"Orders\".\"CarId\" = \"Cars\".\"Id\" WHERE \"Orders\".\"ClientId\" = @ClientId";
            var cars = await db.QueryAsync<Car>(query, new {ClientId = clientId});
            return cars.ToList();
        }
    }

    public async Task<ICollection<Order>> GetOrdersAsync(int clientId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var orders = await db.QueryAsync<Order>("SELECT * FROM \"Orders\" WHERE \"ClientId\" = @ClientId", new {ClientId = clientId});
            return orders.ToList();
        }
    }
}