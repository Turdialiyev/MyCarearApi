using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromForm] FreelancerPortfolio portfolio)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _portfolioService.SaveAsync(userId!, portfolio.Image!, ToModel(portfolio));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] FreelancerPortfolio portfolio)
    {
        try
        {
            var result = await _portfolioService.UpdateAsync(id, portfolio.Image!, ToModel(portfolio));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Available/{id}")]
    public async Task<IActionResult> Update(int id, string available)
    {
        try
        {
            var result = await _portfolioService.UpdateAsync(id, available);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
    private Models.FreelancerPortfolio ToModel(FreelancerPortfolio portfolio) => new()
    {
        FirstName = portfolio.FirstName,
        LastName = portfolio.LastName,
        Price = portfolio.Price,
        PositionId = portfolio.PositionId,
    };
}