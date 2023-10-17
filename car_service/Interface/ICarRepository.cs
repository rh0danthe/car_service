using car_service.Entities;

namespace car_service.Interface;
public interface ICarRepository
{
    public Task<Car> CreateAsync(Car car);
    public Task<Car> GetByIdAsync(int carId);
    public Task<ICollection<Car>> GetAllAsync();
    public Task<Car> UpdateAsync(Car car, int carId);
    public Task<bool> DeleteAsync(int carId);
}