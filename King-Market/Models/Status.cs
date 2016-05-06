using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace King_Market.Models
{
    public class Status
    {
        [Key]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<SaleOrder> SaleOrders { get; set; }

        public virtual ICollection<BuyOrder> BuyOrders { get; set; }
    }
}