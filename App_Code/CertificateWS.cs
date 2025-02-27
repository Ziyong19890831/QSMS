using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// CertificateWS 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class CertificateWS : System.Web.Services.WebService
{
    public class Authentication2 : SoapHeader
    {
        public string Token { get; set; }

    }
    public Authentication2 authentication = new Authentication2();

    public CertificateWS()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }
    [SoapHeader("authentication")]
    [WebMethod]
    public string GetCertificate(string PersonID)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Sql = @"Select P.PName,P.PersonID,R.RoleName,R.RoleSNO,C.MVal 'CTypeClass',QCT.CTypeSNO,QCT.CTypeName,QC.CertID,convert(varchar, QC.CertPublicDate, 111) CertPublicDate,convert(varchar, QC.CertStartDate, 111) CertStartDate ,convert(varchar, QC.CertEndDate, 111) CertEndDate
                        ,replace(QCT.CTypeString,'@',QC.CertID) CTypeString
                        from QS_Certificate QC
                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO
                        Left Join Person P On P.PersonID = QC.PersonID
						Left Join Role R On R.RoleSNO=P.RoleSNO
                        Left Join Config C On C.PVal = QCT.CtypeClass and C.PGroup = 'ICT'
                        where QCT.CtypeClass <> 0 and P.PersonID=@PersonID";
        
        adict.Add("PersonID", PersonID);
        DataTable ObjDT = ObjDH.queryData(Sql, adict);
        string Json = DataTableToJSONWithJSONNet(ObjDT);
        return Json;
    }
    [SoapHeader("authentication")]
    [WebMethod]
    public string GetCertificateForVPN(string PersonID,string CtypeClass)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Sql = @"If Exists(Select 1 From QS_Certificate QC Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO Where PersonID=@PersonID and QCT.CtypeClass=3)
                            Select Top(1) convert(varchar, QC.CertEndDate, 111) CertEndDate,R.RoleName                     
                        from QS_Certificate QC
                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO
                        Left Join Person P On P.PersonID = QC.PersonID
                        Left Join Role R On R.RoleSNO=P.RoleSNO
                        where QCT.CtypeClass=3 and QC.PersonID=@PersonID
                        GROUP BY convert(varchar, QC.CertEndDate, 111),R.RoleName
                        Order By CertEndDate DESC
                        Else
                            Select Top(1) convert(varchar, QC.CertEndDate, 111) CertEndDate,R.RoleName                     
                        from QS_Certificate QC
                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO
                        Left Join Person P On P.PersonID = QC.PersonID
                        Left Join Role R On R.RoleSNO=P.RoleSNO
                        where  QCT.CtypeClass=@CtypeClass and QC.PersonID=@PersonID
                        GROUP BY convert(varchar, QC.CertEndDate, 111),R.RoleName
                        Order By CertEndDate DESC
";

        adict.Add("PersonID", PersonID);
        adict.Add("CtypeClass", CtypeClass);
        DataTable ObjDT = ObjDH.queryData(Sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            //string Json = ObjDT.Rows[0]["CertEndDate"].ToString();
            string Json = DataTableToJSONWithJSONNet(ObjDT);
            return Json;
        }
        else
        {
            return "";
        }
        
        
    }
    [SoapHeader("authentication")]
    [WebMethod]
    public string GetOrganForVPN(string PersonID)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Sql = @"select p.PersonID,p.PName,o.OrganCode,o.OrganName from Person P
left join Organ O on p.OrganSNO=O.OrganSNO
where P.PersonID=@PersonID
";

        adict.Add("PersonID", PersonID);
        DataTable ObjDT = ObjDH.queryData(Sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            //string Json = ObjDT.Rows[0]["CertEndDate"].ToString();
            string Json = DataTableToJSONWithJSONNet(ObjDT);
            return Json;
        }
        else
        {
            return "";
        }


    }
    [SoapHeader("authentication")]
    [WebMethod]
    public string GetAllCertificate()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Sql = @"Select P.PName,P.PersonID,R.RoleName,R.RoleSNO,C.MVal 'CTypeClass',QCT.CTypeSNO,QCT.CTypeName,QC.CertID,convert(varchar, QC.CertPublicDate, 111) CertPublicDate,convert(varchar, QC.CertStartDate, 111) CertStartDate ,convert(varchar, QC.CertEndDate, 111) CertEndDate
                        ,replace(QCT.CTypeString,'@',QC.CertID) CTypeString
                        from QS_Certificate QC
                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO
                        Left Join Person P On P.PersonID = QC.PersonID
						Left Join Role R On R.RoleSNO=P.RoleSNO
                        Left Join Config C On C.PVal = QCT.CtypeClass and C.PGroup = 'ICT'
                        where QCT.CtypeClass <> 0";       
        DataTable ObjDT = ObjDH.queryData(Sql, null);
        string Json = DataTableToJSONWithJSONNet(ObjDT);
        return Json;
    }

    public string DataTableToJSONWithJSONNet(DataTable table)
    {
        string JSONString = string.Empty;
        JSONString = JsonConvert.SerializeObject(table).Replace("[", "").Replace("]", "");
        return JSONString;
    }

    public bool IsValidUser()
    {

        //這段依使用狀況，可以改讀資料庫的帳號密碼或Web.Conifg 等....
        if (authentication.Token == "369b4a473a61a465e47d0a9bd4300cd8d6ff83b8f348eed23aad73c9a86fa2dc")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
