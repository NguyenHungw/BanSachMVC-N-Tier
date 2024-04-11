using Bans.Model;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach2.DataAcess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository 
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
         
        }
        public void Save()
        {
        _db.SaveChanges();
        }

        public void Update(Product product)
        {
            //cap nhat hinh anh no se khac
            var objFromDb = _db.Products.SingleOrDefault(p => p.Id == product.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = product.Name;
                objFromDb.ISBN = product.ISBN;
                objFromDb.Price50 = product.Price50;
                objFromDb.Price100 = product.Price100;
                objFromDb.Description = product.Description;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.Author = product.Author;
                objFromDb.CoverTypeId = product.CoverTypeId;
                //neu product truyen vao co hinh anh moi !=null thi truyen vao con k thi lay hinh anh cu
                if(product.ImageURL!= null)
                {
                    objFromDb.ImageURL = product.ImageURL;
                }
            }
            _db.Products.Update(product);
        }
    }
}
