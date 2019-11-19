using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.QueryResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<int> AdicionarAvaliacaoAsync(string login, string disciplina, string materia, string descricao);
        Task<int> AdicionarQuestaoAvaliacaoAsync(int avaliacaoId, int tipo, string enunciado);
        Task<int> AdicionarOpcaoAvaliacaoAsync(int questaoId, string descricao, bool verdadeira);

        Task<int> AlterarAvaliacaoAsync(int id, string disciplina, string materia, string descricao);
        Task<int> AlterarQuestaoAvaliacaoAsync(int id, int tipo, string enunciado);
        Task<int> AlterarOpcaoAvaliacaoAsync(int id, string descricao, bool verdadeira);

        Task<List<Avaliacao>> ListarAvaliacoesDoProfessorAsync(string login);
        Task<List<AvaliacaoSelectQueryResult>> ListarAvaliacoesSelectAsync(string login);

        Task<AvaliacaoQueryResult> ObterAvaliacaoQueryResultAsync(int id);
        Task<QuestaoQueryResult> ObterQuestaoQueryResultAsync(int id);

        Task ExcluirAvaliacaoAsync(int id);
        Task<int> ExcluirQuestaoAvaliacaoAsync(int id);
        Task<int> ExcluirOpcaoAvaliacaoAsync(int id);
    }
}
