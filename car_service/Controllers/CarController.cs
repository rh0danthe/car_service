using AutoMapper;
using car_service.Dto;
using car_service.Entities;
using car_service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace car_service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController : Controller
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;
    
    public CarController(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCarAsync(CarDto newCar)
    {
        if (newCar == null)
            return BadRequest(ModelState);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var carMap = _mapper.Map<Car>(newCar);
        var carDb = await _carRepository.CreateAsync(carMap);

        if (carDb == null)
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok(carDb);
    }
    
    [HttpGet("{carId}")]
    public async Task<IActionResult> GetCarByIdAsync([FromRoute] int carId)
    {
        var dbCar = await _carRepository.GetByIdAsync(carId);
        return Ok(dbCar);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCarsAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return Ok(cars);
    }

    [HttpPut("{carId}")]
    public async Task<IActionResult> UpdateCarAsync([FromRoute] int carId, CarDto newCar)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var carMap = _mapper.Map<Car>(newCar);
        carMap!.Id = carId;
        var dbCar = await _carRepository.UpdateAsync(carMap);
        return Ok(dbCar);
    }

    [HttpDelete("{carId}")]
    public async Task<IActionResult> DeleteCarAsync([FromRoute] int carId)
    {
        var dbCar = await _carRepository.DeleteAsync(carId);
        return Ok(dbCar);
    }
}