using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// SendTODO 的摘要描述
/// </summary>
public class SendTODO
{
    public SendTODO()
    {
        try
        {

          //邏輯
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void SendTODOHelper(string title, string content_title, string msg, string getP, string postP)
    {
        try
        {

            DataHelper DH = new DataHelper();
            Dictionary<string, object> dicpd = new Dictionary<string, object>();
            string sqltodo = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,state) 
                                        Values(@TODOTITLE,@TODOTEXT,@getPersonSNO,@postPersonSNO,@state)";
            dicpd.Add("TODOTITLE", title);
            dicpd.Add("TODOTEXT", "<a style='font-weight:bold;font-size:16pt;'>" + content_title + "</a></br></br> " + msg + " </br></br></br></br>醫療院所預防保健服務系統~感謝您!");
            dicpd.Add("getPersonSNO", getP);
            dicpd.Add("postPersonSNO", postP);
            dicpd.Add("state", 0);//未讀

            DH.executeNonQuery(sqltodo, dicpd);

            

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}