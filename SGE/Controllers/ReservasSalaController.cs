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
    public class ReservasSalaController : Controller
    {
        private readonly SGEContext _context;

        public ReservasSalaController(SGEContext context)
        {
            _context = context;
        }

        // GET: ReservasSala
        public async Task<IActionResult> Index()
        {
            var sGEContext = _context.ReservasSala.Include(r => r.Sala).Include(r => r.Usuario);
            return View(await sGEContext.ToListAsync());
        }

        // GET: ReservasSala/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaSala = await _context.ReservasSala
                .Include(r => r.Sala)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ReservaSalaId == id);
            if (reservaSala == null)
            {
                return NotFound();
            }

            return View(reservaSala);
        }

        // GET: ReservasSala/Create
        public IActionResult Create()
        {
            ViewData["SalaId"] = new SelectList(_context.Salas, "SalaId", "SalaId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: ReservasSala/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservaSalaId,SalaId,UsuarioId,DataReserva,HoraInicio,HoraFim,CadAtivo,CadInativo")] ReservaSala reservaSala)
        {
            if (ModelState.IsValid)
            {
                reservaSala.ReservaSalaId = Guid.NewGuid();
                _context.Add(reservaSala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaId"] = new SelectList(_context.Salas, "SalaId", "SalaId", reservaSala.SalaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", reservaSala.UsuarioId);
            return View(reservaSala);
        }

        // GET: ReservasSala/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaSala = await _context.ReservasSala.FindAsync(id);
            if (reservaSala == null)
            {
                return NotFound();
            }
            ViewData["SalaId"] = new SelectList(_context.Salas, "SalaId", "SalaId", reservaSala.SalaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", reservaSala.UsuarioId);
            return View(reservaSala);
        }

        // POST: ReservasSala/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReservaSalaId,SalaId,UsuarioId,DataReserva,HoraInicio,HoraFim,CadAtivo,CadInativo")] ReservaSala reservaSala)
        {
            if (id != reservaSala.ReservaSalaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservaSala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaSalaExists(reservaSala.ReservaSalaId))
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
            ViewData["SalaId"] = new SelectList(_context.Salas, "SalaId", "SalaId", reservaSala.SalaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", reservaSala.UsuarioId);
            return View(reservaSala);
        }

        // GET: ReservasSala/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservaSala = await _context.ReservasSala
                .Include(r => r.Sala)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ReservaSalaId == id);
            if (reservaSala == null)
            {
                return NotFound();
            }

            return View(reservaSala);
        }

        // POST: ReservasSala/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservaSala = await _context.ReservasSala.FindAsync(id);
            if (reservaSala != null)
            {
                _context.ReservasSala.Remove(reservaSala);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaSalaExists(Guid id)
        {
            return _context.ReservasSala.Any(e => e.ReservaSalaId == id);
        }
    }
}
