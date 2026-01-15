using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.Contracts.Services;
using CadastroLivros.Core.DataTransferObjects.Requests.Livros;
using CadastroLivros.Core.DataTransferObjects.Responses;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using CadastroLivros.Core.Helpers.Extensions;
using ErrorOr;

namespace CadastroLivros.Core.Services;

public class LivroService(
    ILivroRepository livroRepository,
    IAutorRepository autorRepository,
    IAssuntoRepository assuntoRepository,
    IUnitOfWork unitOfWork) : ILivroService
{
    public async Task<ErrorOr<PagedResult<LivroResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
            return Error.Validation("Livro.PaginaInvalida", "O número da página deve ser maior que zero");

        if (pageSize < 1 || pageSize > 100)
            return Error.Validation("Livro.TamanhoPaginaInvalido", "O tamanho da página deve estar entre 1 e 100");

        var (items, totalCount) = await livroRepository.BuscarTodosAsync(pageNumber, pageSize);

        var response = new PagedResult<LivroResponse>
        {
            Items = [.. items.Select(l => l.ToResponse())],
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return response;
    }

    public async Task<ErrorOr<LivroResponse>> GetAsync(int codigo)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(codigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {codigo} não encontrado");

        return livro.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> AdicionarAsync(CriarLivroRequest request)
    {
        var livroExistente = await livroRepository.BuscarPorCodigoAsync(request.Codigo);
        if (livroExistente != null)
            return Error.Conflict("Livro.JaExiste", $"Já existe um livro com o código {request.Codigo}");
         
        if (request.AutoresCodigos != null && request.AutoresCodigos.Count != 0)
        {
            foreach (var autorCodigo in request.AutoresCodigos)
            {
                var autor = await autorRepository.BuscarPorCodigoAsync(autorCodigo);
                if (autor == null)
                    return Error.NotFound("Autor.NaoEncontrado", $"Autor com código {autorCodigo} não encontrado");
            }
        }
         
        if (request.AssuntosCodigos != null && request.AssuntosCodigos.Count != 0)
        {
            foreach (var assuntoCodigo in request.AssuntosCodigos)
            {
                var assunto = await assuntoRepository.BuscarPorCodigoAsync(assuntoCodigo);
                if (assunto == null)
                    return Error.NotFound("Assunto.NaoEncontrado", $"Assunto com código {assuntoCodigo} não encontrado");
            }
        }

        var livro = new Livro
        {
            Codigo = request.Codigo,
            Titulo = request.Titulo,
            Editora = request.Editora,
            Edicao = request.Edicao,
            AnoPublicacao = request.AnoPublicacao
        };

        if (request.AutoresCodigos != null && request.AutoresCodigos.Count != 0)
        {
            livro.LivroAutores = [.. request.AutoresCodigos.Select(autorCodigo => new LivroAutor
            {
                LivroCodigo = livro.Codigo,
                AutorCodigo = autorCodigo
            })];
        }

        if (request.AssuntosCodigos != null && request.AssuntosCodigos.Count != 0)
        {
            livro.LivroAssuntos = [.. request.AssuntosCodigos.Select(assuntoCodigo => new LivroAssunto
            {
                LivroCodigo = livro.Codigo,
                AssuntoCodigo = assuntoCodigo
            })];
        }

        await livroRepository.AddAsync(livro);
        await unitOfWork.SaveChangesAsync();

        return livro.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> AtualizarAsync(AtualizarLivroRequest request)
    {
        var livroExistente = await livroRepository.BuscarPorCodigoAsync(request.Codigo);
        if (livroExistente == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.Codigo} não encontrado");

        livroExistente.Titulo = request.Titulo;
        livroExistente.Editora = request.Editora;
        livroExistente.Edicao = request.Edicao;
        livroExistente.AnoPublicacao = request.AnoPublicacao;

        try
        {
            await unitOfWork.BeginTransactionAsync();
            await livroRepository.UpdateAsync(livroExistente);
            await unitOfWork.CommitTransactionAsync();

            return livroExistente.ToResponse();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ErrorOr<bool>> RemoverAsync(int codigo)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(codigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {codigo} não encontrado");

        try
        {
            await unitOfWork.BeginTransactionAsync();
            await livroRepository.DeleteAsync(livro);
            await unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
