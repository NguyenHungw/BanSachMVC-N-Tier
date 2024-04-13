using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BanSach2.DataAcess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T-Category
        T GetFistOrDefault(Expression <Func<T, bool>> filter,string? includeProperties=null);
        IEnumerable<T> GetAll(string? includeProperties = null);

       
       // void GetAll(T entity);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
