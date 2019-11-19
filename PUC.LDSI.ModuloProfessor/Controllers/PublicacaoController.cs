using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.ModuloProfessor.Controllers
{
    public class PublicacaoController : BaseController
    {
        private readonly IPublicacaoService _publicacaoService;
        private readonly ITurmaService _turmaService;
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IPublicacaoRepository _publicacaoRepository;

        public PublicacaoController(IPublicacaoService publicacaoService,
                                    ITurmaService turmaService,
                                    IAvaliacaoService avaliacaoService,
                                    IPublicacaoRepository publicacaoRepository,
                                    UserManager<Usuario> user) : base (user)
        {
            _turmaService = turmaService;
            _publicacaoRepository = publicacaoRepository;
            _avaliacaoService = avaliacaoService;
            _publicacaoService = publicacaoService;
        }

        public async Task<IActionResult> Index()
        {
            var publicacoes = await _publicacaoService.ListarPublicacoesDoProfessorAsync(LoginUsuario);

            return View(publicacoes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacao = await _publicacaoRepository.ObterPublicacaoAsync(id.Value);

            if (publicacao == null)
            {
                return NotFound();
            }

            return View(publicacao);
        }

        public IActionResult Create()
        {
            var avaliacoes = _avaliacaoService.ListarAvaliacoesSelectAsync(LoginUsuario).Result;

            var turmas = _turmaService.ListarTurmas();

            ViewData["AvaliacaoId"] = new SelectList(avaliacoes, "Id", "Nome");

            ViewData["TurmaId"] = new SelectList(turmas, "Id", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvaliacaoId,TurmaId,DataPublicacao,DataInicio,DataFim,ValorProva,Id")] Publicacao publicacao)
        {
            if (ModelState.IsValid)
            {
                await _publicacaoService.AdicionarPublicacaoAsync(publicacao.AvaliacaoId, publicacao.TurmaId, publicacao.DataInicio, publicacao.DataFim, publicacao.ValorProva);

                return RedirectToAction(nameof(Index));
            }
            
            var avaliacoes = _avaliacaoService.ListarAvaliacoesSelectAsync(LoginUsuario).Result;

            var turmas = _turmaService.ListarTurmas();

            ViewData["AvaliacaoId"] = new SelectList(avaliacoes, "Id", "Nome", publicacao.AvaliacaoId);

            ViewData["TurmaId"] = new SelectList(turmas, "Id", "Nome", publicacao.TurmaId);

            return View(publicacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacao = await _publicacaoRepository.ObterAsync(id.Value);

            if (publicacao == null)
            {
                return NotFound();
            }

            var avaliacoes = _avaliacaoService.ListarAvaliacoesSelectAsync(LoginUsuario).Result;

            var turmas = _turmaService.ListarTurmas();

            ViewData["AvaliacaoId"] = new SelectList(avaliacoes, "Id", "Nome", publicacao.AvaliacaoId);

            ViewData["TurmaId"] = new SelectList(turmas, "Id", "Nome", publicacao.TurmaId);

            return View(publicacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvaliacaoId,TurmaId,DataPublicacao,DataInicio,DataFim,ValorProva,Id")] Publicacao publicacao)
        {
            if (id != publicacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _publicacaoService.AlterarPublicacaoAsync(publicacao.Id, publicacao.DataInicio, publicacao.DataFim, publicacao.ValorProva);

                return RedirectToAction(nameof(Index));
            }

            var avaliacoes = _avaliacaoService.ListarAvaliacoesSelectAsync(LoginUsuario).Result;

            var turmas = _turmaService.ListarTurmas();

            ViewData["AvaliacaoId"] = new SelectList(avaliacoes, "Id", "Nome", publicacao.AvaliacaoId);

            ViewData["TurmaId"] = new SelectList(turmas, "Id", "Nome", publicacao.TurmaId);
            
            return View(publicacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacao = await _publicacaoRepository.ObterAsync(id.Value);

            if (publicacao == null)
            {
                return NotFound();
            }

            return View(publicacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _publicacaoService.ExcluirPublicacaoAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
