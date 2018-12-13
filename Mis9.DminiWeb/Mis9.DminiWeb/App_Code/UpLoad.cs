using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Class1 的摘要说明
/// </summary>
public class UpLoad
{
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="file">通过form表达提交的文件</param>
    /// <param name="virpath">文件要保存的虚拟路径</param>
    private static void uploadImg(HttpPostedFile file, string dirFullPath)
    {
        if (file.ContentLength > 1024 * 1024 * 4)
        {
            throw new Exception("文件不能大于4M");
        }
        using (Image img = Bitmap.FromStream(file.InputStream))
        {
            img.Save(dirFullPath + file.FileName);
        }
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">通过form表达提交的文件</param>
    /// <param name="virpath">文件要保存的虚拟路径</param>
    private static void uploadFile(HttpPostedFile file, string dirFullPath)
    {
        if (file.ContentLength > 1024 * 1024 * 6)
        {
            throw new Exception("文件不能大于6M");
        }
        file.SaveAs(dirFullPath + file.FileName);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">通过form表达提交的文件</param>
    /// <param name="virpath">文件要保存的虚拟路径</param>
    public static void UploadFile(HttpPostedFile file, string virpath)
    {
        string dirFullPath = HttpContext.Current.Server.MapPath(virpath);
        if (!Directory.Exists(dirFullPath))//如果文件夹不存在，则先创建文件夹
        {
            Directory.CreateDirectory(dirFullPath);
        }
        string type = Path.GetExtension(file.FileName);
        if (type == ".jpg" || type == ".png" || type == ".jpeg")  //图片类型进行限制
        {
            uploadImg(file, dirFullPath);
        }
        else if (type == ".zip" || type == ".rar" || type == ".mp4" || type == ".mp3" || type == ".txt")  //图片类型进行限制
        {
            uploadFile(file, dirFullPath);
        }
        else {
            throw new Exception("文件格式无法识别");
        }

    }
}