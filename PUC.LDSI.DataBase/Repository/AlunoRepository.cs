using System.Linq;
using System.Threading.Tasks;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class AlunoRepository : Repository<Aluno>, IAlunoRepository
    {
        private readonly AppDbContext _context;

        public AlunoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> IncluirNovoAlunoAsync(Aluno aluno)
        {
            _context.Add(aluno);

            var retorno = await _context.SaveChangesAsync();

            return retorno;
        }

        public Aluno ObterPorLogin(string login)
        {
            var retorno = _context.Alunos.Where(x => x.Login == login).FirstOrDefault();

            return retorno;
        }
    }
}
