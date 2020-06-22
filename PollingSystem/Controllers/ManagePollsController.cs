using PollingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PollingSystem.Controllers
{
    public class ManagePollsController : Controller
    {
        // GET: ManagePolls
        private ConductPoll db = new ConductPoll();
        public ActionResult Ongoing()
        {
            return View(db.Elections.Where(x => x.hasEnded == 0).ToList());
        }

        public ActionResult Completed()
        {
            /*var result = db.Contestants.GroupBy(emp => emp.electionCode)
                                    .Select(gr => new
                                    {
                                        Departemnt = gr.Key,
                                        Employee = gr.OrderByDescending(x => x.wage)
                                                     .FirstOrDefault()
                                    });*/

            //var highVotesPerEle = db.Contestants.GroupBy(x => x.electionCode)
              //   .Select(g => g.OrderByDescending(x => x.totalVotes).First());

            //ViewBag.result = highVotesPerEle;

            return View(db.Elections.Where(x => x.hasEnded == 1).ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election ele = db.Elections.Find(id);
            if (ele == null)
            {
                return HttpNotFound();
            }
            return View(ele);
        }

        public ActionResult CompDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election ele = db.Elections.Find(id);

            int maxvotes = db.Contestants.Where(x => (x.electionCode == id)).Max(y => y.totalVotes);
            var winner = db.Contestants.Where(x => (x.electionCode == id && x.totalVotes == maxvotes));
            ViewBag.result = winner.ToList();

            //ViewBag.winnername = winner.Select(x => x.contestant_id);

            var mem = db.Contestants.Include("election").Where(x => x.electionCode == id);
            ViewBag.elememlist = mem.ToList();

            //var highVotesPerEle = db.Contestants.Where();
            //ViewBag.result = highVotesPerEle;

            if (ele == null)
            {
                return HttpNotFound();
            }
            return View(ele);
        }

        public ActionResult ElectionMembers(string id)
        {
            var mem = db.Contestants.Include("election").Where(x => x.electionCode == id);

            return View(mem.ToList());
        }

        public ActionResult MemDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var electionName = db.Contestants.Include("election").Where(x => x.contestant_id == id);

            var electionName = (from s in db.Contestants.Include("election")
                               where s.contestant_id == id
                               select s.election.pollName).FirstOrDefault();
            ViewBag.ename = electionName;

            Contestant con = db.Contestants.Find(id);
            if (con == null)
            {
                return HttpNotFound();
            }
            return View(con);
        }
        [Authorize]
        public ActionResult ConfirmVote(int c_id , string e_id)
        {
            
            bool found;
            var vin = Session["vin"].ToString();
            var checkvoted = db.CastVotes.Where(x => (x.electionCode == e_id && x.voterId == vin)).FirstOrDefault();
            if (checkvoted == null)
            {
                found = false;
                
                var con_name = db.Contestants.Where(x => x.contestant_id == c_id).Select(x => x.contestantName).FirstOrDefault();
                ViewBag.con_name = con_name;

                var incvotes = db.Contestants.Where(x => (x.contestant_id == c_id && x.electionCode == e_id)).FirstOrDefault();
                incvotes.totalVotes = incvotes.totalVotes + 1;
                

                CastVote cv = new CastVote();

                cv.electionCode = e_id;
                cv.voterId = Session["vin"].ToString();
                cv.hasVotedTo = c_id;

                db.CastVotes.Add(cv);
                db.SaveChanges();
            }
            else
            {
                found = true;
            }
            ViewBag.found = found;
            return View();
        }

    }
}