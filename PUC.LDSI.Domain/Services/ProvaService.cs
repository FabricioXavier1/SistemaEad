using PUC.LDSI.Domain.QueryResult;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using PUC.LDSI.Domain.Exception;
using PUC.LDSI.Domain.InputData;
using PUC.LDSI.Domain.Entities;

namespace PUC.LDSI.Domain.Services
{
    public class ProvaService : IProvaService
    {
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IProvaRepository _provaRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IQuestaoRepository _questaoRepository;
        private readonly IOpcaoAvaliacaoRepository _opcaoProvaRepository;

        public ProvaService(IPublicacaoRepository publicacaoRepository,
                            IProvaRepository provaRepository,
                            IAvaliacaoRepository avaliacaoRepository,
                            IAlunoRepository alunoRepository,
                            IQuestaoRepository questaoRepository,
                            IOpcaoAvaliacaoRepository opcaoProvaRepository)
        {
            _provaRepository = provaRepository;
            _alunoRepository = alunoRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _publicacaoRepository = publicacaoRepository;
            _questaoRepository = questaoRepository;
            _opcaoProvaRepository = opcaoProvaRepository;
        }

        public async Task<List<AvaliacaoPublicadaQueryResult>> ListarAvaliacoesPublicadasAsync(string login)
        {
            var aluno = _alunoRepository.ObterPorLogin(login);

            if (aluno == null) throw new DomainException("O aluno não foi localizado!");

            var publicacoes = await _publicacaoRepository.ListarPublicacoesDoAlunoAsync(login);

            var retorno = new List<AvaliacaoPublicadaQueryResult>();

            publicacoes.ForEach(x =>
            {
                var prova = x.Avaliacao.Provas.FirstOrDefault(a => a.AlunoId == aluno.Id);

                retorno.Add(new AvaliacaoPublicadaQueryResult()
                {
                    AlunoId = aluno.Id,
                    AvaliacaoId = x.AvaliacaoId,
                    DataFim = x.DataFim,
                    DataInicio = x.DataInicio,
                    DataPublicacao = x.DataPublicacao,
                    Descricao = x.Avaliacao.Descricao,
                    Disciplina = x.Avaliacao.Disciplina,
                    Materia = x.Avaliacao.Materia,
                    ValorProva = x.ValorProva,
                    PublicacaoId = x.Id,
                    ProvaId = prova == null ? (int?)null : prova.Id,
                    DataRealizacao = prova == null ? (DateTime?)null : prova.DataProva,
                    NotaObtida = prova == null ? (decimal?)null : prova.NotaObtida * x.ValorProva
                });
            });

            return retorno;
        }

        public async Task<ProvaQueryResult> ObterProvaAsync(int publicacaoId, string login)
        {
            var aluno = _alunoRepository.ObterPorLogin(login);

            if (aluno == null) throw new DomainException("O aluno não foi localizado!");

            var publicacao = await _publicacaoRepository.ObterAsync(publicacaoId);

            if (publicacao == null) throw new DomainException("A publicacao não foi localizada!");

            var avaliacaoCompleta = await _avaliacaoRepository.ObterComQuestoresAsync(publicacao.AvaliacaoId);

            var provaCompleta = await _provaRepository.ObterProvaDoAlunoAsync(publicacao.AvaliacaoId, aluno.Id);

            var retorno = new ProvaQueryResult()
            {
                AvaliacaoId = publicacao.AvaliacaoId,
                PublicacaoId = publicacao.Id,
                Questoes = avaliacaoCompleta.Questoes.Select(x => new QuestaoProvaQueryResult()
                {
                    QuestaoId = x.Id,
                    Enunciado = x.Enunciado,
                    Tipo = x.Tipo,
                    Opcoes = x.Opcoes.Select(y => new OpcaoProvaQueryResult()
                    {
                        OpcaoAvaliacaoId = y.Id,
                        QuestaoId = y.QuestaoId,
                        Descricao = y.Descricao
                    }).ToList()
                }).ToList()
            };

            retorno.Questoes.SelectMany(x => x.Opcoes).ToList().ForEach(x =>
            {
                x.Resposta = provaCompleta?.QuestoesProva?
                    .SelectMany(y => y.OpcoesProva)
                    .FirstOrDefault(y => y.OpcaoAvaliacaoId == x.OpcaoAvaliacaoId)?.Resposta ?? false;
            });

            return retorno;
        }

        public void SalvarProva(ProvaInputData provaInputData, string login)
        {
            var aluno = _alunoRepository.ObterPorLogin(login);
            var avaliacao = _avaliacaoRepository.ObterAvaliacaoComQuestoes(provaInputData.AvaliacaoId);
            decimal notaTotalProva = 0;

            Prova prova = new Prova
            {
                DataProva = DateTime.Now,
                AvaliacaoId = provaInputData.AvaliacaoId,
                AlunoId = aluno.Id,
                Aluno = aluno,
                Avaliacao = avaliacao,
                QuestoesProva = new List<QuestaoProva>()
            };

            var questoes = new List<QuestaoProva>();

            foreach (var questao in provaInputData.Questoes)
            {
                var questaoProva = new QuestaoProva
                {
                    QuestaoId = questao.QuestaoId
                };

                var questaoAvaliacao = avaliacao.Questoes.Find(y => y.Id == questao.QuestaoId);
                
                questaoProva.OpcoesProva = new List<OpcaoProva>();

                decimal countVouFCertas = 0;
                decimal totalVouF = 0;

                foreach (var opcao in questao.Opcoes)
                {
                    // Nota questão
                    if (questaoAvaliacao.Tipo == 1)
                    {
                        var idVerdadeira = questaoAvaliacao.Opcoes.Find(x => x.Verdadeira).Id;
                        questaoProva.Nota = questao.Opcoes.Find(y => y.OpcaoAvaliacaoId == idVerdadeira && y.Resposta) == null ? 0 : 1;
                    }
                    else
                    {
                        var vfCerta = questaoAvaliacao.Opcoes.Find(x => x.Id == opcao.OpcaoAvaliacaoId);
                        totalVouF++;

                        if (vfCerta.Verdadeira == opcao.Resposta)
                            countVouFCertas++;
                    }
                    
                    var opcaoProva = new OpcaoProva()
                    {
                        OpcaoAvaliacaoId = opcao.OpcaoAvaliacaoId,
                        Resposta = opcao.Resposta
                    };
                    
                    questaoProva.OpcoesProva.Add(opcaoProva);
                }

                if (questaoAvaliacao.Tipo != 1)
                    questaoProva.Nota = countVouFCertas / totalVouF;

                notaTotalProva += questaoProva.Nota;

                prova.QuestoesProva.Add(questaoProva);
            }

            prova.NotaObtida = notaTotalProva / provaInputData.Questoes.Count;

            _provaRepository.IncluirNovaAvaliacao(prova);
        }
    }
}
