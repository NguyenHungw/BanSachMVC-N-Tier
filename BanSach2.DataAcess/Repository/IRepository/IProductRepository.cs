using Bans.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach2.DataAcess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        
        void Update(Product product);
       // void Save();

    }
}
