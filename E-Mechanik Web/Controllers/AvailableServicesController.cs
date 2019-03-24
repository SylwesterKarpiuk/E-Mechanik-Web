using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Mechanik_Web.Models;
using E_Mechanik_Web.ViewModels;

namespace E_Mechanik_Web.Controllers
{
    public class AvailableServicesController : BaseController
    {

        // GET: AvailableServices
        public ActionResult Index()
        {
            return View(_db.AvailableServices.ToList());
        }

        // GET: AvailableServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableService availableService = _db.AvailableServices.Find(id);
            if (availableService == null)
            {
                return HttpNotFound();
            }
            return View(availableService);
        }


        [Authorize(Roles = "Admin")]
        // GET: AvailableServices/Create
        public ActionResult Create()
        {
            var model = new CreateAvailableServiceViewModel();
            model.Categories = _db.AvailableServiceCategories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(model);
        }

        // POST: AvailableServices/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,AvailableServiceCategoryId")] AvailableService availableService)
        {
            if (ModelState.IsValid)
            {
                _db.AvailableServices.Add(availableService);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(availableService);
        }

        // GET: AvailableServices/Edit/5

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableService availableService = _db.AvailableServices.Find(id);
            if (availableService == null)
            {
                return HttpNotFound();
            }
            return View(availableService);
        }

        // POST: AvailableServices/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,AvailableServiceCategoryId")] AvailableService availableService)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(availableService).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(availableService);
        }

        // GET: AvailableServices/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableService availableService = _db.AvailableServices.Find(id);
            if (availableService == null)
            {
                return HttpNotFound();
            }
            return View(availableService);
        }

        // POST: AvailableServices/Delete/5

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvailableService availableService = _db.AvailableServices.Find(id);
            _db.AvailableServices.Remove(availableService);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
