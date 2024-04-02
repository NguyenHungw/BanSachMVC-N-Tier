using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace Bans.Model
{
    public class CoverType
    {
        [Key] // khoa chinh
        public int ID { get; set; }
        [Required] // not null
        public string Name { get; set; }
        
    }
}
