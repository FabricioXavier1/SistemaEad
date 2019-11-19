using PUC.LDSI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    //public interface IAvaliacaoRepository : IRepository<Avaliacao>
    public interface IAvaliacaoRepository 
    {
        Task<List<Avaliacao>> ListarAvaliacoesDoProfessor(string login);
        Task<int> IncluirNovaAvaliacaoAsync(Avaliacao avaliacao);
        Task<int> AlterarAvaliacaoAsync(Avaliacao avaliacao);
        Task<Avaliacao> ObterAsync(int id);
        Task<Avaliacao> ObterComQuestoresAsync(int id);
        Task ExcluirAvaliacaoAsync(int id);
        Avaliacao ObterAvaliacaoComQuestoes(int id);
    }
}
