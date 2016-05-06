using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace King_Market.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public int ProductId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Product Product { get; set; }
    }
}