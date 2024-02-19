using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGE.Data;
using SGE.Models;

namespace SGE.Controllers
{
    public class AlunosController : Controller
    {
        private readonly SGEContext _context;

        public AlunosController(SGEContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string Email = HttpContext.Session.GetString("email");
                var usuario = _context.Usuarios.Where(a => a.Email == Email).FirstOrDefault();
                Guid idTipoAluno = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault().TipoUsuarioId;
                if (usuario.TipoUsuarioId == idTipoAluno)
                {
                    return RedirectToAction("AcessoNegado", "Home");
                }
            }
            var sGEContext = _context.Alunos.Include(a => a.TipoUsuario);
            return View(await sGEContext.ToListAsync());
        }

        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string Email = HttpContext.Session.GetString("email");
                var usuario = _context.Usuarios.Where(a => a.Email == Email).FirstOrDefault();
                Guid idTipoAluno = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault().TipoUsuarioId;
                if (usuario.TipoUsuarioId == idTipoAluno)
                {
                    return RedirectToAction("AcessoNegado", "Home");
                }
            }

            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.TipoUsuario)
                .FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string Email = HttpContext.Session.GetString("email");
                var usuario = _context.Usuarios.Where(a => a.Email == Email).FirstOrDefault();
                Guid idTipoAluno = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault().TipoUsuarioId;
                if (usuario.TipoUsuarioId == idTipoAluno)
                {
                    return RedirectToAction("AcessoNegado", "Home");
                }
            }

            ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId");
            return View();
        }

        // POST: Alunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoId,Matricula,AlunoNome,Email,Celular,Logradouro,Numero,Cidade,Estado,CEP,Senha,DataNascimento,CadAtivo,DataCadastro,CadInativo,TipoUsuarioId,UrlFoto")] Aluno aluno, string ConfirmeSenha)
        {

            if (ModelState.IsValid)
            {
                if (_context.Alunos.Where(a => a.Email == aluno.Email).FirstOrDefault() != null)
                {
                    ViewData["Erro"] = "Email já cadastrado!";
                    ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId", aluno.TipoUsuarioId);
                    return View(aluno);
                }
                if (aluno.Senha != ConfirmeSenha)
                {
                    ViewData["Erro"] = "As Senhas não conferem!";
                    ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId", aluno.TipoUsuarioId);
                    return View(aluno);
                }
                aluno.AlunoId = Guid.NewGuid();
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                Usuario usuario = new Usuario();
                usuario.UsuarioId = Guid.NewGuid();
                usuario.UsuarioNome = aluno.AlunoNome;
                usuario.Email = aluno.Email;
                usuario.Senha = aluno.Senha;
                usuario.Celular = aluno.Celular;
                usuario.CadAtivo = true;
                usuario.DataCadastro = DateTime.Now;
                usuario.TipoUsuarioId = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault().TipoUsuarioId;
                usuario.TipoUsuario = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault();
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId", aluno.TipoUsuarioId);
            return View(aluno);
        }

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId", aluno.TipoUsuarioId);
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlunoId,Matricula,AlunoNome,Email,Celular,Logradouro,Numero,Cidade,Estado,CEP,Senha,DataNascimento,CadAtivo,DataCadastro,CadInativo,TipoUsuarioId,UrlFoto")] Aluno aluno)
        {
            if (id != aluno.AlunoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.AlunoId))
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
            ViewData["TipoUsuarioId"] = new SelectList(_context.TiposUsuario, "TipoUsuarioId", "TipoUsuarioId", aluno.TipoUsuarioId);
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string Email = HttpContext.Session.GetString("email");
                var usuario = _context.Usuarios.Where(a => a.Email == Email).FirstOrDefault();
                Guid idTipoAluno = _context.TiposUsuario.Where(a => a.Tipo == "Aluno").FirstOrDefault().TipoUsuarioId;
                if (usuario.TipoUsuarioId == idTipoAluno)
                {
                    return RedirectToAction("AcessoNegado", "Home");
                }
            }

            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.TipoUsuario)
                .FirstOrDefaultAsync(m => m.AlunoId == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(Guid id)
        {
            return _context.Alunos.Any(e => e.AlunoId == id);
        }
    }
}
