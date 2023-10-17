using car_service.Entities;

namespace car_service.Dto;

public class OrderDto
{
    public Car Car { get; set; }
    public Client Client { get; set; }
    public string WorkDescription { get; set; }
    public OrderStatus Status { get; set; }
}