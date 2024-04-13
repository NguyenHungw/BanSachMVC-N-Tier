using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BanSach2.DataAcess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> Dbset;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.Dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            Dbset.Add(entity);
        }

        //include category,covertype
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            if (Dbset == null)
            {
                throw new ArgumentNullException(nameof(Dbset), "Dbset is null.");
            }

            IQueryable<T> query = Dbset;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                // Sử dụng mảng kí tự phân cách để tăng tính linh hoạt
                char[] separators = new char[] { ',', ';' };

                foreach (var item in includeProperties.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item.Trim());
                }
            }
            return query.ToList();
        }

        public T GetFistOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            if (Dbset == null)
            {
                throw new ArgumentNullException(nameof(Dbset), "Dbset is null.");
            }

            IQueryable<T> query = Dbset;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                // Sử dụng mảng kí tự phân cách để tăng tính linh hoạt
                char[] separators = new char[] { ',', ';' };

                foreach (var item in includeProperties.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item.Trim());
                }
            }
            query =query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            Dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Dbset.RemoveRange(entities);
        }
    }
}
