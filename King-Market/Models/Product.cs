using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace King_Market.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:C5}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:C5}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal UnitCost { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public int ProductTypeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Stock { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal MinStock { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal MaxStock { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public int UnitMeasureId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual UnitMeasure UnitMeasure { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }

        public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

        public virtual ICollection<BuyOrderDetail> BuyOrderDetails { get; set; }

        public virtual ICollection<IncomingGoodDetail> IncomingGoodDetails { get; set; }
    }
}