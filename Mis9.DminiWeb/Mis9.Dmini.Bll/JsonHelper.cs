using Mis9.CommonTools.Refection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Schema;


namespace Mis9.Dmini.Bll
{
    public class JsonHelper
    {
        public static string ConvertObjectToString<T>(T tv)
        {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

            var stream = new MemoryStream();
            serializer.WriteObject(stream, tv);

            byte[] dataBytes = new byte[stream.Length];

            stream.Position = 0;

            stream.Read(dataBytes, 0, (int)stream.Length);

            return Encoding.UTF8.GetString(dataBytes).Replace("#LINE#", "\\r\\n");

        }
        public static T ConvertStringToObject<T>(string dataString)
        {
            try
            {
                var mStream = new MemoryStream(Encoding.UTF8.GetBytes(dataString));
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(mStream);
            }
            catch
            {

            }

            return default(T);
        }

        public static DataTable GetDataTale<T>(string dataString)
        {
            List<T> ls = ConvertStringToObject<List<T>>(dataString);

            if (ls == null) return null;

            return ObjectTools.GetDataTale<T>(ls, "TAB");

        }
        public static object ConvertStringToObject(string dataString, Type t)
        {
            try
            {
                var mStream = new MemoryStream(Encoding.UTF8.GetBytes(dataString));
                var serializer = new DataContractJsonSerializer(t);

                return serializer.ReadObject(mStream);
            }
            finally
            {

            }
        }

        public static DataTable GetDataTale(string dataString,
            string className, string fileName)
        {
            Object o = ConvertStringToObject(dataString, className, fileName);

            if (o == null)
            {
                return null;
            }

            return ObjectTools.GetDataTale(o, "TAB");
        }

        public static object ConvertStringToObject(string dataString,
            string className, string fileName)
        {
            Type t = GetClassType(className, fileName);

            if (t == null) return null;

            return ConvertStringToObject(dataString, t);
        }

        public static Type GetClassType(string className, string fileName)
        {
            Assembly executingAssembly = null;

            if (string.IsNullOrEmpty(fileName))
            {
                executingAssembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                string assemblyFile = "";
                if (File.Exists(fileName))
                {
                    assemblyFile = fileName;
                }
                else
                {
                    string location = "";
                    int num2 = 0;
                    do
                    {

                        if (num2 == 0)
                        {
                            location = "";
                        }
                        else
                        {
                            //location = Assembly.GetExecutingAssembly().Location;
                            location = System.AppDomain.CurrentDomain.BaseDirectory;

                        }

                        int num = location.LastIndexOf(@"\", StringComparison.CurrentCultureIgnoreCase);
                        if (num < 0)
                            location = location + @"\";

                        if (File.Exists(location + fileName))
                        {
                            assemblyFile = location + fileName;
                            break;
                        }
                        if (File.Exists(location + @"bin\" + fileName))
                        {
                            assemblyFile = location + @"bin\" + fileName;
                            break;
                        }

                        num2++;
                    }
                    while (num2 <= 1);
                }

                if (File.Exists(assemblyFile))
                {
                    executingAssembly = Assembly.LoadFrom(assemblyFile);
                }


            }

            Type t = executingAssembly.GetType(executingAssembly.GetName().Name
                + "." + className);

            return t;
        }

        public static string GetJosnData<T>(DataTable table)
        {
            List<T> ls =
            ObjectTools.GetDataList<T>(
                 table);

            if (ls == null) return "";

            return GetJosnData(ls);
        }

        public static string GetJosnData(DataTable table, string className, string fileName)
        {
            Type t = GetClassType(className, fileName);

            if (t == null) return "[]";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");

            if (table != null)
            {
                int line = 0;
                foreach (DataRow d in table.Rows)
                {
                    Object obj = System.Activator.CreateInstance(t);
                    if (ObjectTools.ConvertDatarowToObj(d, obj))
                    {
                        if (line > 0)
                        {
                            sb.Append(",");
                        }
                        sb.AppendLine(GetJosnData(obj));
                        line += 1;
                    }
                }
            }

            sb.AppendLine("]");

            return sb.ToString();
        }

        public static string GetJosnData(object c)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(c.GetType());
            MemoryStream msObj = new MemoryStream();
            //将序列化之后的Json格式数据写入流中
            js.WriteObject(msObj, c);
            msObj.Position = 0;
            //从0这个位置开始读取流中的数据
            StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            return json;
        }
    }
}
