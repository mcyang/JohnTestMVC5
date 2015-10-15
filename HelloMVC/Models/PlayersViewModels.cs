using System.Collections.Generic;
using System.Web.Mvc;

namespace HelloMVC.Models
{
    public class PlayersViewModel
    {
        // 與 dbo.Player 一模一樣
        public Player player { get; set; }

        // dbo.Player.TeamID 關連到 dbo.Team 取得的Name
        public string TeamName { get; set; }

        // dbo.Team的下拉選單
        public IEnumerable<SelectListItem> TeamList { get; set; }
    }

    public enum Sex
    {
        Female=0,
        Male=1
    }
}