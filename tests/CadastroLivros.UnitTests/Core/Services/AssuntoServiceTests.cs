using AutoBogus;
using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.DataTransferObjects.Requests.Assuntos;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Services;
using ErrorOr;
using Moq;

namespace CadastroLivros.UnitTests.Core.Services;

public class AssuntoServiceTests
{
    private readonly Mock<IAssuntoRepository> _assuntoRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AssuntoService _assuntoService;

    public AssuntoServiceTests()
    {
        _assuntoRepositoryMock = new Mock<IAssuntoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _assuntoService = new AssuntoService(_assuntoRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaValida_DeveRetornarPagedResult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var assuntos = AutoFaker.Generate<Assunto>(5);
        var totalCount = 50;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarTodosAsync(pageNumber, pageSize))
            .ReturnsAsync((assuntos, totalCount));

        // Act
        var resultado = await _assuntoService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(pageNumber, resultado.Value.PageNumber);
        Assert.Equal(pageSize, resultado.Value.PageSize);
        Assert.Equal(totalCount, resultado.Value.TotalCount);
        Assert.Equal(assuntos.Count, resultado.Value.Items.Count);
        _assuntoRepositoryMock.Verify(x => x.BuscarTodosAsync(pageNumber, pageSize), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaInvalida_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 0;
        var pageSize = 10;

        // Act
        var resultado = await _assuntoService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "GetAsync")]
    public async Task GetAsync_ComPageSizeInvalido_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 100001;

        // Act
        var resultado = await _assuntoService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoExistente_DeveRetornarAssunto()
    {
        // Arrange
        var codigo = 1;
        var assunto = AutoFaker.Generate<Assunto>();
        assunto.Codigo = codigo;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(assunto);

        // Act
        var resultado = await _assuntoService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(codigo, resultado.Value.Codigo);
        Assert.Equal(assunto.Descricao, resultado.Value.Descricao);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Assunto?)null);

        // Act
        var resultado = await _assuntoService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComAssuntoNovo_DeveAdicionarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarAssuntoRequest>();

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Assunto?)null);

        _assuntoRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Assunto>()))
            .ReturnsAsync((Assunto assunto) => assunto);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _assuntoService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Descricao, resultado.Value.Descricao);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Assunto>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComCodigoExistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarAssuntoRequest>();
        var assuntoExistente = AutoFaker.Generate<Assunto>();
        assuntoExistente.Codigo = request.Codigo;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(assuntoExistente);

        // Act
        var resultado = await _assuntoService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Conflict, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Assunto>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComAssuntoExistente_DeveAtualizarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarAssuntoRequest>();
        var assuntoExistente = AutoFaker.Generate<Assunto>();
        assuntoExistente.Codigo = request.Codigo;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(assuntoExistente);

        _assuntoRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Assunto>()))
            .ReturnsAsync((Assunto assunto) => assunto);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _assuntoService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Descricao, resultado.Value.Descricao);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Assunto>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarAssuntoRequest>();

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Assunto?)null);

        // Act
        var resultado = await _assuntoService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Assunto>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComAssuntoExistente_DeveRemoverComSucesso()
    {
        // Arrange
        var codigo = 1;
        var assunto = AutoFaker.Generate<Assunto>();
        assunto.Codigo = codigo;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(assunto);

        _assuntoRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Assunto>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _assuntoService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.True(resultado.Value);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Assunto>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AssuntoServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _assuntoRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Assunto?)null);

        // Act
        var resultado = await _assuntoService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _assuntoRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _assuntoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Assunto>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

