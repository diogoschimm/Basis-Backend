using AutoBogus;
using CadastroLivros.Core.Contracts.Bases;
using CadastroLivros.Core.Contracts.Repositories;
using CadastroLivros.Core.DataTransferObjects.Requests.Autores;
using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Services;
using ErrorOr;
using Moq;

namespace CadastroLivros.UnitTests.Core.Services;

public class AutorServiceTests
{
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AutorService _autorService;

    public AutorServiceTests()
    {
        _autorRepositoryMock = new Mock<IAutorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _autorService = new AutorService(_autorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    [Trait("AutorServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaValida_DeveRetornarPagedResult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var autores = AutoFaker.Generate<Autor>(5);
        var totalCount = 50;

        _autorRepositoryMock
            .Setup(x => x.BuscarTodosAsync(pageNumber, pageSize))
            .ReturnsAsync((autores, totalCount));

        // Act
        var resultado = await _autorService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(pageNumber, resultado.Value.PageNumber);
        Assert.Equal(pageSize, resultado.Value.PageSize);
        Assert.Equal(totalCount, resultado.Value.TotalCount);
        Assert.Equal(autores.Count, resultado.Value.Items.Count);
        _autorRepositoryMock.Verify(x => x.BuscarTodosAsync(pageNumber, pageSize), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "GetAsync")]
    public async Task GetAsync_ComPaginaInvalida_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 0;
        var pageSize = 10;

        // Act
        var resultado = await _autorService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("AutorServiceTests", "GetAsync")]
    public async Task GetAsync_ComPageSizeInvalido_DeveRetornarErro()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 100001;

        // Act
        var resultado = await _autorService.GetAsync(pageNumber, pageSize);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Validation, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarTodosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    [Trait("AutorServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoExistente_DeveRetornarAutor()
    {
        // Arrange
        var codigo = 1;
        var autor = AutoFaker.Generate<Autor>();
        autor.Codigo = codigo;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(autor);

        // Act
        var resultado = await _autorService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(codigo, resultado.Value.Codigo);
        Assert.Equal(autor.Nome, resultado.Value.Nome);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "GetAsync")]
    public async Task GetAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Autor?)null);

        // Act
        var resultado = await _autorService.GetAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComAutorNovo_DeveAdicionarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarAutorRequest>();

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Autor?)null);

        _autorRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Autor>()))
            .ReturnsAsync((Autor autor) => autor);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _autorService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Nome, resultado.Value.Nome);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Autor>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "AdicionarAsync")]
    public async Task AdicionarAsync_ComCodigoExistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<CriarAutorRequest>();
        var autorExistente = AutoFaker.Generate<Autor>();
        autorExistente.Codigo = request.Codigo;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(autorExistente);

        // Act
        var resultado = await _autorService.AdicionarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.Conflict, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Autor>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("AutorServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComAutorExistente_DeveAtualizarComSucesso()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarAutorRequest>();
        var autorExistente = AutoFaker.Generate<Autor>();
        autorExistente.Codigo = request.Codigo;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync(autorExistente);

        _autorRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Autor>()))
            .ReturnsAsync((Autor autor) => autor);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _autorService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.NotNull(resultado.Value);
        Assert.Equal(request.Codigo, resultado.Value.Codigo);
        Assert.Equal(request.Nome, resultado.Value.Nome);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Autor>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "AtualizarAsync")]
    public async Task AtualizarAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var request = AutoFaker.Generate<AtualizarAutorRequest>();

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(request.Codigo))
            .ReturnsAsync((Autor?)null);

        // Act
        var resultado = await _autorService.AtualizarAsync(request);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(request.Codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Autor>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait("AutorServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComAutorExistente_DeveRemoverComSucesso()
    {
        // Arrange
        var codigo = 1;
        var autor = AutoFaker.Generate<Autor>();
        autor.Codigo = codigo;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync(autor);

        _autorRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Autor>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _autorService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError == false);
        Assert.True(resultado.Value);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Autor>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("AutorServiceTests", "RemoverAsync")]
    public async Task RemoverAsync_ComCodigoInexistente_DeveRetornarErro()
    {
        // Arrange
        var codigo = 999;

        _autorRepositoryMock
            .Setup(x => x.BuscarPorCodigoAsync(codigo))
            .ReturnsAsync((Autor?)null);

        // Act
        var resultado = await _autorService.RemoverAsync(codigo);

        // Assert
        Assert.True(resultado.IsError);
        Assert.Equal(ErrorType.NotFound, resultado.FirstError.Type);
        _autorRepositoryMock.Verify(x => x.BuscarPorCodigoAsync(codigo), Times.Once);
        _autorRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Autor>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

