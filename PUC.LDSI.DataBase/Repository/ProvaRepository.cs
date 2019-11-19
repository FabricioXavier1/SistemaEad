using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class ProvaRepository : Repository<Prova>, IProvaRepository
    {
        private readonly AppDbContext _context;

        public ProvaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void IncluirNovaAvaliacao(Prova prova)
        {
            _context.Add(prova);
            _context.SaveChanges();
        }

        public async Task<List<Prova>> ListarPorAvaliacaoAsync(int avaliacaoId)
        {
            var query = _context.Provas.Where(x => x.AvaliacaoId == avaliacaoId);

            return await query.ToListAsync();
        }

        public async Task<Prova> ObterProvaDoAlunoAsync(int avaliacaoId, int alunoId)
        {
            var query = from p in _context.Provas
                        where p.AlunoId == alunoId
                           && p.AvaliacaoId == avaliacaoId
                        select p;

            var retorno = await query.Include(x => x.QuestoesProva).ThenInclude(it => it.OpcoesProva).FirstOrDefaultAsync();

            return retorno;
        }
    }
}
