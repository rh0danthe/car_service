using AutoMapper;
using car_service.Dto;
using car_service.Entities;
using car_service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace car_service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICarRepository _carRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;
    
    public OrderController(IOrderRepository orderRepository, IMapper mapper, ICarRepository carRepository,
        IClientRepository clientRepository)
    {
        _orderRepository = orderRepository;
        _carRepository = carRepository;
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(OrderDto newOrder)
    {
        if (newOrder == null)
            return BadRequest(ModelState);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var orderMap = _mapper.Map<Order>(newOrder);
        orderMap.Car = await _carRepository.GetByIdAsync(newOrder.CarId);
        orderMap.Client = await _clientRepository.GetByIdAsync(newOrder.ClientId);
        var orderDb = await _orderRepository.CreateAsync(orderMap);

        if (orderDb == null)
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok(orderDb);
    }
    
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int orderId)
    {
        var dbOrder = await _orderRepository.GetByIdAsync(orderId);
        return Ok(dbOrder);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(orders);
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrderAsync([FromRoute] int orderId, OrderDto newOrder)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var orderMap = _mapper.Map<Order>(newOrder);
        orderMap!.Id = orderId;
        orderMap.Car = await _carRepository.GetByIdAsync(newOrder.CarId);
        orderMap.Client = await _clientRepository.GetByIdAsync(newOrder.ClientId);
        var dbOrder = await _orderRepository.UpdateAsync(orderMap);
        return Ok(dbOrder);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrderAsync([FromRoute] int orderId)
    {
        var dbOrder = await _orderRepository.DeleteAsync(orderId);
        return Ok(dbOrder);
    }
}