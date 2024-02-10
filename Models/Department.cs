using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace skyline.Models
{
    public class Department
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "You  have to  provide a valid name")]
        [MinLength(10, ErrorMessage = "Name  musn't be  less than  10 charcters.")]
        [MaxLength(50, ErrorMessage = "Name  musn't be  less than  50 charcters.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(3500, 35000, ErrorMessage = "AnnualBudget  musn be  between 3500 EGP and  35000 EGP.")]
        public decimal AnnualBudget { get; set; }

        [DisplayName("Start Date and Time")]
        [DataType(DataType.Date)]  
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }

        [ValidateNever]
        public List<Employee> Employees { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? ImagePath { get; set; }
    }
}
