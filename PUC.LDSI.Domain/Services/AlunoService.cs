using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using PUC.LDSI.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<int> IncluirNovoAlunoAsync(string login, string nome, int turmaId)
        {
            var aluno = new Aluno() { Nome = nome, Login = login, TurmaId = turmaId };

            return await _alunoRepository.IncluirNovoAlunoAsync(aluno);
        }
    }
}
