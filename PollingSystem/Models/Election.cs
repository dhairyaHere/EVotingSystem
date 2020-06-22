using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PollingSystem.Models
{
    public class Election
    {
        [Key]
        [Required]
        //[Display(Name = "Election Code")]
        [MinLength(5, ErrorMessage = "The election code should be 5 to 7 characters long.")]
        [MaxLength(7)]
        public string pollCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name="Election Name/Purpose ")]

        public string pollName { get; set; }

        

        //[Required]
        [Display(Name = "Organizer / Host ")]
        public string hostName { get; set; }

        [Required]
        [Display(Name = "Number Of Contestants ")]
        public int noOfContestants { get; set; }

        [Range(0,1)]
        public int hasEnded { get; set; }
    }
}