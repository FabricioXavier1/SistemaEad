using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;

namespace PUC.LDSI.DataBase.Repository
{
    public class OpcaoProvaRepository : Repository<OpcaoProva>, IOpcaoProvaRepository
    {
        private readonly AppDbContext _context;

        public OpcaoProvaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
