using System.Linq.Expressions;
using PatientTest.Core.Entities;

namespace PatientTest.Core.Interfaces.Generic;

public interface IRepository<T> where T : Base
{
    Guid Create(T entity);
    void CreateRange(IEnumerable<T> entityList);
    void Update(T entity);
    void UpdateRange(List<T> entityList);
    void Delete(Guid id);
    void Delete(T entity);
    void DeleteRange(List<T> entityList);
    void DeleteRangeByPredicate(Expression<Func<T, bool>> predicate);
    IQueryable<T1> GetListResultSpec<T1>(Func<IQueryable<T>, IQueryable<T1>> func);
    T1 GetResultSpec<T1>(Func<IQueryable<T>, T1> func);
}