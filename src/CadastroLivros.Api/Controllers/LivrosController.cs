using CadastroLivros.Api.Controllers.Bases;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Livros;
using CadastroLivros.Core.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de livros
/// </summary>
public class LivrosController(ILivroService livroService) : ApiControllerBase
{
    /// <summary>
    /// Lista todos os livros com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
    /// <returns>Lista paginada de livros</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LivroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await livroService.GetAsync(pageNumber, pageSize);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Obtém um livro específico pelo código
    /// </summary>
    /// <param name="codigo">Código do livro</param>
    /// <returns>Dados do livro</returns>
    [HttpGet("{codigo}")]
    [ProducesResponseType(typeof(LivroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync([FromRoute] int codigo)
    {
        var resultado = await livroService.GetAsync(codigo);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Cria um novo livro
    /// </summary>
    /// <param name="request">Dados do livro a ser criado</param>
    /// <returns>Dados do livro criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(LivroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdicionarAsync([FromBody] CriarLivroRequest request)
    {
        var resultado = await livroService.AdicionarAsync(request);
        
        return resultado.Match(
            value => Created(string.Empty, value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Atualiza um livro existente
    /// </summary>
    /// <param name="request">Dados atualizados do livro</param>
    /// <returns>Dados do livro atualizado</returns>
    [HttpPut]
    [ProducesResponseType(typeof(LivroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarLivroRequest request)
    {
        var resultado = await livroService.AtualizarAsync(request);
        
        return resultado.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Remove um livro
    /// </summary>
    /// <param name="codigo">Código do livro a ser removido</param>
    /// <returns>Sem conteúdo</returns>
    [HttpDelete("{codigo}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoverAsync(int codigo)
    {
        var resultado = await livroService.RemoverAsync(codigo);
        
        return resultado.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}
