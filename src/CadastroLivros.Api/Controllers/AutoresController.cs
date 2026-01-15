using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Autores;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;
 
public class AutoresController(IAutorService autorService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await autorService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await autorService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarAutorRequest request)
    {
        var resultado = await autorService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarAutorRequest request)
    {
        var resultado = await autorService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await autorService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
