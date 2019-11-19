using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly AppDbContext _context;
        
        public AvaliacaoRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<int> AlterarAvaliacaoAsync(Avaliacao avaliacao)
        {
            _context.Update(avaliacao);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> IncluirNovaAvaliacaoAsync(Avaliacao avaliacao)
        {
            _context.Add(avaliacao);

            await _context.SaveChangesAsync();

            return avaliacao.Id;
        }

        public async Task<List<Avaliacao>> ListarAvaliacoesDoProfessor(string login)
        {
            var query = _context.Avaliacoes.Include(x => x.Professor).Where(x => x.Professor.Login == login);

            return await query.ToListAsync();
        }

        public async Task<Avaliacao> ObterAsync(int id)
        {
            return await _context.Avaliacoes.FindAsync(id);
        }

        public async Task ExcluirAvaliacaoAsync(int id) {
            var avaliacaoTask = _context.Avaliacoes.FirstOrDefaultAsync(x => x.Id == id);

            var publicacaoTask = _context.Publicacoes.FirstOrDefaultAsync(x => x.AvaliacaoId == id);

            var respostasTask = _context.OpcoesProva.Include(x => x.QuestaoProva).ThenInclude(y => y.Prova).Where(x => x.QuestaoProva.Prova.AvaliacaoId == id).ToListAsync();

            var opcoesTask = _context.OpcoesAvaliacao.Include(x => x.Questao).ThenInclude(y => y.Avaliacao).Where(x => x.Questao.AvaliacaoId == id).ToListAsync();

            publicacaoTask.Wait();
            if (publicacaoTask.Result != null)
                _context.Remove(publicacaoTask.Result);

            respostasTask.Wait();
            if (respostasTask.Result.Count > 0)
                _context.RemoveRange(respostasTask.Result);

            opcoesTask.Wait();
            if (opcoesTask.Result.Count > 0)
                _context.RemoveRange(opcoesTask.Result);

            avaliacaoTask.Wait();
            if (avaliacaoTask.Result != null)
                _context.Remove(avaliacaoTask.Result);

            await _context.SaveChangesAsync();
        }

        public async Task<Avaliacao> ObterComQuestoresAsync(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(x => x.Professor)
                .Include(x => x.Questoes).ThenInclude(y => y.Opcoes)
                .FirstOrDefaultAsync(m => m.Id == id);

            return avaliacao;
        }

        public Avaliacao ObterAvaliacaoComQuestoes(int id)
        {
            var avaliacao = _context.Avaliacoes
                .Include(x => x.Professor)
                .Include(x => x.Questoes).ThenInclude(y => y.Opcoes)
                .FirstOrDefault(m => m.Id == id);

            return avaliacao;
        }
    }
}
