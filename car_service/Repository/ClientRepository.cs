using car_service.Data;
using car_service.Entities;
using car_service.Interface;
using Microsoft.EntityFrameworkCore;

namespace car_service.Repository;

public class ClientRepository : IClientRepository
{
    private readonly DataContext _context;
    
    public ClientRepository(DataContext context) 
    { 
        _context = context;
    }
    
    public async Task<Client> CreateAsync(Client client)
    {
        var dbClient = await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return dbClient.Entity;
    }

    public async Task<Client> GetByIdAsync(int clientId)
    {
        var dbClient = await _context.Clients.FindAsync(clientId);
        if (dbClient != null)
            return dbClient;
        throw new Exception("This ID doesn't exist");
    }

    public async Task<ICollection<Client>> GetAllAsync()
    {
        return await _context.Clients.OrderBy(m => m.Id).ToListAsync();
    }

    public async Task<Client> UpdateAsync(Client client, int clientId)
    {
        var dbClient = await _context.Clients.FindAsync(clientId);
        if (dbClient == null)
            throw new Exception("This ID doesn't exist");
        dbClient.Adress = client.Adress;
        dbClient.Name = client.Name;
        dbClient.Surname = client.Surname;
        dbClient.PhoneNumber = client.PhoneNumber;
        await _context.SaveChangesAsync();
        return dbClient;
    }

    public async Task<bool> DeleteAsync(int clientId)
    {
        var dbClient = await _context.Clients.FindAsync(clientId);
        if (dbClient == null)
            throw new Exception("This ID doesn't exist");
        var res = _context.Clients.Remove(dbClient);
        await _context.SaveChangesAsync();
        return res.State == EntityState.Deleted;
    }

    public async Task<ICollection<Car>> GetCarsAsync(int clientId)
    {
        var dbClient = await _context.Clients.FindAsync(clientId);
        if (dbClient == null)
            throw new Exception("This ID doesn't exist");
        return await _context.Orders.Where(o => o.Client.Id == dbClient.Id).Select(o => o.Car).ToListAsync();
    }

    public async Task<ICollection<Order>> GetOrdersAsync(int clientId)
    {
        var dbClient = await _context.Clients.FindAsync(clientId);
        if (dbClient == null)
            throw new Exception("This ID doesn't exist");
        return await _context.Orders.Where(o => o.Client.Id == dbClient.Id).ToListAsync();
    }
}