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
    IFormaCompraRepository formaCompraRepository,
    IUnitOfWork unitOfWork) : ILivroService
{
    public async Task<ErrorOr<PagedResult<LivroResponse>>> GetAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
            return Error.Validation("Livro.PaginaInvalida", "O número da página deve ser maior que zero");

        if (pageSize < 1 || pageSize > 10000)
            return Error.Validation("Livro.TamanhoPaginaInvalido", "O tamanho da página deve estar entre 1 e 10000");

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

        var autoresTask = livroRepository.BuscarAutoresCompletosAsync(codigo);
        var assuntosTask = livroRepository.BuscarAssuntosCompletosAsync(codigo);
        var formasCompraTask = livroRepository.BuscarFormasCompraCompletasAsync(codigo);

        await Task.WhenAll(autoresTask, assuntosTask, formasCompraTask);

        var autoresData = autoresTask.Result;
        var assuntosData = assuntosTask.Result;
        var formasCompraData = formasCompraTask.Result;

        var autores = autoresData.Select(a => new AutorResponse
        {
            Codigo = a.AutorCodigo,
            Nome = a.AutorNome
        }).ToList();

        var assuntos = assuntosData.Select(a => new AssuntoResponse
        {
            Codigo = a.AssuntoCodigo,
            Descricao = a.AssuntoDescricao
        }).ToList();

        var formasCompra = formasCompraData.Select(fc => new FormaCompraItemResponse
        {
            FormaCompraCodigo = fc.FormaCompraCodigo,
            ValorCompra = fc.ValorCompra,
            Descricao = fc.FormaCompraDescricao
        }).ToList();

        return new LivroResponse
        {
            Codigo = livro.Codigo,
            Titulo = livro.Titulo,
            Editora = livro.Editora,
            Edicao = livro.Edicao,
            AnoPublicacao = livro.AnoPublicacao,
            Autores = autores.Count > 0 ? autores : null,
            Assuntos = assuntos.Count > 0 ? assuntos : null,
            FormasCompra = formasCompra.Count > 0 ? formasCompra : null
        };
    }

    public async Task<ErrorOr<LivroResponse>> AdicionarAsync(CriarLivroRequest request)
    {
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

        if (request.FormasCompra != null && request.FormasCompra.Count != 0)
        {
            foreach (var formaCompraItem in request.FormasCompra)
            {
                var formaCompra = await formaCompraRepository.BuscarPorCodigoAsync(formaCompraItem.FormaCompraCodigo);
                if (formaCompra == null)
                    return Error.NotFound("FormaCompra.NaoEncontrada", $"Forma de compra com código {formaCompraItem.FormaCompraCodigo} não encontrada");
            }
        }

        var livro = new Livro
        {
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

        if (request.FormasCompra != null && request.FormasCompra.Count != 0)
        {
            livro.LivroFormasCompra = [.. request.FormasCompra.Select(fc => new LivroFormaCompra
            {
                LivroCodigo = livro.Codigo,
                FormaCompraCodigo = fc.FormaCompraCodigo,
                ValorCompra = fc.ValorCompra
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

        await livroRepository.UpdateAsync(livroExistente);
        await unitOfWork.SaveChangesAsync();

        return livroExistente.ToResponse();
    }

    public async Task<ErrorOr<bool>> RemoverAsync(int codigo)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(codigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {codigo} não encontrado");

        await livroRepository.DeleteAsync(livro);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ErrorOr<LivroResponse>> AdicionarAutoresAsync(AdicionarAutoresRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Validar se os autores existem
        foreach (var autorCodigo in request.AutoresCodigos)
        {
            var autor = await autorRepository.BuscarPorCodigoAsync(autorCodigo);
            if (autor == null)
                return Error.NotFound("Autor.NaoEncontrado", $"Autor com código {autorCodigo} não encontrado");
        }

        // Verificar quais autores já estão associados ao livro
        var autoresExistentes = await livroRepository.BuscarAutoresCodigosAsync(request.LivroCodigo);

        // Filtrar apenas os autores que ainda não estão associados
        var novosAutoresCodigos = request.AutoresCodigos
            .Where(ac => !autoresExistentes.Contains(ac))
            .ToList();

        if (novosAutoresCodigos.Count == 0)
            return Error.Validation("Livro.AutoresJaAdicionados", "Todos os autores informados já estão associados ao livro");

        // Adicionar novos relacionamentos
        var novosLivroAutores = novosAutoresCodigos.Select(autorCodigo => new LivroAutor
        {
            LivroCodigo = request.LivroCodigo,
            AutorCodigo = autorCodigo
        }).ToList();

        await livroRepository.AdicionarAutoresAsync(novosLivroAutores);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> AdicionarAssuntosAsync(AdicionarAssuntosRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Validar se os assuntos existem
        foreach (var assuntoCodigo in request.AssuntosCodigos)
        {
            var assunto = await assuntoRepository.BuscarPorCodigoAsync(assuntoCodigo);
            if (assunto == null)
                return Error.NotFound("Assunto.NaoEncontrado", $"Assunto com código {assuntoCodigo} não encontrado");
        }

        // Verificar quais assuntos já estão associados ao livro
        var assuntosExistentes = await livroRepository.BuscarAssuntosCodigosAsync(request.LivroCodigo);

        // Filtrar apenas os assuntos que ainda não estão associados
        var novosAssuntosCodigos = request.AssuntosCodigos
            .Where(ac => !assuntosExistentes.Contains(ac))
            .ToList();

        if (novosAssuntosCodigos.Count == 0)
            return Error.Validation("Livro.AssuntosJaAdicionados", "Todos os assuntos informados já estão associados ao livro");

        // Adicionar novos relacionamentos
        var novosLivroAssuntos = novosAssuntosCodigos.Select(assuntoCodigo => new LivroAssunto
        {
            LivroCodigo = request.LivroCodigo,
            AssuntoCodigo = assuntoCodigo
        }).ToList();

        await livroRepository.AdicionarAssuntosAsync(novosLivroAssuntos);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> RemoverAutoresAsync(RemoverAutoresRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Verificar quais autores estão associados ao livro
        var autoresExistentes = await livroRepository.BuscarAutoresCodigosAsync(request.LivroCodigo);

        // Filtrar apenas os autores que estão associados
        var autoresParaRemover = request.AutoresCodigos
            .Where(ac => autoresExistentes.Contains(ac))
            .ToList();

        if (autoresParaRemover.Count == 0)
            return Error.Validation("Livro.AutoresNaoEncontrados", "Nenhum dos autores informados está associado ao livro");

        // Remover os relacionamentos
        await livroRepository.RemoverAutoresAsync(request.LivroCodigo, autoresParaRemover);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> RemoverAssuntosAsync(RemoverAssuntosRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Verificar quais assuntos estão associados ao livro
        var assuntosExistentes = await livroRepository.BuscarAssuntosCodigosAsync(request.LivroCodigo);

        // Filtrar apenas os assuntos que estão associados
        var assuntosParaRemover = request.AssuntosCodigos
            .Where(ac => assuntosExistentes.Contains(ac))
            .ToList();

        if (assuntosParaRemover.Count == 0)
            return Error.Validation("Livro.AssuntosNaoEncontrados", "Nenhum dos assuntos informados está associado ao livro");

        // Remover os relacionamentos
        await livroRepository.RemoverAssuntosAsync(request.LivroCodigo, assuntosParaRemover);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> AdicionarFormasCompraAsync(AdicionarFormasCompraRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Verificar quais formas de compra já estão associadas ao livro
        var formasCompraExistentes = await livroRepository.BuscarFormasCompraCodigosAsync(request.LivroCodigo);

        // Filtrar apenas as formas de compra que ainda não estão associadas
        var novasFormasCompra = request.FormasCompra
            .Where(fc => !formasCompraExistentes.Contains(fc.FormaCompraCodigo))
            .ToList();

        if (novasFormasCompra.Count == 0)
            return Error.Validation("Livro.FormasCompraJaAdicionadas", "Todas as formas de compra informadas já estão associadas ao livro");

        // Adicionar novos relacionamentos
        var novosLivroFormasCompra = novasFormasCompra.Select(fc => new LivroFormaCompra
        {
            LivroCodigo = request.LivroCodigo,
            FormaCompraCodigo = fc.FormaCompraCodigo,
            ValorCompra = fc.ValorCompra
        }).ToList();

        await livroRepository.AdicionarFormasCompraAsync(novosLivroFormasCompra);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }

    public async Task<ErrorOr<LivroResponse>> RemoverFormasCompraAsync(RemoverFormasCompraRequest request)
    {
        var livro = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        if (livro == null)
            return Error.NotFound("Livro.NaoEncontrado", $"Livro com código {request.LivroCodigo} não encontrado");

        // Verificar quais formas de compra estão associadas ao livro
        var formasCompraExistentes = await livroRepository.BuscarFormasCompraCodigosAsync(request.LivroCodigo);

        // Filtrar apenas as formas de compra que estão associadas
        var formasCompraParaRemover = request.FormasCompraCodigos
            .Where(fc => formasCompraExistentes.Contains(fc))
            .ToList();

        if (formasCompraParaRemover.Count == 0)
            return Error.Validation("Livro.FormasCompraNaoEncontradas", "Nenhuma das formas de compra informadas está associada ao livro");

        // Remover os relacionamentos
        await livroRepository.RemoverFormasCompraAsync(request.LivroCodigo, formasCompraParaRemover);
        await unitOfWork.SaveChangesAsync();

        // Buscar o livro atualizado para retornar
        var livroAtualizado = await livroRepository.BuscarPorCodigoAsync(request.LivroCodigo);
        return livroAtualizado!.ToResponse();
    }
}
