using AutoBogus;
using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.DataTransferObjects.Requests.Livros;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Services;
using ErrorOr;
using Moq;

namespace CadastroLivros.UnitTests.Core.Services;

public class LivroServiceTests
{
    private readonly Mock<ILivroRepository> _livroRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly LivroService _livroService;

    public LivroServiceTests()
    {
        _livroRepositoryMock = new Mock<ILivroRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _livroService = new LivroService(_livroRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    [Trait("LivroServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaValida_DeveRetornarPagedResult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var livros = AutoFaker.Generate<Livro>(5);
        var totalCount = 50;

        _livroRepositoryMock
            .Setup(x => x.BuscarTodosAsync(pageNumber, pageSize))
            .ReturnsAsync((livros, totalCount));

        // Act
        var resultado = await _livroService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(pageNumber, resultado.Value.PageNumber);
        Assert.Equal(pageSize, resultado.Value.PageSize);
        Assert.Equal(totalCount, resultado.Value.TotalCount);
        Assert.Equal(livros.Count, resultado.Value.Items.Count);
        _livroRepositoryMock.Verify(x => x.BuscarTodosAsync(pageNumber, pageSize), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaInvalida_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 0;
        var pageSize = 10;

        // Act
        var resultado = await _livroService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "GetAsync")]
    public async Task GetAsync_ComPageSizeInvalido_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 101;

        // Act
        var resultado = await _livroService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoExistente_DeveRetornarLivro()
    {
        // Arrange
        var codigo = 1;
        var livro = AutoFaker.Generate<Livro>();
        livro.Codigo = codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(livro);

        // Act
        var resultado = await _livroService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(codigo, resultado.Value.Codigo);
        Assert.Equal(livro.Titulo, resultado.Value.Titulo);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Livro?)null);

        // Act
        var resultado = await _livroService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComLivroNovo_DeveAdicionarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarLivroRequest>();

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Livro?)null);

        _livroRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Livro>()))
            .ReturnsAsync((Livro livro) => livro);

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _livroService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Titulo, resultado.Value.Titulo);
        Assert.Equal(request.Editora, resultado.Value.Editora);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Livro>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComCodigoExistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarLivroRequest>();
        var livroExistente = AutoFaker.Generate<Livro>();
        livroExistente.Codigo = request.Codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(livroExistente);

        // Act
        var resultado = await _livroService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Conflict, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Livro>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComExcecao_DeveFazerRollback()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarLivroRequest>();

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Livro?)null);

        _livroRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Livro>()))
            .ThrowsAsync(new Exception("Erro ao adicionar"));

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _livroService.AdicionarAsync(request));
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComLivroExistente_DeveAtualizarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarLivroRequest>();
        var livroExistente = AutoFaker.Generate<Livro>();
        livroExistente.Codigo = request.Codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(livroExistente);

        _livroRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Livro>()))
            .ReturnsAsync((Livro livro) => livro);

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _livroService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Titulo, resultado.Value.Titulo);
        Assert.Equal(request.Editora, resultado.Value.Editora);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Livro>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarLivroRequest>();

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Livro?)null);

        // Act
        var resultado = await _livroService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Livro>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComExcecao_DeveFazerRollback()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarLivroRequest>();
        var livroExistente = AutoFaker.Generate<Livro>();
        livroExistente.Codigo = request.Codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(livroExistente);

        _livroRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Livro>()))
            .ThrowsAsync(new Exception("Erro ao atualizar"));

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _livroService.AtualizarAsync(request));
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComLivroExistente_DeveRemoverComSucesso()
    {
        // Arrange
        var codigo = 1;
        var livro = AutoFaker.Generate<Livro>();
        livro.Codigo = codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(livro);

        _livroRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Livro>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _livroService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.True(resultado.Value);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Livro>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("LivroServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Livro?)null);

        // Act
        var resultado = await _livroService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _livroRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _livroRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Livro>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("LivroServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComExcecao_DeveFazerRollback()
    {
        // Arrange
        var codigo = 1;
        var livro = AutoFaker.Generate<Livro>();
        livro.Codigo = codigo;

        _livroRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(livro);

        _livroRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Livro>()))
            .ThrowsAsync(new Exception("Erro ao remover"));

        _unitOfWorkMock
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _livroService.RemoverAsync(codigo));
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
