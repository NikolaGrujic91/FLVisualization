using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using FLVisualization.DAL.EF;
using FLVisualization.Models.Entities.Base;

namespace FLVisualization.DAL.Repos.Base
{
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        private bool disposed = false;
        protected readonly FLVisualizationContext db;
        protected DbSet<T> table;
        
        protected RepoBase()
        {
            db = new FLVisualizationContext();
            table = db.Set<T>();
        }

        protected RepoBase(DbContextOptions<FLVisualizationContext> options)
        {
            db = new FLVisualizationContext(options);
            table = db.Set<T>();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            db.Dispose();
            disposed = true;
        }

        #region Properties

        public FLVisualizationContext Context => db;

        #endregion

        #region IRepo Members

        public int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (RetryLimitExceededException ex)
            {
                Console.WriteLine("Unable to save changes.Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public virtual int Add(T entity, bool persist = true)
        {
            table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        internal T GetEntryFromChangeTracker(int? id)
        {
            return db.ChangeTracker.Entries<T>().Select((EntityEntry e) => (T)e.Entity).FirstOrDefault(x => x.Id == id);
        }

        public T Find(int? id) => table.Find(id);

        public T GetFirst() => table.FirstOrDefault();

        public virtual IEnumerable<T> GetAll() => table;

        internal IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take) => query.Skip(skip).Take(take);

        public virtual IEnumerable<T> GetRange(int skip, int take) => GetRange(table, skip, take);

        public bool HasChanges => db.ChangeTracker.HasChanges();

        public int Count => table.Count();

        #endregion

    }
}
