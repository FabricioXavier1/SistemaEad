using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Exception;
using PUC.LDSI.Domain.QueryResult;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;

namespace PUC.LDSI.Domain.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IQuestaoRepository _questaoRepository;
        private readonly IOpcaoAvaliacaoRepository _opcaoAvaliacaoRepository;

        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository,
                                IQuestaoRepository questaoRepository,
                                IProfessorRepository professorRepository,
                                IOpcaoAvaliacaoRepository opcaoAvaliacaoRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _professorRepository = professorRepository;
            _questaoRepository = questaoRepository;
            _opcaoAvaliacaoRepository = opcaoAvaliacaoRepository;
        }

        public async Task<int> AdicionarQuestaoAvaliacaoAsync(int avaliacaoId, int tipo, string enunciado)
        {
            if (tipo != 1 && tipo != 2)
                throw new DomainException("Tipo inválido! Os tipos válidos são: 1 - Multipla Escolha e 2 - Verdadeiro ou Falso");

            if (string.IsNullOrWhiteSpace(enunciado))
                throw new DomainException("O enunciado precisa ser informado!");

            var questao = new Questao() { AvaliacaoId = avaliacaoId, Tipo = tipo, Enunciado = enunciado };

            _questaoRepository.Adicionar(questao);

            await _questaoRepository.SaveChangesAsync();

            return questao.Id;
        }

        public async Task<int> AdicionarOpcaoAvaliacaoAsync(int questaoId, string descricao, bool verdadeira)
        {
            if (verdadeira)
            {
                var questao = await _questaoRepository.ObterComOpcoesAsync(questaoId);

                if (questao.Tipo == 1 && questao.Opcoes.Find(x => x.Verdadeira) != null)
                    throw new DomainException("Já existe uma opção cadastrada como verdadeira para essa questão!");
            }

            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("A descrição da opção não pode ser vazia!");

            var opcao = new OpcaoAvaliacao()
            {
                Descricao = descricao,
                QuestaoId = questaoId,
                Verdadeira = verdadeira
            };

            _opcaoAvaliacaoRepository.Adicionar(opcao);

            await _opcaoAvaliacaoRepository.SaveChangesAsync();

            return opcao.Id;
        }

        public async Task<int> AlterarAvaliacaoAsync(int id, string disciplina, string materia, string descricao)
        {
            var avaliacao = await _avaliacaoRepository.ObterAsync(id);

            if (avaliacao == null)
                throw new DomainException("Avaliação não encontrada!");

            if (string.IsNullOrWhiteSpace(disciplina))
                throw new DomainException("A disciplina precisa ser informada!");

            if (string.IsNullOrWhiteSpace(materia))
                throw new DomainException("A matéria precisa ser informada!");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("A descrição precisa ser informada!");

            avaliacao.Descricao = descricao;
            avaliacao.Disciplina = disciplina;
            avaliacao.Materia = materia;

            await _avaliacaoRepository.AlterarAvaliacaoAsync(avaliacao);

            return avaliacao.Id;
        }

        public async Task<int> AlterarQuestaoAvaliacaoAsync(int id, int tipo, string enunciado)
        {
            var questao = await _questaoRepository.ObterAsync(id);

            if (questao == null)
                throw new DomainException("A questão não foi encontrada!");

            questao.Tipo = tipo;

            questao.Enunciado = enunciado;

            _questaoRepository.Modificar(questao);

            await _questaoRepository.SaveChangesAsync();

            return questao.AvaliacaoId;
        }

        public async Task<int> AlterarOpcaoAvaliacaoAsync(int id, string descricao, bool verdadeira)
        {
            var opcao = await _opcaoAvaliacaoRepository.ObterAsync(id);

            if (opcao == null)
                throw new DomainException("A opção não foi encontrada!");

            if (verdadeira)
            {
                var questao = await _questaoRepository.ObterComOpcoesAsync(opcao.QuestaoId);

                if (questao.Tipo == 1 && questao.Opcoes.Find(x => x.Id != id && x.Verdadeira) != null)
                    throw new DomainException("Já existe uma opção cadastrada como verdadeira para essa questão!");
            }

            opcao.Descricao = descricao;

            opcao.Verdadeira = verdadeira;

            _opcaoAvaliacaoRepository.Modificar(opcao);

            await _opcaoAvaliacaoRepository.SaveChangesAsync();

            return opcao.QuestaoId;
        }

        public async Task ExcluirAvaliacaoAsync(int id)
        {
            await _avaliacaoRepository.ExcluirAvaliacaoAsync(id);
        }

        public async Task<int> AdicionarAvaliacaoAsync(string login, string disciplina, string materia, string descricao)
        {
            var professor = _professorRepository.ObterPorUsuarioId(login);

            if (professor == null)
                throw new DomainException("O professor não foi encontrado!");

            if (string.IsNullOrWhiteSpace(disciplina))
                throw new DomainException("A disciplina precisa ser informada!");

            if (string.IsNullOrWhiteSpace(materia))
                throw new DomainException("A matéria precisa ser informada!");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("A descrição precisa ser informada!");

            var avaliacao = new Avaliacao()
            {
                Descricao = descricao,
                Materia = materia,
                Disciplina = disciplina,
                ProfessorId = professor.Id
            };

            var id = await _avaliacaoRepository.IncluirNovaAvaliacaoAsync(avaliacao);

            return id;
        }

        public async Task<List<Avaliacao>> ListarAvaliacoesDoProfessorAsync(string login)
        {
            var lista = await _avaliacaoRepository.ListarAvaliacoesDoProfessor(login);

            return lista;
        }

        public async Task<List<AvaliacaoSelectQueryResult>> ListarAvaliacoesSelectAsync(string login)
        {
            var lista = await _avaliacaoRepository.ListarAvaliacoesDoProfessor(login);

            return lista.Select(x => new AvaliacaoSelectQueryResult()
            {
                Id = x.Id,
                Nome = string.Format("{0} - {1} - {2}", x.Disciplina, x.Materia, x.Descricao)
            }).ToList();
        }

        public async Task<AvaliacaoQueryResult> ObterAvaliacaoQueryResultAsync(int id)
        {
            var avaliacao = await _avaliacaoRepository.ObterComQuestoresAsync(id);

            var avaliacaoQueryResult = new AvaliacaoQueryResult()
            {
                AvaliacaoId = avaliacao.Id,
                Professor = avaliacao.Professor.Nome,
                Materia = avaliacao.Materia,
                Disciplina = avaliacao.Disciplina,
                Descricao = avaliacao.Descricao,
                Questoes = avaliacao.Questoes?.Select(x => new QuestaoQueryResult()
                {
                    QuestaoId = x.Id,
                    Enunciado = x.Enunciado,
                    TipoId = x.Tipo,
                    Tipo = (x.Tipo == 1 ? "Multipla Escolha" : "Verdadeiro ou Falso"),
                    Opcoes = x.Opcoes?.Select(y => new OpcaoAvaliacaoQueryResult()
                    {
                        OpcaoAvaliacaoId = y.Id,
                        QuestaoId = y.QuestaoId,
                        AvaliacaoId = x.AvaliacaoId,
                        Descricao = y.Descricao,
                        Verdadeira = y.Verdadeira
                    }).ToList()
                }).ToList()
            };

            return avaliacaoQueryResult;
        }

        public async Task<QuestaoQueryResult> ObterQuestaoQueryResultAsync(int id)
        {
            var questao = await _questaoRepository.ObterComOpcoesAsync(id);

            var questaoResult = new QuestaoQueryResult()
            {
                QuestaoId = questao.Id,
                TipoId = questao.Tipo,
                Tipo = (questao.Tipo == 1 ? "Multipla Escolha" : "Verdadeiro ou Falso"),
                Enunciado = questao.Enunciado,
                Avaliacao = new AvaliacaoQueryResult()
                {
                    AvaliacaoId = questao.AvaliacaoId,
                    Descricao = questao.Avaliacao.Descricao,
                    Disciplina = questao.Avaliacao.Disciplina,
                    Materia = questao.Avaliacao.Materia
                },
                Opcoes = questao.Opcoes.Select(x => new OpcaoAvaliacaoQueryResult()
                {
                    OpcaoAvaliacaoId = x.Id,
                    QuestaoId = x.QuestaoId,
                    AvaliacaoId = questao.AvaliacaoId,
                    Descricao = x.Descricao,
                    Verdadeira = x.Verdadeira
                }).ToList()
            };

            return questaoResult;
        }

        public async Task<int> ExcluirQuestaoAvaliacaoAsync(int id)
        {
            return await _questaoRepository.ExcluirQuestaoAsync(id);
        }

        public async Task<int> ExcluirOpcaoAvaliacaoAsync(int id)
        {
            return await _opcaoAvaliacaoRepository.ExcluirOpcaoAvaliacaoAsync(id);
        }
    }
}
