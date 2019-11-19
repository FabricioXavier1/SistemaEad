using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.ModuloProfessor.Controllers
{
    public class TurmaController : BaseController
    {
        private readonly ITurmaService _turmaService;
        private readonly ITurmaRepository _turmaRepository;

        public TurmaController(ITurmaService turmaService, 
                               ITurmaRepository turmaRepository,
                               UserManager<Usuario> _user) : base(_user)
        {
            _turmaService = turmaService;
            _turmaRepository = turmaRepository;
        }

        public IActionResult Index()
        {
            return View(_turmaRepository.ObterTodos());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Id")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                await _turmaService.AdicionarTurmaAsync(turma.Nome);

                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepository.ObterAsync(id.Value);

            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nome,Id")] Turma turma)
        {
            if (id != turma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _turmaService.AlterarTurmaAsync(turma.Id, turma.Nome);

                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepository.ObterAsync(id.Value);

            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _turmaService.ExcluirAsync(id);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
