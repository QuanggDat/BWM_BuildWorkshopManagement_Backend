using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItemModel
    {
        public Guid id { get; set; }
        public Guid? itemCategoryId { get; set; }
        public string? itemCategoryName { get; set; } 
        public string name { get; set; } = null!;
        public string? image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; } = null!;
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public double price { get; set; }      
        public List<ItemMaterialModel> listMaterial { get; set; } = null!;
        public List<ProcedureItemModel> listProcedure { get; set; } = null!;
    }
  
    public class CreateItemModel
    {
        public Guid itemCategoryId { get; set; }
        public string name { get; set; } = null!;
        public string? image { get; set; } 
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; } = null!;
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public List<AddProcedureItemModel> listProcedure { get; set; } = null!;
        public List<ItemMaterialModel> listMaterial { get; set; } = null!;
    }

    public class AddProcedureItemModel
    {
        public Guid procedureId { get; set; }
        public int priority { get; set; }
    }
    public class ProcedureItemModel
    {
        public Guid procedureId { get; set; }
        public int priority { get; set; }
        public List<Guid>? stepId { get; set; }
    }


    public class ItemMaterialModel
    {
        public Guid materialId { get; set; }
        public int quantity { get; set; }
    }

    public class UpdateItemModel
    {
        public Guid id { get; set; }
        public Guid itemCategoryId { get; set; }     
        public string name { get; set; } = null!;
        public string? image { get; set; }
        //public double length { get; set; } 
        //public double depth { get; set; }
        //public double height { get; set; }
        public string unit { get; set; } = null!;
        //public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public List<AddProcedureItemModel> listProcedure { get; set; } = null!;
        public List<ItemMaterialModel> listMaterial { get; set; } = null!;
    }

    public class DeleteItemModel
    {
        public Guid id { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}

    

