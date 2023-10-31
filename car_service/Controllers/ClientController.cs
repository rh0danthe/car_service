using AutoMapper;
using car_service.Dto;
using car_service.Entities;
using car_service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace car_service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : Controller
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;
    
    public ClientController(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClientAsync(ClientDto newClient)
    {
        if (newClient == null)
            return BadRequest(ModelState);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var clientMap = _mapper.Map<Client>(newClient);
        var clientDb = await _clientRepository.CreateAsync(clientMap);

        if (clientDb == null)
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok(clientDb);
    }
    
    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClientByIdAsync([FromRoute] int clientId)
    {
        var dbClient = await _clientRepository.GetByIdAsync(clientId);
        return Ok(dbClient);
    }
    
    [HttpGet("{clientId}/cars")]
    public async Task<IActionResult> GetClientCarsAsync([FromRoute] int clientId)
    {
        var cars = await _clientRepository.GetCarsAsync(clientId);
        return Ok(cars);
    }
    
    [HttpGet("{clientId}/orders")]
    public async Task<IActionResult> GetClientOrdersAsync([FromRoute] int clientId)
    {
        var orders = await _clientRepository.GetOrdersAsync(clientId);
        return Ok(orders);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return Ok(clients);
    }

    [HttpPut("{clientId}")]
    public async Task<IActionResult> UpdateClientAsync([FromRoute] int clientId, ClientDto newClient)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var clientMap = _mapper.Map<Client>(newClient);
        clientMap!.Id = clientId;
        var dbClient = await _clientRepository.UpdateAsync(clientMap);
        return Ok(dbClient);
    }

    [HttpDelete("{clientId}")]
    public async Task<IActionResult> DeleteClientAsync([FromRoute] int clientId)
    {
        var dbClient = await _clientRepository.DeleteAsync(clientId);
        return Ok(dbClient);
    }
}