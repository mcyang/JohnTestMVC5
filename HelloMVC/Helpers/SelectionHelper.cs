using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using HelloMVC.Models;

namespace HelloMVC.Helpers
{
    // class名稱自己取，但一定要是 static
    public static class SelectionHelper
    {
        // 第1個參數要填「this HtmlHelper helper」，也必須是 static
        // 第2個參數: 下拉選單的名稱，對應 <select id="Name" name="Name"
        // 第3個參數: 選取的項目(新增時沒有，編輯時有)
        // 第4個參數: 自訂的，可有可無，看是否要加入"不拘"的選項。
        // 第5個參數: 自訂的，可有可無，看是否要套用CSS設定。
        public static IHtmlString DDL_Team(this HtmlHelper helper, string Name, int selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }


            using (JohnTestEntities database = new JohnTestEntities())
            {
                foreach (var item in database.Teams.Where(p => p.IsDelete == false))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.ID.ToString(),
                        Selected = item.ID.Equals(selected)
                    });
                }
            }

            return helper.DropDownList(Name, list);
        }
    }
}