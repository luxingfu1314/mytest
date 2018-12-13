using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

/// <summary>
/// PhotoText 图片与文字相结合
/// </summary>
public class PhotoText
{
    public static Image GetPhotoText(string photoPath, string strText, Font textFont, Color textColor, Point textPoint)
    {
        Image retImage = null;

        try
        {
            //加载图片场景
            Image image = Image.FromFile(photoPath);

            retImage = GetPhotoText(image, strText, textFont, textColor, textPoint);
        }
        catch
        {
        }

        return retImage;
    }

    public static Image GetPhotoText(Image image, string strText, Font textFont, Color textColor, Point textPoint)
    {
        Image retImage = null;
        try
        {
            //在图片场景中创建绘图对象Graphics
            Graphics g = Graphics.FromImage(image);

            //将生成的随机字符串画在图片场景
            SolidBrush brush = new SolidBrush(textColor);
            g.DrawString(strText, textFont, brush, textPoint);

            //销毁资源
            g.Dispose();

            retImage = image;
        }
        catch (Exception )
        {
            
        }

        return retImage;
    }
}