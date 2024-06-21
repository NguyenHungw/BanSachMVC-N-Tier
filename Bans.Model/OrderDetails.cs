using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bans.Model
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int OrderID {  get; set; }
        [ForeignKey(nameof(OrderID))]

        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        [ValidateNever]
        public Product product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        
    }
}
