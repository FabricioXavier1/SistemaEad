using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class TurmaRepository : Repository<Turma>, ITurmaRepository
    {
        private readonly AppDbContext _context;

        public TurmaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Turma> ObterComAlunosAsync(int id)
        {
            var turma = await _context.Turmas.Include(x => x.Alunos).Where(x => x.Id == id).FirstOrDefaultAsync();

            return turma;
        }
    }
}
