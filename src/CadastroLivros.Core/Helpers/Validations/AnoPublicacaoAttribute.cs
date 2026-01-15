using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Core.Helpers.Validations;

public class AnoPublicacaoAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is not string anoString)
            return false;

        if (!int.TryParse(anoString, out var ano))
            return false;

        var anoAtual = DateTime.Now.Year;
        return ano >= 1000 && ano <= anoAtual;
    }

    public override string FormatErrorMessage(string name)
    {
        var anoAtual = DateTime.Now.Year;
        return $"O ano de publicação deve estar entre 1000 e {anoAtual}.";
    }
}

