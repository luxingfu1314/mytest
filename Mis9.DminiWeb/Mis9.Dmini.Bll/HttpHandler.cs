using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mis9.Dmini.Bll
{
    public class HttpHandler
    {
        private static HttpHandler instance;

        public static HttpHandler Instance()
        {
            return instance ?? (instance = new HttpHandler());
        }
        public void ProcessRequest(string classname, System.Web.HttpContext context)
        {
            if (string.IsNullOrEmpty(classname)) return;

            Type tp = this.GetType();
            System.Reflection.Assembly a = tp.Assembly;

            Type tp2 = a.GetType(tp.Namespace + "." + classname);

            if (tp2 == null) return;

            object obj = a.CreateInstance(tp2.FullName);

            HttpCommon pr = obj as HttpCommon;

            pr?.ProcessRequest(context);

        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            string classname = context.Request["classname"];
            ProcessRequest(classname, context);

        }
    }
}
