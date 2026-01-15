using CadastroLivros.Core.Bases;

namespace CadastroLivros.Core.Entities;

public class Autor : Entity
{
    public required int Codigo { get; set; }
    public required string Nome { get; set; }
}
