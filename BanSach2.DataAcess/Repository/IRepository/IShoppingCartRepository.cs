﻿using Bans.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach2.DataAcess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
     int IncrementCount(ShoppingCart shoppingCart, int count);

     int DecrementCount(ShoppingCart shoppingCart, int count);

    }
}
