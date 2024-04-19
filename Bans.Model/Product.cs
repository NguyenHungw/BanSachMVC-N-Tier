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
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1,100000000)]
        public double Price50 { get; set; }
        [Range(1, 100000000)]
        public double Price100 { get; set; }

        // Danh sách giá
        [NotMapped] // Không ánh xạ vào cơ sở dữ liệu
        public List<double> ListPrice
        {
            get
            {
                return new List<double> { Price50, Price100 };
            }
        }

        [ValidateNever]

        public string ImageURL {  get; set; }

        [Required]

        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


        [Required]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }
    }
}
