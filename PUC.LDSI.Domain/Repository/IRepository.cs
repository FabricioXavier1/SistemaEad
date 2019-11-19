using PUC.LDSI.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PUC.LDSI.Domain.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        IQueryable<TEntity> ObterTodos();

        IQueryable<TEntity> Consultar(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> ObterAsync(int id);

        void Adicionar(TEntity entity);

        void Modificar(TEntity entity);

        void Remover(int id);

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}
