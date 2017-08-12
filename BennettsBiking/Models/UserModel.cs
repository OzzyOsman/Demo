using BennettsBiking.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace BennettsBiking.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Id = null;
        }

        public Guid? Id { get; set; }

        [Required]
        [MaxLength(20)]
        [CustomStringValidator]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        [CustomStringValidator]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DateValidator]
        [DataType("Date")]
        [Display(Name = "Date of birth")]
        public string DateOfBirth { get; set; }
    }
}