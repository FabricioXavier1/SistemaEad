using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class PublicacaoRepository : Repository<Publicacao>, IPublicacaoRepository
    {
        private readonly AppDbContext _context;

        public PublicacaoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Publicacao>> ListarPublicacoesDoProfessorAsync(int professorId)
        {
            var query = from p in _context.Publicacoes
                        join a in _context.Avaliacoes on p.AvaliacaoId equals a.Id
                        where a.ProfessorId == professorId
                        select p;

            var retorno = await query.Distinct().Include(x => x.Avaliacao).Include(x => x.Turma).ToListAsync();

            return retorno;
        }

        public async Task<List<Publicacao>> ListarPublicacoesPorTurmaAsync(int turmaId)
        {
            var query = _context.Publicacoes.Where(x => x.TurmaId == turmaId);

            return await query.ToListAsync();
        }

        public async Task<Publicacao> ObterPublicacaoAsync(int id)
        {
            var publicacao = await _context.Publicacoes
                .Include(x => x.Turma)
                .Include(x => x.Avaliacao)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return publicacao;
        }

        public async Task<List<Publicacao>> ObterPublicacaoComProvasAsync(int publicacaoId)
        {
            var query = from p in _context.Publicacoes
                        join v in _context.Provas on p.AvaliacaoId equals v.AvaliacaoId
                        join a in _context.Alunos on v.AlunoId equals a.Id
                        where p.Id == publicacaoId 
                           && p.TurmaId == a.TurmaId 
                           && p.AvaliacaoId == p.AvaliacaoId
                        select p;

            return await query.Distinct().ToListAsync();
        }

        public async Task<List<Publicacao>> ListarPublicacoesDoAlunoAsync(string login)
        {
            var query = from p in _context.Publicacoes
                        join a in _context.Alunos on p.TurmaId equals a.TurmaId
                        where a.Login == login
                        select p;

            var pub = await query.Distinct()
                .Include(x => x.Avaliacao).ThenInclude(x => x.Questoes)
                .Include(x => x.Avaliacao).ThenInclude(x => x.Provas).ThenInclude(x => x.QuestoesProva)
                .ToListAsync();

            return pub;
        }
    }
}
