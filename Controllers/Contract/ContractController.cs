using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public partial class ContractController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateContract(CreateContractDto contract)
    {
        if(string.IsNullOrEmpty(contract.BankINN)||
        string.IsNullOrEmpty(contract.BankName)||
        string.IsNullOrEmpty(contract.CardNumber)||
        string.IsNullOrEmpty(contract.INN)||
        string.IsNullOrEmpty(contract.INPS))
        
        return BadRequest("This properties can't be null");

        var cereatedContract = await _contractService.CreateContract(ToModelContractDto(contract));

        if(cereatedContract.Data is null)
        return BadRequest("This contact didn't created");
        
        return Ok(ContractModelToDto(cereatedContract.Data));
    }

    [HttpGet("DagavorItems")]
    public async Task<IActionResult> GetDagavor(int id)
    {
        var dagavorItems = await _contractService.GetDagovorItems(id);

        return Ok(dagavorItems);
    }

    [HttpPost("Save/Dagavor")]
    public IActionResult Save(string html)
    {
        
        return Ok();
    }
}