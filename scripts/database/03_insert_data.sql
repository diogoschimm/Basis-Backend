-- Script para inserir dados iniciais no banco de dados DbLivros
-- Execute este script após criar as tabelas (02_create_tables.sql)

USE DbLivros;
GO

-- =============================================
-- Inserir Formas de Compra
-- =============================================
INSERT INTO FormaCompra (Codigo, Descricao) VALUES (1, 'Balcão');
INSERT INTO FormaCompra (Codigo, Descricao) VALUES (2, 'Self-Service');
INSERT INTO FormaCompra (Codigo, Descricao) VALUES (3, 'Internet');
INSERT INTO FormaCompra (Codigo, Descricao) VALUES (4, 'Evento');
PRINT 'Formas de Compra inseridas com sucesso!';
GO

-- =============================================
-- Inserir Autores
-- =============================================
INSERT INTO Autor (Codigo, Nome) VALUES (1, 'Robert C. Martin');
INSERT INTO Autor (Codigo, Nome) VALUES (2, 'Martin Fowler');
INSERT INTO Autor (Codigo, Nome) VALUES (3, 'Eric Evans');
INSERT INTO Autor (Codigo, Nome) VALUES (4, 'Kent Beck');
INSERT INTO Autor (Codigo, Nome) VALUES (5, 'Erich Gamma');
INSERT INTO Autor (Codigo, Nome) VALUES (6, 'Andrew Hunt');
INSERT INTO Autor (Codigo, Nome) VALUES (7, 'David Thomas');
PRINT 'Autores inseridos com sucesso!';
GO

-- =============================================
-- Inserir Assuntos
-- =============================================
INSERT INTO Assunto (Codigo, Descricao) VALUES (1, 'Desenvolvimento de Software');
INSERT INTO Assunto (Codigo, Descricao) VALUES (2, 'Arquitetura de Software');
INSERT INTO Assunto (Codigo, Descricao) VALUES (3, 'Design Patterns');
INSERT INTO Assunto (Codigo, Descricao) VALUES (4, 'Boas Práticas');
INSERT INTO Assunto (Codigo, Descricao) VALUES (5, 'Metodologias Ágeis');
INSERT INTO Assunto (Codigo, Descricao) VALUES (6, 'Domain-Driven Design');
PRINT 'Assuntos inseridos com sucesso!';
GO

-- =============================================
-- Inserir Livros
-- =============================================
INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (1, 'Código Limpo', 'Alta Books', 1, '2009');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (2, 'Arquitetura Limpa', 'Alta Books', 1, '2018');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (3, 'Refatoração', 'Novatec', 2, '2020');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (4, 'Domain-Driven Design', 'Alta Books', 1, '2016');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (5, 'Padrões de Projeto', 'Bookman', 1, '2000');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (6, 'O Programador Pragmático', 'Alta Books', 2, '2020');

INSERT INTO Livro (Codigo, Titulo, Editora, Edicao, AnoPublicacao) 
VALUES (7, 'TDD - Desenvolvimento Guiado', 'Bookman', 1, '2010');

PRINT 'Livros inseridos com sucesso!';
GO

-- =============================================
-- Inserir Relacionamentos Livro-Autor
-- =============================================
-- Código Limpo - Robert C. Martin
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (1, 1);

-- Arquitetura Limpa - Robert C. Martin
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (2, 1);

-- Refatoração - Martin Fowler
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (3, 2);

-- Domain-Driven Design - Eric Evans
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (4, 3);

-- Padrões de Projeto - Erich Gamma (e outros)
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (5, 5);

-- O Programador Pragmático - Andrew Hunt e David Thomas
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (6, 6);
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (6, 7);

-- TDD - Kent Beck
INSERT INTO LivroAutor (LivroCodigo, AutorCodigo) VALUES (7, 4);

PRINT 'Relacionamentos Livro-Autor inseridos com sucesso!';
GO

-- =============================================
-- Inserir Relacionamentos Livro-Assunto
-- =============================================
-- Código Limpo
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (1, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (1, 4);

-- Arquitetura Limpa
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (2, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (2, 2);

-- Refatoração
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (3, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (3, 4);

-- Domain-Driven Design
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (4, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (4, 2);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (4, 6);

-- Padrões de Projeto
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (5, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (5, 3);

-- O Programador Pragmático
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (6, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (6, 4);

-- TDD
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (7, 1);
INSERT INTO LivroAssunto (LivroCodigo, AssuntoCodigo) VALUES (7, 5);

PRINT 'Relacionamentos Livro-Assunto inseridos com sucesso!';
GO

-- =============================================
-- Inserir Relacionamentos Livro-FormaCompra (com valores)
-- =============================================
-- Código Limpo
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (1, 1, 89.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (1, 2, 84.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (1, 3, 69.90);

-- Arquitetura Limpa
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (2, 1, 99.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (2, 3, 79.90);

-- Refatoração
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (3, 1, 119.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (3, 2, 109.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (3, 3, 89.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (3, 4, 99.90);

-- Domain-Driven Design
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (4, 1, 149.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (4, 3, 129.90);

-- Padrões de Projeto
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (5, 1, 159.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (5, 2, 149.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (5, 3, 139.90);

-- O Programador Pragmático
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (6, 1, 109.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (6, 3, 89.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (6, 4, 79.90);

-- TDD
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (7, 1, 79.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (7, 2, 74.90);
INSERT INTO LivroFormaCompra (LivroCodigo, FormaCompraCodigo, ValorCompra) VALUES (7, 3, 59.90);

PRINT 'Relacionamentos Livro-FormaCompra inseridos com sucesso!';
GO

PRINT 'Todos os dados foram inseridos com sucesso!';
GO

