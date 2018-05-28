using ChocOvation.Models;
using System.ComponentModel.DataAnnotations;

namespace ChocOvation.ViewModels
{
    public class OrderFormViewModel
    {

        [Required]
        public string MaterialName { get; set; }

        [Required]
        public int QuantityPerYear { get; set; }

        //public int OrderID { get; set; }
        //public Order Order { get; set; }
    }
}