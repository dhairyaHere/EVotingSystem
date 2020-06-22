using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PollingSystem.Models
{
    public class VoterDetail
    {
        [Key]
        public string voterId { get; set; }

        [Required]
        public string voterName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string emailId { get; set; }

        [Required]
        [Display(Name = "Age")]
        [Range(18,Int32.MaxValue,ErrorMessage = "The Candidate must be atleast 18 years old to be eligible for voting !")]
        public int age { get; set; }

        [Required]
        [Display(Name = "Locality")]
        public string place { get; set; }
    }
}