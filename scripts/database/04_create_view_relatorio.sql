-- Script para criar a view de relatório agrupado por autor
-- Execute este script após inserir os dados (03_insert_data.sql)

USE DbLivros;
GO

-- =============================================
-- View: Relatório por Autor
-- =============================================
-- Esta view agrupa informações de livros e assuntos por autor
-- Considera que um livro pode ter múltiplos autores
IF EXISTS (SELECT * FROM sys.views WHERE name = 'VW_RelatorioPorAutor')
    DROP VIEW VW_RelatorioPorAutor;
GO

CREATE VIEW VW_RelatorioPorAutor
AS
SELECT 
    a.Codigo AS CodigoAutor,
    a.Nome AS Autor,
    l.Codigo AS CodigoLivro,
    l.Titulo AS TituloLivro,
    l.Editora AS Editora,
    l.Edicao AS Edicao,
    l.AnoPublicacao AS Ano,
    ass.Codigo AS CodigoAssunto,
    ass.Descricao AS AssuntoLivro
FROM 
    Autor a
    INNER JOIN LivroAutor la ON a.Codigo = la.AutorCodigo
    INNER JOIN Livro l ON la.LivroCodigo = l.Codigo
    LEFT JOIN LivroAssunto las ON l.Codigo = las.LivroCodigo
    LEFT JOIN Assunto ass ON las.AssuntoCodigo = ass.Codigo;
GO

PRINT 'View VW_RelatorioPorAutor criada com sucesso!';
GO
 
