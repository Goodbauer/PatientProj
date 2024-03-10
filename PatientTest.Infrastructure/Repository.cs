using Serilog;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PatientTest.Core.Entities;
using PatientTest.Core.Interfaces.Generic;

namespace PatientTest.Infrastructure;

internal class Repository<T> : IRepository<T> where T : Base
    {
        protected readonly Context _context;
        public Repository(Context context) => _context = context;

        public Guid Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            Log.Information("Created: " + entity.ToString());
            return entity.Id;
        }

        public void CreateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
            foreach (var item in entities)
                Log.Information("Created: " + item.ToString());
        }

        public void DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
            foreach (var item in entities)
                Log.Information("Deleted: " + item.ToString());
        }

        public void Delete(Guid id)
        {
            var entity = _context.Set<T>().SingleOrDefault(x => x.Id == id);
            Log.Information("Deleting: " + entity.ToString());
            if(entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
                Log.Information("Deleted");
            }
        }
        public void Delete(T entity)
        {
            Log.Information("Deleting: " + entity.ToString());
            _context.Remove(entity);
            _context.SaveChanges();
            Log.Information("Deleted");
        }

        public void Update(T entity)
        {
            var local = _context.Set<T>().FirstOrDefault(l => l.Id.Equals(entity.Id));
            _context.Entry(local).State = EntityState.Detached;
            Log.Information("Update: " + local.ToString());

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            Log.Information("Updated to : " + entity.ToString());
        }

        public void UpdateRange(List<T> entities)
        {
            foreach (var item in entities)
                Log.Information("Deleted: " + item.ToString());

            entities.ForEach(l => _context.Entry(l).State = EntityState.Modified);
            _context.SaveChanges();

            foreach (var item in entities)
                Log.Information("Deleted: " + item.ToString());
        }

        public IQueryable<T1> GetListResultSpec<T1>(Func<IQueryable<T>, IQueryable<T1>> func) => func(_context.Set<T>().AsNoTracking());
        public T1 GetResultSpec<T1>(Func<IQueryable<T>, T1> func) => func(_context.Set<T>().AsNoTracking());

        public void DeleteRangeByPredicate(Expression<Func<T, bool>> predicate)
        {
            var list = _context.Set<T>().Where(predicate).AsNoTracking().AsEnumerable<T>();
            _context.Set<T>().RemoveRange(list);
            _context.SaveChanges();
            foreach (var item in list)
                Log.Information("Deleted: " + item.ToString());
        }
    }