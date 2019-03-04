using System;
using System.Collections;
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
    public class AvailableServiceCategoriesController : BaseController
    {

        // GET: AvailableServiceCategories
        public ActionResult Index()
        {
            return View(_db.AvailableServiceCategories.ToList());
        }

        // GET: AvailableServiceCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableServiceCategory availableServiceCategories = _db.AvailableServiceCategories.Find(id);
            if (availableServiceCategories == null)
            {
                return HttpNotFound();
            }
            var list = new ArrayList();
            var list2 = availableServiceCategories.Services.ToArray();
            foreach (var item in list2)
            {
                if (list.Contains(item.Name))
                {
                    availableServiceCategories.Services.Remove(item);
                }
                else
                {
                    list.Add(item.Name);
                }
            }
            return View(availableServiceCategories);
        }

        // GET: AvailableServiceCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AvailableServiceCategories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AvailableServiceCategory availableServiceCategories)
        {
            if (ModelState.IsValid)
            {
                _db.AvailableServiceCategories.Add(availableServiceCategories);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(availableServiceCategories);
        }

        // GET: AvailableServiceCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableServiceCategory availableServiceCategories = _db.AvailableServiceCategories.Find(id);
            if (availableServiceCategories == null)
            {
                return HttpNotFound();
            }
            return View(availableServiceCategories);
        }

        // POST: AvailableServiceCategories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AvailableServiceCategory availableServiceCategories)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(availableServiceCategories).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(availableServiceCategories);
        }

        // GET: AvailableServiceCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableServiceCategory availableServiceCategories = _db.AvailableServiceCategories.Find(id);
            if (availableServiceCategories == null)
            {
                return HttpNotFound();
            }
            return View(availableServiceCategories);
        }

        // POST: AvailableServiceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AvailableServiceCategory availableServiceCategories = _db.AvailableServiceCategories.Find(id);
            _db.AvailableServiceCategories.Remove(availableServiceCategories);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetCategoryByFilter(string search = null)
        {
            List<AvailableServiceCategory> model;
            if (!string.IsNullOrEmpty(search))
            {
                model = _db.AvailableServiceCategories.Where(c => c.Name.Contains(search)).ToList();
                var categoryIds = _db.Services.Where(p => p.Name.Contains(search)).Select(p => p.AvailableServiceCategoryId);
                model.AddRange(_db.AvailableServiceCategories.Where(c => categoryIds.Contains(c.Id)));
                model.Distinct();
            }
            else
            {
                model = _db.AvailableServiceCategories.ToList();
            }

            return PartialView(model);
        }
        public ActionResult GetServicesByCategoryName(string name)
        {
            AvailableServiceCategory categories = _db.AvailableServiceCategories.Where(c => c.Name == name).FirstOrDefault();

            return RedirectToAction("Details",categories);
        }
    }
}
