using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MMS.Models;
using MMS.Security;
using PagedList;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]

    public class CategoryMasterController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();        

        public ViewResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var category = from s in db.MMS_Meeting_Category
                           select s;

            if (string.IsNullOrEmpty(sortOrder))
                category = category.OrderBy(x => x.CategoryName);

            if (!String.IsNullOrEmpty(search))
            {
                category = category.Where(x => x.CategoryName.Contains(search));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    category = category.OrderByDescending(x => x.CategoryName);
                    break;

            }
            
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(category.ToPagedList(pageNumber, pageSize));            
        }

        public ActionResult Search(string searchText)
        {
            var category = from s in db.MMS_Meeting_Category
                           select s;

            if (searchText != "")
            {
                category = category.Where(x => x.CategoryName.Contains(searchText));
                category = category.OrderBy(x => x.CategoryName);

            }

            return Json(category.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: CategoryMaster/Create
        public ActionResult Create()
        {
            //return View();
            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryName")] MMS_Meeting_Category model)
        {
            var category = CategoryMasterViewModel.CreateCategory(model);
            if (category == true)
            {

                TempData["Success"] = "New category successfully added!";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["AlreadyExisted"] = model.CategoryName + " " + "already exist.";
                return RedirectToAction("Index");
            }

        }

        // GET: CategoryMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MMS_Meeting_Category model = db.MMS_Meeting_Category.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            //return View(mMS_Meeting_Category);
            return PartialView("_Edit", model);
        }

        // POST: CategoryMaster/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryName")] MMS_Meeting_Category model)
        {
            var category = CategoryMasterViewModel.EditCategory(model);

            if (category == true)
            {

                TempData["Success"] = "New category successfully added!";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["AlreadyExisted"] = model.CategoryName + " " + "already exist.";
                return RedirectToAction("Index");
            }


        }

        // GET: CategoryMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MMS_Meeting_Category model = CategoryMasterViewModel.DeleteCategory(id, "get");
            if (model == null)
            {
                return HttpNotFound();
            }
            //return View(mMS_Meeting_Category);
            return PartialView("_Delete", model);
        }

        // POST: CategoryMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["Success"] = null;

            MMS_Meeting_Category model = CategoryMasterViewModel.DeleteCategory(id, "post");
            if (model != null)
                TempData["Success"] = "Successfully Deleted!";
            else
                TempData["Error"] = "Assigned category should not be deleted!";

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
