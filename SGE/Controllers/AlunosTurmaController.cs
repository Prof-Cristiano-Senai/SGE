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
    public class AlunosTurmaController : Controller
    {
        private readonly SGEContext _context;

        public AlunosTurmaController(SGEContext context)
        {
            _context = context;
        }

        // GET: AlunosTurma
        public async Task<IActionResult> Index()
        {
            var sGEContext = _context.AlunosTurma.Include(a => a.Aluno).Include(a => a.Turma);
            return View(await sGEContext.ToListAsync());
        }

        // GET: AlunosTurma/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoTurma = await _context.AlunosTurma
                .Include(a => a.Aluno)
                .Include(a => a.Turma)
                .FirstOrDefaultAsync(m => m.AlunoTurmaId == id);
            if (alunoTurma == null)
            {
                return NotFound();
            }

            return View(alunoTurma);
        }

        // GET: AlunosTurma/Create
        public IActionResult Create()
        {
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "AlunoId", "AlunoId");
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "TurmaId");
            return View();
        }

        // POST: AlunosTurma/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoTurmaId,AlunoId,TurmaId")] AlunoTurma alunoTurma)
        {
            if (ModelState.IsValid)
            {
                alunoTurma.AlunoTurmaId = Guid.NewGuid();
                _context.Add(alunoTurma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "AlunoId", "AlunoId", alunoTurma.AlunoId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "TurmaId", alunoTurma.TurmaId);
            return View(alunoTurma);
        }

        // GET: AlunosTurma/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoTurma = await _context.AlunosTurma.FindAsync(id);
            if (alunoTurma == null)
            {
                return NotFound();
            }
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "AlunoId", "AlunoId", alunoTurma.AlunoId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "TurmaId", alunoTurma.TurmaId);
            return View(alunoTurma);
        }

        // POST: AlunosTurma/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlunoTurmaId,AlunoId,TurmaId")] AlunoTurma alunoTurma)
        {
            if (id != alunoTurma.AlunoTurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alunoTurma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoTurmaExists(alunoTurma.AlunoTurmaId))
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
            ViewData["AlunoId"] = new SelectList(_context.Alunos, "AlunoId", "AlunoId", alunoTurma.AlunoId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "TurmaId", alunoTurma.TurmaId);
            return View(alunoTurma);
        }

        // GET: AlunosTurma/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunoTurma = await _context.AlunosTurma
                .Include(a => a.Aluno)
                .Include(a => a.Turma)
                .FirstOrDefaultAsync(m => m.AlunoTurmaId == id);
            if (alunoTurma == null)
            {
                return NotFound();
            }

            return View(alunoTurma);
        }

        // POST: AlunosTurma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var alunoTurma = await _context.AlunosTurma.FindAsync(id);
            if (alunoTurma != null)
            {
                _context.AlunosTurma.Remove(alunoTurma);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoTurmaExists(Guid id)
        {
            return _context.AlunosTurma.Any(e => e.AlunoTurmaId == id);
        }
    }
}
