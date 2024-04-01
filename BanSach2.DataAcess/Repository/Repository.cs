﻿using BanSach2.DataAcess.Repository.IRepository;
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

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = Dbset;
            return query.ToList();
        }

        public T GetFistOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = Dbset;
            query=query.Where(filter);
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