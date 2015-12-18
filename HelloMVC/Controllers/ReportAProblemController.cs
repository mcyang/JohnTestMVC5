using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloMVC.Models;

namespace HelloMVC.Controllers
{
    /// <summary>
    /// 前台:回報問題
    /// </summary>
    public class ReportAProblemController : Controller
    {
        JohnTestEntities db = new JohnTestEntities();

        #region 使用CLEditor做問題回報
        // GET: ReportAProblem
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index([Bind(Include = "ID,Title,Description")] ReportProblem aproblem, FormCollection fc)
        {
            aproblem.Description = fc["Description"];
            aproblem.CreateOn = DateTime.Now;
            aproblem.CreateBy = 1;
            aproblem.ModifyOn = DateTime.Now;
            aproblem.ModifyBy = 1;

            if (ModelState.IsValid)
            {
                db.ReportProblems.Add(aproblem);
                db.SaveChanges();

                return RedirectToAction("Success");
            }
            return View(aproblem);
        }

        #endregion

        #region 使用CKEditor做問題回報
        public ActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index2([Bind(Include = "ID,Title,Description")] ReportProblem aproblem, FormCollection fc)
        {
            aproblem.Description = fc["Description"];
            aproblem.CreateOn = DateTime.Now;
            aproblem.CreateBy = 1;
            aproblem.ModifyOn = DateTime.Now;
            aproblem.ModifyBy = 1;

            if (ModelState.IsValid)
            {
                db.ReportProblems.Add(aproblem);
                db.SaveChanges();

                return RedirectToAction("Success");
            }
            return View(aproblem);
        }


        #endregion
        public ActionResult Success()
        {
            return View();
        }
    }
}