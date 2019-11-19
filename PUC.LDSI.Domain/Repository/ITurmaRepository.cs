using PUC.LDSI.Domain.Entities;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface ITurmaRepository : IRepository<Turma>
    {
        Task<Turma> ObterComAlunosAsync(int id);
    }
}
