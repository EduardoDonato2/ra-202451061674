using CryptoTracker.Api.Models;
using CryptoTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarteirasController : ControllerBase
{
    private readonly ICarteiraService _carteiraService;

    public CarteirasController(ICarteiraService carteiraService)
    {
        _carteiraService = carteiraService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CarteiraCripto>>> ObterTodas()
    {
        var carteiras = await _carteiraService.ObterTodasAsync();
        return Ok(carteiras); // HTTP 200
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarteiraCripto>> ObterPorId(string id)
    {
        var carteira = await _carteiraService.ObterPorIdAsync(id);

        if (carteira is null)
            return NotFound(); // HTTP 404

        return Ok(carteira); // HTTP 200
    }

    [HttpPost]
    public async Task<ActionResult<CarteiraCripto>> Criar(CarteiraCripto carteira)
    {
        try
        {
            var novaCarteira = await _carteiraService.CriarAsync(carteira);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaCarteira.Id }, novaCarteira); // HTTP 201
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // HTTP 400
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(string id, CarteiraCripto carteira)
    {
        try
        {
            await _carteiraService.AtualizarAsync(id, carteira);
            return NoContent(); // HTTP 204
        }
        catch (KeyNotFoundException)
        {
            return NotFound(); // HTTP 404
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(string id)
    {
        try
        {
            await _carteiraService.RemoverAsync(id);
            return NoContent(); // HTTP 204
        }
        catch (KeyNotFoundException)
        {
            return NotFound(); // HTTP 404
        }
    }
}