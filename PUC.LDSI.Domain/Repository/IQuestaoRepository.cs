using PUC.LDSI.Domain.Entities;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface IQuestaoRepository : IRepository<Questao>
    {
        Task<int> ExcluirQuestaoAsync(int id);
        Task<Questao> ObterComOpcoesAsync(int id);
    }
}
