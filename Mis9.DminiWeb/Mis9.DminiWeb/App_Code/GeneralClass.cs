using System;
using System.Collections.Generic;

/// <summary>
/// GeneralClass 的摘要说明
/// </summary>
public class GeneralClass
{
    //取验证码
    public static string CreateSnCode(int sncodeLength)
    {
        var list = new List<string>();
        for (var c = 'A'; c <= 'Z'; c++)
        {
            if (c != 'O')
            {
                list.Add(c.ToString());
            }
        }
        for (var c = 'a'; c <= 'z'; c++)
        {
            if ((c != 'l') && (c != 'o'))
            {
                list.Add(c.ToString());
            }
        }
        for (var c = '2'; c <= '9'; c++)
        {
            list.Add(c.ToString());
        }
        var rd = new Random();
        var str = string.Empty;
        for (int i = 0; i < sncodeLength; i++)
        {
            str += list[rd.Next(list.Count)];
        }

        return str;
    }
}