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
    public class CompanyRepository : Repository<Company>, ICompanyRepository 
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
         
        }
/*        public void Save()
        {
        _db.SaveChanges();
        }*/

        public void Update(Company company)
        {
            _db.Companys.Update(company);
        }
    }
}
