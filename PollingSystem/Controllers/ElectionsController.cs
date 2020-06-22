using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PollingSystem.Models;

namespace PollingSystem.Controllers
{
    public class ElectionsController : Controller
    {
        private ConductPoll db = new ConductPoll();

        // GET: Elections
        [Authorize]
        public ActionResult Index()
        {
            //var vin = Session["vin"].ToString();
            //var myname = db.VoterDetails.FirstOrDefault(x => x.voterId == vin).voterName;
            
            
            //var myele = db.Elections.Where(x => x.hostName == myname);
            return View(db.Elections.ToList());
            
         }

        // GET: Elections/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election election = db.Elections.Find(id);
            var mem = db.Contestants.Include("election").Where(x => x.electionCode == id);

            ViewBag.elememlist = mem.ToList();

            if (election == null)
            {
                return HttpNotFound();
            }
            return View(election);
        }

        // GET: Elections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pollCode,pollName,hostName,noOfContestants")] Election election)
        {
            if (ModelState.IsValid)
            {
                Session.Add("candy",election.noOfContestants);
                Session.Add("code", election.pollCode);
                Session.Add("electionName", election.pollName);

                election.hasEnded = 0;
                db.Elections.Add(election);
                db.SaveChanges();
                return RedirectToAction("Create","Contestants");
            }

            return View(election);
        }

        public ActionResult AddCandy(string e_id)
        {
            

            return RedirectToAction("AddParticipant","Contestants");
        }

        // GET: Elections/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election election = db.Elections.Find(id);
            if (election == null)
            {
                return HttpNotFound();
            }
            Session.Add("onecandy", id);
            return View(election);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit(string hostName,string pollName)
        {
            if (hostName !=null && pollName != null)
            {
                string id = Session["onecandy"].ToString();
               Election ele = db.Elections.FirstOrDefault(x => x.pollCode == id);
                ele.hostName = hostName;
                ele.pollName = pollName;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Elections/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election election = db.Elections.Find(id);
            if (election == null)
            {
                return HttpNotFound();
            }
            return View(election);
        }

        // POST: Elections/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(string id)
        {
            Election election = db.Elections.Find(id);
            db.Elections.Remove(election);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ClosePoll(string id)
        {
            //var endele = db.Elections.Where(x => (x.pollCode == id)).FirstOrDefault();
            //endele.hasEnded = 1;

            int maxvotes = db.Contestants.Where(x => (x.electionCode == id )).Max(y => y.totalVotes);

            var winner = db.Contestants.Include("election").Where(x => (x.electionCode == id && x.totalVotes == maxvotes));

            ViewBag.count = winner.Count();

            return View(winner);
        }

        public ActionResult DeclareWinner(int c_id,string e_id,int noc)
        {
            var endele = db.Elections.Where(x => (x.pollCode == e_id)).FirstOrDefault();
            endele.hasEnded = 1;
            if (noc > 1)
            {
                var incvotes = db.Contestants.Where(x => (x.contestant_id == c_id && x.electionCode == e_id)).FirstOrDefault();
                incvotes.totalVotes = incvotes.totalVotes + 1;
            }
            db.SaveChanges();



            var con_name = db.Contestants.Where(x => x.contestant_id == c_id).Select(x => x.contestantName).FirstOrDefault();
            ViewBag.con_name = con_name;

            var votes = db.Contestants.Where(x => x.contestant_id == c_id).Select(x => x.totalVotes).FirstOrDefault();
            ViewBag.votes = votes;

            var ele_name = db.Elections.Where(x => x.pollCode == e_id).Select(x => x.pollName).FirstOrDefault();
            ViewBag.ele_name = ele_name;


            var body = "<p><b> {0} </b></p><p> This is to inform you that candidate "+con_name+" has won the election "+ele_name+" by securing "+votes+" votes. <br/> Thank you for participating in the election !</p>";
            var message = new MailMessage();
            List<CastVote> maillist = db.CastVotes.Include("Voter").Where(x => x.electionCode == e_id).ToList();
            Collection<MailAddress> l = new Collection<MailAddress>();
            for (int i=0;i<maillist.Count();i++)
            {
                message.To.Add(new MailAddress(maillist[i].Voter.emailId));
            }
            message.To.Add(new MailAddress("dhairya2308@gmail.com"));
            // replace with valid value 
            message.From = new MailAddress("demo56651@gmail.com");  // replace with valid value
            message.Subject = "Results are out for "+ele_name;
            message.Body = string.Format(body, "Election Committee");
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "demo56651@gmail.com",  // replace with valid value
                    Password = "Demo56651@123456"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.Send(message);
                
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
