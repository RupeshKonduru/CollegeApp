using CollegeApp.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Model
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Student Name should not excced 30 char")]
        public string StdName { get; set; }
        [EmailAddress(ErrorMessage = "Pls Enter Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pls Enter Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Pls Enter Password")]
        //[StringLength(30, ErrorMessage = "Password should not excced 30 char")]
        //public string Password { get; set; }

        //[Compare(nameof(Password))]
        //public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "Pls Enter Age")]
        //[Range(18, 100)]
        //public int Age { get; set; }

        //[Phone]
        //public decimal Mobile { get; set; }
        public DateTime DOB { get; set; }

        //[DateCheck]
        //public DateTime DateofAddmision { get; set; }
    }
}
