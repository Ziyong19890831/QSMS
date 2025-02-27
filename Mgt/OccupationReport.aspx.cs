using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public partial class Mgt_OccupationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }
    public  void Report(DateTime dateYear)
    {
        string dateYearString = dateYear.ToString("yyyy");
        string FirstSeasonStart = dateYearString + "/01/01 00:00:00";//1月到3月底
        string FirstSeasonEnd = dateYearString + "/03/30 23:59:59";//1月到3月底
        string SecondSeasonStart = dateYearString + "/04/01 00:00:00";//4月到6月
        string SecondSeasonEnd = dateYearString + "/06/30 23:59:59";//4月到6月
        string ThirdSeasonStart = dateYearString + "/07/01 00:00:00";//7月到9月
        string ThirdSeasonEnd = dateYearString + "/09/30 23:59:59";//7月到9月
        string fourthSeasonStart = dateYearString + "/10/01 00:00:00";//10月到12月
        string fourthSeasonEnd = dateYearString + "/12/31 23:59:59";//10月到12月
        Dictionary<string, string> Season = new Dictionary<string, string>();
        Season.Add(FirstSeasonStart, FirstSeasonEnd);
        Season.Add(SecondSeasonStart, SecondSeasonEnd);
        Season.Add(ThirdSeasonStart, ThirdSeasonEnd);
        Season.Add(fourthSeasonStart, fourthSeasonEnd);
        List<int> LRoleSNO = new List<int>();
        LRoleSNO.Add(10);
        LRoleSNO.Add(11);
        LRoleSNO.Add(12);
        LRoleSNO.Add(13);
        DataHelper ObjDH = new DataHelper();
        DataTable CertEndResultTable = new DataTable("OccupationReport");
        CertEndResultTable.Columns.Add("醫", typeof(string));
        CertEndResultTable.Columns.Add("牙", typeof(string));
        CertEndResultTable.Columns.Add("藥", typeof(string));
        CertEndResultTable.Columns.Add("衛", typeof(string));
        DataTable IsChangeResultTable = new DataTable("OccupationReport");
        IsChangeResultTable.Columns.Add("醫", typeof(string));
        IsChangeResultTable.Columns.Add("牙", typeof(string));
        IsChangeResultTable.Columns.Add("藥", typeof(string));
        IsChangeResultTable.Columns.Add("衛", typeof(string));
        DataTable NoChangeResultTable = new DataTable("OccupationReport");
        NoChangeResultTable.Columns.Add("醫", typeof(string));
        NoChangeResultTable.Columns.Add("牙", typeof(string));
        NoChangeResultTable.Columns.Add("藥", typeof(string));
        NoChangeResultTable.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusOne = new DataTable("OccupationReport");
        ResultTableminusOne.Columns.Add("醫", typeof(string));
        ResultTableminusOne.Columns.Add("牙", typeof(string));
        ResultTableminusOne.Columns.Add("藥", typeof(string));
        ResultTableminusOne.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusTwo = new DataTable("OccupationReport");
        ResultTableminusTwo.Columns.Add("醫", typeof(string));
        ResultTableminusTwo.Columns.Add("牙", typeof(string));
        ResultTableminusTwo.Columns.Add("藥", typeof(string));
        ResultTableminusTwo.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusThree = new DataTable("OccupationReport");
        ResultTableminusThree.Columns.Add("醫", typeof(string));
        ResultTableminusThree.Columns.Add("牙", typeof(string));
        ResultTableminusThree.Columns.Add("藥", typeof(string));
        ResultTableminusThree.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusFour = new DataTable("OccupationReport");
        ResultTableminusFour.Columns.Add("醫", typeof(string));
        ResultTableminusFour.Columns.Add("牙", typeof(string));
        ResultTableminusFour.Columns.Add("藥", typeof(string));
        ResultTableminusFour.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusFive = new DataTable("OccupationReport");
        ResultTableminusFive.Columns.Add("醫", typeof(string));
        ResultTableminusFive.Columns.Add("牙", typeof(string));
        ResultTableminusFive.Columns.Add("藥", typeof(string));
        ResultTableminusFive.Columns.Add("衛", typeof(string));
        DataRow CertEndResultTableRow;        
        DataRow IsChangeResultTableRow;
        DataRow NoChangeResultTableRow;
        DataRow ResultTableminusOneRow;
        ResultTableminusOneRow = ResultTableminusOne.NewRow();
        ResultTableminusOne.Rows.Add(ResultTableminusOneRow);
        DataRow ResultTableminusTwoRow;
        ResultTableminusTwoRow = ResultTableminusTwo.NewRow();
        ResultTableminusTwo.Rows.Add(ResultTableminusTwoRow);
        DataRow ResultTableminusThreeRow;
        ResultTableminusThreeRow = ResultTableminusThree.NewRow();
        ResultTableminusThree.Rows.Add(ResultTableminusThreeRow);
        DataRow ResultTableminusFoureRow;
        ResultTableminusFoureRow = ResultTableminusFour.NewRow();
        ResultTableminusFour.Rows.Add(ResultTableminusFoureRow);
        DataRow ResultTableminusFiveRow;
        ResultTableminusFiveRow = ResultTableminusFive.NewRow();
        ResultTableminusFive.Rows.Add(ResultTableminusFiveRow);
        for (int i = 0; i < Season.Count; i++)
        {
            var item = Season.ElementAt(i);
            CertEndResultTableRow = CertEndResultTable.NewRow();
            CertEndResultTable.Rows.Add(CertEndResultTableRow);
            for (int j = 0; j < Season.Count; j++)
            {
                
                Dictionary<string, Object> adict = new Dictionary<string, object>();
       //         if (chk_PrsnContract.Checked == false)
       //         {
       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getFirstCertEndDateCount as(
       //                     	Select P.PersonID,P.RoleSNO from Person P
       //                     Left JOIN QS_Certificate QC On QC.PersonID=P.PersonID
       //                      Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0
          
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.*,CC.PersonID from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
       //                     ),getResult as(
       //                     Select distinct PersonID from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO
							//)
							//Select Count(gR.PersonID) '人數' from getResult gR";

       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             CertEndResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
       //         else
       //         {
       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getPrsnDistinctPrsnID as (
							//SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
							//),getFirstCertEndDateCount as(
       //                     	Select PC.PrsnID,P.RoleSNO from getPrsnDistinctPrsnID PC
       //                     Left Join Person P On PC.PrsnID=P.PersonID
       //                     Left JOIN QS_Certificate QC On QC.PersonID=PC.PrsnID
       //                     Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0
       //                     Group by P.RoleSNO,PC.PrsnID
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.RoleSNO,Count(CC.PrsnID)'人數' from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
							//	Group by gRN.RoleSNO
       //                     )
       //                     Select * from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO";
       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             CertEndResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
                
            }
        }
        for (int i = 0; i < Season.Count; i++)
        {
            var item = Season.ElementAt(i);
            IsChangeResultTableRow = IsChangeResultTable.NewRow();
            IsChangeResultTable.Rows.Add(IsChangeResultTableRow);
            for (int j = 0; j < Season.Count; j++)
            {
       //         if (chk_PrsnContract.Checked == false)
       //         {
       //             Dictionary<string, Object> adict = new Dictionary<string, object>();

       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getFirstCertEndDateCount as(
       //                     	Select P.PersonID,P.RoleSNO from Person P
       //                     Left JOIN QS_Certificate QC On QC.PersonID=P.PersonID
       //                      Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0 And IsChange=1
          
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.*,CC.PersonID from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
       //                     ),getResult as(
       //                     Select distinct PersonID from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO
							//)
							//Select Count(gR.PersonID) '人數' from getResult gR";

       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             IsChangeResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
       //         else
       //         {
       //             Dictionary<string, Object> adict = new Dictionary<string, object>();
       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getPrsnDistinctPrsnID as (
							//SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
							//),getFirstCertEndDateCount as(
       //                     	Select PC.PrsnID,P.RoleSNO from getPrsnDistinctPrsnID PC
       //                     Left Join Person P On PC.PrsnID=P.PersonID
       //                     Left JOIN QS_Certificate QC On QC.PersonID=PC.PrsnID
       //                     Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0 And IsChange=1
       //                     Group by P.RoleSNO,PC.PrsnID
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.RoleSNO,Count(CC.PrsnID)'人數' from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
							//	Group by gRN.RoleSNO
       //                     )
       //                     Select * from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO";

       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             IsChangeResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
                
            }
        }
        for (int i = 0; i < Season.Count; i++)
        {
            var item = Season.ElementAt(i);
            NoChangeResultTableRow = NoChangeResultTable.NewRow();
            NoChangeResultTable.Rows.Add(NoChangeResultTableRow);
            for (int j = 0; j < Season.Count; j++)
            {
       //         if (chk_PrsnContract.Checked == false)
       //         {
       //             Dictionary<string, Object> adict = new Dictionary<string, object>();

       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getFirstCertEndDateCount as(
       //                     	Select P.PersonID,P.RoleSNO from Person P
       //                     Left JOIN QS_Certificate QC On QC.PersonID=P.PersonID
       //                      Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0 And IsChange=0
          
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.*,CC.PersonID from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
       //                     ),getResult as(
       //                     Select distinct PersonID from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO
							//)
							//Select Count(gR.PersonID) '人數' from getResult gR";

       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             NoChangeResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
       //         else
       //         {
       //             Dictionary<string, Object> adict = new Dictionary<string, object>();

       //             string SQL = @"With getRoleName as(
       //                     	Select RoleSNO,RoleName from Role where Role.IsAdmin=0
       //                     ),getPrsnDistinctPrsnID as (
							//SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
							//),getFirstCertEndDateCount as(
       //                     	Select PC.PrsnID,P.RoleSNO from getPrsnDistinctPrsnID PC
       //                     Left Join Person P On PC.PrsnID=P.PersonID
       //                     Left JOIN QS_Certificate QC On QC.PersonID=PC.PrsnID
       //                     Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
       //                     where CertEndDate Between @dateYear and @FirstSeasonEnd And QCT.CtypeClass <> 0 And IsChange=0
       //                     Group by P.RoleSNO,PC.PrsnID
       //                     ),getFirstCertEndDateResult as(
       //                     	Select gRN.RoleSNO,Count(CC.PrsnID)'人數' from getRoleName gRN
       //                     	Left Join getFirstCertEndDateCount CC On CC.RoleSNO=gRN.RoleSNO
							//	Group by gRN.RoleSNO
       //                     )
       //                     Select * from getFirstCertEndDateResult gr where RoleSNO=@RoleSNO";

       //             adict.Add("dateYear", item.Key);
       //             adict.Add("FirstSeasonEnd", item.Value);
       //             adict.Add("RoleSNO", LRoleSNO[j].ToString());
       //             DataTable ObjDT = ObjDH.queryData(SQL, adict);
       //             NoChangeResultTable.Rows[i][j] = ObjDT.Rows[0]["人數"].ToString();
       //         }
               
            }
        }
//        if (chk_PrsnContract.Checked == false)
//        {
//            string ResultTableminusOneSQL = @"with DiscontPerson as (
//Select R.RoleSNO,R.RoleName,P.PersonID from Person P
//                        Left Join QS_Certificate QC On P.PersonID=QC.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleName,P.PersonID, R.RoleSNO
//)
//						Select Distinct RoleSNO,RoleName,Count(PersonID)'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminusOne = new Dictionary<string, object>();
//            adictResultTableminusOne.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(1));
//            adictResultTableminusOne.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(1));
//            ResultTableminusOne = ObjDH.queryData(ResultTableminusOneSQL, adictResultTableminusOne);
//        }
//        else
//        {
//            string ResultTableminusOneSQL = @"
//                        With getPrsnDistinctPrsnID as (
//	                        SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
//                        )
//                        Select R.RoleName,Count(*)'EndCert' from QS_Certificate QC
//                        Left Join getPrsnDistinctPrsnID PC On PC.PrsnID=QC.PersonID
//                        Left Join Person P On PC.PrsnID=P.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleSNO,R.RoleName
//                        Order by R.RoleSNO";
//            Dictionary<string, Object> adictResultTableminusOne = new Dictionary<string, object>();
//            adictResultTableminusOne.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(1));
//            adictResultTableminusOne.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(1));
//            ResultTableminusOne = ObjDH.queryData(ResultTableminusOneSQL, adictResultTableminusOne);
//        }
//        if (chk_PrsnContract.Checked == false)
//        {
//            string ResultTableminuTwoSQL = @"with DiscontPerson as (
//                    Select R.RoleSNO,R.RoleName,P.PersonID from Person P
//                        Left Join QS_Certificate QC On P.PersonID=QC.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between  @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleName,P.PersonID, R.RoleSNO
//                        )
//						Select Distinct RoleSNO,RoleName,Count(PersonID)'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuTwoSQL = new Dictionary<string, object>();
//            adictResultTableminuTwoSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(2));
//            adictResultTableminuTwoSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(2));
//            ResultTableminusTwo = ObjDH.queryData(ResultTableminuTwoSQL, adictResultTableminuTwoSQL);
//        }
//        else
//        {
//            string ResultTableminuTwoSQL = @"With getPrsnDistinctPrsnID as (
//	                        SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
//                        ),DiscontPerson as (
//                        Select R.RoleSNO,R.RoleName,PC.[PrsnID] from getPrsnDistinctPrsnID PC
//                        Left Join QS_Certificate QC On PC.PrsnID=QC.PersonID
//                        Left Join Person P On PC.PrsnID=P.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//						Group By R.RoleName,PC.[PrsnID], R.RoleSNO
//						)
//                       Select Distinct RoleSNO,RoleName,Count([PrsnID])'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuTwoSQL = new Dictionary<string, object>();
//            adictResultTableminuTwoSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(2));
//            adictResultTableminuTwoSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(2));
//            ResultTableminusTwo = ObjDH.queryData(ResultTableminuTwoSQL, adictResultTableminuTwoSQL);
//        }
//        if (chk_PrsnContract.Checked == false)
//        {
//            string ResultTableminuThreeSQL = @"with DiscontPerson as (
//                    Select R.RoleSNO,R.RoleName,P.PersonID from Person P
//                        Left Join QS_Certificate QC On P.PersonID=QC.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between  @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleName,P.PersonID, R.RoleSNO
//                        )
//						Select Distinct RoleSNO,RoleName,Count(PersonID)'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuThreeSQL = new Dictionary<string, object>();
//            adictResultTableminuThreeSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(3));
//            adictResultTableminuThreeSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(3));
//            ResultTableminusThree = ObjDH.queryData(ResultTableminuThreeSQL, adictResultTableminuThreeSQL);
//        }
//        else
//        {
//            string ResultTableminuThreeSQL = @"With getPrsnDistinctPrsnID as (
//	                        SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
//                        ),DiscontPerson as (
//                        Select R.RoleSNO,R.RoleName,PC.[PrsnID] from getPrsnDistinctPrsnID PC
//                        Left Join QS_Certificate QC On PC.PrsnID=QC.PersonID
//                        Left Join Person P On PC.PrsnID=P.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//						Group By R.RoleName,PC.[PrsnID], R.RoleSNO
//						)
//                       Select Distinct RoleSNO,RoleName,Count([PrsnID])'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuThreeSQL = new Dictionary<string, object>();
//            adictResultTableminuThreeSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(3));
//            adictResultTableminuThreeSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(3));
//            ResultTableminusThree = ObjDH.queryData(ResultTableminuThreeSQL, adictResultTableminuThreeSQL);
//        }
//        if (chk_PrsnContract.Checked == false)
//        {
//            string ResultTableminuFourSQL = @"with DiscontPerson as (
//                    Select R.RoleSNO,R.RoleName,P.PersonID from Person P
//                        Left Join QS_Certificate QC On P.PersonID=QC.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between  @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleName,P.PersonID, R.RoleSNO
//                        )
//						Select Distinct RoleSNO,RoleName,Count(PersonID)'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuFourSQL = new Dictionary<string, object>();
//            adictResultTableminuFourSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(4));
//            adictResultTableminuFourSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(4));
//            ResultTableminusFour = ObjDH.queryData(ResultTableminuFourSQL, adictResultTableminuFourSQL);
//        }
//        else
//        {
//            string ResultTableminuFourSQL = @"With getPrsnDistinctPrsnID as (
//	                        SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
//                        ),DiscontPerson as (
//                        Select R.RoleSNO,R.RoleName,PC.[PrsnID] from getPrsnDistinctPrsnID PC
//                        Left Join QS_Certificate QC On PC.PrsnID=QC.PersonID
//                        Left Join Person P On PC.PrsnID=P.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//						Group By R.RoleName,PC.[PrsnID], R.RoleSNO
//						)
//                       Select Distinct RoleSNO,RoleName,Count([PrsnID])'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuFourSQL = new Dictionary<string, object>();
//            adictResultTableminuFourSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(4));
//            adictResultTableminuFourSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(4));
//            ResultTableminusFour = ObjDH.queryData(ResultTableminuFourSQL, adictResultTableminuFourSQL);
//        }
//        if (chk_PrsnContract.Checked == false)
//        {
//            string ResultTableminuFiveSQL = @"with DiscontPerson as (
//                    Select R.RoleSNO,R.RoleName,P.PersonID from Person P
//                        Left Join QS_Certificate QC On P.PersonID=QC.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between  @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//                        Group By R.RoleName,P.PersonID, R.RoleSNO
//                        )
//						Select Distinct RoleSNO,RoleName,Count(PersonID)'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuFiveSQL = new Dictionary<string, object>();
//            adictResultTableminuFiveSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(4));
//            adictResultTableminuFiveSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(4));
//            ResultTableminusFive = ObjDH.queryData(ResultTableminuFiveSQL, adictResultTableminuFiveSQL);
//        }
//        else
//        {
//            string ResultTableminuFiveSQL = @"With getPrsnDistinctPrsnID as (
//	                        SELECT distinct [PrsnID] FROM [QSMS].[dbo].[PrsnContract]
//                        ),DiscontPerson as (
//                        Select R.RoleSNO,R.RoleName,PC.[PrsnID] from getPrsnDistinctPrsnID PC
//                        Left Join QS_Certificate QC On PC.PrsnID=QC.PersonID
//                        Left Join Person P On PC.PrsnID=P.PersonID
//                        Left Join Role R On R.RoleSNO=P.RoleSNO
//                        Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
//                        where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and R.RoleSNO is not null
//                        and QCT.CGroup is not null
//						Group By R.RoleName,PC.[PrsnID], R.RoleSNO
//						)
//                       Select Distinct RoleSNO,RoleName,Count([PrsnID])'EndCert' from DiscontPerson DP
//						Group by RoleName,RoleSNO
//						Order by  RoleSNO";
//            Dictionary<string, Object> adictResultTableminuFiveSQL = new Dictionary<string, object>();
//            adictResultTableminuFiveSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart).AddYears(4));
//            adictResultTableminuFiveSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd).AddYears(4));
//            ResultTableminusFive = ObjDH.queryData(ResultTableminuFiveSQL, adictResultTableminuFiveSQL);
//        }
        #region EXCEL製表
        using (var excel = new ExcelPackage())
        {

            // 建立分頁
            var ws = excel.Workbook.Worksheets.Add("查詢年度各季資料統計");
            ExcelWorksheet sheet1 = excel.Workbook.Worksheets[0];
            // 寫入資料
            sheet1.Cells[1, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1911).ToString("yyy") + "年";
            sheet1.Cells[1, 1, 1, 16].Merge = true;
            sheet1.Cells[1, 1, 1, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[1, 1, 1, 16].Style.Font.Size = 12;
            sheet1.Cells[1, 1, 1, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[1, 1, 1, 16].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet1.Cells[2, 1].Value = "證書到期人數";
            sheet1.Cells[2, 1, 2, 16].Merge = true;
            sheet1.Cells[2, 1, 2, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[2, 1, 2, 16].Style.Font.Size = 12;
            sheet1.Cells[2, 1, 2, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[2, 1, 2, 16].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            sheet1.Cells[3, 1].Value = "第一季";
            sheet1.Cells[3, 1, 3, 4].Merge = true;
            sheet1.Cells[3, 1, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[3, 1, 3, 4].Style.Font.Size = 12;
            sheet1.Cells[3, 5].Value = "第二季";
            sheet1.Cells[3, 5, 3, 8].Merge = true;
            sheet1.Cells[3, 5, 3, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[3, 5, 3, 8].Style.Font.Size = 12;
            sheet1.Cells[3, 9].Value = "第三季";
            sheet1.Cells[3, 9, 3, 12].Merge = true;
            sheet1.Cells[3, 9, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[3, 9, 3, 12].Style.Font.Size = 12;
            sheet1.Cells[3, 13].Value = "第四季";
            sheet1.Cells[3, 13, 3, 16].Merge = true;
            sheet1.Cells[3, 13, 3, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[3, 13, 3, 16].Style.Font.Size = 12;
            for (int i = 1; i <= 16; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[4, i].Value = "醫";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[4, i].Value = "牙";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[4, i].Value = "藥";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[4, i].Value = "衛";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }


            sheet1.Cells[5, 1].Value = CertEndResultTable.Rows[0][0].ToString();
            sheet1.Cells[5, 2].Value = CertEndResultTable.Rows[0][1].ToString();
            sheet1.Cells[5, 3].Value = CertEndResultTable.Rows[0][2].ToString();
            sheet1.Cells[5, 4].Value = CertEndResultTable.Rows[0][3].ToString();
            sheet1.Cells[5, 5].Value = CertEndResultTable.Rows[1][0].ToString();
            sheet1.Cells[5, 6].Value = CertEndResultTable.Rows[1][1].ToString();
            sheet1.Cells[5, 7].Value = CertEndResultTable.Rows[1][2].ToString();
            sheet1.Cells[5, 8].Value = CertEndResultTable.Rows[1][3].ToString();
            sheet1.Cells[5, 9].Value = CertEndResultTable.Rows[2][0].ToString();
            sheet1.Cells[5, 10].Value = CertEndResultTable.Rows[2][1].ToString();
            sheet1.Cells[5, 11].Value = CertEndResultTable.Rows[2][2].ToString();
            sheet1.Cells[5, 12].Value = CertEndResultTable.Rows[2][3].ToString();
            sheet1.Cells[5, 13].Value = CertEndResultTable.Rows[3][0].ToString();
            sheet1.Cells[5, 14].Value = CertEndResultTable.Rows[3][1].ToString();
            sheet1.Cells[5, 15].Value = CertEndResultTable.Rows[3][2].ToString();
            sheet1.Cells[5, 16].Value = CertEndResultTable.Rows[3][3].ToString();

            sheet1.Cells[6, 1].Value = "已換證人數";
            sheet1.Cells[6, 1, 6, 16].Merge = true;
            sheet1.Cells[6, 1, 6, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[6, 1, 6, 16].Style.Font.Size = 12;
            sheet1.Cells[6, 1, 6, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[6, 1, 6, 16].Style.Fill.BackgroundColor.SetColor(Color.LightSeaGreen);
            sheet1.Cells[7, 1].Value = "第一季";
            sheet1.Cells[7, 1, 7, 4].Merge = true;
            sheet1.Cells[7, 1, 7, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[7, 1, 7, 4].Style.Font.Size = 12;
            sheet1.Cells[7, 5].Value = "第二季";
            sheet1.Cells[7, 5, 7, 8].Merge = true;
            sheet1.Cells[7, 5, 7, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[7, 5, 7, 8].Style.Font.Size = 12;
            sheet1.Cells[7, 9].Value = "第三季";
            sheet1.Cells[7, 9, 7, 12].Merge = true;
            sheet1.Cells[7, 9, 7, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[7, 9, 7, 12].Style.Font.Size = 12;
            sheet1.Cells[7, 13].Value = "第四季";
            sheet1.Cells[7, 13, 7, 16].Merge = true;
            sheet1.Cells[7, 13, 7, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[7, 13, 7, 16].Style.Font.Size = 12;

            for (int i = 1; i <= 16; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[8, i].Value = "醫";
                        sheet1.Cells[8, i, 8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[8, i].Value = "牙";
                        sheet1.Cells[8, i, 8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[8, i].Value = "藥";
                        sheet1.Cells[8, i, 8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[8, i].Value = "衛";
                        sheet1.Cells[8, i, 8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            sheet1.Cells[9, 1].Value = IsChangeResultTable.Rows[0][0].ToString();
            sheet1.Cells[9, 2].Value = IsChangeResultTable.Rows[0][1].ToString();
            sheet1.Cells[9, 3].Value = IsChangeResultTable.Rows[0][2].ToString();
            sheet1.Cells[9, 4].Value = IsChangeResultTable.Rows[0][3].ToString();
            sheet1.Cells[9, 5].Value = IsChangeResultTable.Rows[1][0].ToString();
            sheet1.Cells[9, 6].Value = IsChangeResultTable.Rows[1][1].ToString();
            sheet1.Cells[9, 7].Value = IsChangeResultTable.Rows[1][2].ToString();
            sheet1.Cells[9, 8].Value = IsChangeResultTable.Rows[1][3].ToString();
            sheet1.Cells[9, 9].Value = IsChangeResultTable.Rows[2][0].ToString();
            sheet1.Cells[9, 10].Value = IsChangeResultTable.Rows[2][1].ToString();
            sheet1.Cells[9, 11].Value = IsChangeResultTable.Rows[2][2].ToString();
            sheet1.Cells[9, 12].Value = IsChangeResultTable.Rows[2][3].ToString();
            sheet1.Cells[9, 13].Value = IsChangeResultTable.Rows[3][0].ToString();
            sheet1.Cells[9, 14].Value = IsChangeResultTable.Rows[3][1].ToString();
            sheet1.Cells[9, 15].Value = IsChangeResultTable.Rows[3][2].ToString();
            sheet1.Cells[9, 16].Value = IsChangeResultTable.Rows[3][3].ToString();

            sheet1.Cells[10, 1].Value = "未換證人數";
            sheet1.Cells[10, 1, 10, 16].Merge = true;
            sheet1.Cells[10, 1, 10, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[10, 1, 10, 16].Style.Font.Size = 12;
            sheet1.Cells[10, 1, 10, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[10, 1, 10, 16].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            sheet1.Cells[11, 1].Value = "第一季";
            sheet1.Cells[11, 1, 11, 4].Merge = true;
            sheet1.Cells[11, 1, 11, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[11, 1, 11, 4].Style.Font.Size = 12;
            sheet1.Cells[11, 5].Value = "第二季";
            sheet1.Cells[11, 5, 11, 8].Merge = true;
            sheet1.Cells[11, 5, 11, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[11, 5, 11, 8].Style.Font.Size = 12;
            sheet1.Cells[11, 9].Value = "第三季";
            sheet1.Cells[11, 9, 11, 12].Merge = true;
            sheet1.Cells[11, 9, 11, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[11, 9, 11, 12].Style.Font.Size = 12;
            sheet1.Cells[11, 13].Value = "第四季";
            sheet1.Cells[11, 13, 11, 16].Merge = true;
            sheet1.Cells[11, 13, 11, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[11, 13, 11, 16].Style.Font.Size = 12;
            for (int i = 1; i <= 16; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[12, i].Value = "醫";
                        sheet1.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[12, i].Value = "牙";
                        sheet1.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[12, i].Value = "藥";
                        sheet1.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[12, i].Value = "衛";
                        sheet1.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            sheet1.Cells[13, 1].Value = NoChangeResultTable.Rows[0][0].ToString();
            sheet1.Cells[13, 2].Value = NoChangeResultTable.Rows[0][1].ToString();
            sheet1.Cells[13, 3].Value = NoChangeResultTable.Rows[0][2].ToString();
            sheet1.Cells[13, 4].Value = NoChangeResultTable.Rows[0][3].ToString();
            sheet1.Cells[13, 5].Value = NoChangeResultTable.Rows[1][0].ToString();
            sheet1.Cells[13, 6].Value = NoChangeResultTable.Rows[1][1].ToString();
            sheet1.Cells[13, 7].Value = NoChangeResultTable.Rows[1][2].ToString();
            sheet1.Cells[13, 8].Value = NoChangeResultTable.Rows[1][3].ToString();
            sheet1.Cells[13, 9].Value = NoChangeResultTable.Rows[2][0].ToString();
            sheet1.Cells[13, 10].Value = NoChangeResultTable.Rows[2][1].ToString();
            sheet1.Cells[13, 11].Value = NoChangeResultTable.Rows[2][2].ToString();
            sheet1.Cells[13, 12].Value = NoChangeResultTable.Rows[2][3].ToString();
            sheet1.Cells[13, 13].Value = NoChangeResultTable.Rows[3][0].ToString();
            sheet1.Cells[13, 14].Value = NoChangeResultTable.Rows[3][1].ToString();
            sheet1.Cells[13, 15].Value = NoChangeResultTable.Rows[3][2].ToString();
            sheet1.Cells[13, 16].Value = NoChangeResultTable.Rows[3][3].ToString();

            sheet1.Cells[14, 1].Value = "換證率";
            sheet1.Cells[14, 1, 14, 16].Merge = true;
            sheet1.Cells[14, 1, 14, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[14, 1, 14, 16].Style.Font.Size = 12;
            sheet1.Cells[14, 1, 14, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[14, 1, 14, 16].Style.Fill.BackgroundColor.SetColor(Color.LightPink);
            sheet1.Cells[15, 1].Value = "第一季";
            sheet1.Cells[15, 1, 15, 4].Merge = true;
            sheet1.Cells[15, 1, 15, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[15, 1, 15, 4].Style.Font.Size = 12;
            sheet1.Cells[15, 5].Value = "第二季";
            sheet1.Cells[15, 5, 15, 8].Merge = true;
            sheet1.Cells[15, 5, 15, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[15, 5, 15, 8].Style.Font.Size = 12;
            sheet1.Cells[15, 9].Value = "第三季";
            sheet1.Cells[15, 9, 15, 12].Merge = true;
            sheet1.Cells[15, 9, 15, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[15, 9, 15, 12].Style.Font.Size = 12;
            sheet1.Cells[15, 13].Value = "第四季";
            sheet1.Cells[15, 13, 15, 16].Merge = true;
            sheet1.Cells[15, 13, 15, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[15, 13, 15, 16].Style.Font.Size = 12;
            for (int i = 1; i <= 16; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[16, i].Value = "醫";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[16, i].Value = "牙";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[16, i].Value = "藥";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[16, i].Value = "衛";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            sheet1.Cells[17, 1].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[0][0]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[0][0]) / Convert.ToDecimal(CertEndResultTable.Rows[0][0]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 2].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[0][1]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[0][1]) / Convert.ToDecimal(CertEndResultTable.Rows[0][1]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 3].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[0][2]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[0][2]) / Convert.ToDecimal(CertEndResultTable.Rows[0][2]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 4].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[0][3]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[0][3]) / Convert.ToDecimal(CertEndResultTable.Rows[0][3]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 5].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[1][0]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[1][0]) / Convert.ToDecimal(CertEndResultTable.Rows[1][0]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 6].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[1][1]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[1][1]) / Convert.ToDecimal(CertEndResultTable.Rows[1][1]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 7].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[1][2]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[1][2]) / Convert.ToDecimal(CertEndResultTable.Rows[1][2]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 8].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[1][3]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[1][3]) / Convert.ToDecimal(CertEndResultTable.Rows[1][3]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 9].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[2][0]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[2][0]) / Convert.ToDecimal(CertEndResultTable.Rows[2][0]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 10].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[2][1]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[2][1]) / Convert.ToDecimal(CertEndResultTable.Rows[2][1]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 11].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[2][2]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[2][2]) / Convert.ToDecimal(CertEndResultTable.Rows[2][2]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 12].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[2][3]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[2][3]) / Convert.ToDecimal(CertEndResultTable.Rows[2][3]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 13].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[3][0]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[3][0]) / Convert.ToDecimal(CertEndResultTable.Rows[3][0]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 14].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[3][1]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[3][1]) / Convert.ToDecimal(CertEndResultTable.Rows[3][1]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 15].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[3][2]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[3][2]) / Convert.ToDecimal(CertEndResultTable.Rows[3][2]) * 100, 2)).ToString() + "%";
            sheet1.Cells[17, 16].Value = (Convert.ToDecimal(IsChangeResultTable.Rows[3][3]) == 0 ? 0 : Math.Round(Convert.ToDecimal(IsChangeResultTable.Rows[3][3]) / Convert.ToDecimal(CertEndResultTable.Rows[3][3]) * 100, 2)).ToString() + "%";

            var ws2 = excel.Workbook.Worksheets.Add("各年證書到期人數統計");
            ExcelWorksheet sheet2 = excel.Workbook.Worksheets[1];
            sheet2.Cells[1, 1].Value = "各年證書到期人數";
            sheet2.Cells[1, 1, 1, 4].Merge = true;
            sheet2.Cells[1, 1, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[1, 1, 1, 4].Style.Font.Size = 12;
            sheet2.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet2.Cells[2, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1910).ToString("yyy") + "年";
            sheet2.Cells[2, 1, 2, 4].Merge = true;
            sheet2.Cells[2, 1, 2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[2, 1, 2, 4].Style.Font.Size = 12;
            sheet2.Cells[2, 1, 2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[2, 1, 2, 4].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            for (int i = 1; i <= 4; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[3, i].Value = "醫";
                        sheet2.Cells[3, i, 3, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[3, i, 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[3, i, 3, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 2:
                        sheet2.Cells[3, i].Value = "牙";
                        sheet2.Cells[3, i, 3, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[3, i, 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[3, i, 3, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 3:
                        sheet2.Cells[3, i].Value = "藥";
                        sheet2.Cells[3, i, 3, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[3, i, 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[3, i, 3, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 0:
                        sheet2.Cells[3, i].Value = "衛";
                        sheet2.Cells[3, i, 3, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[3, i, 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[3, i, 3, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                }

            }
            for (int i = 1; i <= ResultTableminusOne.Rows.Count; i++)
            {

                sheet2.Cells[4, i].Value = ResultTableminusOne.Rows[i - 1]["EndCert"].ToString();
                sheet2.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
            sheet2.Cells[5, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1909).ToString("yyy") + "年";
            sheet2.Cells[5, 1, 5, 4].Merge = true;
            sheet2.Cells[5, 1, 5, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[5, 1, 5, 4].Style.Font.Size = 12;
            sheet2.Cells[5, 1, 5, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[5, 1, 5, 4].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            for (int i = 1; i <= 4; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[6, i].Value = "醫";
                        sheet2.Cells[6, i, 6, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[6, i, 6, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[6, i, 6, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 2:
                        sheet2.Cells[6, i].Value = "牙";
                        sheet2.Cells[6, i, 6, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[6, i, 6, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[6, i, 6, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 3:
                        sheet2.Cells[6, i].Value = "藥";
                        sheet2.Cells[6, i, 6, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[6, i, 6, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[6, i, 6, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 0:
                        sheet2.Cells[6, i].Value = "衛";
                        sheet2.Cells[6, i, 6, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[6, i, 6, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[6, i, 6, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                }

            }
            for (int i = 1; i <= ResultTableminusTwo.Rows.Count; i++)
            {

                sheet2.Cells[7, i].Value = ResultTableminusTwo.Rows[i - 1]["EndCert"].ToString();
                sheet2.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
            sheet2.Cells[8, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1908).ToString("yyy") + "年";
            sheet2.Cells[8, 1, 8, 4].Merge = true;
            sheet2.Cells[8, 1, 8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[8, 1, 8, 4].Style.Font.Size = 12;
            sheet2.Cells[8, 1, 8, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[8, 1, 8, 4].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            for (int i = 1; i <= 4; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[9, i].Value = "醫";
                        sheet2.Cells[9, i, 9, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[9, i, 9, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[9, i, 9, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 2:
                        sheet2.Cells[9, i].Value = "牙";
                        sheet2.Cells[9, i, 9, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[9, i, 9, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[9, i, 9, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 3:
                        sheet2.Cells[9, i].Value = "藥";
                        sheet2.Cells[9, i, 9, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[9, i, 9, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[9, i, 9, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 0:
                        sheet2.Cells[9, i].Value = "衛";
                        sheet2.Cells[9, i, 9, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[9, i, 9, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[9, i, 9, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                }

            }
            for (int i = 1; i <= ResultTableminusThree.Rows.Count; i++)
            {

                sheet2.Cells[10, i].Value = ResultTableminusThree.Rows[i - 1]["EndCert"].ToString();
                sheet2.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }

            sheet2.Cells[11, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1907).ToString("yyy") + "年";
            sheet2.Cells[11, 1, 11, 4].Merge = true;
            sheet2.Cells[11, 1, 11, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[11, 1, 11, 4].Style.Font.Size = 12;
            sheet2.Cells[11, 1, 11, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[11, 1, 11, 4].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            for (int i = 1; i <= 4; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[12, i].Value = "醫";
                        sheet2.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[12, i, 12, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[12, i, 12, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 2:
                        sheet2.Cells[12, i].Value = "牙";
                        sheet2.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[12, i, 12, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[12, i, 12, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 3:
                        sheet2.Cells[12, i].Value = "藥";
                        sheet2.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[12, i, 12, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[12, i, 12, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                    case 0:
                        sheet2.Cells[12, i].Value = "衛";
                        sheet2.Cells[12, i, 12, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[12, i, 12, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet2.Cells[12, i, 12, i].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        break;
                }

            }
            for (int i = 1; i <= ResultTableminusFour.Rows.Count; i++)
            {

                sheet2.Cells[13, i].Value = ResultTableminusFour.Rows[i - 1]["EndCert"].ToString();
                sheet2.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
            #endregion
            #region EXCEL輸出
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;  filename=" + "職類統計" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(excel.GetAsByteArray());
            Response.End();
            #endregion
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
        DateTime dt;
        //if (DateTime.TryParseExact(txt_Year.Text, "yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out dt))
        //{
        //    Report(dt);
        //}

    }
    protected void btnCity_Click(object sender, EventArgs e)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
        DateTime dt;
        if (DateTime.TryParseExact(txt_YearTwo.Text, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
           
            
            
            btnExport_data();
        }
    }
    public void ReportNew(DateTime dateYear)
    {
        string dateYearString = dateYear.ToString("yyyy");
        string FirstSeasonStart = dateYearString + "/01/01 00:00:00";//1月到3月底
        string FirstSeasonEnd = dateYearString + "/03/30 23:59:59";//1月到3月底
        string SecondSeasonStart = dateYearString + "/04/01 00:00:00";//4月到6月
        string SecondSeasonEnd = dateYearString + "/06/30 23:59:59";//4月到6月
        string ThirdSeasonStart = dateYearString + "/07/01 00:00:00";//7月到9月
        string ThirdSeasonEnd = dateYearString + "/09/30 23:59:59";//7月到9月
        string fourthSeasonStart = dateYearString + "/10/01 00:00:00";//10月到12月
        string fourthSeasonEnd = dateYearString + "/12/31 23:59:59";//10月到12月
        Dictionary<string, string> Season = new Dictionary<string, string>();
        Season.Add(FirstSeasonStart, FirstSeasonEnd);
        Season.Add(SecondSeasonStart, SecondSeasonEnd);
        Season.Add(ThirdSeasonStart, ThirdSeasonEnd);
        Season.Add(fourthSeasonStart, fourthSeasonEnd);
        List<int> LRoleSNO = new List<int>();
        LRoleSNO.Add(10);
        LRoleSNO.Add(11);
        LRoleSNO.Add(12);
        LRoleSNO.Add(13);
        DataHelper ObjDH = new DataHelper();
        DataTable ResultTableminusOne = new DataTable("ReportCertificateStatistic");
        ResultTableminusOne.Columns.Add("醫", typeof(string));
        ResultTableminusOne.Columns.Add("牙", typeof(string));
        ResultTableminusOne.Columns.Add("藥", typeof(string));
        ResultTableminusOne.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusTwo = new DataTable("ReportCertificateStatistic");
        ResultTableminusTwo.Columns.Add("醫", typeof(string));
        ResultTableminusTwo.Columns.Add("牙", typeof(string));
        ResultTableminusTwo.Columns.Add("藥", typeof(string));
        ResultTableminusTwo.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusThree = new DataTable("ReportCertificateStatistic");
        ResultTableminusThree.Columns.Add("醫", typeof(string));
        ResultTableminusThree.Columns.Add("牙", typeof(string));
        ResultTableminusThree.Columns.Add("藥", typeof(string));
        ResultTableminusThree.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusFour = new DataTable("ReportCertificateStatistic");
        ResultTableminusFour.Columns.Add("醫", typeof(string));
        ResultTableminusFour.Columns.Add("牙", typeof(string));
        ResultTableminusFour.Columns.Add("藥", typeof(string));
        ResultTableminusFour.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusFive = new DataTable("ReportCertificateStatistic");
        ResultTableminusFive.Columns.Add("醫", typeof(string));
        ResultTableminusFive.Columns.Add("牙", typeof(string));
        ResultTableminusFive.Columns.Add("藥", typeof(string));
        ResultTableminusFive.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusSix = new DataTable("ReportCertificateStatistic");
        ResultTableminusSix.Columns.Add("醫", typeof(string));
        ResultTableminusSix.Columns.Add("牙", typeof(string));
        ResultTableminusSix.Columns.Add("藥", typeof(string));
        ResultTableminusSix.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusSeven = new DataTable("ReportCertificateStatistic");
        ResultTableminusSeven.Columns.Add("醫", typeof(string));
        ResultTableminusSeven.Columns.Add("牙", typeof(string));
        ResultTableminusSeven.Columns.Add("藥", typeof(string));
        ResultTableminusSeven.Columns.Add("衛", typeof(string));
        DataTable ResultTableminusEight = new DataTable("ReportCertificateStatistic");
        ResultTableminusEight.Columns.Add("醫", typeof(string));
        ResultTableminusEight.Columns.Add("牙", typeof(string));
        ResultTableminusEight.Columns.Add("藥", typeof(string));
        ResultTableminusEight.Columns.Add("衛", typeof(string));
        DataRow ResultTableminusOneRow;
        ResultTableminusOneRow = ResultTableminusOne.NewRow();
        ResultTableminusOne.Rows.Add(ResultTableminusOneRow);
        DataRow ResultTableminusTwoRow;
        ResultTableminusTwoRow = ResultTableminusTwo.NewRow();
        ResultTableminusTwo.Rows.Add(ResultTableminusTwoRow);
        DataRow ResultTableminusThreeRow;
        ResultTableminusThreeRow = ResultTableminusThree.NewRow();
        ResultTableminusThree.Rows.Add(ResultTableminusThreeRow);
        DataRow ResultTableminusFoureRow;
        ResultTableminusFoureRow = ResultTableminusFour.NewRow();
        ResultTableminusFour.Rows.Add(ResultTableminusFoureRow);
        DataRow ResultTableminusFiveRow;
        ResultTableminusFiveRow = ResultTableminusFive.NewRow();
        ResultTableminusFive.Rows.Add(ResultTableminusFiveRow);
        DataHelper objDH = new DataHelper();
        DataTable objCity = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='A'", null);
        string ResultTableminusOneSQL = @"        select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode != '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminusOne = new Dictionary<string, object>();
        adictResultTableminusOne.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart));
        adictResultTableminusOne.Add("fourthSeasonEnd", Convert.ToDateTime(FirstSeasonEnd));
        ResultTableminusOne = ObjDH.queryData(ResultTableminusOneSQL, adictResultTableminusOne);
        foreach (DataRow dr in objCity.Rows)
        {
            string city = dr[2].ToString();
        }
        string ResultTableminuTwoSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode != '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuTwoSQL = new Dictionary<string, object>();
        adictResultTableminuTwoSQL.Add("FirstSeasonStart", Convert.ToDateTime(SecondSeasonStart));
        adictResultTableminuTwoSQL.Add("fourthSeasonEnd", Convert.ToDateTime(SecondSeasonEnd));
        ResultTableminusTwo = ObjDH.queryData(ResultTableminuTwoSQL, adictResultTableminuTwoSQL);

        string ResultTableminuThreeSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode != '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuThreeSQL = new Dictionary<string, object>();
        adictResultTableminuThreeSQL.Add("FirstSeasonStart", Convert.ToDateTime(ThirdSeasonStart));
        adictResultTableminuThreeSQL.Add("fourthSeasonEnd", Convert.ToDateTime(ThirdSeasonEnd));
        ResultTableminusThree = ObjDH.queryData(ResultTableminuThreeSQL, adictResultTableminuThreeSQL);

        string ResultTableminuFourSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode != '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuFourSQL = new Dictionary<string, object>();
        adictResultTableminuFourSQL.Add("FirstSeasonStart", Convert.ToDateTime(fourthSeasonStart));
        adictResultTableminuFourSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd));
        ResultTableminusFour = ObjDH.queryData(ResultTableminuFourSQL, adictResultTableminuFourSQL);

        string ResultTableminuFiveSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode = '000' and QCT.CTypeSNO !='12'

)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuFiveSQL = new Dictionary<string, object>();
        adictResultTableminuFiveSQL.Add("FirstSeasonStart", Convert.ToDateTime(FirstSeasonStart));
        adictResultTableminuFiveSQL.Add("fourthSeasonEnd", Convert.ToDateTime(FirstSeasonEnd));
        ResultTableminusFive = ObjDH.queryData(ResultTableminuFiveSQL, adictResultTableminuFiveSQL);
        string ResultTableminuSixSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode = '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuSixSQL = new Dictionary<string, object>();
        adictResultTableminuSixSQL.Add("FirstSeasonStart", Convert.ToDateTime(SecondSeasonStart));
        adictResultTableminuSixSQL.Add("fourthSeasonEnd", Convert.ToDateTime(SecondSeasonEnd));
        ResultTableminusSix = ObjDH.queryData(ResultTableminuSixSQL, adictResultTableminuSixSQL);
        string ResultTableminuSevenSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode = '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuSevenSQL = new Dictionary<string, object>();
        adictResultTableminuSevenSQL.Add("FirstSeasonStart", Convert.ToDateTime(ThirdSeasonStart));
        adictResultTableminuSevenSQL.Add("fourthSeasonEnd", Convert.ToDateTime(ThirdSeasonEnd));
        ResultTableminusSeven = ObjDH.queryData(ResultTableminuSevenSQL, adictResultTableminuSevenSQL);
        string ResultTableminuEightSQL = @"select* from(
Select distinct QC.PersonID, p.RoleSNO, AREA_NAME
from QS_Certificate QC
Left Join QS_CertificateType QCT On QCT.CTypeSNO= QC.CTypeSNO
Left Join Person P On P.PersonID= QC.PersonID
Left Join Role R On R.RoleSNO= P.RoleSNO
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'where QC.CertEndDate Between @FirstSeasonStart and @fourthSeasonEnd and p.RoleSNO is not null and Org.OrganCode = '000' and QCT.CTypeSNO !='12'
)t
PIVOT(
--設定彙總欄位及方式
Count(PersonID)
-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
FOR AREA_NAME IN(台北市, 台中市, 台南市, 高雄市, 基隆市, 新竹市, 嘉義市, 新北市, 桃園市, 新竹縣, 宜蘭縣, 苗栗縣, 彰化縣, 南投縣, 雲林縣, 嘉義縣, 屏東縣, 澎湖縣, 花蓮縣, 台東縣, 金門縣, 連江縣)
) p";
        Dictionary<string, Object> adictResultTableminuEightSQL = new Dictionary<string, object>();
        adictResultTableminuEightSQL.Add("FirstSeasonStart", Convert.ToDateTime(fourthSeasonStart));
        adictResultTableminuEightSQL.Add("fourthSeasonEnd", Convert.ToDateTime(fourthSeasonEnd));
        ResultTableminusEight = ObjDH.queryData(ResultTableminuEightSQL, adictResultTableminuEightSQL);
        using (var excel = new ExcelPackage())
        {
            #region 第一季
            // 建立分頁
            var ws = excel.Workbook.Worksheets.Add("第一季");
            ExcelWorksheet sheet1 = excel.Workbook.Worksheets[0];
            // 寫入資料
            sheet1.Cells[1, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1911).ToString("yyy") + "年";
            sheet1.Cells[1, 1, 1, 20].Merge = true;
            sheet1.Cells[1, 1, 1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[1, 1, 1, 20].Style.Font.Size = 12;
            sheet1.Cells[1, 1, 1, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[1, 1, 1, 20].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet1.Cells[2, 1].Value = "當前證書效期內人數統計(依人員執登縣市別)";
            sheet1.Cells[2, 1, 2, 20].Merge = true;
            sheet1.Cells[2, 1, 2, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[2, 1, 2, 20].Style.Font.Size = 12;
            sheet1.Cells[2, 1, 2, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[2, 1, 2, 20].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            #region 無現職單位
            sheet1.Cells[15, 9].Value = "無現職單位";
            sheet1.Cells[15, 9, 15, 12].Merge = true;
            sheet1.Cells[15, 9, 15, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells[15, 9, 15, 12].Style.Font.Size = 12;
            sheet1.Cells[15, 9, 15, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet1.Cells[15, 9, 15, 12].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            sheet1.Cells[16, 9].Value = "醫";
            sheet1.Cells[16, 10].Value = "牙";
            sheet1.Cells[16, 11].Value = "藥";
            sheet1.Cells[16, 12].Value = "衛";
            sheet1.Cells[17, 9].Value = 0;
            sheet1.Cells[17, 10].Value = 0;
            sheet1.Cells[17, 11].Value = 0;
            sheet1.Cells[17, 12].Value = 0;
            for (int i = 0; i < ResultTableminusFive.Rows.Count; i++)
            {
                if (ResultTableminusFive.Rows[i][0].ToString() == "10")//西
                {
                    sheet1.Cells[17, 9].Value = ResultTableminusFive.Rows[i][1];
                }
                if (ResultTableminusFive.Rows[i][0].ToString() == "11")//牙
                {
                    sheet1.Cells[17, 10].Value = ResultTableminusFive.Rows[i][1];
                }
                if (ResultTableminusFive.Rows[i][0].ToString() == "12")//藥
                {
                    sheet1.Cells[17, 11].Value = ResultTableminusFive.Rows[i][1];
                }
                if (ResultTableminusFive.Rows[i][0].ToString() == "13")//衛
                {
                    sheet1.Cells[17, 12].Value = ResultTableminusFive.Rows[i][1];
                }
            }
            #endregion
            for (int i = 0; i < 22; i++)//縣市清單
            {
                int j = i / 5;
                int k = i % 5;
                sheet1.Cells[3 * j + 3, 4 * k + 1].Value = objCity.Rows[i][2];
                sheet1.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Merge = true;
                sheet1.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet1.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Font.Size = 12;
                sheet1.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet1.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            }
            for (int i = 1; i <= 20; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[4, i].Value = "醫";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[7, i].Value = "醫";
                        sheet1.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[10, i].Value = "醫";
                        sheet1.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[13, i].Value = "醫";
                        sheet1.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[4, i].Value = "牙";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[7, i].Value = "牙";
                        sheet1.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[10, i].Value = "牙";
                        sheet1.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[13, i].Value = "牙";
                        sheet1.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[4, i].Value = "藥";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[7, i].Value = "藥";
                        sheet1.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[10, i].Value = "藥";
                        sheet1.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[13, i].Value = "藥";
                        sheet1.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[4, i].Value = "衛";
                        sheet1.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[7, i].Value = "衛";
                        sheet1.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[10, i].Value = "衛";
                        sheet1.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[13, i].Value = "衛";
                        sheet1.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 8; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet1.Cells[16, i].Value = "醫";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet1.Cells[16, i].Value = "牙";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet1.Cells[16, i].Value = "藥";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet1.Cells[16, i].Value = "衛";
                        sheet1.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 20; i++)
            {
                sheet1.Cells[5, i].Value = 0;
                sheet1.Cells[8, i].Value = 0;
                sheet1.Cells[11, i].Value = 0;
                sheet1.Cells[14, i].Value = 0;
            }
            sheet1.Cells[17, 1].Value = 0;
            sheet1.Cells[17, 2].Value = 0;
            sheet1.Cells[17, 3].Value = 0;
            sheet1.Cells[17, 4].Value = 0;
            sheet1.Cells[17, 5].Value = 0;
            sheet1.Cells[17, 6].Value = 0;
            sheet1.Cells[17, 7].Value = 0;
            sheet1.Cells[17, 8].Value = 0;
            for (int i = 0; i < ResultTableminusOne.Rows.Count; i++)
            {
                if (ResultTableminusOne.Rows[i][0].ToString() == "10")//西
                {
                    sheet1.Cells[5, 1].Value = ResultTableminusOne.Rows[i][1];
                    sheet1.Cells[5, 5].Value = ResultTableminusOne.Rows[i][2];
                    sheet1.Cells[5, 9].Value = ResultTableminusOne.Rows[i][3];
                    sheet1.Cells[5, 13].Value = ResultTableminusOne.Rows[i][4];
                    sheet1.Cells[5, 17].Value = ResultTableminusOne.Rows[i][5];
                    sheet1.Cells[8, 1].Value = ResultTableminusOne.Rows[i][6];
                    sheet1.Cells[8, 5].Value = ResultTableminusOne.Rows[i][7];
                    sheet1.Cells[8, 9].Value = ResultTableminusOne.Rows[i][8];
                    sheet1.Cells[8, 13].Value = ResultTableminusOne.Rows[i][9];
                    sheet1.Cells[8, 17].Value = ResultTableminusOne.Rows[i][10];
                    sheet1.Cells[11, 1].Value = ResultTableminusOne.Rows[i][11];
                    sheet1.Cells[11, 5].Value = ResultTableminusOne.Rows[i][12];
                    sheet1.Cells[11, 9].Value = ResultTableminusOne.Rows[i][13];
                    sheet1.Cells[11, 13].Value = ResultTableminusOne.Rows[i][14];
                    sheet1.Cells[11, 17].Value = ResultTableminusOne.Rows[i][15];
                    sheet1.Cells[14, 1].Value = ResultTableminusOne.Rows[i][16];
                    sheet1.Cells[14, 5].Value = ResultTableminusOne.Rows[i][17];
                    sheet1.Cells[14, 9].Value = ResultTableminusOne.Rows[i][18];
                    sheet1.Cells[14, 13].Value = ResultTableminusOne.Rows[i][19];
                    sheet1.Cells[14, 17].Value = ResultTableminusOne.Rows[i][20];
                    sheet1.Cells[17, 1].Value = ResultTableminusOne.Rows[i][21];
                    sheet1.Cells[17, 5].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "11")//牙
                {
                    sheet1.Cells[5, 2].Value = ResultTableminusOne.Rows[i][1];
                    sheet1.Cells[5, 6].Value = ResultTableminusOne.Rows[i][2];
                    sheet1.Cells[5, 10].Value = ResultTableminusOne.Rows[i][3];
                    sheet1.Cells[5, 14].Value = ResultTableminusOne.Rows[i][4];
                    sheet1.Cells[5, 18].Value = ResultTableminusOne.Rows[i][5];
                    sheet1.Cells[8, 2].Value = ResultTableminusOne.Rows[i][6];
                    sheet1.Cells[8, 6].Value = ResultTableminusOne.Rows[i][7];
                    sheet1.Cells[8, 10].Value = ResultTableminusOne.Rows[i][8];
                    sheet1.Cells[8, 14].Value = ResultTableminusOne.Rows[i][9];
                    sheet1.Cells[8, 18].Value = ResultTableminusOne.Rows[i][10];
                    sheet1.Cells[11, 2].Value = ResultTableminusOne.Rows[i][11];
                    sheet1.Cells[11, 6].Value = ResultTableminusOne.Rows[i][12];
                    sheet1.Cells[11, 10].Value = ResultTableminusOne.Rows[i][13];
                    sheet1.Cells[11, 14].Value = ResultTableminusOne.Rows[i][14];
                    sheet1.Cells[11, 18].Value = ResultTableminusOne.Rows[i][15];
                    sheet1.Cells[14, 2].Value = ResultTableminusOne.Rows[i][16];
                    sheet1.Cells[14, 6].Value = ResultTableminusOne.Rows[i][17];
                    sheet1.Cells[14, 10].Value = ResultTableminusOne.Rows[i][18];
                    sheet1.Cells[14, 14].Value = ResultTableminusOne.Rows[i][19];
                    sheet1.Cells[14, 18].Value = ResultTableminusOne.Rows[i][20];
                    sheet1.Cells[17, 2].Value = ResultTableminusOne.Rows[i][21];
                    sheet1.Cells[17, 6].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "12")//藥
                {
                    sheet1.Cells[5, 3].Value = ResultTableminusOne.Rows[i][1];
                    sheet1.Cells[5, 7].Value = ResultTableminusOne.Rows[i][2];
                    sheet1.Cells[5, 11].Value = ResultTableminusOne.Rows[i][3];
                    sheet1.Cells[5, 15].Value = ResultTableminusOne.Rows[i][4];
                    sheet1.Cells[5, 19].Value = ResultTableminusOne.Rows[i][5];
                    sheet1.Cells[8, 3].Value = ResultTableminusOne.Rows[i][6];
                    sheet1.Cells[8, 7].Value = ResultTableminusOne.Rows[i][7];
                    sheet1.Cells[8, 11].Value = ResultTableminusOne.Rows[i][8];
                    sheet1.Cells[8, 15].Value = ResultTableminusOne.Rows[i][9];
                    sheet1.Cells[8, 19].Value = ResultTableminusOne.Rows[i][10];
                    sheet1.Cells[11, 3].Value = ResultTableminusOne.Rows[i][11];
                    sheet1.Cells[11, 7].Value = ResultTableminusOne.Rows[i][12];
                    sheet1.Cells[11, 11].Value = ResultTableminusOne.Rows[i][13];
                    sheet1.Cells[11, 15].Value = ResultTableminusOne.Rows[i][14];
                    sheet1.Cells[11, 19].Value = ResultTableminusOne.Rows[i][15];
                    sheet1.Cells[14, 3].Value = ResultTableminusOne.Rows[i][16];
                    sheet1.Cells[14, 7].Value = ResultTableminusOne.Rows[i][17];
                    sheet1.Cells[14, 11].Value = ResultTableminusOne.Rows[i][18];
                    sheet1.Cells[14, 15].Value = ResultTableminusOne.Rows[i][19];
                    sheet1.Cells[14, 19].Value = ResultTableminusOne.Rows[i][20];
                    sheet1.Cells[17, 3].Value = ResultTableminusOne.Rows[i][21];
                    sheet1.Cells[17, 7].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "13")//衛
                {
                    sheet1.Cells[5, 4].Value = ResultTableminusOne.Rows[i][1];
                    sheet1.Cells[5, 8].Value = ResultTableminusOne.Rows[i][2];
                    sheet1.Cells[5, 12].Value = ResultTableminusOne.Rows[i][3];
                    sheet1.Cells[5, 16].Value = ResultTableminusOne.Rows[i][4];
                    sheet1.Cells[5, 20].Value = ResultTableminusOne.Rows[i][5];
                    sheet1.Cells[8, 4].Value = ResultTableminusOne.Rows[i][6];
                    sheet1.Cells[8, 8].Value = ResultTableminusOne.Rows[i][7];
                    sheet1.Cells[8, 12].Value = ResultTableminusOne.Rows[i][8];
                    sheet1.Cells[8, 16].Value = ResultTableminusOne.Rows[i][9];
                    sheet1.Cells[8, 20].Value = ResultTableminusOne.Rows[i][10];
                    sheet1.Cells[11, 4].Value = ResultTableminusOne.Rows[i][11];
                    sheet1.Cells[11, 8].Value = ResultTableminusOne.Rows[i][12];
                    sheet1.Cells[11, 12].Value = ResultTableminusOne.Rows[i][13];
                    sheet1.Cells[11, 16].Value = ResultTableminusOne.Rows[i][14];
                    sheet1.Cells[11, 20].Value = ResultTableminusOne.Rows[i][15];
                    sheet1.Cells[14, 4].Value = ResultTableminusOne.Rows[i][16];
                    sheet1.Cells[14, 8].Value = ResultTableminusOne.Rows[i][17];
                    sheet1.Cells[14, 12].Value = ResultTableminusOne.Rows[i][18];
                    sheet1.Cells[14, 16].Value = ResultTableminusOne.Rows[i][19];
                    sheet1.Cells[14, 20].Value = ResultTableminusOne.Rows[i][20];
                    sheet1.Cells[17, 4].Value = ResultTableminusOne.Rows[i][21];
                    sheet1.Cells[17, 8].Value = ResultTableminusOne.Rows[i][22];
                }
            }

            #endregion
            #region 第二季
            var ws2 = excel.Workbook.Worksheets.Add("第二季");
            ExcelWorksheet sheet2 = excel.Workbook.Worksheets[1];
            sheet2.Cells[1, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1911).ToString("yyy") + "年";
            sheet2.Cells[1, 1, 1, 20].Merge = true;
            sheet2.Cells[1, 1, 1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[1, 1, 1, 20].Style.Font.Size = 12;
            sheet2.Cells[1, 1, 1, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[1, 1, 1, 20].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet2.Cells[2, 1].Value = "當前證書效期內人數統計(依人員執登縣市別)";
            sheet2.Cells[2, 1, 2, 20].Merge = true;
            sheet2.Cells[2, 1, 2, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[2, 1, 2, 20].Style.Font.Size = 12;
            sheet2.Cells[2, 1, 2, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[2, 1, 2, 20].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            #region 無現職單位
            sheet2.Cells[15, 9].Value = "無現職單位";
            sheet2.Cells[15, 9, 15, 12].Merge = true;
            sheet2.Cells[15, 9, 15, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells[15, 9, 15, 12].Style.Font.Size = 12;
            sheet2.Cells[15, 9, 15, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet2.Cells[15, 9, 15, 12].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            sheet2.Cells[16, 9].Value = "醫";
            sheet2.Cells[16, 10].Value = "牙";
            sheet2.Cells[16, 11].Value = "藥";
            sheet2.Cells[16, 12].Value = "衛";
            sheet2.Cells[17, 9].Value = 0;
            sheet2.Cells[17, 10].Value = 0;
            sheet2.Cells[17, 11].Value = 0;
            sheet2.Cells[17, 12].Value = 0;
            for (int i = 0; i < ResultTableminusSix.Rows.Count; i++)
            {
                if (ResultTableminusSix.Rows[i][0].ToString() == "10")//西
                {
                    sheet2.Cells[17, 9].Value = ResultTableminusSix.Rows[i][1];
                }
                if (ResultTableminusSix.Rows[i][0].ToString() == "11")//牙
                {
                    sheet2.Cells[17, 10].Value = ResultTableminusSix.Rows[i][1];
                }
                if (ResultTableminusSix.Rows[i][0].ToString() == "12")//藥
                {
                    sheet2.Cells[17, 11].Value = ResultTableminusSix.Rows[i][1];
                }
                if (ResultTableminusSix.Rows[i][0].ToString() == "13")//衛
                {
                    sheet2.Cells[17, 12].Value = ResultTableminusSix.Rows[i][1];
                }
            }
            #endregion
            for (int i = 0; i < 22; i++)//縣市清單
            {
                int j = i / 5;
                int k = i % 5;
                sheet2.Cells[3 * j + 3, 4 * k + 1].Value = objCity.Rows[i][2];
                sheet2.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Merge = true;
                sheet2.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet2.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Font.Size = 12;
                sheet2.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet2.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            }
            for (int i = 1; i <= 20; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[4, i].Value = "醫";
                        sheet2.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[7, i].Value = "醫";
                        sheet2.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[10, i].Value = "醫";
                        sheet2.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[13, i].Value = "醫";
                        sheet2.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet2.Cells[4, i].Value = "牙";
                        sheet2.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[7, i].Value = "牙";
                        sheet2.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[10, i].Value = "牙";
                        sheet2.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[13, i].Value = "牙";
                        sheet2.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet2.Cells[4, i].Value = "藥";
                        sheet2.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[7, i].Value = "藥";
                        sheet2.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[10, i].Value = "藥";
                        sheet2.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[13, i].Value = "藥";
                        sheet2.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet2.Cells[4, i].Value = "衛";
                        sheet2.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[7, i].Value = "衛";
                        sheet2.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[10, i].Value = "衛";
                        sheet2.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet2.Cells[13, i].Value = "衛";
                        sheet2.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 8; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet2.Cells[16, i].Value = "醫";
                        sheet2.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet2.Cells[16, i].Value = "牙";
                        sheet2.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet2.Cells[16, i].Value = "藥";
                        sheet2.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet2.Cells[16, i].Value = "衛";
                        sheet2.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 20; i++)
            {
                sheet2.Cells[5, i].Value = 0;
                sheet2.Cells[8, i].Value = 0;
                sheet2.Cells[11, i].Value = 0;
                sheet2.Cells[14, i].Value = 0;
            }
            sheet2.Cells[17, 1].Value = 0;
            sheet2.Cells[17, 2].Value = 0;
            sheet2.Cells[17, 3].Value = 0;
            sheet2.Cells[17, 4].Value = 0;
            sheet2.Cells[17, 5].Value = 0;
            sheet2.Cells[17, 6].Value = 0;
            sheet2.Cells[17, 7].Value = 0;
            sheet2.Cells[17, 8].Value = 0;
            for (int i = 0; i < ResultTableminusTwo.Rows.Count; i++)
            {
                if (ResultTableminusTwo.Rows[i][0].ToString() == "10")//西
                {
                    sheet2.Cells[5, 1].Value = ResultTableminusOne.Rows[i][1];
                    sheet2.Cells[5, 5].Value = ResultTableminusOne.Rows[i][2];
                    sheet2.Cells[5, 9].Value = ResultTableminusOne.Rows[i][3];
                    sheet2.Cells[5, 13].Value = ResultTableminusOne.Rows[i][4];
                    sheet2.Cells[5, 17].Value = ResultTableminusOne.Rows[i][5];
                    sheet2.Cells[8, 1].Value = ResultTableminusOne.Rows[i][6];
                    sheet2.Cells[8, 5].Value = ResultTableminusOne.Rows[i][7];
                    sheet2.Cells[8, 9].Value = ResultTableminusOne.Rows[i][8];
                    sheet2.Cells[8, 13].Value = ResultTableminusOne.Rows[i][9];
                    sheet2.Cells[8, 17].Value = ResultTableminusOne.Rows[i][10];
                    sheet2.Cells[11, 1].Value = ResultTableminusOne.Rows[i][11];
                    sheet2.Cells[11, 5].Value = ResultTableminusOne.Rows[i][12];
                    sheet2.Cells[11, 9].Value = ResultTableminusOne.Rows[i][13];
                    sheet2.Cells[11, 13].Value = ResultTableminusOne.Rows[i][14];
                    sheet2.Cells[11, 17].Value = ResultTableminusOne.Rows[i][15];
                    sheet2.Cells[14, 1].Value = ResultTableminusOne.Rows[i][16];
                    sheet2.Cells[14, 5].Value = ResultTableminusOne.Rows[i][17];
                    sheet2.Cells[14, 9].Value = ResultTableminusOne.Rows[i][18];
                    sheet2.Cells[14, 13].Value = ResultTableminusOne.Rows[i][19];
                    sheet2.Cells[14, 17].Value = ResultTableminusOne.Rows[i][20];
                    sheet2.Cells[17, 1].Value = ResultTableminusOne.Rows[i][21];
                    sheet2.Cells[17, 5].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "11")//牙
                {
                    sheet2.Cells[5, 2].Value = ResultTableminusOne.Rows[i][1];
                    sheet2.Cells[5, 6].Value = ResultTableminusOne.Rows[i][2];
                    sheet2.Cells[5, 10].Value = ResultTableminusOne.Rows[i][3];
                    sheet2.Cells[5, 14].Value = ResultTableminusOne.Rows[i][4];
                    sheet2.Cells[5, 18].Value = ResultTableminusOne.Rows[i][5];
                    sheet2.Cells[8, 2].Value = ResultTableminusOne.Rows[i][6];
                    sheet2.Cells[8, 6].Value = ResultTableminusOne.Rows[i][7];
                    sheet2.Cells[8, 10].Value = ResultTableminusOne.Rows[i][8];
                    sheet2.Cells[8, 14].Value = ResultTableminusOne.Rows[i][9];
                    sheet2.Cells[8, 18].Value = ResultTableminusOne.Rows[i][10];
                    sheet2.Cells[11, 2].Value = ResultTableminusOne.Rows[i][11];
                    sheet2.Cells[11, 6].Value = ResultTableminusOne.Rows[i][12];
                    sheet2.Cells[11, 10].Value = ResultTableminusOne.Rows[i][13];
                    sheet2.Cells[11, 14].Value = ResultTableminusOne.Rows[i][14];
                    sheet2.Cells[11, 18].Value = ResultTableminusOne.Rows[i][15];
                    sheet2.Cells[14, 2].Value = ResultTableminusOne.Rows[i][16];
                    sheet2.Cells[14, 6].Value = ResultTableminusOne.Rows[i][17];
                    sheet2.Cells[14, 10].Value = ResultTableminusOne.Rows[i][18];
                    sheet2.Cells[14, 14].Value = ResultTableminusOne.Rows[i][19];
                    sheet2.Cells[14, 18].Value = ResultTableminusOne.Rows[i][20];
                    sheet2.Cells[17, 2].Value = ResultTableminusOne.Rows[i][21];
                    sheet2.Cells[17, 6].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "12")//藥
                {
                    sheet2.Cells[5, 3].Value = ResultTableminusOne.Rows[i][1];
                    sheet2.Cells[5, 7].Value = ResultTableminusOne.Rows[i][2];
                    sheet2.Cells[5, 11].Value = ResultTableminusOne.Rows[i][3];
                    sheet2.Cells[5, 15].Value = ResultTableminusOne.Rows[i][4];
                    sheet2.Cells[5, 19].Value = ResultTableminusOne.Rows[i][5];
                    sheet2.Cells[8, 3].Value = ResultTableminusOne.Rows[i][6];
                    sheet2.Cells[8, 7].Value = ResultTableminusOne.Rows[i][7];
                    sheet2.Cells[8, 11].Value = ResultTableminusOne.Rows[i][8];
                    sheet2.Cells[8, 15].Value = ResultTableminusOne.Rows[i][9];
                    sheet2.Cells[8, 19].Value = ResultTableminusOne.Rows[i][10];
                    sheet2.Cells[11, 3].Value = ResultTableminusOne.Rows[i][11];
                    sheet2.Cells[11, 7].Value = ResultTableminusOne.Rows[i][12];
                    sheet2.Cells[11, 11].Value = ResultTableminusOne.Rows[i][13];
                    sheet2.Cells[11, 15].Value = ResultTableminusOne.Rows[i][14];
                    sheet2.Cells[11, 19].Value = ResultTableminusOne.Rows[i][15];
                    sheet2.Cells[14, 3].Value = ResultTableminusOne.Rows[i][16];
                    sheet2.Cells[14, 7].Value = ResultTableminusOne.Rows[i][17];
                    sheet2.Cells[14, 11].Value = ResultTableminusOne.Rows[i][18];
                    sheet2.Cells[14, 15].Value = ResultTableminusOne.Rows[i][19];
                    sheet2.Cells[14, 19].Value = ResultTableminusOne.Rows[i][20];
                    sheet2.Cells[17, 3].Value = ResultTableminusOne.Rows[i][21];
                    sheet2.Cells[17, 7].Value = ResultTableminusOne.Rows[i][22];
                }
                if (ResultTableminusOne.Rows[i][0].ToString() == "13")//衛
                {
                    sheet2.Cells[5, 4].Value = ResultTableminusOne.Rows[i][1];
                    sheet2.Cells[5, 8].Value = ResultTableminusOne.Rows[i][2];
                    sheet2.Cells[5, 12].Value = ResultTableminusOne.Rows[i][3];
                    sheet2.Cells[5, 16].Value = ResultTableminusOne.Rows[i][4];
                    sheet2.Cells[5, 20].Value = ResultTableminusOne.Rows[i][5];
                    sheet2.Cells[8, 4].Value = ResultTableminusOne.Rows[i][6];
                    sheet2.Cells[8, 8].Value = ResultTableminusOne.Rows[i][7];
                    sheet2.Cells[8, 12].Value = ResultTableminusOne.Rows[i][8];
                    sheet2.Cells[8, 16].Value = ResultTableminusOne.Rows[i][9];
                    sheet2.Cells[8, 20].Value = ResultTableminusOne.Rows[i][10];
                    sheet2.Cells[11, 4].Value = ResultTableminusOne.Rows[i][11];
                    sheet2.Cells[11, 8].Value = ResultTableminusOne.Rows[i][12];
                    sheet2.Cells[11, 12].Value = ResultTableminusOne.Rows[i][13];
                    sheet2.Cells[11, 16].Value = ResultTableminusOne.Rows[i][14];
                    sheet2.Cells[11, 20].Value = ResultTableminusOne.Rows[i][15];
                    sheet2.Cells[14, 4].Value = ResultTableminusOne.Rows[i][16];
                    sheet2.Cells[14, 8].Value = ResultTableminusOne.Rows[i][17];
                    sheet2.Cells[14, 12].Value = ResultTableminusOne.Rows[i][18];
                    sheet2.Cells[14, 16].Value = ResultTableminusOne.Rows[i][19];
                    sheet2.Cells[14, 20].Value = ResultTableminusOne.Rows[i][20];
                    sheet2.Cells[17, 4].Value = ResultTableminusOne.Rows[i][21];
                    sheet2.Cells[17, 8].Value = ResultTableminusOne.Rows[i][22];
                }
            }

            #endregion
            #region 第三季
            var ws3 = excel.Workbook.Worksheets.Add("第三季");
            ExcelWorksheet sheet3 = excel.Workbook.Worksheets[2];
            sheet3.Cells[1, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1911).ToString("yyy") + "年";
            sheet3.Cells[1, 1, 1, 20].Merge = true;
            sheet3.Cells[1, 1, 1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet3.Cells[1, 1, 1, 20].Style.Font.Size = 12;
            sheet3.Cells[1, 1, 1, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet3.Cells[1, 1, 1, 20].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet3.Cells[2, 1].Value = "當前證書效期內人數統計(依人員執登縣市別)";
            sheet3.Cells[2, 1, 2, 20].Merge = true;
            sheet3.Cells[2, 1, 2, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet3.Cells[2, 1, 2, 20].Style.Font.Size = 12;
            sheet3.Cells[2, 1, 2, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet3.Cells[2, 1, 2, 20].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            #region 無現職單位
            sheet3.Cells[15, 9].Value = "無現職單位";
            sheet3.Cells[15, 9, 15, 12].Merge = true;
            sheet3.Cells[15, 9, 15, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet3.Cells[15, 9, 15, 12].Style.Font.Size = 12;
            sheet3.Cells[15, 9, 15, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet3.Cells[15, 9, 15, 12].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            sheet3.Cells[16, 9].Value = "醫";
            sheet3.Cells[16, 10].Value = "牙";
            sheet3.Cells[16, 11].Value = "藥";
            sheet3.Cells[16, 12].Value = "衛";
            sheet3.Cells[17, 9].Value = 0;
            sheet3.Cells[17, 10].Value = 0;
            sheet3.Cells[17, 11].Value = 0;
            sheet3.Cells[17, 12].Value = 0;
            for (int i = 0; i < ResultTableminusSeven.Rows.Count; i++)
            {
                if (ResultTableminusSeven.Rows[i][0].ToString() == "10")//西
                {
                    sheet3.Cells[17, 9].Value = ResultTableminusSeven.Rows[i][1];
                }
                if (ResultTableminusSeven.Rows[i][0].ToString() == "11")//牙
                {
                    sheet3.Cells[17, 10].Value = ResultTableminusSeven.Rows[i][1];
                }
                if (ResultTableminusSeven.Rows[i][0].ToString() == "12")//藥
                {
                    sheet3.Cells[17, 11].Value = ResultTableminusSeven.Rows[i][1];
                }
                if (ResultTableminusSeven.Rows[i][0].ToString() == "13")//衛
                {
                    sheet3.Cells[17, 12].Value = ResultTableminusSeven.Rows[i][1];
                }
            }
            #endregion
            for (int i = 0; i < 22; i++)//縣市清單
            {
                int j = i / 5;
                int k = i % 5;
                sheet3.Cells[3 * j + 3, 4 * k + 1].Value = objCity.Rows[i][2];
                sheet3.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Merge = true;
                sheet3.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet3.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Font.Size = 12;
                sheet3.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet3.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            }
            for (int i = 1; i <= 20; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet3.Cells[4, i].Value = "醫";
                        sheet3.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[7, i].Value = "醫";
                        sheet3.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[10, i].Value = "醫";
                        sheet3.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[13, i].Value = "醫";
                        sheet3.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet3.Cells[4, i].Value = "牙";
                        sheet3.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[7, i].Value = "牙";
                        sheet3.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[10, i].Value = "牙";
                        sheet3.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[13, i].Value = "牙";
                        sheet3.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet3.Cells[4, i].Value = "藥";
                        sheet3.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[7, i].Value = "藥";
                        sheet3.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[10, i].Value = "藥";
                        sheet3.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[13, i].Value = "藥";
                        sheet3.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet3.Cells[4, i].Value = "衛";
                        sheet3.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[7, i].Value = "衛";
                        sheet3.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[10, i].Value = "衛";
                        sheet3.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet3.Cells[13, i].Value = "衛";
                        sheet3.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 8; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet3.Cells[16, i].Value = "醫";
                        sheet3.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet3.Cells[16, i].Value = "牙";
                        sheet3.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet3.Cells[16, i].Value = "藥";
                        sheet3.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet3.Cells[16, i].Value = "衛";
                        sheet3.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 20; i++)
            {
                sheet3.Cells[5, i].Value = 0;
                sheet3.Cells[8, i].Value = 0;
                sheet3.Cells[11, i].Value = 0;
                sheet3.Cells[14, i].Value = 0;
            }
            sheet3.Cells[17, 1].Value = 0;
            sheet3.Cells[17, 2].Value = 0;
            sheet3.Cells[17, 3].Value = 0;
            sheet3.Cells[17, 4].Value = 0;
            sheet3.Cells[17, 5].Value = 0;
            sheet3.Cells[17, 6].Value = 0;
            sheet3.Cells[17, 7].Value = 0;
            sheet3.Cells[17, 8].Value = 0;
            for (int i = 0; i < ResultTableminusThree.Rows.Count; i++)
            {
                if (ResultTableminusThree.Rows[i][0].ToString() == "10")//西
                {
                    sheet3.Cells[5, 1].Value = ResultTableminusThree.Rows[i][1];
                    sheet3.Cells[5, 5].Value = ResultTableminusThree.Rows[i][2];
                    sheet3.Cells[5, 9].Value = ResultTableminusThree.Rows[i][3];
                    sheet3.Cells[5, 13].Value = ResultTableminusThree.Rows[i][4];
                    sheet3.Cells[5, 17].Value = ResultTableminusThree.Rows[i][5];
                    sheet3.Cells[8, 1].Value = ResultTableminusThree.Rows[i][6];
                    sheet3.Cells[8, 5].Value = ResultTableminusThree.Rows[i][7];
                    sheet3.Cells[8, 9].Value = ResultTableminusThree.Rows[i][8];
                    sheet3.Cells[8, 13].Value = ResultTableminusThree.Rows[i][9];
                    sheet3.Cells[8, 17].Value = ResultTableminusThree.Rows[i][10];
                    sheet3.Cells[11, 1].Value = ResultTableminusThree.Rows[i][11];
                    sheet3.Cells[11, 5].Value = ResultTableminusThree.Rows[i][12];
                    sheet3.Cells[11, 9].Value = ResultTableminusThree.Rows[i][13];
                    sheet3.Cells[11, 13].Value = ResultTableminusThree.Rows[i][14];
                    sheet3.Cells[11, 17].Value = ResultTableminusThree.Rows[i][15];
                    sheet3.Cells[14, 1].Value = ResultTableminusThree.Rows[i][16];
                    sheet3.Cells[14, 5].Value = ResultTableminusThree.Rows[i][17];
                    sheet3.Cells[14, 9].Value = ResultTableminusThree.Rows[i][18];
                    sheet3.Cells[14, 13].Value = ResultTableminusThree.Rows[i][19];
                    sheet3.Cells[14, 17].Value = ResultTableminusThree.Rows[i][20];
                    sheet3.Cells[17, 1].Value = ResultTableminusThree.Rows[i][21];
                    sheet3.Cells[17, 5].Value = ResultTableminusThree.Rows[i][22];
                }
                if (ResultTableminusThree.Rows[i][0].ToString() == "11")//牙
                {
                    sheet3.Cells[5, 2].Value = ResultTableminusThree.Rows[i][1];
                    sheet3.Cells[5, 6].Value = ResultTableminusThree.Rows[i][2];
                    sheet3.Cells[5, 10].Value = ResultTableminusThree.Rows[i][3];
                    sheet3.Cells[5, 14].Value = ResultTableminusThree.Rows[i][4];
                    sheet3.Cells[5, 18].Value = ResultTableminusThree.Rows[i][5];
                    sheet3.Cells[8, 2].Value = ResultTableminusThree.Rows[i][6];
                    sheet3.Cells[8, 6].Value = ResultTableminusThree.Rows[i][7];
                    sheet3.Cells[8, 10].Value = ResultTableminusThree.Rows[i][8];
                    sheet3.Cells[8, 14].Value = ResultTableminusThree.Rows[i][9];
                    sheet3.Cells[8, 18].Value = ResultTableminusThree.Rows[i][10];
                    sheet3.Cells[11, 2].Value = ResultTableminusThree.Rows[i][11];
                    sheet3.Cells[11, 6].Value = ResultTableminusThree.Rows[i][12];
                    sheet3.Cells[11, 10].Value = ResultTableminusThree.Rows[i][13];
                    sheet3.Cells[11, 14].Value = ResultTableminusThree.Rows[i][14];
                    sheet3.Cells[11, 18].Value = ResultTableminusThree.Rows[i][15];
                    sheet3.Cells[14, 2].Value = ResultTableminusThree.Rows[i][16];
                    sheet3.Cells[14, 6].Value = ResultTableminusThree.Rows[i][17];
                    sheet3.Cells[14, 10].Value = ResultTableminusThree.Rows[i][18];
                    sheet3.Cells[14, 14].Value = ResultTableminusThree.Rows[i][19];
                    sheet3.Cells[14, 18].Value = ResultTableminusThree.Rows[i][20];
                    sheet3.Cells[17, 2].Value = ResultTableminusThree.Rows[i][21];
                    sheet3.Cells[17, 6].Value = ResultTableminusThree.Rows[i][22];
                }
                if (ResultTableminusThree.Rows[i][0].ToString() == "12")//藥
                {
                    sheet3.Cells[5, 3].Value = ResultTableminusThree.Rows[i][1];
                    sheet3.Cells[5, 7].Value = ResultTableminusThree.Rows[i][2];
                    sheet3.Cells[5, 11].Value = ResultTableminusThree.Rows[i][3];
                    sheet3.Cells[5, 15].Value = ResultTableminusThree.Rows[i][4];
                    sheet3.Cells[5, 19].Value = ResultTableminusThree.Rows[i][5];
                    sheet3.Cells[8, 3].Value = ResultTableminusThree.Rows[i][6];
                    sheet3.Cells[8, 7].Value = ResultTableminusThree.Rows[i][7];
                    sheet3.Cells[8, 11].Value = ResultTableminusThree.Rows[i][8];
                    sheet3.Cells[8, 15].Value = ResultTableminusThree.Rows[i][9];
                    sheet3.Cells[8, 19].Value = ResultTableminusThree.Rows[i][10];
                    sheet3.Cells[11, 3].Value = ResultTableminusThree.Rows[i][11];
                    sheet3.Cells[11, 7].Value = ResultTableminusThree.Rows[i][12];
                    sheet3.Cells[11, 11].Value = ResultTableminusThree.Rows[i][13];
                    sheet3.Cells[11, 15].Value = ResultTableminusThree.Rows[i][14];
                    sheet3.Cells[11, 19].Value = ResultTableminusThree.Rows[i][15];
                    sheet3.Cells[14, 3].Value = ResultTableminusThree.Rows[i][16];
                    sheet3.Cells[14, 7].Value = ResultTableminusThree.Rows[i][17];
                    sheet3.Cells[14, 11].Value = ResultTableminusThree.Rows[i][18];
                    sheet3.Cells[14, 15].Value = ResultTableminusThree.Rows[i][19];
                    sheet3.Cells[14, 19].Value = ResultTableminusThree.Rows[i][20];
                    sheet3.Cells[17, 3].Value = ResultTableminusThree.Rows[i][21];
                    sheet3.Cells[17, 7].Value = ResultTableminusThree.Rows[i][22];
                }
                if (ResultTableminusThree.Rows[i][0].ToString() == "13")//衛
                {
                    sheet3.Cells[5, 4].Value = ResultTableminusThree.Rows[i][1];
                    sheet3.Cells[5, 8].Value = ResultTableminusThree.Rows[i][2];
                    sheet3.Cells[5, 12].Value = ResultTableminusThree.Rows[i][3];
                    sheet3.Cells[5, 16].Value = ResultTableminusThree.Rows[i][4];
                    sheet3.Cells[5, 20].Value = ResultTableminusThree.Rows[i][5];
                    sheet3.Cells[8, 4].Value = ResultTableminusThree.Rows[i][6];
                    sheet3.Cells[8, 8].Value = ResultTableminusThree.Rows[i][7];
                    sheet3.Cells[8, 12].Value = ResultTableminusThree.Rows[i][8];
                    sheet3.Cells[8, 16].Value = ResultTableminusThree.Rows[i][9];
                    sheet3.Cells[8, 20].Value = ResultTableminusThree.Rows[i][10];
                    sheet3.Cells[11, 4].Value = ResultTableminusThree.Rows[i][11];
                    sheet3.Cells[11, 8].Value = ResultTableminusThree.Rows[i][12];
                    sheet3.Cells[11, 12].Value = ResultTableminusThree.Rows[i][13];
                    sheet3.Cells[11, 16].Value = ResultTableminusThree.Rows[i][14];
                    sheet3.Cells[11, 20].Value = ResultTableminusThree.Rows[i][15];
                    sheet3.Cells[14, 4].Value = ResultTableminusThree.Rows[i][16];
                    sheet3.Cells[14, 8].Value = ResultTableminusThree.Rows[i][17];
                    sheet3.Cells[14, 12].Value = ResultTableminusThree.Rows[i][18];
                    sheet3.Cells[14, 16].Value = ResultTableminusThree.Rows[i][19];
                    sheet3.Cells[14, 20].Value = ResultTableminusThree.Rows[i][20];
                    sheet3.Cells[17, 4].Value = ResultTableminusThree.Rows[i][21];
                    sheet3.Cells[17, 8].Value = ResultTableminusThree.Rows[i][22];
                }
            }
            #endregion
            #region 第四季
            var ws4 = excel.Workbook.Worksheets.Add("第四季");
            ExcelWorksheet sheet4 = excel.Workbook.Worksheets[3];
            sheet4.Cells[1, 1].Value = Convert.ToDateTime(dateYear).AddYears(-1911).ToString("yyy") + "年";
            sheet4.Cells[1, 1, 1, 20].Merge = true;
            sheet4.Cells[1, 1, 1, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet4.Cells[1, 1, 1, 20].Style.Font.Size = 12;
            sheet4.Cells[1, 1, 1, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet4.Cells[1, 1, 1, 20].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            sheet4.Cells[2, 1].Value = "當前證書效期內人數統計(依人員執登縣市別)";
            sheet4.Cells[2, 1, 2, 20].Merge = true;
            sheet4.Cells[2, 1, 2, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet4.Cells[2, 1, 2, 20].Style.Font.Size = 12;
            sheet4.Cells[2, 1, 2, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet4.Cells[2, 1, 2, 20].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            # region 無現職單位
            sheet4.Cells[15, 9].Value = "無現職單位";
            sheet4.Cells[15, 9, 15, 12].Merge = true;
            sheet4.Cells[15, 9, 15, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet4.Cells[15, 9, 15, 12].Style.Font.Size = 12;
            sheet4.Cells[15, 9, 15, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet4.Cells[15, 9, 15, 12].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            sheet4.Cells[16, 9].Value = "醫";
            sheet4.Cells[16, 10].Value = "牙";
            sheet4.Cells[16, 11].Value = "藥";
            sheet4.Cells[16, 12].Value = "衛";
            sheet4.Cells[17, 9].Value = 0;
            sheet4.Cells[17, 10].Value = 0;
            sheet4.Cells[17, 11].Value = 0;
            sheet4.Cells[17, 12].Value = 0;
            for (int i = 0; i < ResultTableminusEight.Rows.Count; i++)
            {
                if (ResultTableminusEight.Rows[i][0].ToString() == "10")//西
                {
                    sheet4.Cells[17, 9].Value = ResultTableminusEight.Rows[i][1];
                }
                if (ResultTableminusEight.Rows[i][0].ToString() == "11")//牙
                {
                    sheet4.Cells[17, 10].Value = ResultTableminusEight.Rows[i][1];
                }
                if (ResultTableminusEight.Rows[i][0].ToString() == "12")//藥
                {
                    sheet4.Cells[17, 11].Value = ResultTableminusEight.Rows[i][1];
                }
                if (ResultTableminusEight.Rows[i][0].ToString() == "13")//衛
                {
                    sheet4.Cells[17, 12].Value = ResultTableminusEight.Rows[i][1];
                }
            }
            #endregion
            for (int i = 0; i < 22; i++)//縣市清單
            {
                int j = i / 5;
                int k = i % 5;
                sheet4.Cells[3 * j + 3, 4 * k + 1].Value = objCity.Rows[i][2];
                sheet4.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Merge = true;
                sheet4.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet4.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Font.Size = 12;
                sheet4.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet4.Cells[3 * j + 3, 4 * k + 1, 3 * j + 3, 4 * k + 4].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            }
            for (int i = 1; i <= 20; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet4.Cells[4, i].Value = "醫";
                        sheet4.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[7, i].Value = "醫";
                        sheet4.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[10, i].Value = "醫";
                        sheet4.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[13, i].Value = "醫";
                        sheet4.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet4.Cells[4, i].Value = "牙";
                        sheet4.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[7, i].Value = "牙";
                        sheet4.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[10, i].Value = "牙";
                        sheet4.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[13, i].Value = "牙";
                        sheet4.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet4.Cells[4, i].Value = "藥";
                        sheet4.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[7, i].Value = "藥";
                        sheet4.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[10, i].Value = "藥";
                        sheet4.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[13, i].Value = "藥";
                        sheet4.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet4.Cells[4, i].Value = "衛";
                        sheet4.Cells[4, i, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[7, i].Value = "衛";
                        sheet4.Cells[7, i, 7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[10, i].Value = "衛";
                        sheet4.Cells[10, i, 10, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet4.Cells[13, i].Value = "衛";
                        sheet4.Cells[13, i, 13, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 8; i++)
            {
                int Left = i % 4;
                switch (Left)
                {
                    case 1:
                        sheet4.Cells[16, i].Value = "醫";
                        sheet4.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 2:
                        sheet4.Cells[16, i].Value = "牙";
                        sheet4.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 3:
                        sheet4.Cells[16, i].Value = "藥";
                        sheet4.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case 0:
                        sheet4.Cells[16, i].Value = "衛";
                        sheet4.Cells[16, i, 16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }

            }
            for (int i = 1; i <= 20; i++)
            {
                sheet4.Cells[5, i].Value = 0;
                sheet4.Cells[8, i].Value = 0;
                sheet4.Cells[11, i].Value = 0;
                sheet4.Cells[14, i].Value = 0;
            }
            sheet4.Cells[17, 1].Value = 0;
            sheet4.Cells[17, 2].Value = 0;
            sheet4.Cells[17, 3].Value = 0;
            sheet4.Cells[17, 4].Value = 0;
            sheet4.Cells[17, 5].Value = 0;
            sheet4.Cells[17, 6].Value = 0;
            sheet4.Cells[17, 7].Value = 0;
            sheet4.Cells[17, 8].Value = 0;
            for (int i = 0; i < ResultTableminusFour.Rows.Count; i++)
            {
                if (ResultTableminusFour.Rows[i][0].ToString() == "10")//西
                {
                    sheet4.Cells[5, 1].Value = ResultTableminusFour.Rows[i][1];
                    sheet4.Cells[5, 5].Value = ResultTableminusFour.Rows[i][2];
                    sheet4.Cells[5, 9].Value = ResultTableminusFour.Rows[i][3];
                    sheet4.Cells[5, 13].Value = ResultTableminusFour.Rows[i][4];
                    sheet4.Cells[5, 17].Value = ResultTableminusFour.Rows[i][5];
                    sheet4.Cells[8, 1].Value = ResultTableminusFour.Rows[i][6];
                    sheet4.Cells[8, 5].Value = ResultTableminusFour.Rows[i][7];
                    sheet4.Cells[8, 9].Value = ResultTableminusFour.Rows[i][8];
                    sheet4.Cells[8, 13].Value = ResultTableminusFour.Rows[i][9];
                    sheet4.Cells[8, 17].Value = ResultTableminusFour.Rows[i][10];
                    sheet4.Cells[11, 1].Value = ResultTableminusFour.Rows[i][11];
                    sheet4.Cells[11, 5].Value = ResultTableminusFour.Rows[i][12];
                    sheet4.Cells[11, 9].Value = ResultTableminusFour.Rows[i][13];
                    sheet4.Cells[11, 13].Value = ResultTableminusFour.Rows[i][14];
                    sheet4.Cells[11, 17].Value = ResultTableminusFour.Rows[i][15];
                    sheet4.Cells[14, 1].Value = ResultTableminusFour.Rows[i][16];
                    sheet4.Cells[14, 5].Value = ResultTableminusFour.Rows[i][17];
                    sheet4.Cells[14, 9].Value = ResultTableminusFour.Rows[i][18];
                    sheet4.Cells[14, 13].Value = ResultTableminusFour.Rows[i][19];
                    sheet4.Cells[14, 17].Value = ResultTableminusFour.Rows[i][20];
                    sheet4.Cells[17, 1].Value = ResultTableminusFour.Rows[i][21];
                    sheet4.Cells[17, 5].Value = ResultTableminusFour.Rows[i][22];
                }
                if (ResultTableminusFour.Rows[i][0].ToString() == "11")//牙
                {
                    sheet4.Cells[5, 2].Value = ResultTableminusFour.Rows[i][1];
                    sheet4.Cells[5, 6].Value = ResultTableminusFour.Rows[i][2];
                    sheet4.Cells[5, 10].Value = ResultTableminusFour.Rows[i][3];
                    sheet4.Cells[5, 14].Value = ResultTableminusFour.Rows[i][4];
                    sheet4.Cells[5, 18].Value = ResultTableminusFour.Rows[i][5];
                    sheet4.Cells[8, 2].Value = ResultTableminusFour.Rows[i][6];
                    sheet4.Cells[8, 6].Value = ResultTableminusFour.Rows[i][7];
                    sheet4.Cells[8, 10].Value = ResultTableminusFour.Rows[i][8];
                    sheet4.Cells[8, 14].Value = ResultTableminusFour.Rows[i][9];
                    sheet4.Cells[8, 18].Value = ResultTableminusFour.Rows[i][10];
                    sheet4.Cells[11, 2].Value = ResultTableminusFour.Rows[i][11];
                    sheet4.Cells[11, 6].Value = ResultTableminusFour.Rows[i][12];
                    sheet4.Cells[11, 10].Value = ResultTableminusFour.Rows[i][13];
                    sheet4.Cells[11, 14].Value = ResultTableminusFour.Rows[i][14];
                    sheet4.Cells[11, 18].Value = ResultTableminusFour.Rows[i][15];
                    sheet4.Cells[14, 2].Value = ResultTableminusFour.Rows[i][16];
                    sheet4.Cells[14, 6].Value = ResultTableminusFour.Rows[i][17];
                    sheet4.Cells[14, 10].Value = ResultTableminusFour.Rows[i][18];
                    sheet4.Cells[14, 14].Value = ResultTableminusFour.Rows[i][19];
                    sheet4.Cells[14, 18].Value = ResultTableminusFour.Rows[i][20];
                    sheet4.Cells[17, 2].Value = ResultTableminusFour.Rows[i][21];
                    sheet4.Cells[17, 6].Value = ResultTableminusFour.Rows[i][22];
                }
                if (ResultTableminusFour.Rows[i][0].ToString() == "12")//藥
                {
                    sheet4.Cells[5, 3].Value = ResultTableminusFour.Rows[i][1];
                    sheet4.Cells[5, 7].Value = ResultTableminusFour.Rows[i][2];
                    sheet4.Cells[5, 11].Value = ResultTableminusFour.Rows[i][3];
                    sheet4.Cells[5, 15].Value = ResultTableminusFour.Rows[i][4];
                    sheet4.Cells[5, 19].Value = ResultTableminusFour.Rows[i][5];
                    sheet4.Cells[8, 3].Value = ResultTableminusFour.Rows[i][6];
                    sheet4.Cells[8, 7].Value = ResultTableminusFour.Rows[i][7];
                    sheet4.Cells[8, 11].Value = ResultTableminusFour.Rows[i][8];
                    sheet4.Cells[8, 15].Value = ResultTableminusFour.Rows[i][9];
                    sheet4.Cells[8, 19].Value = ResultTableminusFour.Rows[i][10];
                    sheet4.Cells[11, 3].Value = ResultTableminusFour.Rows[i][11];
                    sheet4.Cells[11, 7].Value = ResultTableminusFour.Rows[i][12];
                    sheet4.Cells[11, 11].Value = ResultTableminusFour.Rows[i][13];
                    sheet4.Cells[11, 15].Value = ResultTableminusFour.Rows[i][14];
                    sheet4.Cells[11, 19].Value = ResultTableminusFour.Rows[i][15];
                    sheet4.Cells[14, 3].Value = ResultTableminusFour.Rows[i][16];
                    sheet4.Cells[14, 7].Value = ResultTableminusFour.Rows[i][17];
                    sheet4.Cells[14, 11].Value = ResultTableminusFour.Rows[i][18];
                    sheet4.Cells[14, 15].Value = ResultTableminusFour.Rows[i][19];
                    sheet4.Cells[14, 19].Value = ResultTableminusFour.Rows[i][20];
                    sheet4.Cells[17, 3].Value = ResultTableminusFour.Rows[i][21];
                    sheet4.Cells[17, 7].Value = ResultTableminusFour.Rows[i][22];
                }
                if (ResultTableminusFour.Rows[i][0].ToString() == "13")//衛
                {
                    sheet4.Cells[5, 4].Value = ResultTableminusFour.Rows[i][1];
                    sheet4.Cells[5, 8].Value = ResultTableminusFour.Rows[i][2];
                    sheet4.Cells[5, 12].Value = ResultTableminusFour.Rows[i][3];
                    sheet4.Cells[5, 16].Value = ResultTableminusFour.Rows[i][4];
                    sheet4.Cells[5, 20].Value = ResultTableminusFour.Rows[i][5];
                    sheet4.Cells[8, 4].Value = ResultTableminusFour.Rows[i][6];
                    sheet4.Cells[8, 8].Value = ResultTableminusFour.Rows[i][7];
                    sheet4.Cells[8, 12].Value = ResultTableminusFour.Rows[i][8];
                    sheet4.Cells[8, 16].Value = ResultTableminusFour.Rows[i][9];
                    sheet4.Cells[8, 20].Value = ResultTableminusFour.Rows[i][10];
                    sheet4.Cells[11, 4].Value = ResultTableminusFour.Rows[i][11];
                    sheet4.Cells[11, 8].Value = ResultTableminusFour.Rows[i][12];
                    sheet4.Cells[11, 12].Value = ResultTableminusFour.Rows[i][13];
                    sheet4.Cells[11, 16].Value = ResultTableminusFour.Rows[i][14];
                    sheet4.Cells[11, 20].Value = ResultTableminusFour.Rows[i][15];
                    sheet4.Cells[14, 4].Value = ResultTableminusFour.Rows[i][16];
                    sheet4.Cells[14, 8].Value = ResultTableminusFour.Rows[i][17];
                    sheet4.Cells[14, 12].Value = ResultTableminusFour.Rows[i][18];
                    sheet4.Cells[14, 16].Value = ResultTableminusFour.Rows[i][19];
                    sheet4.Cells[14, 20].Value = ResultTableminusFour.Rows[i][20];
                    sheet4.Cells[17, 4].Value = ResultTableminusFour.Rows[i][21];
                    sheet4.Cells[17, 8].Value = ResultTableminusFour.Rows[i][22];
                }
            }
            #endregion
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;  filename=" + "縣市統計" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(excel.GetAsByteArray());
            Response.End();
        }
    }
    public void ReportCertificate()
    {
        var file = new FileInfo(Server.MapPath("~/SysFile/證書(明)管理統計報表.xlsx"));
        using (var package = new ExcelPackage(file))
        {
            // 獲取工作表
            var sheet1 = package.Workbook.Worksheets["OOO年新取證"];
            DateTime now = DateTime.Now;
            int year = now.Year - 1911; // 將西元年轉換成民國年
            int month = now.Month;
            int day = now.Day;
            string taiwanYear = year.ToString();
            string TodayDate = string.Format("{0}年{1}月{2}日", year, month, day);

            // 寫入新資料
            sheet1.Cells[1, 1].Value = taiwanYear + "年新訓取得戒菸服務人員資格證明之人數統計";
            sheet1.Cells[28, 1].Value = "備註1：上揭資料統計至"+ TodayDate;
            sheet1.Cells[29, 1].Value = "備註2：上揭資料統計為" + taiwanYear + "年新訓取得戒菸服務人員資格證明之人數，不包括屆期之更新人數。";
            // 儲存 Excel 檔案
            //package.Save();

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();

        }

    }
    public void btnExport_data()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string path = Server.MapPath("~/SysFile/證書(明)管理統計報表.xls");
        DataTable _DT = RenderFromXLS(path);
        MemoryStream ms = new MemoryStream();
        FileStream fs = new FileStream(path, FileMode.Open);
        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.GetSheet("當年新取證");
        HSSFSheet sheet2 = (HSSFSheet)hssfworkbook.GetSheet("戒菸服務人員資格證書(明)");
        HSSFSheet sheet3 = (HSSFSheet)hssfworkbook.GetSheet("戒菸師資證書");

        DateTime now = DateTime.Now;
        int year = now.Year - 1911; // 將西元年轉換成民國年
        int month = now.Month;
        int day = now.Day;
        string TodayDate = string.Format("{0}年{1}月{2}日", year, month, day);

        Dictionary<string, object> wDict = new Dictionary<string, object>();

        string taiwanYearString = (DateTime.Now.Year - 1911).ToString();
        string taiwanYearAddOne = (DateTime.Now.Year - 1910).ToString();
        string taiwanYearAddTwo = (DateTime.Now.Year - 1909).ToString();
        string taiwanYearAddThree = (DateTime.Now.Year - 1908).ToString();
        string taiwanYearAddFour = (DateTime.Now.Year - 1907).ToString();
        string taiwanYearAddFive = (DateTime.Now.Year - 1906).ToString();
        string taiwanYearAddSix = (DateTime.Now.Year - 1905).ToString();
        DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime lastDayOfYear = new DateTime(DateTime.Now.Year, 12, 31);
        DateTime firstDayOfOneYear = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
        DateTime lastDayOfOneYear = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
        DateTime firstDayOfTwoYear = new DateTime(DateTime.Now.AddYears(2).Year, 1, 1);
        DateTime lastDayOfTwoYear = new DateTime(DateTime.Now.AddYears(2).Year, 12, 31);
        DateTime firstDayOfThreeYear = new DateTime(DateTime.Now.AddYears(3).Year, 1, 1);
        DateTime lastDayOfThreeYear = new DateTime(DateTime.Now.AddYears(3).Year, 12, 31);
        DateTime firstDayOfFourYear = new DateTime(DateTime.Now.AddYears(4).Year, 1, 1);
        DateTime lastDayOfFourYear = new DateTime(DateTime.Now.AddYears(4).Year, 12, 31);
        DateTime firstDayOfFiveYear = new DateTime(DateTime.Now.AddYears(5).Year, 1, 1);
        DateTime lastDayOfFiveYear = new DateTime(DateTime.Now.AddYears(5).Year, 12, 31);
        DateTime firstDayOfSixYear = new DateTime(DateTime.Now.AddYears(6).Year, 1, 1);
        DateTime lastDayOfSixYear = new DateTime(DateTime.Now.AddYears(6).Year, 12, 31);
        string StartDate = firstDayOfYear.ToString("yyyy-MM-dd");
        string EndDate = lastDayOfYear.ToString("yyyy-MM-dd");
        string OneStartDate = firstDayOfOneYear.ToString("yyyy-MM-dd");
        string OneEndDate = lastDayOfOneYear.ToString("yyyy-MM-dd");
        string TwoStartDate = firstDayOfTwoYear.ToString("yyyy-MM-dd");
        string TwoEndDate = lastDayOfTwoYear.ToString("yyyy-MM-dd");
        string ThreeStartDate = firstDayOfThreeYear.ToString("yyyy-MM-dd");
        string ThreeEndDate = lastDayOfThreeYear.ToString("yyyy-MM-dd");
        string FourStartDate = firstDayOfFourYear.ToString("yyyy-MM-dd");
        string FourEndDate = lastDayOfFourYear.ToString("yyyy-MM-dd");
        string FiveStartDate = firstDayOfFiveYear.ToString("yyyy-MM-dd");
        string FiveEndDate = lastDayOfFiveYear.ToString("yyyy-MM-dd");
        string SixStartDate = firstDayOfSixYear.ToString("yyyy-MM-dd");
        string SixEndDate = lastDayOfSixYear.ToString("yyyy-MM-dd");
        adict.Add("OneStartDate", OneStartDate);
        adict.Add("OneEndDate", OneEndDate);
        adict.Add("TwoStartDate", TwoStartDate);
        adict.Add("TwoEndDate", TwoEndDate);
        adict.Add("ThreeStartDate", ThreeStartDate);
        adict.Add("ThreeEndDate", ThreeEndDate);
        adict.Add("FourStartDate", FourStartDate);
        adict.Add("FourEndDate", FourEndDate);
        adict.Add("FiveStartDate", FiveStartDate);
        adict.Add("FiveEndDate", FiveEndDate);
        adict.Add("SixStartDate", SixStartDate);
        adict.Add("SixEndDate", SixEndDate);
        DateTime AddOneYear = new DateTime(DateTime.Now.AddYears(1).Date.Year, 12, 31);
        DateTime AddTwoYear = new DateTime(DateTime.Now.AddYears(2).Date.Year, 12, 31);
        DateTime AddThreeYear = new DateTime(DateTime.Now.AddYears(3).Date.Year, 12, 31);
        DateTime AddFourYear = new DateTime(DateTime.Now.AddYears(4).Date.Year, 12, 31);
        DateTime AddFiveYear = new DateTime(DateTime.Now.AddYears(5).Date.Year, 12, 31);
        DateTime AddSixYear = new DateTime(DateTime.Now.AddYears(6).Date.Year, 12, 31);
        string AddOneYearstring = AddOneYear.ToString("yyyy-MM-dd");
        string AddTwoYearstring = AddTwoYear.ToString("yyyy-MM-dd");
        string AddThreeYearstring = AddThreeYear.ToString("yyyy-MM-dd");
        string AddFourYearstring = AddFourYear.ToString("yyyy-MM-dd");
        string AddFiveYearstring = AddFiveYear.ToString("yyyy-MM-dd");
        string AddSixYearstring = AddSixYear.ToString("yyyy-MM-dd");

        //標題樣式
        ICellStyle HeaderStyle = hssfworkbook.CreateCellStyle();
        IFont headfont = hssfworkbook.CreateFont();
        headfont.FontName = "微軟正黑體";
        headfont.FontHeightInPoints = 14;
        headfont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
        HeaderStyle.Alignment = HorizontalAlignment.Center; //水平置中
        HeaderStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        HeaderStyle.SetFont(headfont);

        ICellStyle FirstStyle = hssfworkbook.CreateCellStyle();
        IFont Firstfont = hssfworkbook.CreateFont();
        Firstfont.FontName = "微軟正黑體";
        Firstfont.FontHeightInPoints = 12;
        Firstfont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
        FirstStyle.Alignment = HorizontalAlignment.Center; //水平置中
        FirstStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        FirstStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        FirstStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        FirstStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        FirstStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        FirstStyle.SetFont(Firstfont);

        ICellStyle DataStyle = hssfworkbook.CreateCellStyle();
        IFont datafont = hssfworkbook.CreateFont();
        datafont.FontName = "微軟正黑體";
        datafont.FontHeightInPoints = 12;
        datafont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
        DataStyle.SetFont(datafont);

        ICellStyle BackStyle = hssfworkbook.CreateCellStyle();
        IFont backfont = hssfworkbook.CreateFont();
        backfont.FontName = "微軟正黑體";
        backfont.FontHeightInPoints = 14;
        backfont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
        BackStyle.Alignment = HorizontalAlignment.Left; //水平置中
        BackStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        BackStyle.SetFont(backfont);

        #region 第一頁
        sheet1.GetRow(0).CreateCell(0).SetCellValue(taiwanYearString + "年新訓取得戒菸服務人員資格證明之人數統計");
        sheet1.GetRow(27).CreateCell(0).SetCellValue("備註1：上揭資料統計至"+ TodayDate + "止。");
        sheet1.GetRow(28).CreateCell(0).SetCellValue("備註2：上揭資料統計為" + taiwanYearString + "年新訓取得戒菸服務人員資格證明之人數，不包括屆期之更新人數。");
        sheet1.GetRow(0).GetCell(0).CellStyle = HeaderStyle;
        sheet1.GetRow(27).GetCell(0).CellStyle = DataStyle;
        sheet1.GetRow(28).GetCell(0).CellStyle = DataStyle;

        string sql = @"select RoleSNO,count(1)count from QS_Certificate　QC
left join Person P on QC.PersonID = P.PersonID
where CTypeSNO = 75 and CertPublicDate>= @StartDate  and CertPublicDate<= @EndDate
group by RoleSNO";//各身分別今年取得新證明人數
        adict.Add("StartDate", StartDate);
        adict.Add("EndDate", EndDate);
        DataTable ObjDt = ObjDH.queryData(sql, adict);
        sheet1.GetRow(3).CreateCell(1).SetCellValue(ObjDt.Rows[0][1].ToString());
        sheet1.GetRow(3).CreateCell(2).SetCellValue(ObjDt.Rows[1][1].ToString());
        sheet1.GetRow(3).CreateCell(3).SetCellValue(ObjDt.Rows[2][1].ToString());
        sheet1.GetRow(3).CreateCell(4).SetCellValue(ObjDt.Rows[3][1].ToString());

        string sqlCity = @"select* from(
select CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME,RoleSNO,QC.PersonID　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO=75 and CertPublicDate>=@StartDate  and CertPublicDate<=@EndDate
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";//各身分別今年取得新證明人數(執業登記縣市)
        DataTable CityDt = ObjDH.queryData(sqlCity, adict);
        sheet1.GetRow(4).CreateCell(1).SetCellValue(CityDt.Rows[0][1].ToString());
        sheet1.GetRow(4).CreateCell(2).SetCellValue(CityDt.Rows[1][1].ToString());
        sheet1.GetRow(4).CreateCell(3).SetCellValue(CityDt.Rows[2][1].ToString());
        sheet1.GetRow(4).CreateCell(4).SetCellValue(CityDt.Rows[3][1].ToString());
        sheet1.GetRow(5).CreateCell(1).SetCellValue(CityDt.Rows[0][2].ToString());
        sheet1.GetRow(5).CreateCell(2).SetCellValue(CityDt.Rows[1][2].ToString());
        sheet1.GetRow(5).CreateCell(3).SetCellValue(CityDt.Rows[2][2].ToString());
        sheet1.GetRow(5).CreateCell(4).SetCellValue(CityDt.Rows[3][2].ToString());
        sheet1.GetRow(6).CreateCell(1).SetCellValue(CityDt.Rows[0][3].ToString());
        sheet1.GetRow(6).CreateCell(2).SetCellValue(CityDt.Rows[1][3].ToString());
        sheet1.GetRow(6).CreateCell(3).SetCellValue(CityDt.Rows[2][3].ToString());
        sheet1.GetRow(6).CreateCell(4).SetCellValue(CityDt.Rows[3][3].ToString());
        sheet1.GetRow(7).CreateCell(1).SetCellValue(CityDt.Rows[0][4].ToString());
        sheet1.GetRow(7).CreateCell(2).SetCellValue(CityDt.Rows[1][4].ToString());
        sheet1.GetRow(7).CreateCell(3).SetCellValue(CityDt.Rows[2][4].ToString());
        sheet1.GetRow(7).CreateCell(4).SetCellValue(CityDt.Rows[3][4].ToString());
        sheet1.GetRow(8).CreateCell(1).SetCellValue(CityDt.Rows[0][5].ToString());
        sheet1.GetRow(8).CreateCell(2).SetCellValue(CityDt.Rows[1][5].ToString());
        sheet1.GetRow(8).CreateCell(3).SetCellValue(CityDt.Rows[2][5].ToString());
        sheet1.GetRow(8).CreateCell(4).SetCellValue(CityDt.Rows[3][5].ToString());
        sheet1.GetRow(9).CreateCell(1).SetCellValue(CityDt.Rows[0][6].ToString());
        sheet1.GetRow(9).CreateCell(2).SetCellValue(CityDt.Rows[1][6].ToString());
        sheet1.GetRow(9).CreateCell(3).SetCellValue(CityDt.Rows[2][6].ToString());
        sheet1.GetRow(9).CreateCell(4).SetCellValue(CityDt.Rows[3][6].ToString());
        sheet1.GetRow(10).CreateCell(1).SetCellValue(CityDt.Rows[0][7].ToString());
        sheet1.GetRow(10).CreateCell(2).SetCellValue(CityDt.Rows[1][7].ToString());
        sheet1.GetRow(10).CreateCell(3).SetCellValue(CityDt.Rows[2][7].ToString());
        sheet1.GetRow(10).CreateCell(4).SetCellValue(CityDt.Rows[3][7].ToString());
        sheet1.GetRow(11).CreateCell(1).SetCellValue(CityDt.Rows[0][8].ToString());
        sheet1.GetRow(11).CreateCell(2).SetCellValue(CityDt.Rows[1][8].ToString());
        sheet1.GetRow(11).CreateCell(3).SetCellValue(CityDt.Rows[2][8].ToString());
        sheet1.GetRow(11).CreateCell(4).SetCellValue(CityDt.Rows[3][8].ToString());
        sheet1.GetRow(12).CreateCell(1).SetCellValue(CityDt.Rows[0][9].ToString());
        sheet1.GetRow(12).CreateCell(2).SetCellValue(CityDt.Rows[1][9].ToString());
        sheet1.GetRow(12).CreateCell(3).SetCellValue(CityDt.Rows[2][9].ToString());
        sheet1.GetRow(12).CreateCell(4).SetCellValue(CityDt.Rows[3][9].ToString());
        sheet1.GetRow(13).CreateCell(1).SetCellValue(CityDt.Rows[0][10].ToString());
        sheet1.GetRow(13).CreateCell(2).SetCellValue(CityDt.Rows[1][10].ToString());
        sheet1.GetRow(13).CreateCell(3).SetCellValue(CityDt.Rows[2][10].ToString());
        sheet1.GetRow(13).CreateCell(4).SetCellValue(CityDt.Rows[3][10].ToString());
        sheet1.GetRow(14).CreateCell(1).SetCellValue(CityDt.Rows[0][11].ToString());
        sheet1.GetRow(14).CreateCell(2).SetCellValue(CityDt.Rows[1][11].ToString());
        sheet1.GetRow(14).CreateCell(3).SetCellValue(CityDt.Rows[2][11].ToString());
        sheet1.GetRow(14).CreateCell(4).SetCellValue(CityDt.Rows[3][11].ToString());
        sheet1.GetRow(15).CreateCell(1).SetCellValue(CityDt.Rows[0][12].ToString());
        sheet1.GetRow(15).CreateCell(2).SetCellValue(CityDt.Rows[1][12].ToString());
        sheet1.GetRow(15).CreateCell(3).SetCellValue(CityDt.Rows[2][12].ToString());
        sheet1.GetRow(15).CreateCell(4).SetCellValue(CityDt.Rows[3][12].ToString());
        sheet1.GetRow(16).CreateCell(1).SetCellValue(CityDt.Rows[0][13].ToString());
        sheet1.GetRow(16).CreateCell(2).SetCellValue(CityDt.Rows[1][13].ToString());
        sheet1.GetRow(16).CreateCell(3).SetCellValue(CityDt.Rows[2][13].ToString());
        sheet1.GetRow(16).CreateCell(4).SetCellValue(CityDt.Rows[3][13].ToString());
        sheet1.GetRow(17).CreateCell(1).SetCellValue(CityDt.Rows[0][14].ToString());
        sheet1.GetRow(17).CreateCell(2).SetCellValue(CityDt.Rows[1][14].ToString());
        sheet1.GetRow(17).CreateCell(3).SetCellValue(CityDt.Rows[2][14].ToString());
        sheet1.GetRow(17).CreateCell(4).SetCellValue(CityDt.Rows[3][14].ToString());
        sheet1.GetRow(18).CreateCell(1).SetCellValue(CityDt.Rows[0][15].ToString());
        sheet1.GetRow(18).CreateCell(2).SetCellValue(CityDt.Rows[1][15].ToString());
        sheet1.GetRow(18).CreateCell(3).SetCellValue(CityDt.Rows[2][15].ToString());
        sheet1.GetRow(18).CreateCell(4).SetCellValue(CityDt.Rows[3][15].ToString());
        sheet1.GetRow(19).CreateCell(1).SetCellValue(CityDt.Rows[0][16].ToString());
        sheet1.GetRow(19).CreateCell(2).SetCellValue(CityDt.Rows[1][16].ToString());
        sheet1.GetRow(19).CreateCell(3).SetCellValue(CityDt.Rows[2][16].ToString());
        sheet1.GetRow(19).CreateCell(4).SetCellValue(CityDt.Rows[3][16].ToString());
        sheet1.GetRow(20).CreateCell(1).SetCellValue(CityDt.Rows[0][17].ToString());
        sheet1.GetRow(20).CreateCell(2).SetCellValue(CityDt.Rows[1][17].ToString());
        sheet1.GetRow(20).CreateCell(3).SetCellValue(CityDt.Rows[2][17].ToString());
        sheet1.GetRow(20).CreateCell(4).SetCellValue(CityDt.Rows[3][17].ToString());
        sheet1.GetRow(21).CreateCell(1).SetCellValue(CityDt.Rows[0][18].ToString());
        sheet1.GetRow(21).CreateCell(2).SetCellValue(CityDt.Rows[1][18].ToString());
        sheet1.GetRow(21).CreateCell(3).SetCellValue(CityDt.Rows[2][18].ToString());
        sheet1.GetRow(21).CreateCell(4).SetCellValue(CityDt.Rows[3][18].ToString());
        sheet1.GetRow(22).CreateCell(1).SetCellValue(CityDt.Rows[0][19].ToString());
        sheet1.GetRow(22).CreateCell(2).SetCellValue(CityDt.Rows[1][19].ToString());
        sheet1.GetRow(22).CreateCell(3).SetCellValue(CityDt.Rows[2][19].ToString());
        sheet1.GetRow(22).CreateCell(4).SetCellValue(CityDt.Rows[3][19].ToString());
        sheet1.GetRow(23).CreateCell(1).SetCellValue(CityDt.Rows[0][20].ToString());
        sheet1.GetRow(23).CreateCell(2).SetCellValue(CityDt.Rows[1][20].ToString());
        sheet1.GetRow(23).CreateCell(3).SetCellValue(CityDt.Rows[2][20].ToString());
        sheet1.GetRow(23).CreateCell(4).SetCellValue(CityDt.Rows[3][20].ToString());
        sheet1.GetRow(24).CreateCell(1).SetCellValue(CityDt.Rows[0][21].ToString());
        sheet1.GetRow(24).CreateCell(2).SetCellValue(CityDt.Rows[1][21].ToString());
        sheet1.GetRow(24).CreateCell(3).SetCellValue(CityDt.Rows[2][21].ToString());
        sheet1.GetRow(24).CreateCell(4).SetCellValue(CityDt.Rows[3][21].ToString());
        sheet1.GetRow(25).CreateCell(1).SetCellValue(CityDt.Rows[0][22].ToString());
        sheet1.GetRow(25).CreateCell(2).SetCellValue(CityDt.Rows[1][22].ToString());
        sheet1.GetRow(25).CreateCell(3).SetCellValue(CityDt.Rows[2][22].ToString());
        sheet1.GetRow(25).CreateCell(4).SetCellValue(CityDt.Rows[3][22].ToString());
        sheet1.GetRow(26).CreateCell(1).SetCellValue(CityDt.Rows[0][23].ToString());
        sheet1.GetRow(26).CreateCell(2).SetCellValue(CityDt.Rows[1][23].ToString());
        sheet1.GetRow(26).CreateCell(3).SetCellValue(CityDt.Rows[2][23].ToString());
        sheet1.GetRow(26).CreateCell(4).SetCellValue(CityDt.Rows[3][23].ToString());

        sheet1.GetRow(3).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(3).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(3).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(3).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(4).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(4).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(4).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(4).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(5).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(5).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(5).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(5).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(6).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(6).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(6).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(6).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(7).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(7).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(7).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(7).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(8).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(8).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(8).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(8).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(9).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(9).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(9).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(9).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(10).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(10).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(10).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(10).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(11).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(11).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(11).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(11).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(12).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(12).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(12).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(12).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(13).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(13).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(13).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(13).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(14).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(14).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(14).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(14).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(15).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(15).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(15).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(15).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(16).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(16).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(16).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(16).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(17).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(17).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(17).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(17).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(18).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(18).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(18).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(18).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(19).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(19).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(19).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(19).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(20).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(20).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(20).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(20).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(21).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(21).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(21).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(21).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(22).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(22).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(22).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(22).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(23).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(23).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(23).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(23).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(24).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(24).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(24).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(24).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(25).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(25).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(25).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(25).GetCell(4).CellStyle = FirstStyle;
        sheet1.GetRow(26).GetCell(1).CellStyle = FirstStyle;
        sheet1.GetRow(26).GetCell(2).CellStyle = FirstStyle;
        sheet1.GetRow(26).GetCell(3).CellStyle = FirstStyle;
        sheet1.GetRow(26).GetCell(4).CellStyle = FirstStyle;

        #endregion
        #region 第二頁
        sheet2.GetRow(2).CreateCell(0).SetCellValue(taiwanYearString + "年證書(明)");
        sheet2.GetRow(9).CreateCell(0).SetCellValue(taiwanYearAddOne + "年證書(明)屆期人數");
        sheet2.GetRow(10).CreateCell(0).SetCellValue(taiwanYearAddTwo + "年證書(明)屆期人數");
        sheet2.GetRow(11).CreateCell(0).SetCellValue(taiwanYearAddThree + "年證書(明)屆期人數");
        sheet2.GetRow(12).CreateCell(0).SetCellValue(taiwanYearAddFour + "年證書(明)屆期人數");
        sheet2.GetRow(13).CreateCell(0).SetCellValue(taiwanYearAddFive + "年證書(明)屆期人數");
        sheet2.GetRow(14).CreateCell(0).SetCellValue(taiwanYearAddSix + "年證書(明)屆期人數");
        sheet2.GetRow(39).CreateCell(0).SetCellValue("備註1：上揭資料統計至"+ TodayDate + "止");
        sheet2.GetRow(39).GetCell(0).CellStyle = BackStyle;

        string sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=10";
        DataTable CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(9).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());
        sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=10";
        CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(10).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());
        sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=10";
        CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(11).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());
        sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=10";
        CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(12).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());
        sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=10";
        CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(13).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());
        sqlCSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=10";
        CSixYearsDt = ObjDH.queryData(sqlCSixYears, adict);
        sheet2.GetRow(14).CreateCell(2).SetCellValue(CSixYearsDt.Rows.Count.ToString());

        string sqlCCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO  in(1,53) and CertEndDate>=Getdate() and CertEndDate<=@EndDate  and P.rolesno=10
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable CCityDt = ObjDH.queryData(sqlCCity, adict);
        if (CCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(2).SetCellValue(CCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(2).SetCellValue(CCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(2).SetCellValue(CCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(2).SetCellValue(CCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(2).SetCellValue(CCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(2).SetCellValue(CCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(2).SetCellValue(CCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(2).SetCellValue(CCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(2).SetCellValue(CCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(2).SetCellValue(CCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(2).SetCellValue(CCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(2).SetCellValue(CCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(2).SetCellValue(CCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(2).SetCellValue(CCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(2).SetCellValue(CCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(2).SetCellValue(CCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(2).SetCellValue(CCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(2).SetCellValue(CCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(2).SetCellValue(CCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(2).SetCellValue(CCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(2).SetCellValue(CCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(2).SetCellValue(CCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(2).SetCellValue(CCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(2).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(2).SetCellValue("0");
        }      

        string sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=10";
        DataTable DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(9).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());
        sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=10";
        DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(10).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());
        sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=10";
        DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(11).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());
        sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=10";
        DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(12).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());
        sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=10";
        DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(13).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());
        sqlDSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=10";
        DSixYearsDt = ObjDH.queryData(sqlDSixYears, adict);
        sheet2.GetRow(14).CreateCell(3).SetCellValue(DSixYearsDt.Rows.Count.ToString());

        string sqlDCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO  in(3,55) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=10
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable DCityDt = ObjDH.queryData(sqlDCity, adict);
        if (DCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(3).SetCellValue(DCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(3).SetCellValue(DCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(3).SetCellValue(DCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(3).SetCellValue(DCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(3).SetCellValue(DCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(3).SetCellValue(DCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(3).SetCellValue(DCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(3).SetCellValue(DCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(3).SetCellValue(DCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(3).SetCellValue(DCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(3).SetCellValue(DCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(3).SetCellValue(DCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(3).SetCellValue(DCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(3).SetCellValue(DCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(3).SetCellValue(DCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(3).SetCellValue(DCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(3).SetCellValue(DCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(3).SetCellValue(DCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(3).SetCellValue(DCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(3).SetCellValue(DCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(3).SetCellValue(DCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(3).SetCellValue(DCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(3).SetCellValue(DCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(3).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(3).SetCellValue("0");
        }
        

        string sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=10";
        DataTable ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(9).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());
        sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=10";
        ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(10).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());
        sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=10";
        ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(11).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());
        sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=10";
        ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(12).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());
        sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=10";
        ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(13).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());
        sqlESixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=10";
        ESixYearsDt = ObjDH.queryData(sqlESixYears, adict);
        sheet2.GetRow(14).CreateCell(4).SetCellValue(ESixYearsDt.Rows.Count.ToString());

        string sqlECity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO  in(4,5,51) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=10
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable ECityDt = ObjDH.queryData(sqlECity, adict);
        if (ECityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(4).SetCellValue(ECityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(4).SetCellValue(ECityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(4).SetCellValue(ECityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(4).SetCellValue(ECityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(4).SetCellValue(ECityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(4).SetCellValue(ECityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(4).SetCellValue(ECityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(4).SetCellValue(ECityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(4).SetCellValue(ECityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(4).SetCellValue(ECityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(4).SetCellValue(ECityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(4).SetCellValue(ECityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(4).SetCellValue(ECityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(4).SetCellValue(ECityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(4).SetCellValue(ECityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(4).SetCellValue(ECityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(4).SetCellValue(ECityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(4).SetCellValue(ECityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(4).SetCellValue(ECityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(4).SetCellValue(ECityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(4).SetCellValue(ECityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(4).SetCellValue(ECityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(4).SetCellValue(ECityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(4).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(4).SetCellValue("0");
        }
        

        string sqlC4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//醫師戒菸治療證書屆期已更新人數
        DataTable CisChangeDt = ObjDH.queryData(sqlC4, adict);
        int C4count = CisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(2).SetCellValue(C4count);
        string sqlC5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//醫師戒菸治療證書屆期未更新人數
        DataTable CisUnChangeDt = ObjDH.queryData(sqlC5, adict);
        int C5count = CisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(2).SetCellValue(C5count);
        int C3count = C4count + C5count;
        sheet2.GetRow(2).CreateCell(2).SetCellValue(C3count);
        float Cpercentage = 0;
        if (C3count > 0)
        {
            Cpercentage = (float)C4count / C3count * 100;
        }
        sheet2.GetRow(5).CreateCell(2).SetCellValue(Cpercentage.ToString("0.00") + "%");
        string sqlD4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期已更新人數
        DataTable DisChangeDt = ObjDH.queryData(sqlD4, adict);
        int D4count = DisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(3).SetCellValue(D4count);
        string sqlD5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//醫師戒菸衛教證書屆期未更新人數
        DataTable DisUnChangeDt = ObjDH.queryData(sqlD5, adict);
        int D5count = DisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(3).SetCellValue(D5count);
        int D3count = D4count + D5count;
        sheet2.GetRow(2).CreateCell(3).SetCellValue(D3count);
        float Dpercentage = 0;
        if (D3count > 0)
        {
            Dpercentage = (float)D4count / D3count * 100;
        }
        sheet2.GetRow(5).CreateCell(3).SetCellValue(Dpercentage.ToString("0.00") + "%");
        string sqlE4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期已更新人數
        DataTable EisChangeDt = ObjDH.queryData(sqlE4, adict);
        int E4count = EisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(4).SetCellValue(E4count);
        string sqlE5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//醫師戒菸衛教證書屆期未更新人數
        DataTable EisUnChangeDt = ObjDH.queryData(sqlE5, adict);
        int E5count = EisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(4).SetCellValue(E5count);
        int E3count = E4count + E5count;
        sheet2.GetRow(2).CreateCell(4).SetCellValue(E3count);
        float Epercentage = 0;
        if (E3count > 0)
        {
            Epercentage = (float)E4count / E3count * 100;
        }
        sheet2.GetRow(5).CreateCell(4).SetCellValue(Epercentage.ToString("0.00") + "%");
        string sqlF4 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=10";//醫師戒菸衛教證書屆期已更新人數
        DataTable FisChangeDt = ObjDH.queryData(sqlF4, adict);
        int F4count = FisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(5).SetCellValue(F4count);
        string sqlF5 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=10";//醫師戒菸衛教證書屆期未更新人數
        DataTable FisUnChangeDt = ObjDH.queryData(sqlF5, adict);
        int F5count = FisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(5).SetCellValue(F5count);
        int F3count = F4count + F5count;
        sheet2.GetRow(2).CreateCell(5).SetCellValue(F3count);
        float Fpercentage = 0;
        if (F3count > 0)
        {
            Fpercentage = (float)F4count / F3count * 100;
        }
        sheet2.GetRow(5).CreateCell(5).SetCellValue(Fpercentage.ToString("0.00") + "%");
        string sqlG4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//牙醫師戒菸治療證書屆期已更新人數
        DataTable GisChangeDt = ObjDH.queryData(sqlG4, adict);
        int G4count = GisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(6).SetCellValue(G4count);
        string sqlG5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//牙醫師戒菸治療證書屆期未更新人數
        DataTable GisUnChangeDt = ObjDH.queryData(sqlG5, adict);
        int G5count = GisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(6).SetCellValue(G5count);
        int G3count = G4count + G5count;
        sheet2.GetRow(2).CreateCell(6).SetCellValue(G3count);
        float Gpercentage = 0;
        if (G3count > 0)
        {
            Gpercentage = (float)G4count / G3count * 100;
        }
        sheet2.GetRow(5).CreateCell(6).SetCellValue(Gpercentage.ToString("0.00") + "%");
        string sqlH4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//牙醫師戒菸衛教證書屆期已更新人數
        DataTable HisChangeDt = ObjDH.queryData(sqlH4, adict);
        int H4count = HisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(7).SetCellValue(H4count);
        string sqlH5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//牙醫師戒菸衛教證書屆期未更新人數
        DataTable HisUnChangeDt = ObjDH.queryData(sqlH5, adict);
        int H5count = HisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(7).SetCellValue(H5count);
        int H3count = H4count + H5count;
        sheet2.GetRow(2).CreateCell(7).SetCellValue(H3count);
        float Hpercentage = 0;
        if (H3count > 0)
        {
            Hpercentage = (float)H4count / H3count * 100;
        }
        sheet2.GetRow(5).CreateCell(7).SetCellValue(Hpercentage.ToString("0.00") + "%");
        string sqlI4 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=11";//牙醫師戒戒菸服務人員資格證明屆期已更新人數
        DataTable IisChangeDt = ObjDH.queryData(sqlI4, adict);
        int I4count = IisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(8).SetCellValue(I4count);
        string sqlI5 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=11";//牙醫師戒菸服務人員資格證明屆期未更新人數
        DataTable IisUnChangeDt = ObjDH.queryData(sqlI5, adict);
        int I5count = IisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(8).SetCellValue(I5count);
        int I3count = I4count + I5count;
        sheet2.GetRow(2).CreateCell(8).SetCellValue(I3count);
        float Ipercentage = 0;
        if (I3count > 0)
        {
            Ipercentage = (float)I4count / I3count * 100;
        }
        sheet2.GetRow(5).CreateCell(8).SetCellValue(Ipercentage.ToString("0.00") + "%");
        string sqlJ4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52) and QC.CertEndDate=@EndDate  and P.rolesno=12),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=12)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//藥師戒菸衛教證書屆期已更新人數
        DataTable JisChangeDt = ObjDH.queryData(sqlJ4, adict);
        int J4count = JisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(9).SetCellValue(J4count);
        string sqlJ5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52) and QC.CertEndDate=@EndDate  and P.rolesno=12),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=12)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//藥師戒菸衛教證書屆期未更新人數
        DataTable JisUnChangeDt = ObjDH.queryData(sqlJ5, adict);
        int J5count = JisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(9).SetCellValue(J5count);
        int J3count = J4count + J5count;
        sheet2.GetRow(2).CreateCell(9).SetCellValue(J3count);
        float Jpercentage = 0;
        if (J3count > 0)
        {
            Jpercentage = (float)J4count / J3count * 100;
        }
        sheet2.GetRow(5).CreateCell(9).SetCellValue(Jpercentage.ToString("0.00") + "%");
        string sqlK4 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=12";//藥師戒菸服務人員資格證明屆期已更新人數
        DataTable KisChangeDt = ObjDH.queryData(sqlK4, adict);
        int K4count = KisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(10).SetCellValue(K4count);
        string sqlK5 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=12";//藥師戒菸服務人員資格證明屆期未更新人數
        DataTable KisUnChangeDt = ObjDH.queryData(sqlK5, adict);
        int K5count = KisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(10).SetCellValue(K5count);
        int K3count = K4count + K5count;
        sheet2.GetRow(2).CreateCell(10).SetCellValue(K3count);
        float Kpercentage = 0;
        if (K3count > 0)
        {
             Kpercentage = (float)K4count / K3count * 100;
        }
        sheet2.GetRow(5).CreateCell(10).SetCellValue(K3count.ToString("0.00") + "%");
        string sqlL4 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=13),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=13)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is　not null";//衛教師戒菸衛教證書屆期已更新人數
        DataTable LisChangeDt = ObjDH.queryData(sqlL4, adict);
        int L4count = LisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(11).SetCellValue(L4count);
        string sqlL5 = @"with oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=13),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=13)
select * from oldCertificate left join newCertificate
on oldCertificate.PersonID=newCertificate.PersonID　where newCertificate.PersonID is null";//衛教師戒菸衛教證書屆期未更新人數
        DataTable LisUnChangeDt = ObjDH.queryData(sqlL5, adict);
        int L5count = LisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(11).SetCellValue(L5count);
        int L3count = L4count + L5count;
        sheet2.GetRow(2).CreateCell(11).SetCellValue(L3count);
        float Lpercentage = 0;
        if (L3count > 0)
        {
            Lpercentage = (float)L4count / L3count * 100;
        }
        sheet2.GetRow(5).CreateCell(11).SetCellValue(Lpercentage.ToString("0.00") + "%");
        string sqlM4 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=13";//衛教師戒菸服務人員資格證明屆期已更新人數
        DataTable MisChangeDt = ObjDH.queryData(sqlM4, adict);
        int M4count = MisChangeDt.Rows.Count;
        sheet2.GetRow(3).CreateCell(12).SetCellValue(M4count);
        string sqlM5 = @"select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=13";//衛教師戒菸服務人員資格證明屆期未更新人數
        DataTable MisUnChangeDt = ObjDH.queryData(sqlM5, adict);
        int M5count = MisUnChangeDt.Rows.Count;
        sheet2.GetRow(4).CreateCell(12).SetCellValue(M5count);
        int M3count = M4count + M5count;
        sheet2.GetRow(2).CreateCell(12).SetCellValue(M3count);
        float Mpercentage = 0;
        if (M3count > 0)
        {
            Mpercentage = (float)M4count / M3count * 100;
        }
        sheet2.GetRow(5).CreateCell(12).SetCellValue(Mpercentage.ToString("0.00") + "%");

        #region 合約人員
        string PrsnsqlC4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　oldCertificate　left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//醫師戒菸治療證書屆期已更新人數
        DataTable PrsnCisChangeDt = ObjDH.queryData(PrsnsqlC4, adict);
        int PrsnC4count = PrsnCisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(2).SetCellValue(PrsnC4count);
        string PrsnsqlC5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID),
oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(1,53) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　 oldCertificate　left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//醫師戒菸治療證書屆期未更新人數
        DataTable PrsnCisUnChangeDt = ObjDH.queryData(PrsnsqlC5, adict);
        int PrsnC5count = PrsnCisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(2).SetCellValue(PrsnC5count);
        int PrsnC3count = PrsnC4count + PrsnC5count;
        sheet2.GetRow(6).CreateCell(2).SetCellValue(PrsnC3count);
        string PrsnsqlD4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期已更新人數
        DataTable PrsnDisChangeDt = ObjDH.queryData(PrsnsqlD4, adict);
        int PrsnD4count = PrsnDisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(3).SetCellValue(PrsnD4count);
        string PrsnsqlD5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//醫師戒菸衛教證書屆期未更新人數
        DataTable PrsnDisUnChangeDt = ObjDH.queryData(PrsnsqlD5, adict);
        int PrsnD5count = PrsnDisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(3).SetCellValue(PrsnD5count);
        int PrsnD3count = PrsnD4count + PrsnD5count;
        sheet2.GetRow(6).CreateCell(3).SetCellValue(PrsnD3count);
        string PrsnsqlE4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期已更新人數
        DataTable PrsnEisChangeDt = ObjDH.queryData(PrsnsqlE4, adict);
        int PrsnE4count = PrsnEisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(4).SetCellValue(PrsnE4count);
        string PrsnsqlE5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=10),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=10)
select * from　　oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//醫師戒菸衛教證書屆期未更新人數
        DataTable PrsnEisUnChangeDt = ObjDH.queryData(PrsnsqlE5, adict);
        int PrsnE5count = PrsnEisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(4).SetCellValue(PrsnE5count);
        int PrsnE3count = PrsnE4count + PrsnE5count;
        sheet2.GetRow(6).CreateCell(4).SetCellValue(PrsnE3count);
        string PrsnsqlF4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=10)
select * from　newCertificate　left join allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期已更新人數
        DataTable PrsnFisChangeDt = ObjDH.queryData(PrsnsqlF4, adict);
        int PrsnF4count = PrsnFisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(5).SetCellValue(PrsnF4count);
        string PrsnsqlF5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=10)
select * from　newCertificate　left join allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//醫師戒菸衛教證書屆期未更新人數
        DataTable PrsnFisUnChangeDt = ObjDH.queryData(PrsnsqlF5, adict);
        int PrsnF5count = PrsnFisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(5).SetCellValue(PrsnF5count);
        int PrsnF3count = PrsnF4count + PrsnF5count;
        sheet2.GetRow(6).CreateCell(5).SetCellValue(PrsnF3count);
        string PrsnsqlG4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from　　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//牙醫師戒菸治療證書屆期已更新人數
        DataTable PrsnGisChangeDt = ObjDH.queryData(PrsnsqlG4, adict);
        int PrsnG4count = PrsnGisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(6).SetCellValue(PrsnG4count);
        string PrsnsqlG5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from　　oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//牙醫師戒菸治療證書屆期未更新人數
        DataTable PrsnGisUnChangeDt = ObjDH.queryData(PrsnsqlG5, adict);
        int PrsnG5count = PrsnGisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(6).SetCellValue(PrsnG5count);
        int PrsnG3count = PrsnG4count + PrsnG5count;
        sheet2.GetRow(6).CreateCell(6).SetCellValue(PrsnG3count);
        string PrsnsqlH4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from　　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//牙醫師戒菸衛教證書屆期已更新人數
        DataTable PrsnHisChangeDt = ObjDH.queryData(PrsnsqlH4, adict);
        int PrsnH4count = PrsnHisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(7).SetCellValue(PrsnH4count);
        string PrsnsqlH5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and QC.CertEndDate=@EndDate  and P.rolesno=11),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=11)
select * from　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join　allPrsn　on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//牙醫師戒菸衛教證書屆期未更新人數
        DataTable PrsnHisUnChangeDt = ObjDH.queryData(PrsnsqlH5, adict);
        int PrsnH5count = PrsnHisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(7).SetCellValue(PrsnH5count);
        int PrsnH3count = PrsnH4count + PrsnH5count;
        sheet2.GetRow(6).CreateCell(7).SetCellValue(PrsnH3count);

        string PrsnsqlI4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=11)
select * from　newCertificate　left join 　allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//牙醫師戒戒菸服務人員資格證明屆期已更新人數
        DataTable PrsnIisChangeDt = ObjDH.queryData(PrsnsqlI4, adict);
        int PrsnI4count = PrsnIisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(8).SetCellValue(PrsnI4count);
        string PrsnsqlI5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=11)
select * from　newCertificate　left join allPrsn 　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//牙醫師戒菸服務人員資格證明屆期未更新人數
        DataTable PrsnIisUnChangeDt = ObjDH.queryData(PrsnsqlI5, adict);
        int PrsnI5count = PrsnIisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(8).SetCellValue(PrsnI5count);
        int PrsnI3count = PrsnI4count + PrsnI5count;
        sheet2.GetRow(6).CreateCell(8).SetCellValue(PrsnI3count);
        string PrsnsqlJ4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52) and QC.CertEndDate=@EndDate  and P.rolesno=12),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=12)
select * from　　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID
left join allPrsn on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//藥師戒菸衛教證書屆期已更新人數
        DataTable PrsnJisChangeDt = ObjDH.queryData(PrsnsqlJ4, adict);
        int PrsnJ4count = PrsnJisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(9).SetCellValue(PrsnJ4count);
        string PrsnsqlJ5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52) and QC.CertEndDate=@EndDate  and P.rolesno=12),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=12)
select * from　　 oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//藥師戒菸衛教證書屆期未更新人數
        DataTable PrsnJisUnChangeDt = ObjDH.queryData(PrsnsqlJ5, adict);
        int PrsnJ5count = PrsnJisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(9).SetCellValue(PrsnJ5count);
        int PrsnJ3count = PrsnJ4count + PrsnJ5count;
        sheet2.GetRow(6).CreateCell(9).SetCellValue(PrsnJ3count);
        string PrsnsqlK4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=12)
select * from　newCertificate　left join  allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//藥師戒菸服務人員資格證明屆期已更新人數
        DataTable PrsnKisChangeDt = ObjDH.queryData(PrsnsqlK4, adict);
        int PrsnK4count = PrsnKisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(10).SetCellValue(PrsnK4count);
        string PrsnsqlK5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=12)
select * from　newCertificate　left join allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//藥師戒菸服務人員資格證明屆期未更新人數
        DataTable PrsnKisUnChangeDt = ObjDH.queryData(PrsnsqlK5, adict);
        int PrsnK5count = PrsnKisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(10).SetCellValue(PrsnK5count);
        int PrsnK3count = PrsnK4count + PrsnK5count;
        sheet2.GetRow(6).CreateCell(10).SetCellValue(PrsnK3count);
        string PrsnsqlL4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=13),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=13)
select * from　　oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is　not null";//衛教師戒菸衛教證書屆期已更新人數
        DataTable PrsnLisChangeDt = ObjDH.queryData(PrsnsqlL4, adict);
        int PrsnL4count = PrsnLisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(11).SetCellValue(PrsnL4count);
        string PrsnsqlL5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,oldCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and QC.CertEndDate=@EndDate  and P.rolesno=13),
newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=getdate()  and P.rolesno=13)
select * from　　oldCertificate　
left join newCertificate　on oldCertificate.PersonID=newCertificate.PersonID　
left join allPrsn on allPrsn.PrsnID=oldCertificate.PersonID
where newCertificate.PersonID is null";//衛教師戒菸衛教證書屆期未更新人數
        DataTable PrsnLisUnChangeDt = ObjDH.queryData(PrsnsqlL5, adict);
        int PrsnL5count = PrsnLisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(11).SetCellValue(PrsnL5count);
        int PrsnL3count = PrsnL4count + PrsnL5count;
        sheet2.GetRow(6).CreateCell(11).SetCellValue(PrsnL3count);
        string PrsnsqlM4 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.ModifyDT>=@StartDate and QC.ModifyDT<=@EndDate and P.rolesno=13)
select * from　newCertificate　left join allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//衛教師戒菸服務人員資格證明屆期已更新人數
        DataTable PrsnMisChangeDt = ObjDH.queryData(PrsnsqlM4, adict);
        int PrsnM4count = PrsnMisChangeDt.Rows.Count;
        sheet2.GetRow(7).CreateCell(12).SetCellValue(PrsnM4count);
        string PrsnsqlM5 = @"with allPrsn　as(select PrsnID,max(PrsnEndDate)PrsnEndDate from PrsnContract where PrsnEndDate is null or PrsnEndDate=''　 group by PrsnID)
,newCertificate as(select QC.PersonID 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and QC.CertEndDate>=@StartDate and QC.CertEndDate<=@EndDate and P.rolesno=13)
select * from　newCertificate　left join allPrsn　on allPrsn.PrsnID=newCertificate.PersonID　
where newCertificate.PersonID is　not null";//衛教師戒菸服務人員資格證明屆期未更新人數
        DataTable PrsnMisUnChangeDt = ObjDH.queryData(PrsnsqlM5, adict);
        int PrsnM5count = PrsnMisUnChangeDt.Rows.Count;
        sheet2.GetRow(8).CreateCell(12).SetCellValue(PrsnM5count);
        int PrsnM3count = PrsnM4count + PrsnM5count;
        sheet2.GetRow(6).CreateCell(12).SetCellValue(PrsnM3count);
        #endregion

        string sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=10";
        DataTable FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(9).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());
        sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=10";
        FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(10).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());
        sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=10";
        FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(11).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());
        sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=10";
        FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(12).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());
        sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=10";
        FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(13).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());
        sqlFSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=10";
        FSixYearsDt = ObjDH.queryData(sqlFSixYears, adict);
        sheet2.GetRow(14).CreateCell(5).SetCellValue(FSixYearsDt.Rows.Count.ToString());

        string sqlFCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO=75 and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=10
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable FCityDt = ObjDH.queryData(sqlFCity, adict);
        if (FCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(5).SetCellValue(FCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(5).SetCellValue(FCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(5).SetCellValue(FCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(5).SetCellValue(FCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(5).SetCellValue(FCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(5).SetCellValue(FCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(5).SetCellValue(FCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(5).SetCellValue(FCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(5).SetCellValue(FCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(5).SetCellValue(FCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(5).SetCellValue(FCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(5).SetCellValue(FCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(5).SetCellValue(FCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(5).SetCellValue(FCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(5).SetCellValue(FCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(5).SetCellValue(FCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(5).SetCellValue(FCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(5).SetCellValue(FCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(5).SetCellValue(FCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(5).SetCellValue(FCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(5).SetCellValue(FCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(5).SetCellValue(FCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(5).SetCellValue(FCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(5).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(5).SetCellValue("0");
        }
        

        string sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=11";
        DataTable GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(9).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());
        sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=11";
        GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(10).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());
        sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=11";
        GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(11).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());
        sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=11";
        GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(12).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());
        sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=11";
        GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(13).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());
                sqlGSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(2,54) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=11";
        GSixYearsDt = ObjDH.queryData(sqlGSixYears, adict);
        sheet2.GetRow(14).CreateCell(6).SetCellValue(GSixYearsDt.Rows.Count.ToString());

        string sqlGCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO in(2,54) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=11
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable GCityDt = ObjDH.queryData(sqlGCity, adict);
        if (GCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(6).SetCellValue(GCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(6).SetCellValue(GCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(6).SetCellValue(GCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(6).SetCellValue(GCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(6).SetCellValue(GCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(6).SetCellValue(GCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(6).SetCellValue(GCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(6).SetCellValue(GCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(6).SetCellValue(GCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(6).SetCellValue(GCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(6).SetCellValue(GCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(6).SetCellValue(GCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(6).SetCellValue(GCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(6).SetCellValue(GCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(6).SetCellValue(GCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(6).SetCellValue(GCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(6).SetCellValue(GCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(6).SetCellValue(GCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(6).SetCellValue(GCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(6).SetCellValue(GCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(6).SetCellValue(GCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(6).SetCellValue(GCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(6).SetCellValue(GCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(6).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(6).SetCellValue("0");
        }
      

        string sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=11";
        DataTable HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(9).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());
        sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=11";
        HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(10).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());
        sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=11";
        HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(11).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());
        sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=11";
        HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(12).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());
        sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=11";
        HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(13).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());
        sqlHSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(3,55) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=11";
        HSixYearsDt = ObjDH.queryData(sqlHSixYears, adict);
        sheet2.GetRow(14).CreateCell(7).SetCellValue(HSixYearsDt.Rows.Count.ToString());

        string sqlHCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO in(3,55) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=11
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable HCityDt = ObjDH.queryData(sqlHCity, adict);
        if (HCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(7).SetCellValue(HCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(7).SetCellValue(HCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(7).SetCellValue(HCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(7).SetCellValue(HCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(7).SetCellValue(HCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(7).SetCellValue(HCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(7).SetCellValue(HCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(7).SetCellValue(HCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(7).SetCellValue(HCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(7).SetCellValue(HCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(7).SetCellValue(HCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(7).SetCellValue(HCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(7).SetCellValue(HCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(7).SetCellValue(HCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(7).SetCellValue(HCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(7).SetCellValue(HCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(7).SetCellValue(HCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(7).SetCellValue(HCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(7).SetCellValue(HCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(7).SetCellValue(HCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(7).SetCellValue(HCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(7).SetCellValue(HCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(7).SetCellValue(HCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(7).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(7).SetCellValue("0");
        }
        

        string sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=11";
        DataTable ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(9).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());
        sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=11";
        ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(10).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());
        sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=11";
        ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(11).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());
        sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=11";
        ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(12).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());
        sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=11";
        ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(13).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());
        sqlISixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=11";
        ISixYearsDt = ObjDH.queryData(sqlISixYears, adict);
        sheet2.GetRow(14).CreateCell(8).SetCellValue(ISixYearsDt.Rows.Count.ToString());

        string sqlICity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO=75 and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=11
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable ICityDt = ObjDH.queryData(sqlICity, adict);
        if (ICityDt.Rows.Count > 0)
        {
            sheet2.GetRow(16).CreateCell(8).SetCellValue(ICityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(8).SetCellValue(ICityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(8).SetCellValue(ICityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(8).SetCellValue(ICityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(8).SetCellValue(ICityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(8).SetCellValue(ICityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(8).SetCellValue(ICityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(8).SetCellValue(ICityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(8).SetCellValue(ICityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(8).SetCellValue(ICityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(8).SetCellValue(ICityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(8).SetCellValue(ICityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(8).SetCellValue(ICityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(8).SetCellValue(ICityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(8).SetCellValue(ICityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(8).SetCellValue(ICityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(8).SetCellValue(ICityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(8).SetCellValue(ICityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(8).SetCellValue(ICityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(8).SetCellValue(ICityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(8).SetCellValue(ICityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(8).SetCellValue(ICityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(8).SetCellValue(ICityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(8).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(8).SetCellValue("0");
        }
  

        string sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=12";
        DataTable JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(9).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());
        sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=12";
        JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(10).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());
        sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=12";
        JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(11).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());
        sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=12";
        JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(12).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());
        sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=12";
        JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(13).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());
        sqlJSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(6,7,52,4,5,51) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=12";
        JSixYearsDt = ObjDH.queryData(sqlJSixYears, adict);
        sheet2.GetRow(14).CreateCell(9).SetCellValue(JSixYearsDt.Rows.Count.ToString());

        string sqlJCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO in(6,7,52) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=12
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable JCityDt = ObjDH.queryData(sqlJCity, adict);
        if (JCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(9).SetCellValue(JCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(9).SetCellValue(JCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(9).SetCellValue(JCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(9).SetCellValue(JCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(9).SetCellValue(JCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(9).SetCellValue(JCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(9).SetCellValue(JCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(9).SetCellValue(JCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(9).SetCellValue(JCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(9).SetCellValue(JCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(9).SetCellValue(JCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(9).SetCellValue(JCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(9).SetCellValue(JCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(9).SetCellValue(JCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(9).SetCellValue(JCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(9).SetCellValue(JCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(9).SetCellValue(JCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(9).SetCellValue(JCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(9).SetCellValue(JCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(9).SetCellValue(JCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(9).SetCellValue(JCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(9).SetCellValue(JCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(9).SetCellValue(JCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(9).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(9).SetCellValue("0");
        }
        

        string sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=12";
        DataTable KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(9).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());
        sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=12";
        KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(10).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());
        sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=12";
        KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(11).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());
        sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=12";
        KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(12).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());
        sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=12";
        KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(13).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());
                sqlKSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=12";
        KSixYearsDt = ObjDH.queryData(sqlKSixYears, adict);
        sheet2.GetRow(14).CreateCell(10).SetCellValue(KSixYearsDt.Rows.Count.ToString());

        string sqlKCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO=75 and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=12
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable KCityDt = ObjDH.queryData(sqlKCity, adict);
        if (KCityDt.Rows.Count>0)
        {
            sheet2.GetRow(16).CreateCell(10).SetCellValue(KCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(10).SetCellValue(KCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(10).SetCellValue(KCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(10).SetCellValue(KCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(10).SetCellValue(KCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(10).SetCellValue(KCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(10).SetCellValue(KCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(10).SetCellValue(KCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(10).SetCellValue(KCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(10).SetCellValue(KCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(10).SetCellValue(KCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(10).SetCellValue(KCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(10).SetCellValue(KCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(10).SetCellValue(KCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(10).SetCellValue(KCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(10).SetCellValue(KCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(10).SetCellValue(KCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(10).SetCellValue(KCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(10).SetCellValue(KCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(10).SetCellValue(KCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(10).SetCellValue(KCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(10).SetCellValue(KCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(10).SetCellValue(KCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(10).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(10).SetCellValue("0");
        }
        

        string sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=13";
        DataTable LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(9).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());
        sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=13";
        LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(10).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());
        sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=13";
        LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(11).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());
        sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=13";
        LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(12).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());
        sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=13";
        LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(13).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());
        sqlLSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO in(4,5,51) and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=13";
        LSixYearsDt = ObjDH.queryData(sqlLSixYears, adict);
        sheet2.GetRow(14).CreateCell(11).SetCellValue(LSixYearsDt.Rows.Count.ToString());

        string sqlLCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位' 
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO in(4,5,51) and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=13
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable LCityDt = ObjDH.queryData(sqlLCity, adict);
        if (LCityDt.Rows.Count > 0)
        {
            sheet2.GetRow(16).CreateCell(11).SetCellValue(LCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(11).SetCellValue(LCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(11).SetCellValue(LCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(11).SetCellValue(LCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(11).SetCellValue(LCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(11).SetCellValue(LCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(11).SetCellValue(LCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(11).SetCellValue(LCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(11).SetCellValue(LCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(11).SetCellValue(LCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(11).SetCellValue(LCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(11).SetCellValue(LCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(11).SetCellValue(LCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(11).SetCellValue(LCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(11).SetCellValue(LCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(11).SetCellValue(LCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(11).SetCellValue(LCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(11).SetCellValue(LCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(11).SetCellValue(LCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(11).SetCellValue(LCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(11).SetCellValue(LCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(11).SetCellValue(LCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(11).SetCellValue(LCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(11).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(11).SetCellValue("0");
        }
        

        string sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@OneStartDate and CertEndDate<=@OneEndDate and P.rolesno=13";
        DataTable MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(9).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());
        sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@TwoStartDate and CertEndDate<=@TwoEndDate and P.rolesno=13";
        MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(10).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());
        sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@ThreeStartDate and CertEndDate<=@ThreeEndDate and P.rolesno=13";
        MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(11).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());
        sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FourStartDate and CertEndDate<=@FourEndDate and P.rolesno=13";
        MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(12).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());
        sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@FiveStartDate and CertEndDate<=@FiveEndDate and P.rolesno=13";
        MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(13).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());
        sqlMSixYears = @"select QC.PersonID,CONVERT(VARCHAR(19), QC.CertEndDate, 120)CertEndDate 　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
where CTypeSNO=75 and CertEndDate>=@SixStartDate and CertEndDate<=@SixEndDate and P.rolesno=13";
        MSixYearsDt = ObjDH.queryData(sqlMSixYears, adict);
        sheet2.GetRow(14).CreateCell(12).SetCellValue(MSixYearsDt.Rows.Count.ToString());

        string sqlMCity = @"	select * from(
select QC.PersonID,CASE 
        WHEN AREA_NAME IS NULL THEN '無現職單位'
        WHEN Org.OrganCode='000' THEN '無現職單位' 
        ELSE AREA_NAME 
    END as AREA_NAME　 from QS_Certificate　QC
left join Person P on QC.PersonID=P.PersonID
Left Join Organ Org On Org.OrganSNO= P.OrganSNO
Left Join CD_AREA CA On CA.AREA_CODE= Org.AreaCodeA And CA.AREA_TYPE= 'A'
where CTypeSNO=75 and CertEndDate>=Getdate() and CertEndDate<=@EndDate and P.rolesno=13
    )t
    PIVOT(
    --設定彙總欄位及方式
    Count(PersonID)
    -- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
    FOR AREA_NAME IN(基隆市,台北市,新北市,桃園市,新竹市,新竹縣,苗栗縣,台中市,彰化縣,南投縣,雲林縣,嘉義市,嘉義縣,台南市,高雄市,屏東縣,台東縣,花蓮縣,宜蘭縣,澎湖縣,金門縣,連江縣,無現職單位)
    ) p";
        DataTable MCityDt = ObjDH.queryData(sqlMCity, adict);
        if (MCityDt.Rows.Count > 0)
        {
            sheet2.GetRow(16).CreateCell(12).SetCellValue(MCityDt.Rows[0][0].ToString());
            sheet2.GetRow(17).CreateCell(12).SetCellValue(MCityDt.Rows[0][1].ToString());
            sheet2.GetRow(18).CreateCell(12).SetCellValue(MCityDt.Rows[0][2].ToString());
            sheet2.GetRow(19).CreateCell(12).SetCellValue(MCityDt.Rows[0][3].ToString());
            sheet2.GetRow(20).CreateCell(12).SetCellValue(MCityDt.Rows[0][4].ToString());
            sheet2.GetRow(21).CreateCell(12).SetCellValue(MCityDt.Rows[0][5].ToString());
            sheet2.GetRow(22).CreateCell(12).SetCellValue(MCityDt.Rows[0][6].ToString());
            sheet2.GetRow(23).CreateCell(12).SetCellValue(MCityDt.Rows[0][7].ToString());
            sheet2.GetRow(24).CreateCell(12).SetCellValue(MCityDt.Rows[0][8].ToString());
            sheet2.GetRow(25).CreateCell(12).SetCellValue(MCityDt.Rows[0][9].ToString());
            sheet2.GetRow(26).CreateCell(12).SetCellValue(MCityDt.Rows[0][10].ToString());
            sheet2.GetRow(27).CreateCell(12).SetCellValue(MCityDt.Rows[0][11].ToString());
            sheet2.GetRow(28).CreateCell(12).SetCellValue(MCityDt.Rows[0][12].ToString());
            sheet2.GetRow(29).CreateCell(12).SetCellValue(MCityDt.Rows[0][13].ToString());
            sheet2.GetRow(30).CreateCell(12).SetCellValue(MCityDt.Rows[0][14].ToString());
            sheet2.GetRow(31).CreateCell(12).SetCellValue(MCityDt.Rows[0][15].ToString());
            sheet2.GetRow(32).CreateCell(12).SetCellValue(MCityDt.Rows[0][16].ToString());
            sheet2.GetRow(33).CreateCell(12).SetCellValue(MCityDt.Rows[0][17].ToString());
            sheet2.GetRow(34).CreateCell(12).SetCellValue(MCityDt.Rows[0][18].ToString());
            sheet2.GetRow(35).CreateCell(12).SetCellValue(MCityDt.Rows[0][19].ToString());
            sheet2.GetRow(36).CreateCell(12).SetCellValue(MCityDt.Rows[0][20].ToString());
            sheet2.GetRow(37).CreateCell(12).SetCellValue(MCityDt.Rows[0][21].ToString());
            sheet2.GetRow(38).CreateCell(12).SetCellValue(MCityDt.Rows[0][22].ToString());
        }
        else
        {
            sheet2.GetRow(16).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(17).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(18).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(19).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(20).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(21).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(22).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(23).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(24).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(25).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(26).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(27).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(28).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(29).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(30).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(31).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(32).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(33).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(34).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(35).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(36).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(37).CreateCell(12).SetCellValue("0");
            sheet2.GetRow(38).CreateCell(12).SetCellValue("0");
        }
        for(int i = 16; i <= 38; i++)
        {
            for(int j = 2; j <= 12; j++)
            {
                sheet2.GetRow(i).GetCell(j).CellStyle = FirstStyle;
            }
        }
        for (int m = 2; m <= 14; m++)
        {
            for (int n = 2; n <= 12; n++)
            {
                sheet2.GetRow(m).GetCell(n).CellStyle = FirstStyle;
            }
        }
        #endregion
        #region 第三頁
        string sqlTeacherA = @"select sum(1) from QS_Certificate QC
left join Person P on QC.PersonID=p.PersonID
where CTypeSNO in(8,9,12,13,59)　and CertEndDate>getdate() and RoleSNO=10
group by RoleSNO";//西醫師師資
        DataTable TeacherADt = ObjDH.queryData(sqlTeacherA, adict);
        if (TeacherADt.Rows.Count > 0)
        {
            string val = TeacherADt.Rows[0][0].ToString();
            sheet3.GetRow(1).CreateCell(1).SetCellValue(val);
        }
        else
        {
            sheet3.GetRow(1).CreateCell(1).SetCellValue("0");
        }
        string sqlTeacherB = @"select sum(1) from QS_Certificate QC
left join Person P on QC.PersonID=p.PersonID
where CTypeSNO in(8,9,12,13,59)　and CertEndDate>getdate() and RoleSNO=11
group by RoleSNO";//牙醫師師資
        DataTable TeacherBDt = ObjDH.queryData(sqlTeacherB, adict);
        if (TeacherBDt.Rows.Count > 0)
        {
            string val = TeacherBDt.Rows[0][0].ToString();
            sheet3.GetRow(1).CreateCell(2).SetCellValue(val);
        }
        else
        {
            sheet3.GetRow(1).CreateCell(2).SetCellValue("0");
        }
        string sqlTeacherC = @"select sum(1) from QS_Certificate QC
left join Person P on QC.PersonID=p.PersonID
where CTypeSNO in(8,9,12,13,59)　and CertEndDate>getdate() and RoleSNO=12
group by RoleSNO";//藥師師資
        DataTable TeacherCDt = ObjDH.queryData(sqlTeacherC, adict);
        if (TeacherCDt.Rows.Count > 0)
        {
            string val = TeacherCDt.Rows[0][0].ToString();
            sheet3.GetRow(1).CreateCell(3).SetCellValue(val);
        }
        else
        {
            sheet3.GetRow(1).CreateCell(3).SetCellValue("0");
        }
        string sqlTeacherD = @"select sum(1) from QS_Certificate QC
left join Person P on QC.PersonID=p.PersonID
where CTypeSNO in(8,9,12,13,59)　and CertEndDate>getdate() and RoleSNO=13
group by RoleSNO";//衛教師師資
        DataTable TeacherDDt = ObjDH.queryData(sqlTeacherD, adict);
        if (TeacherDDt.Rows.Count > 0)
        {
            string val = TeacherDDt.Rows[0][0].ToString();
            sheet3.GetRow(1).CreateCell(4).SetCellValue(val);
        }
        else
        {
            sheet3.GetRow(1).CreateCell(4).SetCellValue("0");
        }
        sheet3.GetRow(1).GetCell(1).CellStyle = FirstStyle;
        sheet3.GetRow(1).GetCell(2).CellStyle = FirstStyle;
        sheet3.GetRow(1).GetCell(3).CellStyle = FirstStyle;
        sheet3.GetRow(1).GetCell(4).CellStyle = FirstStyle;
        #endregion
        //內文樣式
        ICellStyle BodyStyle = hssfworkbook.CreateCellStyle();
        IFont Bodyfont = hssfworkbook.CreateFont();
        Bodyfont.FontName = "微軟正黑體";
        Bodyfont.FontHeightInPoints = 12;
        BodyStyle.Alignment = HorizontalAlignment.Left; //水平置中
        BodyStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
        BodyStyle.SetFont(Bodyfont);

        hssfworkbook.Write(ms);
        byte[] byt = ms.ToArray();
        Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") +  HttpUtility.UrlEncode("證書(明)管理統計報表.xls"));
        Response.AddHeader("Content-Length", byt.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(byt);
        byt = null;
        ms.Close();
        ms.Dispose();
    }
    public static DataTable RenderFromXLS(string xFile)
    {
        FileStream fs = new FileStream(xFile, FileMode.Open);
        HSSFWorkbook workbook = new HSSFWorkbook(fs);
        HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);

        DataTable dt = new DataTable();

        HSSFRow header = (HSSFRow)sheet.GetRow(0);
        int cellCount = header.LastCellNum;

        #region Generate Header
        for (int i = header.FirstCellNum; i < cellCount; i++)
        {
            if (!(header.GetCell(i) == null))
            {
                HSSFCell cell = (HSSFCell)header.GetCell(i);
                DataColumn column = new DataColumn(cell.ToString());
                dt.Columns.Add(column);
            }
        }
        #endregion

        #region Generate Data
        int rowCount = sheet.LastRowNum;
        for (int i = sheet.FirstRowNum; i <= rowCount; i++)
        {
            HSSFRow row = (HSSFRow)sheet.GetRow(i);
            DataRow data = dt.NewRow();

            for (int j = row.FirstCellNum; j < cellCount; j++)
            {
                if (!(row.GetCell(j) == null))
                    data[j] = row.GetCell(j).ToString();
            }
            dt.Rows.Add(data);
        }
        #endregion

        fs.Close();
        workbook = null;
        sheet = null;

        return dt;
    }
}