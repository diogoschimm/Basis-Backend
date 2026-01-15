-- Script para criar as tabelas do banco de dados DbLivros
-- Execute este script após criar o banco de dados (01_create_database.sql)

USE DbLivros;
GO

-- =============================================
-- Tabela: Autor
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Autor')
BEGIN
    CREATE TABLE Autor (
        Codigo INT NOT NULL,
        Nome VARCHAR(40) NOT NULL,
        CONSTRAINT PK_Autor PRIMARY KEY (Codigo)
    );
    PRINT 'Tabela Autor criada com sucesso!';
END
GO

-- =============================================
-- Tabela: Assunto
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Assunto')
BEGIN
    CREATE TABLE Assunto (
        Codigo INT NOT NULL,
        Descricao VARCHAR(40) NOT NULL,
        CONSTRAINT PK_Assunto PRIMARY KEY (Codigo)
    );
    PRINT 'Tabela Assunto criada com sucesso!';
END
GO

-- =============================================
-- Tabela: FormaCompra
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FormaCompra')
BEGIN
    CREATE TABLE FormaCompra (
        Codigo INT NOT NULL,
        Descricao VARCHAR(40) NOT NULL,
        CONSTRAINT PK_FormaCompra PRIMARY KEY (Codigo)
    );
    PRINT 'Tabela FormaCompra criada com sucesso!';
END
GO

-- =============================================
-- Tabela: Livro
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Livro')
BEGIN
    CREATE TABLE Livro (
        Codigo INT NOT NULL,
        Titulo VARCHAR(40) NOT NULL,
        Editora VARCHAR(40) NOT NULL,
        Edicao INT NOT NULL,
        AnoPublicacao VARCHAR(4) NOT NULL,
        CONSTRAINT PK_Livro PRIMARY KEY (Codigo)
    );
    PRINT 'Tabela Livro criada com sucesso!';
END
GO

-- =============================================
-- Tabela: LivroAutor (Relacionamento N:N entre Livro e Autor)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LivroAutor')
BEGIN
    CREATE TABLE LivroAutor (
        LivroCodigo INT NOT NULL,
        AutorCodigo INT NOT NULL,
        CONSTRAINT PK_LivroAutor PRIMARY KEY (LivroCodigo, AutorCodigo),
        CONSTRAINT FK_LivroAutor_Livro FOREIGN KEY (LivroCodigo) 
            REFERENCES Livro(Codigo) ON DELETE CASCADE,
        CONSTRAINT FK_LivroAutor_Autor FOREIGN KEY (AutorCodigo) 
            REFERENCES Autor(Codigo)
    );
    PRINT 'Tabela LivroAutor criada com sucesso!';
END
GO

-- =============================================
-- Tabela: LivroAssunto (Relacionamento N:N entre Livro e Assunto)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LivroAssunto')
BEGIN
    CREATE TABLE LivroAssunto (
        LivroCodigo INT NOT NULL,
        AssuntoCodigo INT NOT NULL,
        CONSTRAINT PK_LivroAssunto PRIMARY KEY (LivroCodigo, AssuntoCodigo),
        CONSTRAINT FK_LivroAssunto_Livro FOREIGN KEY (LivroCodigo) 
            REFERENCES Livro(Codigo) ON DELETE CASCADE,
        CONSTRAINT FK_LivroAssunto_Assunto FOREIGN KEY (AssuntoCodigo) 
            REFERENCES Assunto(Codigo)
    );
    PRINT 'Tabela LivroAssunto criada com sucesso!';
END
GO

-- =============================================
-- Tabela: LivroFormaCompra (Relacionamento N:N entre Livro e FormaCompra com valor)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LivroFormaCompra')
BEGIN
    CREATE TABLE LivroFormaCompra (
        LivroCodigo INT NOT NULL,
        FormaCompraCodigo INT NOT NULL,
        ValorCompra DECIMAL(18, 2) NOT NULL,
        CONSTRAINT PK_LivroFormaCompra PRIMARY KEY (LivroCodigo, FormaCompraCodigo),
        CONSTRAINT FK_LivroFormaCompra_Livro FOREIGN KEY (LivroCodigo) 
            REFERENCES Livro(Codigo) ON DELETE CASCADE,
        CONSTRAINT FK_LivroFormaCompra_FormaCompra FOREIGN KEY (FormaCompraCodigo) 
            REFERENCES FormaCompra(Codigo)
    );
    PRINT 'Tabela LivroFormaCompra criada com sucesso!';
END
GO

PRINT 'Todas as tabelas foram criadas com sucesso!';
GO

