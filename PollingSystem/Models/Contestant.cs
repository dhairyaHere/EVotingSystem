using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace PollingSystem.Models
{
    public class Contestant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int contestant_id { get; set; }

        [Required]
        [Display(Name = "Candidate's Name")]
        public string contestantName { get; set; }

        [Range(0, Int32.MaxValue)]
        public int totalVotes { get; set; }

        [Display(Name = "Candidate's Age")]
        [Required]
        [Range(18, Int32.MaxValue, ErrorMessage = "Candidate must be atleast 18 y/o to fight election :)")]
        public int age { get; set; }

        [Display(Name = "Candidate's Locality")]
        public string place { get; set; }

        [DataType(DataType.Upload)]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.JPG|.PNG|.GIF)$", ErrorMessage = "Only Image files allowed.")]
        public string symbol { get; set; }

        /*
        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }
        */

        public string electionCode { get; set; }
        [ForeignKey("electionCode")]
        public Election election { get; set; }
    }
}