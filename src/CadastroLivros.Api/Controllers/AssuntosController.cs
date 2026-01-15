using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;
using CadastroLivros.Core.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de assuntos
/// </summary>
public class AssuntosController(IAssuntoService assuntoService) : ApiControllerBase
{
    /// <summary>
    /// Lista todos os assuntos com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
    /// <returns>Lista paginada de assuntos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AssuntoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await assuntoService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Obtém um assunto específico pelo código
    /// </summary>
    /// <param name="codigo">Código do assunto</param>
    /// <returns>Dados do assunto</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(AssuntoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await assuntoService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Cria um novo assunto
    /// </summary>
    /// <param name="request">Dados do assunto a ser criado</param>
    /// <returns>Dados do assunto criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AssuntoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarAssuntoRequest request)
    {
        var resultado = await assuntoService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Atualiza um assunto existente
    /// </summary>
    /// <param name="request">Dados atualizados do assunto</param>
    /// <returns>Dados do assunto atualizado</returns>
    [HttpPut]
    [ProducesResponseType(typeof(AssuntoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarAssuntoRequest request)
    {
        var resultado = await assuntoService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Remove um assunto
    /// </summary>
    /// <param name="codigo">Código do assunto a ser removido</param>
    /// <returns>Sem conteúdo</returns>
    [HttpDelete("{codigo}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await assuntoService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
