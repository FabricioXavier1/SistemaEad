using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUC.LDSI.Domain.Command;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.QueryResult;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.ModuloProfessor.Controllers
{
    public class OpcaoAvaliacaoController : BaseController
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IOpcaoAvaliacaoRepository _opcaoAvaliacaoRepository;

        public OpcaoAvaliacaoController(UserManager<Usuario> user, IOpcaoAvaliacaoRepository opcaoAvaliacaoRepository, IAvaliacaoService avaliacaoService) : base(user)
        {
            _avaliacaoService = avaliacaoService;
            _opcaoAvaliacaoRepository = opcaoAvaliacaoRepository;
        }

        public async Task<IActionResult> Index(int? questaoId)
        {
            if (questaoId == null)
            {
                return NotFound();
            }

            var questao = await _avaliacaoService.ObterQuestaoQueryResultAsync(questaoId.Value);

            return View(questao);
        }

        public IActionResult Create(int? questaoId)
        {
            if (questaoId == null)
            {
                return NotFound();
            }

            ViewData["QuestaoId"] = questaoId.Value;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestaoId,Descricao,Verdadeira")] OpcaoAvaliacaoQueryResult opcaoAvaliacao)
        {
            if (ModelState.IsValid)
            {
                await _avaliacaoService.AdicionarOpcaoAvaliacaoAsync(opcaoAvaliacao.QuestaoId, opcaoAvaliacao.Descricao, opcaoAvaliacao.Verdadeira);

                return RedirectToAction(nameof(Index), new { questaoId = opcaoAvaliacao.QuestaoId });
            }

            ViewData["QuestaoId"] = opcaoAvaliacao.QuestaoId;

            return View(opcaoAvaliacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcaoAvaliacao = await _opcaoAvaliacaoRepository.ObterAsync(id.Value);

            if (opcaoAvaliacao == null)
            {
                return NotFound();
            }

            return View(opcaoAvaliacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Verdadeira")] OpcaoAvaliacaoCommand opcaoAvaliacao)
        {
            if (id != opcaoAvaliacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var parentId = await _avaliacaoService.AlterarOpcaoAvaliacaoAsync(opcaoAvaliacao.Id, opcaoAvaliacao.Descricao, Convert.ToBoolean(opcaoAvaliacao.Verdadeira));

                return RedirectToAction(nameof(Index), new { questaoId = parentId });
            }

            return View(opcaoAvaliacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opcaoAvaliacao = await _opcaoAvaliacaoRepository.ObterAsync(id.Value);

            if (opcaoAvaliacao == null)
            {
                return NotFound();
            }

            return View(opcaoAvaliacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parentId = await _avaliacaoService.ExcluirOpcaoAvaliacaoAsync(id);

            return RedirectToAction(nameof(Index), new { questaoId = parentId });
        }
    }
}
