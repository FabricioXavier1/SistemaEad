using PUC.LDSI.Domain.Entities;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface IOpcaoAvaliacaoRepository : IRepository<OpcaoAvaliacao>
    {
        Task<int> ExcluirOpcaoAvaliacaoAsync(int id);
    }
}
