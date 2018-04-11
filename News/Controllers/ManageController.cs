using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Timers;
using System.Threading;

namespace News.Controllers
{
    public class ManageController : Controller
    {
        
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Authority()
        {
            try
            {
                var username = Request["username"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format(" select authority from dbo.account where username = '{0}'", username);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Authority");
                string Authority = ds.Tables["Authority"].Rows[0][0].ToString();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success", authority= Authority });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult submit()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.NewsPage(TITLE,AUTHOR,ORIGINAL,NEWSTYPE,DATE,ISRELEASE,ISTOP,NEWSCONTENT) values('{0}','{1}','{2}','{3}',cast(nullif('{4}','') as DateTime),'{5}','{6}','{7}')", form["title"], form["author"], form["original"], form["newstype"], form["date"], form["release"], form["top"], form["newscontent"]);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        


        public ActionResult NewsEdit()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "  select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP] from dbo.NewsPage order by [ISRELEASE] DESC,[ISTOP] DESC,[DATE] DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "NewsPage");
            DataTable dt = ds.Tables["NewsPage"];
            ViewData["NewsList"] = dt;
            sqlcon.Close();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Edit()
        {
            try
            {
                string NEWSID = Request["NEWSID"];
                string STATUS = Request["STATUS"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (STATUS == "get")
                {
                    string sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT] from dbo.NewsPage where ID = '{0}'", NEWSID);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "CommontList");
                    DataTable dt = ds.Tables["CommontList"];
                    DataRow dr = dt.Rows[0];
                    List<string> newslist = new List<string>();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        newslist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success", news = newslist });
                }
                else
                {
                    var sql = string.Format(" update dbo.NewsPage set [TITLE]='{0}',[AUTHOR]='{1}',[ORIGINAL]='{2}',[NEWSTYPE]='{3}',[DATE]=cast(nullif('{4}','') as DateTime),[ISRELEASE]='{5}',[ISTOP]='{6}',[NEWSCONTENT]='{7}' where [ID]='{8}'", form["title"], form["author"], form["original"], form["newstype"], form["date"], form["release"], form["top"], form["newscontent"], form["id"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult delete()
        {
            try
            {
                string newsid = Request["newsid"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("delete from dbo.NewsPage where id in ({0})", newsid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }


        public ActionResult Comment()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[COMMONTNUM] from dbo.NewsPage where [ISRELEASE] = '是' order by DATE DESC,COMMONTNUM DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "CommontList");
            DataTable dt = ds.Tables["CommontList"];
            ViewData["CommontList"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult Commentmana()
        {
            return View();
        }
        public ActionResult Commentdelete()
        {
            try
            {

                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }

        public ActionResult Account()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select [ID],[USERNAME],[PASSWORD],[AUTHORITY],[LOGABLE] from dbo.ACCOUNT order by [LOGABLE] DESC,[AUTHORITY] DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "ACCOUNT");
            DataTable dt = ds.Tables["ACCOUNT"];
            ViewData["ACCOUNT"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult CreateAccount()
        {
            return View();
        }
        public ActionResult Accountcreate()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.ACCOUNT(USERNAME,PASSWORD,AUTHORITY,LOGABLE) values('{0}','{1}','{2}','{3}')", form["username"], form["password"], form["authority"], form["logable"]);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult AccountEdit()
        {
            try
            {
                string ACCOUNTID = Request["ACCOUNTID"];
                string STATUS = Request["STATUS"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (STATUS == "get")
                {
                    string sql = string.Format(" select [ID],[USERNAME],[PASSWORD],[AUTHORITY],[LOGABLE] from dbo.ACCOUNT where [ID]='{0}'", ACCOUNTID);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "CommontList");
                    DataTable dt = ds.Tables["CommontList"];
                    DataRow dr = dt.Rows[0];
                    List<string> accountlist = new List<string>();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        accountlist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success", news = accountlist });
                }
                else
                {
                    string sql = string.Format(" update dbo.ACCOUNT set [USERNAME]='{0}',[PASSWORD]='{1}',[AUTHORITY]='{2}',[LOGABLE]='{3}' where [ID]='{4}'",form["username"],form["password"],form["authority"],form["logable"],form["accountid"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult Accountdelete()
        {
            try
            {
                string accountid = Request["accountid"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("delete from dbo.ACCOUNT where id = '{0}'", accountid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }


        public ActionResult Newscontent()
        {
            return View();
        }


        public ActionResult search(string[] str)
        {
            try
            {
                string table = Request["table"];
                string column = Request["column"];
                string str1 = Request["str"];
                string which = Request["which"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                var where1 = "";
                var ordertype = "";
                if(which == "comment")
                {
                    where1 = "and [ISRELEASE] = '是'";
                    ordertype = "[DATE] DESC,COMMONTNUM DESC";
                }
                else if(which == "newsedit")
                {
                    ordertype = "ISRELEASE DESC,[DATE] DESC";
                }
                else if(which == "pic")
                {
                    ordertype = "STATUS DESC,[CREATETIME] DESC";
                }
                else if(which == "filed")
                {
                    where1 = "and [ISFILED] = '是'";
                }
                if (column == "newstype")
                {
                    var newstypestr = "";
                    
                    if (str[0] == "")
                    {
                        sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where 1=1 " + where1 + " order by " + ordertype, table);
                    }
                    else
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (i == 0)
                            {
                                newstypestr = "'" + str[i] + "'";
                            }
                            else
                            {
                                newstypestr = newstypestr + "," + "'" + str[i] + "'";
                            }
                        }
                        sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where newstype in ({1})" + where1 + "order by " + ordertype, table, newstypestr);
                    }
                }
                else
                {
                    if(which == "pic")
                    {
                        sql = string.Format("select [ID],[TITLE],[TOURL],[STATUS],[CREATETIME] from {0} where {1} LIKE '%{2}%' order by " + ordertype, table,column,str1);
                    }
                    else
                    {
                        sql = string.Format("  select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where {1} LIKE '%{2}%'" + where1 + "order by [DATE] DESC,COMMONTNUM DESC", table, column, str1);
                    }
                }
                
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "SearchList");
                DataTable dt = ds.Tables["SearchList"];
                List<List<string>> searchlist = new List<List<string>>();
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    List<string> searchlist1 = new List<string>();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        searchlist1.Add(dr[j].ToString());
                    }
                    searchlist.Add(searchlist1);
                }
                return Json(new { msg = "success", rs = searchlist });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult sort()
        {
            try
            {
                string sort = Request["sort"];
                string type = Request["type"];
                string column = Request["column"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                if(type == "comment")
                {
                    sql = string.Format(" select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[COMMONTNUM] from dbo.NewsPage where [ISRELEASE] = '是' order by {0} {1} ",column,sort);
                }
                else if(type == "newsedit")
                {
                    sql = string.Format(" select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP] from dbo.NewsPage order by {0} {1} ", column, sort);
                }
                else
                {
                    sql = string.Format(" select [ID],[NEWSTYPE],[TITLE],[AUTHOR],convert(varchar(16),[DATE],120) [DATE],[ISFILED] from dbo.NewsPage where [ISFILED] = '是' order by {0} {1} ", column, sort);
                }
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "SearchList");
                DataTable dt = ds.Tables["SearchList"];
                List<List<string>> sortlist = new List<List<string>>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    List<string> sortlist1 = new List<string>();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sortlist1.Add(dr[j].ToString());
                    }
                    sortlist.Add(sortlist1);
                }
                return Json(new { msg = "success", sortlist = sortlist });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult Picauto()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "  select * from dbo.PIC order by STATUS DESC,CREATETIME DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "PICLIST");
            DataTable dt = ds.Tables["PICLIST"];
            ViewData["PICLIST"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult Picupload()
        {
            try
            {
                var type = Request["type"];
                var num = Request["num"];
                var path = "";
                var url = "";
                HttpPostedFileBase imgFile = Request.Files[0];
                if(num != "")
                {
                    path = "INDEX/" + type + "_" + num;
                    imgFile.SaveAs(Server.MapPath("~/imglist/") + path + imgFile.FileName.Substring(imgFile.FileName.LastIndexOf("."), (imgFile.FileName.Length - imgFile.FileName.LastIndexOf("."))));
                    url = "/imglist/" + path + imgFile.FileName.Substring(imgFile.FileName.LastIndexOf("."), (imgFile.FileName.Length - imgFile.FileName.LastIndexOf(".")));
                }
                else
                {
                    path = type + "/" + imgFile.FileName;
                    if (System.IO.File.Exists(Server.MapPath("~/imglist/") + path))
                    {
                        string fail = "名为" + imgFile.FileName + "的文件已存在，请确认后重新上传";
                        return Json(new { code = 0, msg = fail });
                    }
                    else
                    {
                        imgFile.SaveAs(Server.MapPath("~/imglist/") + path);
                    }
                    url = "/imglist/" + type + "/" + imgFile.FileName;
                }
                
                return Json(new { msg = "success", url = url });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult setPic()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.PIC(TITLE,PICORIGIN,TOURL,STATUS,CREATETIME) values('{0}','{1}','{2}','{3}','{4}')", form["pictitle"], form["picorigin"], form["tourl"], form["status"], DateTime.Now);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult editPic()
        {
            try
            {
                var id = Request["id"];
                var type = Request["type"];
                string sql = "";
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (type == "get")
                {
                    sql = string.Format("select * from dbo.PIC where id = '{0}'",id);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "piclist");
                    DataTable dt = ds.Tables["piclist"];
                    DataRow dr = dt.Rows[0];
                    List<string> piclist = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        piclist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    return Json(new { msg = "success", piclist = piclist });
                }
                else
                {
                    var form = Request.Form;
                    sql = string.Format(" update dbo.PIC set [TITLE]='{0}',[PICORIGIN]='{1}',[TOURL]='{2}',[STATUS]='{3}' where [ID]='{4}'", form["pictitle"], form["picorigin"], form["tourl"], form["status"], form["picid"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult deletePic()
        {
            try
            {
                var type = Request["type"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                if (type == "pic")
                {
                    var filename = Request["filename"];
                    if (System.IO.File.Exists(Server.MapPath("~/imglist/") + filename))
                    {
                        System.IO.File.Delete(Server.MapPath("~/imglist/") + filename);
                    }
                }
                else
                {
                    var id = Request["id"];
                    sql = string.Format("delete from dbo.PIC where id in({0})", id);
                }
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult Filed()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISFILED] from dbo.NewsPage where [ISFILED] = '是' order by date desc";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Filed");
            DataTable dt = ds.Tables["Filed"];
            ViewData["Filed"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult CancelFile()
        {
            try
            {
                string newsid = Request["newsid"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("update NewsPage set ISFILED = '否' where id in ({0})", newsid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult File()
        {
            try
            {
                string newsid = Request["newsid"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("update NewsPage set ISFILED = '是' where id in ({0})", newsid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }

        public ActionResult indexConfig()
        {
            try
            {
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                for (int i = 1; i <= 10; i++)
                {
                    SqlCommand sqlcommand = new SqlCommand(("select * from dbo.PIC where NEWSTYPE = '" + trans(i) + "' and TYPENUM <> 'null' order by TYPENUM"), sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "piclist");
                    DataTable dt = ds.Tables["piclist"];
                    ViewData[trans(i)] = dt;
                    sqlcommand = null;
                }

                sqlcon.Close();
                sqlcon = null;
                return View();
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex },JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult ConfigPic()
        {
            try
            {
                var type = Request["type"];
                var num = Request["num"];
                var SG = Request["SG"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();

                if(SG == "get")
                {
                    var sql = string.Format("select * from dbo.PIC where NEWSTYPE = '{0}' and TYPENUM = '{1}'", type, num);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "piclist");
                    DataTable dt = ds.Tables["piclist"];
                    DataRow dr = dt.Rows[0];
                    List<string> piclist = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        piclist.Add(dr[i].ToString());
                    }

                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success", piclist = piclist });
                }
                else
                {
                    var form = Request.Form;
                    var sql = string.Format(" update dbo.PIC set [TITLE]='{0}',[RESUME]='{1}',[PICORIGIN]='{2}',[TOURL]='{3}' where [NEWSTYPE]='{4}' and [TYPENUM]='{5}'", form["pictitle"], form["resume"], form["picorigin"], form["tourl"], form["type"], form["typenum"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }

        public string trans(int i)
        {
            string str = "";
            switch (i)
            {
                case 1: str = "SHDT"; break;
                case 2: str = "JRCJ"; break;
                case 3: str = "JQTY"; break;
                case 4: str = "KJQY"; break;
                case 5: str = "QCZX"; break;
                case 6: str = "FC"; break;
                case 7: str = "JS"; break;
                case 8: str = "YL"; break;
                case 9: str = "JK"; break;
                case 0: str = "QT"; break;
            }
            return str;
        }
    }

    public class WebTimer_AutoRepayment
    {
        static WebTimer_AutoRepayment()
        {
            _WebTimerTask = new WebTimer_AutoRepayment();
        }
        public static WebTimer_AutoRepayment Instance()
        {
            return _WebTimerTask;
        }

        private void ExecuteMain()
        {
            //定义你自己要执行的Job
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = string.Format(" delete from NewsPage where DATEDIFF(day,NewsPage.DATE,convert(char(50),GETDATE(),21)) >= 365  ");
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            sqlcommand.ExecuteNonQuery();
        }
        #region Timer 计时器定义
        

        private static int Period = 24 * 60 * 60 * 1000;
        ///

        /// 调用 callback 之前延迟的时间量（以毫秒为单位）。指定 Timeout.Infinite 以防止计时器开始计时。指定零 (0) 以立即启动计时器。
        ///

        private static int dueTime = 3 * 1000;//三分钟后启动

        ///第几次执行
        ///

        private long Times = 0;
        ///

        /// 实例化一个对象
        ///

        private static readonly WebTimer_AutoRepayment _WebTimerTask = null;
        private System.Threading.Timer WebTimerObj = null;
        ///

        /// 是否正在执行中
        ///

        private int _IsRunning;
        ///

        /// 开始
        ///

        public void Start()
        {
            if (WebTimerObj == null)
            {
                DateTime now = DateTime.Now;
                int hour = now.Hour;
                int minute = now.Minute;
                int second = now.Second;
                if (hour >= 12)
                {
                    dueTime = 0;//立即启动
                }
                else
                {
                    dueTime = (12 - hour) * (60-minute) * (60-second) * 1000;//到某个时间点的23时启动
                }
                WebTimerObj = new System.Threading.Timer(new TimerCallback(WebTimer_Callback), null, dueTime, Period);
            }
        }
        ///

        /// WebTimer的主函数
        ///

        /// 
        private void WebTimer_Callback(object sender)
        {
            try
            {
                if (Interlocked.Exchange(ref _IsRunning, 1) == 0)
                {
                    ExecuteMain();
                    Times++;
                    Times = (Times % 100000);
                }
            }
            catch
            {
            }
            finally
            {
                Interlocked.Exchange(ref _IsRunning, 0);
            }
        }
        ///

        /// 停止
        ///

        public void Stop()
        {
            if (WebTimerObj != null)
            {
                WebTimerObj.Dispose();
                WebTimerObj = null;
            }
        }
        #endregion
    }
}
