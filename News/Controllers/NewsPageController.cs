using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace News.Controllers
{
    public class NewsPageController : Controller
    {

        //public ActionResult submit(List<NewsPage> obj)
        //{
        //    try
        //    {
        //        List<NewsPage> obj2 = obj;
        //        SqlConnection sqlcon = new SqlConnection("NEWS");
        //        var sql = string.Format("insert into dbo.NewsPage(TITLE,AUTHOR,ORIGINAL,NEWSTYPE,DATE,ISRELEASE,ISTOP) values({0},{1},{2},{3},{4},{5},{6})");
        //        return Json(new { message = "success", obj = obj });

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { message = ex });
        //    }
        //}


    }

    //public class NewsPage
    //{
    //    public string title { set; get; }
    //    public string author { set; get; }
    //    public string original { set; get; }
    //    public string newstype { set; get; }
    //    public string date { set; get; }
    //    public string release { set; get; }
    //    public string top { set; get; }
    //}
}