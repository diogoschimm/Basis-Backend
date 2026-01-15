using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;

namespace CadastroLivros.Core.Contracts.Repositories;

public interface ILivroRepository : IRepositoryBase<Livro>
{
    Task<Livro?> BuscarPorCodigoAsync(int codigo);
    Task<(List<Livro> items, int totalCount)> BuscarTodosAsync(int pageNumber, int pageSize);
    
    // Métodos para LivroAutor
    Task<List<int>> BuscarAutoresCodigosAsync(int livroCodigo);
    Task AdicionarAutoresAsync(List<LivroAutor> livroAutores);
    Task RemoverAutoresAsync(int livroCodigo, List<int> autoresCodigos);
    
    // Métodos para LivroAssunto
    Task<List<int>> BuscarAssuntosCodigosAsync(int livroCodigo);
    Task AdicionarAssuntosAsync(List<LivroAssunto> livroAssuntos);
    Task RemoverAssuntosAsync(int livroCodigo, List<int> assuntosCodigos);
    
    // Métodos para LivroFormaCompra
    Task<List<int>> BuscarFormasCompraCodigosAsync(int livroCodigo);
    Task AdicionarFormasCompraAsync(List<LivroFormaCompra> livroFormasCompra);
    Task RemoverFormasCompraAsync(int livroCodigo, List<int> formasCompraCodigos);
}