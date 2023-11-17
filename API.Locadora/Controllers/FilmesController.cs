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
    public class FilmesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Filmes
        public async Task<IActionResult> Index()
        {
            //foreach (var item in listFilmes)
            //{
            //    item.Genero = await _context.Genero.FindAsync(item.GeneroId);
            //}
            return View(_context.Filme.Include(x => x.Genero).ToList());
        }

        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.Include(x => x.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }
            else

                return View(filme);
        }

        // GET: Filmes/Create
        public IActionResult Create()
        {
            List<Genero> generos = this._context.Genero.ToList();
            ViewBag.Generos = generos;
            return View();
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataCriacao,GeneroId,Ativo")] Filme filme)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Add(filme);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log do erro ou outras ações necessárias
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro durante a inserção.");

                        return RedirectToAction(nameof(Index));

                    }
                }
            }
            return View(filme);
        }

        // GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.Include(x => x.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }
            List<Genero> generos = this._context.Genero.ToList();

            ViewBag.Generos = generos;
            return View(filme);
        }

        // POST: Filmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataCriacao,GeneroId,Ativo")] Filme filme)
        {
            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(filme);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                        transaction.Rollback();
                        if (!FilmeExists(filme.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log do erro ou outras ações necessárias
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro durante a inserção.");

                        return RedirectToAction(nameof(Index));

                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(filme);
        }

        // GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.Include(x => x.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var filme = await _context.Filme.FindAsync(id);
                    _context.Filme.Remove(filme);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log do erro ou outras ações necessárias
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro durante a inserção.");

                    return RedirectToAction(nameof(Index));

                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSelecteds(int[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Length > 0)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var id in selectedIds)
                        {
                            var filme = await _context.Filme.FindAsync(id);
                            if (filme != null)
                            {
                                _context.Filme.Remove(filme);
                            }
                        }
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log do erro ou outras ações necessárias
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro durante a inserção.");

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private bool FilmeExists(int id)
        {
            return _context.Filme.Any(e => e.Id == id);
        }
    }
}
