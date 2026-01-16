using CadastroLivros.Core.Bases;

namespace CadastroLivros.Core.Entities;

public class FormaCompra : Entity
{
    public int Codigo { get; set; }
    public required string Descricao { get; set; }
}
