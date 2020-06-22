using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PollingSystem.Models;

namespace PollingSystem.Controllers
{
    public class ContestantsController : Controller
    {
        private ConductPoll db = new ConductPoll();

        // GET: Contestants
        public ActionResult Index()
        {
            var contestants = db.Contestants.Include(c => c.election);
            return View(contestants.ToList());
        }

        // GET: Contestants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Find(id);
            if (contestant == null)
            {
                return HttpNotFound();
            }
            return View(contestant);
        }

        // GET: Contestants/Create
        public ActionResult Create()
        {
            ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName");

            var candy = Convert.ToInt32(Session["candy"].ToString());
            var code = Session["code"].ToString();
            var electionName = Session["electionName"].ToString();

            ViewBag.candy = candy;
            ViewBag.code = code;
            ViewBag.electionName = electionName;

            return View();
        }

        // POST: Contestants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] symbol,FormCollection form)                      
        {
            if (ModelState.IsValid)
            {
                
                Contestant[] mul = new Contestant[Convert.ToInt32(Session["candy"].ToString())];
                string name,age,place;
                //string symbol;

                
                    for (int i = 0; i < Convert.ToInt32(Session["candy"].ToString()); i++)
                    {
                        mul[i] = new Contestant();

                        name = "name_" + i.ToString();
                        mul[i].contestantName = form[name];

                        age = "age_" + i.ToString();
                        mul[i].age = Convert.ToInt32(form[age]);

                        place = "place_" + i.ToString();
                        mul[i].place = form[place];

                    //symbol = "symbol_" + i.ToString();
                    //mul[i].symbol = form[symbol];
                    if (symbol[i] != null)
                    {
                        string path1 = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(symbol[i].FileName));
                        
                        mul[i].symbol = "~/Content/Images/" + symbol[i].FileName;
                        symbol[i].SaveAs(path1);
                    }



                    mul[i].totalVotes = 0;
                    mul[i].electionCode = Session["code"].ToString();

                        db.Contestants.Add(mul[i]);
                        //db.SaveChanges();
                    }
                

                db.SaveChanges();

                Session.Remove("candy");
                Session.Remove("code");
                Session.Remove("electionName");
 
                return RedirectToAction("Index","Elections");
            }

            //ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName", contestant.electionCode);
            return View();
        }


        public ActionResult AddParticipant()
        {
            ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName");

            string ecode = Session["onecandy"].ToString();

            Election election = db.Elections.Where(x => x.pollCode == ecode).FirstOrDefault();

            ViewBag.elect = election;

            return View();
        }
        [HttpPost]
        public ActionResult AddParticipant([Bind(Include = "contestant_id,contestantName,totalVotes,age,place,symbol")] Contestant contestant, HttpPostedFileBase symbol)
        {
            if (ModelState.IsValid)
            {
                contestant.totalVotes = 0;
                contestant.electionCode = Session["onecandy"].ToString();

                

                var mod = db.Elections.Where(x => x.pollCode == contestant.electionCode).FirstOrDefault();
                mod.noOfContestants = mod.noOfContestants + 1;

                if (symbol != null)
                {
                    string path1 = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(symbol.FileName));

                    contestant.symbol = "~/Content/Images/" + symbol.FileName;
                    symbol.SaveAs(path1);
                }
                db.Contestants.Add(contestant);
                db.SaveChanges();

                Session.Remove("onecandy");

                return RedirectToAction("Index", "Elections");


            }

            //ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName", contestant.electionCode);
            return View();
        }

        // GET: Contestants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Find(id);
            if (contestant == null)
            {
                return HttpNotFound();
            }
            ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName", contestant.electionCode);
            return View(contestant);
        }

        // POST: Contestants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "contestant_id,contestantName,age,place,symbol,electionCode", Exclude ="totalVotes")] Contestant contestant)
        {
            if (ModelState.IsValid)
            {
                Contestant y= db.Contestants.FirstOrDefault(x => x.contestant_id == contestant.contestant_id);
                y.age = contestant.age;
                y.contestantName = contestant.contestantName;
                y.place = contestant.place;
                //y.symbol = contestant.symbol;
                db.SaveChanges();
                return RedirectToAction("Index","Elections");
            }
            ViewBag.electionCode = new SelectList(db.Elections, "pollCode", "pollName", contestant.electionCode);
            return View(contestant);
        }

        // GET: Contestants/Delete/5
        public ActionResult Delete(int? c_id , string e_id)
        {
            if (c_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contestant contestant = db.Contestants.Include("election").FirstOrDefault(x=>x.contestant_id == c_id);       
            if (contestant == null)
            {
                return HttpNotFound();
            }
            Session.Add("electionid",e_id);
            return View(contestant);
        }

        // POST: Contestants/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int contestant_id)
        {
            string electionid = Session["electionid"].ToString();

            Contestant contestant = db.Contestants.Find(contestant_id);
            db.Contestants.Remove(contestant);

            var election = db.Elections.Where(x => x.pollCode == electionid).FirstOrDefault();

            election.noOfContestants = election.noOfContestants - 1;

            db.SaveChanges();
            Session.Remove("electionid");
            return RedirectToAction("Index","Elections");
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
