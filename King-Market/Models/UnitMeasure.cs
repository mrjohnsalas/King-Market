using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace King_Market.Models
{
    public class UnitMeasure
    {
        [Key]
        [Display(Name = "Unit Measure")]
        public int UnitMeasureId { get; set; }

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(5, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 1)]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}