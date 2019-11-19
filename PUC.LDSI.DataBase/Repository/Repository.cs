using Microsoft.EntityFrameworkCore;
using PUC.LDSI.Domain.Entities;
using PUC.LDSI.Domain.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PUC.LDSI.DataBase.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected AppDbContext DbContext;
        protected DbSet<TEntity> DbEntity;

        protected Repository(AppDbContext context)
        {
            DbContext = context;

            DbEntity = DbContext.Set<TEntity>();
        }

        public virtual void Adicionar(TEntity obj)
        {
            DbEntity.Add(obj);
        }

        public virtual void Modificar(TEntity obj)
        {
            DbEntity.Update(obj);
        }

        public virtual async Task<TEntity> ObterAsync(int id)
        {
            return await DbEntity.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual IQueryable<TEntity> ObterTodos()
        {
            return DbEntity.AsNoTracking().AsQueryable();
        }

        public virtual IQueryable<TEntity> Consultar(Expression<Func<TEntity, bool>> predicate)
        {
            return DbEntity.AsNoTracking().Where(predicate);
        }

        public virtual void Remover(int id)
        {
            DbEntity.Remove(DbEntity.Find(id));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
