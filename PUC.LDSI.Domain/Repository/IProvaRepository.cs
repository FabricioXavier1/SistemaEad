using PUC.LDSI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface IProvaRepository : IRepository<Prova>
    {
        Task<List<Prova>> ListarPorAvaliacaoAsync(int avaliacaoId);
        Task<Prova> ObterProvaDoAlunoAsync(int avaliacaoId, int alunoId);
        void IncluirNovaAvaliacao(Prova prova);
    }
}
