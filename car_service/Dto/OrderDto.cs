using car_service.Entities;

namespace car_service.Dto;

public class OrderDto
{
    public int CarId { get; set; }
    public int ClientId { get; set; }
    public string WorkDescription { get; set; }
    public OrderStatus Status { get; set; }
}