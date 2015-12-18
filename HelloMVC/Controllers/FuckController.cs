using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC.Controllers
{
    public class FuckController : Controller
    {
        HelloMVC.Models.JohnTestEntities db = new Models.JohnTestEntities();
        // GET: Fuck
        public ActionResult Index()
        {
            return View(db.Fucks.ToList());
        }

        // GET: Fuck/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Fuck/Create
        public ActionResult Create()
        {
            HelloMVC.ViewModels.FuckViewModel vm = new ViewModels.FuckViewModel();
            vm.fuckList = db.Fucks.ToList();
            return View(vm);
        }

        // POST: Fuck/Create
        [HttpPost]
        public ActionResult Create(HelloMVC.ViewModels.FuckViewModel vm, FormCollection fc)
        {
            try
            {
                // TODO: Add insert logic here
                for (var i= 0; i< vm.fuckList.Count; i++)
                {
                    var a = Convert.ToBoolean(fc["Fuck" + i]);
                    HelloMVC.Models.Fuck fuck = db.Fucks.Find(i + 1);
                    fuck.Name = vm.fuckList[i].Name;
                    fuck.IsOK = a;
                    db.Entry(fuck).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fuck/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Fuck/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fuck/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Fuck/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
