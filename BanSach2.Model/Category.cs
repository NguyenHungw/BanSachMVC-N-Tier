using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BanSach2.Model
{
    public class Category
    {
        [Key] // khoa chinh
        public int ID { get; set; }
        [Required] // not null
        public string Name { get; set; }
        [Display(Name = "Display Order")]
        [Range(1, 100, ErrorMessage = "Values for  {0} must be between {1} and {2}.")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
