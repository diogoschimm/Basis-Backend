using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.DataTransferObjects.DataResults;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface ILivroRepository : IRepositoryBase<Livro>
{
    Task<Livro?> BuscarPorCodigoAsync(int codigo);
    Task<(List<Livro> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
    
    // LivroAutor
    Task<List<int>> BuscarAutoresCodigosAsync(int livroCodigo);
    Task AdicionarAutoresAsync(List<LivroAutor> livroAutores);
    Task RemoverAutoresAsync(int livroCodigo, List<int> autoresCodigos);
    
    // LivroAssunto
    Task<List<int>> BuscarAssuntosCodigosAsync(int livroCodigo);
    Task AdicionarAssuntosAsync(List<LivroAssunto> livroAssuntos);
    Task RemoverAssuntosAsync(int livroCodigo, List<int> assuntosCodigos);
    
    // LivroFormaCompra
    Task<List<int>> BuscarFormasCompraCodigosAsync(int livroCodigo);
    Task AdicionarFormasCompraAsync(List<LivroFormaCompra> livroFormasCompra);
    Task RemoverFormasCompraAsync(int livroCodigo, List<int> formasCompraCodigos);
    
    // Métodos para buscar dados completos
    Task<List<LivroAutorDataResult>> BuscarAutoresCompletosAsync(int livroCodigo);
    Task<List<LivroAssuntoDataResult>> BuscarAssuntosCompletosAsync(int livroCodigo);
    Task<List<LivroFormaCompraDataResult>> BuscarFormasCompraCompletasAsync(int livroCodigo);
}