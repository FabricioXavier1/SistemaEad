using PUC.LDSI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface IPublicacaoRepository : IRepository<Publicacao>
    {
        Task<List<Publicacao>> ListarPublicacoesPorTurmaAsync(int turmaId);
        Task<List<Publicacao>> ObterPublicacaoComProvasAsync(int publicacaoId);
        Task<List<Publicacao>> ListarPublicacoesDoProfessorAsync(int professorId);
        Task<List<Publicacao>> ListarPublicacoesDoAlunoAsync(string login);
        Task<Publicacao> ObterPublicacaoAsync(int id);
    }
}
