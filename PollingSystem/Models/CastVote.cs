using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PollingSystem.Models
{
    public class CastVote
    {
        [Key]
        public int vote_id { get; set; }

        
        public int hasVotedTo { get; set; }
        [ForeignKey("hasVotedTo")]
        public Contestant contestant { get; set; }
        public string voterId { get; set; }
        [ForeignKey("voterId")]
        public VoterDetail Voter { get; set; }
        public string electionCode { get; set; }
        [ForeignKey("electionCode")]
        public Election election{ get; set; }
    }
}