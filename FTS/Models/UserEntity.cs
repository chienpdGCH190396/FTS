using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FTS.Models
{
    public class UserEntity : IdentityUser
    {
        public string Name { get; set; }

        [RegularExpression("[0-9]*$", ErrorMessage = "Invalid Age")]
        public string Age { get; set; }

        [RegularExpression(@"(((19|20)\d\d)\-(0[1-9]|1[0-2])\-((0|1)[0-9]|2[0-9]|3[0-1]))$", ErrorMessage = "Invalid date format.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy.MM.dd}")]
        public string DOB { get; set; }

        public string Education { get; set; }

        [Display(Name = "Main Programming Language")]
        public string MainProgrammingLanguage { get; set; }

        [RegularExpression("[0-9]*$", ErrorMessage = "Invalid TOEIC")]
        public string TOEIC { get; set; }

        public string Experience { get; set; }

        public string Department { get; set; }

        public string Location { get; set; }

        [RegularExpression("[0-9]*$", ErrorMessage = "Invalid Telephone")]
        public string Telephone { get; set; }

        [Display(Name = "Working Place")]
        public string WorkingPlace { get; set; }

        public string Role { get; set; }

        public UserEntity()
        {

        }


        public string ToSeparatedString(string separator)
        {
            return $"{this.Id}{separator}" +
                $"{this.Name}{separator}" +
                $"{this.UserName}{separator}" +
                $"{this.Email}{separator}" +
                $"{this.Role}";
        }
    }
}