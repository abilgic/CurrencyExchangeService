using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ConversionLog
    {
        [Key]
        public int Id { get; set; }
        public string Currency1 { get; set; }
        public string Currency2 { get; set; }
        public double Amount { get; set; }
        public double Result { get; set; }
        public DateTime Createdate { get; set; }
    }
}
