using PUC.LDSI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services.Interfaces
{
    public interface IPublicacaoService
    {
        Task<int> AdicionarPublicacaoAsync(int avaliacaoId, int turmaId, DateTime dataInicio, DateTime dataFim, int valorProva);
        Task<int> AlterarPublicacaoAsync(int id, DateTime dataInicio, DateTime dataFim, int valorProva);
        Task<List<Publicacao>> ListarPublicacoesDoProfessorAsync(string login);
        Task ExcluirPublicacaoAsync(int id);
    }
}
