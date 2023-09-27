using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Price
    {
        [Key] public int id {  get; set; }
        public double totalPrice { get; set; }
        public double damagePrice { get; set; }
    }
}
