using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bans.Model.ViewModel
{
    public class ProductVM
    {
         public Product product {  get; set; }
        //convert list category thanh list item
        public IEnumerable<SelectListItem> CategoryList { get; set; }
         
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }

    }
}
