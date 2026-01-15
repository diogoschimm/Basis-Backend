using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.FormasCompra;
using CadastroLivros.Core.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de formas de compra
/// </summary>
public class FormasCompraController(IFormaCompraService formaCompraService) : ApiControllerBase
{
    /// <summary>
    /// Lista todas as formas de compra com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
    /// <returns>Lista paginada de formas de compra</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FormaCompraResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await formaCompraService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Obtém uma forma de compra específica pelo código
    /// </summary>
    /// <param name="codigo">Código da forma de compra</param>
    /// <returns>Dados da forma de compra</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(FormaCompraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await formaCompraService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Cria uma nova forma de compra
    /// </summary>
    /// <param name="request">Dados da forma de compra a ser criada</param>
    /// <returns>Dados da forma de compra criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FormaCompraResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarFormaCompraRequest request)
    {
        var resultado = await formaCompraService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Atualiza uma forma de compra existente
    /// </summary>
    /// <param name="request">Dados atualizados da forma de compra</param>
    /// <returns>Dados da forma de compra atualizada</returns>
    [HttpPut]
    [ProducesResponseType(typeof(FormaCompraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarFormaCompraRequest request)
    {
        var resultado = await formaCompraService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Remove uma forma de compra
    /// </summary>
    /// <param name="codigo">Código da forma de compra a ser removida</param>
    /// <returns>Sem conteúdo</returns>
    [HttpDelete("{codigo}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await formaCompraService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}

