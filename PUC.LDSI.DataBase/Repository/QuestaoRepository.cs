using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace PUC.LDSI.DataBase.Repository
{
    public class QuestaoRepository : Repository<Questao>, IQuestaoRepository
    {
        private readonly AppDbContext _context;

        public QuestaoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> ExcluirQuestaoAsync(int id)
        {
            var opcoes = _context.OpcoesAvaliacao.Where(x => x.QuestaoId == id).ToList();

            var respostas = _context.OpcoesProva.Include(x => x.QuestaoProva).Where(x => x.QuestaoProva.QuestaoId == id).ToList();

            var questao = _context.Questoes.Where(x => x.Id == id).FirstOrDefault();

            _context.RemoveRange(respostas);

            _context.RemoveRange(opcoes);

            _context.Remove(questao);

            await _context.SaveChangesAsync();

            return questao.AvaliacaoId;
        }

        public async Task<Questao> ObterComOpcoesAsync(int id)
        {
            var questao = await _context.Questoes.Include(a => a.Avaliacao).Include(o => o.Opcoes).Where(x => x.Id == id).FirstOrDefaultAsync();

            return questao;
        }
    }
}
