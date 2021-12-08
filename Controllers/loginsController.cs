using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HereWeGo.Models;

namespace HereWeGo.Controllers
{
    public class loginsController : Controller
    {
        private OstoksetDBEntities2 db = new OstoksetDBEntities2();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult kirjautuminen()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(login LoginModel)
        {
            OstoksetDBEntities2 db = new OstoksetDBEntities2();
 
            var LoggedUser = db.login.SingleOrDefault(x => x.username == LoginModel.username && x.password == LoginModel.password);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successful login";
                Session["UserName"] = LoggedUser.username;
                if(LoggedUser.username == "1337G")
                {
                    Session["Admin"] = LoggedUser.username;
                }
                return RedirectToAction("Index", "stores");
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessful";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("kirjautuminen", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("kirjautuminen", "logins");
        }
        // GET: logins
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("kirjautuminen", "logins");
            }
            else
            {
                return View(db.login.ToList());
            }
        }

        // GET: logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            login login = db.login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: logins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,username,password")] login login)
        {
            if (ModelState.IsValid)
            {
                db.login.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(login);
        }

        // GET: logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            login login = db.login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,username,password")] login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(login);
        }

        // GET: logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            login login = db.login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            login login = db.login.Find(id);
            db.login.Remove(login);
            db.SaveChanges();
            return RedirectToAction("Index");
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
