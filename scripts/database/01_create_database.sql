-- Script para criar o banco de dados DbLivros
-- Execute este script no SQL Server Management Studio ou Azure Data Studio

USE master;
GO

-- Verifica se o banco de dados já existe e remove se necessário
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DbLivros')
BEGIN
    ALTER DATABASE DbLivros SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DbLivros;
END
GO

-- Cria o banco de dados
CREATE DATABASE DbLivros;
GO

-- Seleciona o banco de dados criado
USE DbLivros;
GO

PRINT 'Banco de dados DbLivros criado com sucesso!';
GO

