﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bans.Model
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1,1000)]
        public int count { get; set; }
        public int ProductId { get; set; }

    }
}
