using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;
 
public class AssuntosController(IAssuntoService assuntoService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await assuntoService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await assuntoService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarAssuntoRequest request)
    {
        var resultado = await assuntoService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarAssuntoRequest request)
    {
        var resultado = await assuntoService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await assuntoService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
