using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelloMVC.Models;

namespace HelloMVC.Controllers
{
    public class PlayersController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: Players
        public ActionResult Index()
        {
            var query = from p in db.Players.Where(m => m.IsDelete == false)
                        join t in db.Teams on p.TeamID equals t.ID
                        select new {p, t.Name };

            List<PlayersViewModel> list = new List<PlayersViewModel>();
            foreach (var q in query)
            {
                PlayersViewModel playersVM = new PlayersViewModel();
                playersVM.PlayerID = q.p.ID;
                playersVM.PlayerName = q.p.Name;
                playersVM.Height = q.p.Height ?? 0;
                playersVM.Weight = q.p.Weight ?? 0;
                playersVM.Age = q.p.Age ?? 0;
                playersVM.TeamName = q.Name;

                list.Add(playersVM);
            }

            return View(list);
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = (from player in db.Players.Where(m => m.IsDelete == false)
                        join team in db.Teams on player.TeamID equals team.ID
                        select new { player, team.Name }).Where(m => m.player.ID == id).First();

            PlayersViewModel playersVM = new PlayersViewModel();
            playersVM.PlayerID = query.player.ID;
            playersVM.PlayerName = query.player.Name;
            playersVM.Height = query.player.Height ?? 0;
            playersVM.Weight = query.player.Weight ?? 0;
            playersVM.Age = query.player.Age ?? 0;
            playersVM.TeamName = query.Name;

            //Player player = db.Players.Find(id);
            if (playersVM == null)
            {
                return HttpNotFound();
            }
            return View(playersVM);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            //建立下拉選單-資料來源:Team資料表
            List<SelectListItem> items = new List<SelectListItem>();    //下拉選單容器
            var query = db.Teams.Where(p => p.IsDelete == false);       //資料來源
            foreach (var q in query)
            {
                items.Add(new SelectListItem()
                {
                    Text = q.Name,
                    Value = q.ID.ToString()
                });
            }
            ViewData["TeamList"] = items;   //將做好的下拉選單打包給ViewData做前後台的傳遞

            return View();
        }

        // POST: Players/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Height,Weight,Age,TeamID,IsDelete")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //建立下拉選單-資料來源:Team資料表
            List<SelectListItem> items = new List<SelectListItem>();    //下拉選單容器
            var query = db.Teams.Where(p => p.IsDelete == false);       //資料來源
            foreach (var q in query)
            {
                items.Add(new SelectListItem
                {
                    Text = q.Name,
                    Value = q.ID.ToString()
                });
            }
            ViewData["TeamList"] = items;   //將做好的下拉選單打包給ViewData做前後台的傳遞

            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Height,Weight,Age,TeamID,IsDelete")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = (from player in db.Players.Where(m => m.IsDelete == false)
                         join team in db.Teams on player.TeamID equals team.ID
                         select new { player, team.Name }).Where(m => m.player.ID == id).First();

            PlayersViewModel playersVM = new PlayersViewModel();
            playersVM.PlayerID = query.player.ID;
            playersVM.PlayerName = query.player.Name;
            playersVM.Height = query.player.Height ?? 0;
            playersVM.Weight = query.player.Weight ?? 0;
            playersVM.Age = query.player.Age ?? 0;
            playersVM.TeamName = query.Name;

            if (playersVM == null)
            {
                return HttpNotFound();
            }
            return View(playersVM);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            player.IsDelete = true;
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
