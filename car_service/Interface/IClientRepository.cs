using car_service.Entities;

namespace car_service.Interface;

public interface IClientRepository
{
    public Task<Client> CreateAsync(Client client);
    public Task<Client> GetByIdAsync(int clientId);
    public Task<ICollection<Client>> GetAllAsync();
    public Task<Client> UpdateAsync(Client client);
    public Task<bool> DeleteAsync(int clientId);
    public Task<ICollection<Car>> GetCarsAsync(int clientId);
    public Task<ICollection<Order>> GetOrdersAsync(int clientId);
}