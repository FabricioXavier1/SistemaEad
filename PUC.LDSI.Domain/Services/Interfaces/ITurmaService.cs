using PUC.LDSI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services.Interfaces
{
    public interface ITurmaService
    {
        Task<int> AdicionarTurmaAsync(string descricao);
        Task<int> AlterarTurmaAsync(int id, string descricao);
        List<Turma> ListarTurmas();
        Task<Turma> ObterAsync(int? id);
        Task ExcluirAsync(int id);
    }
}
