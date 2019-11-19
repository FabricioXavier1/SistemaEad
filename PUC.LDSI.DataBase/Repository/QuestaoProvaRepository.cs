using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class QuestaoProvaRepository : Repository<QuestaoProva>, IQuestaoProvaRepository
    {
        private readonly AppDbContext _context;

        public QuestaoProvaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
