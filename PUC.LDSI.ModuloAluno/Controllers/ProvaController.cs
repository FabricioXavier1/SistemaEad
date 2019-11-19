using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.InputData;
using PUC.LDSI.Domain.QueryResult;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.ModuloAluno.Controllers
{
    public class ProvaController : BaseController
    {
        private readonly IProvaService _provaService;
        private readonly ITurmaService _turmaService;
        private readonly IAvaliacaoService _avaliacaoService;

        public ProvaController(IAvaliacaoService avaliacaoService,
                               ITurmaService turmaService,
                               IProvaService provaService,
                               UserManager<Usuario> user) : base(user)
        {
            _turmaService = turmaService;

            _avaliacaoService = avaliacaoService;

            _provaService = provaService;
        }

        public async Task<IActionResult> Index()
        {
            var publicacoes = await _provaService.ListarAvaliacoesPublicadasAsync(LoginUsuario);

            return View(publicacoes);
        }

        public async Task<IActionResult> PerformTest(int publicacaoid, int? questaoid)
        {
            var publicacoes = await _provaService.ListarAvaliacoesPublicadasAsync(LoginUsuario);

            var publicacao = publicacoes.FirstOrDefault(x => x.PublicacaoId == publicacaoid);

            if (publicacao == null)
            {
                return NotFound();
            }

            return View(publicacao);
        }

        [HttpGet]
        public IActionResult ObterProvaCompleta()
        {
            var publicacaoId = int.Parse("0" + Request.Query["id"]);

            var prova = _provaService.ObterProvaAsync(publicacaoId, LoginUsuario).Result;

            return Json(prova);
        }

        [HttpPost]
        public IActionResult ConcluirProva([FromBody]ProvaInputData provaInputData)
        {
            try
            {
                if (provaInputData?.AvaliacaoId > 0)
                {
                    _provaService.SalvarProva(provaInputData, LoginUsuario);

                    return Json(new { Success = true });
                }
                else
                    return Json(new { Success = false, Error = "O objeto recebido está corrompido." });
            }
            catch (System.Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message });
            }
        }

    }
}
