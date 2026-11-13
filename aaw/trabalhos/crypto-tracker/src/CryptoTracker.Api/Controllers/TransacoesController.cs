using CryptoTracker.Api.Models;
using CryptoTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace CryptoTracker.Api.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransacaoAtivo>>> ObterTodas()
    {
        var transacoes = await _transacaoService.ObterTodasAsync();
        return Ok(transacoes);
    }

    [HttpGet("carteira/{carteiraId}")]
    public async Task<ActionResult<List<TransacaoAtivo>>> ObterPorCarteira(string carteiraId)
    {
        var transacoes = await _transacaoService.ObterPorCarteiraIdAsync(carteiraId);
        return Ok(transacoes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransacaoAtivo>> ObterPorId(string id)
    {
        var transacao = await _transacaoService.ObterPorIdAsync(id);
        if (transacao is null)
            return NotFound();

        return Ok(transacao);
    }

    [HttpPost]
    public async Task<ActionResult<TransacaoAtivo>> Registrar(TransacaoAtivo transacao)
    {
        try
        {
            var novaTransacao = await _transacaoService.RegistrarTransacaoAsync(transacao);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaTransacao.Id }, novaTransacao);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(string id)
    {
        try
        {
            await _transacaoService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}