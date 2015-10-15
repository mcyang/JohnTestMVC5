using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HelloMVC.Models;
using NPOI.HSSF.UserModel;
using System.IO;

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
                        select new { p , t.Name };
        
            List<PlayersViewModel> list = new List<PlayersViewModel>();
            foreach (var q in query)
            {
                PlayersViewModel playersVM = new PlayersViewModel();
                playersVM.player = q.p;
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
            playersVM.player = query.player;
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
        public ActionResult Create(FormCollection fc) //[Bind(Include = "ID,Name,Height,Weight,Age,TeamID,IsDelete")] Player player
        {
            // 透過FormCollection將表單的資料打包回後端，並作前處理
            string name = fc["Name"];
            double height = 0;
            double weight = 0;
            int age = 0;
            int teamid = 0;

            //資料前處理
            double.TryParse(fc["Height"], out height);
            double.TryParse(fc["Weight"],out weight);
            int.TryParse(fc["Age"], out age);
            int.TryParse(fc["TeamList"],out teamid);

            if (ModelState.IsValid)
            {
                Player player = new Player();
                player.Name = name;
                player.Height = height;
                player.Weight = weight;
                player.Age = age;
                player.TeamID = teamid;

                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Player playerData = db.Players.Find(id);
            if (playerData == null)
            {
                return HttpNotFound();
            }
            

            //建立下拉選單方法1-資料來源:Team資料表
            List<SelectListItem> items = new List<SelectListItem>();    //下拉選單容器
            var query = db.Teams.Where(p => p.IsDelete == false);       //資料來源
            foreach (var q in query)
            {
                items.Add(new SelectListItem
                {
                    Text = q.Name,
                    Value = q.ID.ToString(),
                    Selected = q.ID.Equals(playerData.TeamID)
                });
            }
            ViewBag.TeamSelectListItem = items;   //將做好的下拉選單打包給ViewBag做前後台的傳遞

            //建立下拉選單方法2-資料來源:Team資料表
            SelectList selectlist = new SelectList(query,"ID","Name");
            ViewBag.TeamSelectList = selectlist;

            //建立下拉選單方法3-將資料塞到ViewModel
            SelectList DDL_TeamList = new SelectList(query, "ID", "Name",playerData.TeamID);
            PlayersViewModel viewModel = new PlayersViewModel()
            {
                player = playerData,
                TeamList = DDL_TeamList
            };


            return View(viewModel);
        }

        // POST: Players/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayersViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Player player = new Player();
                player = viewModel.player;

                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
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
            playersVM.player = query.player;
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

        // GET: Excel處理頁面
        public ActionResult Excel()
        {
            return View();
        }

        // GET: Excel處理頁面
        public ActionResult Open()
        {
            HSSFWorkbook workbook = new HSSFWorkbook(); // for .xls
            //NPOI.XSSF.UserModel.XSSFWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(); // for .xlsx
            MemoryStream ms = new MemoryStream();
            
            // 新增試算表。 
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("試算表 A");
            workbook.CreateSheet("試算表 B");
            workbook.CreateSheet("試算表 C");

            // 建立儲存格樣式。 
            NPOI.SS.UserModel.ICellStyle style1 = workbook.CreateCellStyle();
            style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index2;
            style1.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            NPOI.SS.UserModel.ICellStyle style2 = workbook.CreateCellStyle();
            style2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index2;
            style2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

            //// 插入資料值。 
            //sheet.CreateRow(0).CreateCell(0).SetCellValue("0");
            //sheet.CreateRow(1).CreateCell(0).SetCellValue("1");
            //sheet.CreateRow(2).CreateCell(0).SetCellValue("2");
            //sheet.CreateRow(3).CreateCell(0).SetCellValue("3");
            //sheet.CreateRow(4).CreateCell(0).SetCellValue("4");
            //sheet.CreateRow(5).CreateCell(0).SetCellValue("5");

            // 設定儲存格樣式與資料。 
            NPOI.SS.UserModel.ICell cell = sheet.CreateRow(0).CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue(0);

            cell = sheet.CreateRow(1).CreateCell(0);
            cell.CellStyle = style2;
            cell.SetCellValue(1);

            cell = sheet.CreateRow(2).CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue(2);

            cell = sheet.CreateRow(3).CreateCell(0);
            cell.CellStyle = style2;
            cell.SetCellValue(3);

            cell = sheet.CreateRow(4).CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue(4);

            workbook.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=EmptyWorkbook.xls"));
            Response.BinaryWrite(ms.ToArray());

            workbook = null;
            ms.Close();
            ms.Dispose();

            return View();
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
