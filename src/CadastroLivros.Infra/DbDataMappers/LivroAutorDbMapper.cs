using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class LivroAutorDbMapper : IEntityTypeConfiguration<LivroAutor>
{
    public void Configure(EntityTypeBuilder<LivroAutor> builder)
    {
        builder.ToTable("LivroAutor");

        builder.HasKey(la => new { la.LivroCodigo, la.AutorCodigo });

        builder.Property(la => la.LivroCodigo)
            .HasColumnName("LivroCodigo")
            .IsRequired();

        builder.Property(la => la.AutorCodigo)
            .HasColumnName("AutorCodigo")
            .IsRequired();

        builder.HasOne<Autor>()
            .WithMany()
            .HasForeignKey(la => la.AutorCodigo);
    }
}

