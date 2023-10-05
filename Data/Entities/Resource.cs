using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
<<<<<<<< HEAD:Data/Entities/ItemMaterial.cs
    public class ItemMaterial
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
        [ForeignKey("itemId")]
        public Guid itemId { get; set; }
        public Item item { get; set; }

        [ForeignKey("materialId")]
        public Guid materialId { get; set; }
        public Material material { get; set; }
========
    public class Resource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        public string link { get; set; }

        [ForeignKey("reportId")]
        public Guid reportId { get; set; }
        public Report Report { get; set; }

>>>>>>>> ThanhDuc:Data/Entities/Resource.cs
    }
}
