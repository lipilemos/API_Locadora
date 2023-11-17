using API.Locadora.Data;
using API.Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Locadora.Tests
{
    public class FilmeServiceTest
    {
        private readonly ApplicationDbContext _dbContext;

        public FilmeServiceTest(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AdicionarFilme(Filme filme)
        {
            _dbContext.Filme.Add(filme);
            _dbContext.SaveChanges();
        }

        public Filme ObterFilmePorId(int filmeId)
        {
            return _dbContext.Filme.Find(filmeId);
        }
    }
}
