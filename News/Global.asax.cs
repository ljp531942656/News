using News.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace News
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebTimer_AutoRepayment.Instance().Start();
        }

        void Application_End(object sender, EventArgs e)
        {
            WebTimer_AutoRepayment.Instance().Stop();
        }
    }

}
