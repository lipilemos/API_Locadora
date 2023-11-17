using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Locadora.Data;
using API.Locadora.Models;

namespace API.Locadora.Controllers
{
    public class LocacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locacao
        public async Task<IActionResult> Index()
        {            
            return View(this._context.Locacao.Include(c => c.Filmes).ToList());
        }

        // GET: Locacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locacao = await _context.Locacao.Include(f=>f.Filmes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locacao == null)
            {
                return NotFound();
            }

            return View(locacao);
        }

        // GET: Locacao/Create
        public IActionResult Create()
        {
            List<Filme> filmes = this._context.Filme.Where(x=>x.LocacaoId == null).ToList();

            if (filmes.Count > 0)
            {
                ViewBag.Filmes = filmes;
                ViewData["Filmes"] = new SelectList(filmes, "Id", "Nome");
            }
            else
            {
                filmes.Add(new Filme(){ Id=0,Nome="--SEM FILMES DISPONIVEIS--" });
                ViewBag.Filmes = filmes;
                ViewData["Filmes"] = new SelectList(filmes, "Id", "Nome");
            }
            return View();
        }

        // POST: Locacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CpfCliente,Filmes,DataLocacao")] Locacao locacao, string films)
        {
            string[] listaFilmes = films?.Split(",");
            if (ModelState.IsValid && listaFilmes?.Length > 0 && !listaFilmes.Contains("0"))
            {                
                _context.Add(locacao);
                await _context.SaveChangesAsync();
                
                foreach (var item in listaFilmes)
                {
                    var filme = await _context.Filme.FindAsync(Int32.Parse(item));
                    filme.LocacaoId = locacao.Id;

                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: Locacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locacao = await _context.Locacao.Include(f => f.Filmes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locacao == null)
            {
                return NotFound();
            }
            List<Filme> filmes = this._context.Filme.Where(x => x.LocacaoId == null || x.LocacaoId == locacao.Id).ToList();
            ViewBag.Filmes = new SelectList(filmes, "Id", "Nome"); ;

            return View(locacao);
        }

        // POST: Locacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CpfCliente,Filmes,DataLocacao")] Locacao locacao, string films)
        {
            if (id != locacao.Id)
            {
                return NotFound();
            }
            string[] listaFilmes = films?.Split(",");

            if (ModelState.IsValid && listaFilmes?.Length > 0)
            {
                try
                {
                    var filmes = _context.Filme.Where(x => x.LocacaoId == locacao.Id).ToList();
                    foreach (var item in filmes)
                    {
                        item.LocacaoId = null;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var item in listaFilmes)
                    {
                        var filme = await _context.Filme.FindAsync(Int32.Parse(item));
                        filme.LocacaoId = locacao.Id;

                        _context.Update(filme);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(locacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocacaoExists(locacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(locacao);
        }

        // GET: Locacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locacao = await _context.Locacao.Include(f => f.Filmes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locacao == null)
            {
                return NotFound();
            }

            return View(locacao);
        }

        // POST: Locacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locacao = await _context.Locacao.Include(f => f.Filmes)
                .FirstOrDefaultAsync(m => m.Id == id);
            //foreach (var filme in locacao.Filmes)
            //{
            //    filme.LocacaoId = null;
            //    filme.Locacao = null;
            //    _context.Update(filme);
            //    await _context.SaveChangesAsync();
            //}
            locacao.Filmes = new List<Filme>();
            _context.Locacao.Remove(locacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocacaoExists(int id)
        {
            return _context.Locacao.Any(e => e.Id == id);
        }
    }
}
