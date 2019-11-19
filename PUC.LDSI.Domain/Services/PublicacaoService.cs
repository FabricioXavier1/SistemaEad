using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Exception;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services
{
    public class PublicacaoService : IPublicacaoService
    {
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IProvaRepository _provaRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public PublicacaoService(IPublicacaoRepository publicacaoRepository,
                                 IProvaRepository provaRepository,
                                 IProfessorRepository professorRepository,
                                 IAvaliacaoRepository avaliacaoRepository)
        {
            _provaRepository = provaRepository;
            _professorRepository = professorRepository;
            _publicacaoRepository = publicacaoRepository;
            _avaliacaoRepository = avaliacaoRepository;
        }

        public async Task<int> AdicionarPublicacaoAsync(int avaliacaoId, int turmaId, DateTime dataInicio, DateTime dataFim, int valorProva)
        {
            if (dataFim < dataInicio)
                throw new DomainException("A data de fim não pode ser anterior à data de início!");

            var publicacoesTurma = await _publicacaoRepository.ListarPublicacoesPorTurmaAsync(turmaId);

            if (publicacoesTurma.Find(x => x.AvaliacaoId == avaliacaoId) != null)
                throw new DomainException("Essa avaliação já foi publicada para esta turma!");

            var avaliacao = await _avaliacaoRepository.ObterComQuestoresAsync(avaliacaoId);

            if (avaliacao.Questoes.Find(x => x.Opcoes.Count < 4) != null)
                throw new DomainException("Essa avaliação não está completa! É necessário que todas as questões tenham ao menos 4 opções!");

            var publicacao = new Publicacao()
            {
                AvaliacaoId = avaliacaoId,
                TurmaId = turmaId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                ValorProva = valorProva,
                DataPublicacao = DateTime.Now
            };

            _publicacaoRepository.Adicionar(publicacao);

            await _publicacaoRepository.SaveChangesAsync();

            return publicacao.Id;
        }

        public async Task<int> AlterarPublicacaoAsync(int id, DateTime dataInicio, DateTime dataFim, int valorProva)
        {
            if (dataFim < dataInicio)
                throw new DomainException("A data de fim não pode ser anterior à data de início!");

            var publicacao = await _publicacaoRepository.ObterAsync(id);

            if (publicacao.ValorProva != valorProva)
            {
                var provas = await _provaRepository.ListarPorAvaliacaoAsync(publicacao.AvaliacaoId);

                if (provas?.Count > 0)
                    throw new DomainException("Não é permitido alterar o valor da publicação quando a prova já foi feita por algum aluno!");
            }

            publicacao.DataInicio = dataInicio;
            publicacao.DataFim = dataFim;
            publicacao.ValorProva = valorProva;

            _publicacaoRepository.Modificar(publicacao);

            await _publicacaoRepository.SaveChangesAsync();

            return publicacao.Id;
        }

        public async Task ExcluirPublicacaoAsync(int id)
        {
            var publicacao = await _publicacaoRepository.ObterAsync(id);

            var provas = await _provaRepository.ListarPorAvaliacaoAsync(publicacao.AvaliacaoId);

            if (provas?.Count > 0)
                throw new DomainException("Não é permitido excluir uma publicação quando a prova já foi feita por algum aluno!");

            _publicacaoRepository.Remover(id);

            await _publicacaoRepository.SaveChangesAsync();
        }

        public async Task<List<Publicacao>> ListarPublicacoesDoProfessorAsync(string login)
        {
            var professor = _professorRepository.ObterPorUsuarioId(login);

            var publicacoes = await _publicacaoRepository.ListarPublicacoesDoProfessorAsync(professor.Id);

            return publicacoes;
        }
    }
}
