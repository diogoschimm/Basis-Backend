using CadastroLivros.Core.Bases;

namespace CadastroLivros.Core.Entities;

public class Livro : Entity
{ 
    public required int Codigo { get; set; }
    public required string Titulo { get; set; }
    public required string Editora { get; set; }
    public required int Edicao { get; set; }
    public required string AnoPublicacao { get; set; }

}
