using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Mechanik_Web.Models;

namespace E_Mechanik_Web.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        // GET: Messages
        [Authorize]
        public ActionResult Index()
        {
            var message = _db.Messages.Where(c=>c.ReceiverName == User.Identity.Name);
            if (message != null)
            {
                return View(message);
            }
            return HttpNotFound();
        }

        // GET: Messages/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = _db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReceiverName,Subject,Text")] Message message)
        {
            if (ModelState.IsValid)
            {
                var x = _db.Users.Where(c => c.Email == message.ReceiverName).Select(c => c.Email).FirstOrDefault() ;
                if (x!=null)
                {
                    message.SendTime = DateTime.Now.ToString("mm:hh | dd.MM.yyyy");
                    message.SenderName = User.Identity.Name;
                    _db.Messages.Add(message);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Użytkownik, do którego chcesz wysłać wiadomość nie istnieje!";
                    return View();
                }
               
            }

            return View(message);
        }

        [Authorize]
        // POST: Messages/Delete/5
        public ActionResult Delete(int id)
        {
            Message message = _db.Messages.Find(id);
            if (message!=null)
            {
                if (message.ReceiverName==User.Identity.Name)
                {
                    _db.Messages.Remove(message);
                    _db.SaveChanges();
                    return RedirectToAction("index");
                }
            }

            return HttpNotFound();


        }
        [Authorize]
        public ActionResult Reply(int id)
        {
            Message message = _db.Messages.Where(c => c.Id == id).FirstOrDefault();
            string z = message.SenderName;
            message.SenderName = message.ReceiverName;
            message.ReceiverName = z;
            message.Subject = "Re: " + message.Subject;
            message.Text = "Od: " + message.ReceiverName + ": \"" + message.Text + "\"\n\n"; 
            return View(message);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "ReceiverName,Subject,Text")] Message message)
        {
            if (ModelState.IsValid)
            {
                var x = _db.Users.Where(c => c.Email == message.ReceiverName).Select(c => c.Email).FirstOrDefault();
                if (x != null)
                {
                    message.SendTime = DateTime.Now.ToString("mm:hh | dd.MM.yyyy");
                    message.SenderName = User.Identity.Name;
                    _db.Messages.Add(message);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Użytkownik, do którego chcesz wysłać wiadomość nie istnieje!";
                    return View();
                }

            }

            return View(message);
        }

    }
}
