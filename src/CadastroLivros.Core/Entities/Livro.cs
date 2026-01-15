using CadastroLivros.Core.Bases;
using CadastroLivros.Core.Entities.Ternarias;

namespace CadastroLivros.Core.Entities;

public class Livro : Entity
{ 
    public required int Codigo { get; set; }
    public required string Titulo { get; set; }
    public required string Editora { get; set; }
    public required int Edicao { get; set; }
    public required string AnoPublicacao { get; set; }

    public ICollection<LivroAutor> LivroAutores { get; set; } = [];
    public ICollection<LivroAssunto> LivroAssuntos { get; set; } = [];
    public ICollection<LivroFormaCompra> LivroFormasCompra { get; set; } = [];
}