using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Autores;
using CadastroLivros.Core.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de autores
/// </summary>
public class AutoresController(IAutorService autorService) : ApiControllerBase
{
    /// <summary>
    /// Lista todos os autores com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
    /// <returns>Lista paginada de autores</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AutorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await autorService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Obtém um autor específico pelo código
    /// </summary>
    /// <param name="codigo">Código do autor</param>
    /// <returns>Dados do autor</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await autorService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Cria um novo autor
    /// </summary>
    /// <param name="request">Dados do autor a ser criado</param>
    /// <returns>Dados do autor criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarAutorRequest request)
    {
        var resultado = await autorService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Atualiza um autor existente
    /// </summary>
    /// <param name="request">Dados atualizados do autor</param>
    /// <returns>Dados do autor atualizado</returns>
    [HttpPut]
    [ProducesResponseType(typeof(AutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarAutorRequest request)
    {
        var resultado = await autorService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Remove um autor
    /// </summary>
    /// <param name="codigo">Código do autor a ser removido</param>
    /// <returns>Sem conteúdo</returns>
    [HttpDelete("{codigo}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await autorService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
