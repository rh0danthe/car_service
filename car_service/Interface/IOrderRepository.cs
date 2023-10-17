using car_service.Entities;

namespace car_service.Interface;

public interface IOrderRepository
{
    public Task<Order> CreateAsync(Order order);
    public Task<Order> GetByIdAsync(int orderId);
    public Task<ICollection<Order>> GetAllAsync();
    public Task<Order> UpdateAsync(Order order, int orderId);
    public Task<bool> DeleteAsync(int orderId);
}