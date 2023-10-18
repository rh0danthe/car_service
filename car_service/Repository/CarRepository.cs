using car_service.Data;
using car_service.Interface;
using car_service.Entities;
using Microsoft.EntityFrameworkCore;

namespace car_service.Repository;

public class CarRepository :  ICarRepository
{
    private readonly DataContext _context;
    
    public CarRepository(DataContext context) 
    { 
        _context = context;
    }
    
    public async Task<Car> CreateAsync(Car car)
    {
        var dbCar = await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
        return dbCar.Entity;
    }

    public async Task<Car> GetByIdAsync(int carId)
    {
        var dbCar = await _context.Cars.FindAsync(carId);
        if (dbCar != null)
            return dbCar;
        throw new Exception("This ID doesn't exist");
    }

    public async Task<ICollection<Car>> GetAllAsync()
    {
         return await _context.Cars.OrderBy(m => m.Id).ToListAsync();
    }

    public async Task<Car> UpdateAsync(Car car, int carId)
    {
        var dbCar = await _context.Cars.FindAsync(carId);
        if (dbCar == null)
            throw new Exception("This ID doesn't exist");
        dbCar.Brand = car.Brand;
        dbCar.Model = car.Model;
        dbCar.VINcode = car.VINcode;
        dbCar.YearOfRelease = car.YearOfRelease;
        //var res = _context.Cars.Update(dbCar);
        await _context.SaveChangesAsync();
        return dbCar;
    }

    public async Task<bool> DeleteAsync(int carId)
    {
        var dbCar = await _context.Cars.FindAsync(carId);
        if (dbCar == null)
            throw new Exception("This ID doesn't exist");
        var res = _context.Cars.Remove(dbCar);
        bool deleted = res.State == EntityState.Deleted;
        await _context.SaveChangesAsync();
        return deleted;
    }
}