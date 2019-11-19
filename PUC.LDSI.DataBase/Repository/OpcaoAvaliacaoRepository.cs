using System.Linq;
using System.Threading.Tasks;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class OpcaoAvaliacaoRepository : Repository<OpcaoAvaliacao>, IOpcaoAvaliacaoRepository
    {
        private readonly AppDbContext _context;

        public OpcaoAvaliacaoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> ExcluirOpcaoAvaliacaoAsync(int id)
        {
            var opcao = await base.ObterAsync(id);

            var respostas = _context.OpcoesProva.Where(x => x.OpcaoAvaliacaoId == id).ToList();

            _context.RemoveRange(respostas);

            _context.Remove(opcao);

            await _context.SaveChangesAsync();

            return opcao.QuestaoId;
        }
    }
}
