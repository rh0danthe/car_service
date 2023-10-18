using car_service.Data;
using car_service.Entities;
using car_service.Interface;
using Microsoft.EntityFrameworkCore;

namespace car_service.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _context;
    
    public OrderRepository(DataContext context) 
    { 
        _context = context;
    }
    
    public async Task<Order> CreateAsync(Order order)
    {
        var dbOrder = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return dbOrder.Entity;
    }

    public async Task<Order> GetByIdAsync(int orderId)
    {
        var dbOrder = await _context.Orders.FindAsync(orderId);
        if (dbOrder != null)
            return dbOrder;
        throw new Exception("This ID doesn't exist");
    }

    public async Task<ICollection<Order>> GetAllAsync()
    {
        return await _context.Orders.OrderBy(m => m.Id).ToListAsync();
    }

    public async Task<Order> UpdateAsync(Order order, int orderId)
    {
        var dbOrder = await _context.Orders.FindAsync(orderId);
        if (dbOrder == null)
            throw new Exception("This ID doesn't exist");
        dbOrder.Car = order.Car;
        dbOrder.Client = order.Client;
        dbOrder.WorkDescription = order.WorkDescription;
        dbOrder.AcceptanceTime = order.AcceptanceTime;
        dbOrder.Status = order.Status;
        await _context.SaveChangesAsync();
        return dbOrder;
    }

    public async Task<bool> DeleteAsync(int orderId)
    {
        var dbOrder = await _context.Orders.FindAsync(orderId);
        if (dbOrder == null)
            throw new Exception("This ID doesn't exist");
        var res = _context.Orders.Remove(dbOrder);
        await _context.SaveChangesAsync();
        return res.State == EntityState.Deleted;
    }
}