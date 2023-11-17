using API.Locadora.Data;
using API.Locadora.Models;
using API.Locadora.Tests;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class FilmeTests 
{
    [Fact]
    public void AdicionarFilmeDeveAdicionarAoContexto()
    {
        // Arrange
        var dbContextMock = new Mock<ApplicationDbContext>();
        var filme = new Filme { Nome = "Filme Teste", Ativo = true, DataCriacao = DateTime.Now, GeneroId = 1, Genero = new Genero { Id = 1, Nome="Teste Genero", Ativo= true, DataCriacao = DateTime.Now}  };

        dbContextMock.Setup(x => x.Filme.Add(It.IsAny<Filme>()));
        dbContextMock.Setup(x => x.SaveChanges()).Returns(1);

        // Act
        var filmeService = new FilmeServiceTest(dbContextMock.Object);
        filmeService.AdicionarFilme(filme);

        // Assert
        dbContextMock.Verify(x => x.Filme.Add(filme), Times.Once);
        dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void ObterFilmePorIdDeveRetornarFilmeCorreto()
    {
        // Arrange
        var dbContextMock = new Mock<ApplicationDbContext>();
        var filmes = new List<Filme>
        {
            new Filme { Id = 1, Nome = "Filme 1", Ativo = true, DataCriacao = DateTime.Now, GeneroId = 1, Genero = new Genero { Id = 1, Nome="Teste Genero", Ativo= true, DataCriacao = DateTime.Now}  },
            new Filme { Id = 2, Nome = "Filme 2", Ativo = true, DataCriacao = DateTime.Now, GeneroId = 2, Genero = new Genero { Id = 2, Nome="Teste Genero 2", Ativo= true, DataCriacao = DateTime.Now}   }
        }.AsQueryable();

        var filmeDbSetMock = new Mock<DbSet<Filme>>();
        filmeDbSetMock.As<IQueryable<Filme>>().Setup(m => m.Provider).Returns(filmes.Provider);
        filmeDbSetMock.As<IQueryable<Filme>>().Setup(m => m.Expression).Returns(filmes.Expression);
        filmeDbSetMock.As<IQueryable<Filme>>().Setup(m => m.ElementType).Returns(filmes.ElementType);
        filmeDbSetMock.As<IQueryable<Filme>>().Setup(m => m.GetEnumerator()).Returns(filmes.GetEnumerator());

        dbContextMock.Setup(x => x.Filme).Returns(filmeDbSetMock.Object);

        // Act
        var filmeService = new FilmeServiceTest(dbContextMock.Object);
        var resultado = filmeService.ObterFilmePorId(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Filme 1", resultado.Nome);
    }
}