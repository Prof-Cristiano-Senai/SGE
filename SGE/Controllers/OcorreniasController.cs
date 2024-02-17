using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGE.Data;
using SGE.Models;

namespace SGE.Controllers
{
    public class OcorreniasController : Controller
    {
        private readonly SGEContext _context;

        public OcorreniasController(SGEContext context)
        {
            _context = context;
        }

        // GET: Ocorrenias
        public async Task<IActionResult> Index()
        {
            var sGEContext = _context.Ocorrencias.Include(o => o.TipoOcorrencia).Include(o => o.Usuario);
            return View(await sGEContext.ToListAsync());
        }

        // GET: Ocorrenias/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocorrenia = await _context.Ocorrencias
                .Include(o => o.TipoOcorrencia)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.OcorreniaId == id);
            if (ocorrenia == null)
            {
                return NotFound();
            }

            return View(ocorrenia);
        }

        // GET: Ocorrenias/Create
        public IActionResult Create()
        {
            ViewData["TipoOcorrenciaId"] = new SelectList(_context.TiposOcorrencia, "TipoOcorrenciaId", "TipoOcorrenciaId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Ocorrenias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OcorreniaId,TipoOcorrenciaId,UsuarioId,DataOcorrencia,Descricao,CadAtivo,CadInativo,Finalizado,DataFinalizado")] Ocorrenia ocorrenia)
        {
            if (ModelState.IsValid)
            {
                ocorrenia.OcorreniaId = Guid.NewGuid();
                _context.Add(ocorrenia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoOcorrenciaId"] = new SelectList(_context.TiposOcorrencia, "TipoOcorrenciaId", "TipoOcorrenciaId", ocorrenia.TipoOcorrenciaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ocorrenia.UsuarioId);
            return View(ocorrenia);
        }

        // GET: Ocorrenias/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocorrenia = await _context.Ocorrencias.FindAsync(id);
            if (ocorrenia == null)
            {
                return NotFound();
            }
            ViewData["TipoOcorrenciaId"] = new SelectList(_context.TiposOcorrencia, "TipoOcorrenciaId", "TipoOcorrenciaId", ocorrenia.TipoOcorrenciaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ocorrenia.UsuarioId);
            return View(ocorrenia);
        }

        // POST: Ocorrenias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OcorreniaId,TipoOcorrenciaId,UsuarioId,DataOcorrencia,Descricao,CadAtivo,CadInativo,Finalizado,DataFinalizado")] Ocorrenia ocorrenia)
        {
            if (id != ocorrenia.OcorreniaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ocorrenia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OcorreniaExists(ocorrenia.OcorreniaId))
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
            ViewData["TipoOcorrenciaId"] = new SelectList(_context.TiposOcorrencia, "TipoOcorrenciaId", "TipoOcorrenciaId", ocorrenia.TipoOcorrenciaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", ocorrenia.UsuarioId);
            return View(ocorrenia);
        }

        // GET: Ocorrenias/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocorrenia = await _context.Ocorrencias
                .Include(o => o.TipoOcorrencia)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.OcorreniaId == id);
            if (ocorrenia == null)
            {
                return NotFound();
            }

            return View(ocorrenia);
        }

        // POST: Ocorrenias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ocorrenia = await _context.Ocorrencias.FindAsync(id);
            if (ocorrenia != null)
            {
                _context.Ocorrencias.Remove(ocorrenia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OcorreniaExists(Guid id)
        {
            return _context.Ocorrencias.Any(e => e.OcorreniaId == id);
        }
    }
}
