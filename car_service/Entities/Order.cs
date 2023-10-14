namespace car_service.Entities;
using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    public int Id { get; set; }
    public Car Car { get; set; }
    public Client Client { get; set; }
    public DateTime AcceptanceTime { get; set; } = DateTime.UtcNow;
    public string WorkDescription { get; set; }
    public OrderStatus Status { get; set; }
}