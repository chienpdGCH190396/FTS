using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTS.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class CreateAccountForm
    {
        //[Required]
        [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class CreateTraineeForm
    {
        //[Required]
        [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        [Display(Name = "Date of birth")]
        public string DOB { get; set; }

        public string Education { get; set; }

        [Display(Name = "Main Programming Language")]
        public string MainProgrammingLanguage { get; set; }

        public string TOEIC { get; set; }

        public string Experience { get; set; }

        public string Department { get; set; }   
        
        public string Location { get; set; }
    }

}
