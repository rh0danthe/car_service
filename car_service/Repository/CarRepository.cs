using car_service.Interface;
using car_service.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace car_service.Repository;

public class CarRepository :  ICarRepository
{
    private string connectionString;
    public CarRepository(IConfiguration config)
    {
        connectionString = config.GetConnectionString("DefaultConnection");
    }
    public async Task<Car> CreateAsync(Car car)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "INSERT INTO \"Cars\" (\"Brand\", \"Model\", \"YearOfRelease\", \"VINcode\") VALUES(@Brand, @Model, @YearOfRelease, @VINcode) RETURNING *";
            return await db.QueryFirstOrDefaultAsync<Car>(query, car);
        }
    }

    public async Task<Car> GetByIdAsync(int carId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            string query = "SELECT * FROM \"Cars\" WHERE \"Id\" = @id";
            var parameters = new { id = carId };
            return await db.QueryFirstOrDefaultAsync<Car>(query, parameters);
        }
    }

    public async Task<ICollection<Car>> GetAllAsync()
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var cars = await db.QueryAsync<Car>("SELECT * FROM \"Cars\"");
            return cars.ToList();
        }
    }

    public async Task<Car> UpdateAsync(Car car)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query =
                "UPDATE \"Cars\" SET \"Brand\" = @Brand, \"Model\" = @Model, \"YearOfRelease\" = @YearOfRelease, \"VINcode\" = @VINcode WHERE \"Id\" = @Id RETURNING *";
            return await db.QueryFirstOrDefaultAsync<Car>(query, car);
        }
    }

    public async Task<bool> DeleteAsync(int carId)
    {
        await using (var db = new NpgsqlConnection(connectionString))
        {
            await db.OpenAsync();
            var query = "DELETE FROM \"Cars\" WHERE \"Id\" = @id";
            var res = await db.ExecuteAsync(query, new { id = carId });
            return res > 0;
        }
    }
}