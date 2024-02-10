using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Security.Principal;

namespace skyline.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "You  have to  provide a valid full name")]
        [MinLength(10, ErrorMessage = "Full Name  musn't be  less than  10 charcters.")]
        [MaxLength(50, ErrorMessage = "Full Name  musn't be  less than  50 charcters.")]
        public string FullName { get; set; }

        [DisplayName("Occupation")]
        [Required(ErrorMessage = "You  have to  provide a valid Position")]
        [MinLength(2, ErrorMessage = "Position  musn't be  less than  2 charcters.")]
        [MaxLength(20, ErrorMessage = "Position  musn't be  less than  20 charcters.")]
        public string Position { get; set; }

        [Range(3500, 35000, ErrorMessage = "Sallary  musn be  between 3500 EGP and  35000 EGP.")]
        public decimal Sallary{ get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Join Date and Time")]
        public DateTime JoinDateTime { get; set; }
        public bool IsActive{ get; set; }

        [DataType(DataType.Time)]
        public DateTime AttendenceTime { get; set; }

        [DisplayName("Phone")]
        [RegularExpression("^0\\d{10}$", ErrorMessage="Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Code { get; set; }

        [NotMapped]
        [Compare("Code", ErrorMessage= "Code and ConfirmCode Do Not Match.")]
        [DataType(DataType.Password)]
        public string ConfirmCode { get; set; }

        [DisplayName("Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid department.")]
        public int DepartmentId { get; set; }

        [ValidateNever]
        public Department Department { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? ImagePath { get; set; }
    }
}
