using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services.Interfaces
{
    public interface IAlunoService
    {
        Task<int> IncluirNovoAlunoAsync(string login, string nome, int turmaId);
    }
}
