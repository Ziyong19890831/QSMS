using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// EventRole 的摘要描述
/// </summary>
public class EventRole
{
    public EventRole()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public static bool CheckCertificateforRole10Teacher(string PersonID, string CtypeSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select CtypeSNO from Person P
                        Left Join QS_Certificate QC On QC.PersonID=P.PersonID
                        Where P.PersonID=@PersonID and CtypeSNO=@CtypeSNO and QC.CertEndDate > getdate()";
        adict.Add("PersonID", PersonID);
        adict.Add("CtypeSNO", CtypeSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static string GetClass(string EventSNO)
    {
        string Class = "";
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"SELECT ER.Class1
                        FROM [Event] E
                        Left Join EventRole ER On ER.ERSNO=E.ERSNO
                        where E.EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            Class = ObjDT.Rows[0]["Class1"].ToString(); //1核心實體 //2專門實體
            return Class;
        }
        else
        {
            return "";
        }

    }
    public static int CheckPersonCertificateDate(string PersonID)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("personID", PersonID);
        string sql = @"Select * from Person P
                        Left Join QS_Certificate QC On QC.personID=P.PersonID
                        where P.PersonID=@PersonID";
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        ArrayList arrayList = new ArrayList();
        for (int j = 0; j < ObjDT.Rows.Count; j++)
        {
            if (String.IsNullOrEmpty(ObjDT.Rows[j]["CertEndDate"].ToString())) continue;
            if (DateTime.Now > Convert.ToDateTime(ObjDT.Rows[j]["CertEndDate"]))
            {

                arrayList.Add(1);

            }
            else
            {
                arrayList.Add(0);
            }
        }

        if (arrayList.Contains(0))
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static string CheckClass(string ERSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"SELECT ER.Class1
                    FROM [Event] E
                    Left Join EventRole ER On ER.ERSNO=E.ERSNO
                    where ER.ERSNO=@ERSNO
                        ";
        aDict.Add("ERSNO", ERSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            if (ObjDT.Rows[0]["Class1"].ToString() == "1")//核心須關閉進階與初進階
            {
                return "1";
            }
            else if (ObjDT.Rows[0]["Class1"].ToString() == "2")//專門
            {
                return "2";
            }
            else
            {//繼續教育
                return "3";
            }
        }
        else
        {
            return "0";
        }
    }
    public static bool CheckCertificate(string PersonID, string CtypeSNO, string CBL_CertificateValue)
    {

        switch (CBL_CertificateValue)
        {
            case "0":
                break;
            case "1":
                break;
            case "2":
                break;
            case "3":
                break;
        }
        return true;
    }
    public static bool getCertificateStatus(string PersonID, string CtypeSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = @"Select * from QS_Certificate where PersonID=@PersonID And CtypeSNO=@CtypeSNO";
        aDict.Add("PersonID", PersonID);
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public static bool CheckIntegerType(string Value)
    {
        int Integer;
        var isInteger = int.TryParse(Value, out Integer);
        if (!isInteger)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}