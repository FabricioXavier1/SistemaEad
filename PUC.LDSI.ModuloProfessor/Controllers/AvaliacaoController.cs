using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.ModuloProfessor.Controllers
{
    public class AvaliacaoController : BaseController
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public AvaliacaoController(UserManager<Usuario> user,
                                   IAvaliacaoRepository avaliacaoRepository,
                                   IAvaliacaoService avaliacaoService) : base(user)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _avaliacaoService = avaliacaoService;
        }

        public async Task<IActionResult> Index()
        {
            var avaliacoes = await _avaliacaoRepository.ListarAvaliacoesDoProfessor(LoginUsuario);

            return View(avaliacoes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Disciplina,Materia,Descricao")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                await _avaliacaoService.AdicionarAvaliacaoAsync(LoginUsuario, avaliacao.Disciplina, avaliacao.Materia, avaliacao.Descricao);

                return RedirectToAction(nameof(Index));
            }

            return View(avaliacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _avaliacaoRepository.ObterAsync(id.Value);

            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Disciplina,Materia,Descricao")] Avaliacao avaliacao)
        {
            if (id != avaliacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _avaliacaoService.AlterarAvaliacaoAsync(avaliacao.Id, avaliacao.Disciplina, avaliacao.Materia, avaliacao.Descricao);

                return RedirectToAction(nameof(Index));
            }

            return View(avaliacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var avaliacao = await _avaliacaoRepository.ObterAsync(id.Value);

            if (avaliacao == null) { return NotFound(); }

            return View(avaliacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _avaliacaoService.ExcluirAvaliacaoAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
