-- Script para criar o banco de dados UniSystem
-- Este script será executado automaticamente quando o container SQL Server iniciar

-- Criar o banco de dados se não existir
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UniSystem')
BEGIN
    CREATE DATABASE [UniSystem];
    PRINT 'Banco de dados UniSystem criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco de dados UniSystem já existe.';
END

-- Usar o banco UniSystem
USE [UniSystem];

-- Verificar se a tabela Users já existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    PRINT 'Tabela Users não existe. Será criada pelo Entity Framework.';
END
ELSE
BEGIN
    PRINT 'Tabela Users já existe.';
END

PRINT 'Inicialização do banco de dados concluída!';
