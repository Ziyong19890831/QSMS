using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Schedule_Person : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Person_Click(object sender, EventArgs e)
    {
        string Type ="";
        if (Request["type"] !=null)
        {
            Type = Request.QueryString["type"].ToString();
        }        

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        //aDict.Add("Name", txt_Name.Value);
        //aDict.Add("Email", txt_Email.Value);

        //更新S01系統使用者的資料到Person表
        string sql = @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S01',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP1  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S01系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP1 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S01' ;
                    ";

        //更新S01系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S01'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP1 WHERE USERSSN IS NOT NULL );
                    ";

        //更新婦幼系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S02',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP2  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新婦幼系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP2 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S02' ;
                    ";

        //更新婦幼系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S02'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP2 WHERE USERSSN IS NOT NULL );
                    ";

        //更新S03系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S03',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP3  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S03系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP3 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S03' ;
                    ";

        //更新S03系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S03'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP3 WHERE USERSSN IS NOT NULL );
                    ";

        //更新S04系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S04',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP4  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S04系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP4 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S04' ;
                    ";

        //更新S04系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S04'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP4 WHERE USERSSN IS NOT NULL );
                    ";

        //更新S05系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S05',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP5  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S05系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP5 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S05' ;
                    ";

        //更新S05系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S05'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP5 WHERE USERSSN IS NOT NULL );
                    ";

        //更新S06系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select USERSSN,'S06',USERSSN,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,USERNAME,USERMAIL 
                        from AP6  
                        where USERSSN is not null and USERSSN <> ' ' 
                        AND USERSSN NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S06系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT USERSSN FROM AP6 WHERE USERSSN is not null)  
                    AND SYSTEM_ID = 'S06' ;
                    ";

        //更新S06系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S06'
                    AND SysPAccount IN ( SELECT USERSSN FROM AP6 WHERE USERSSN IS NOT NULL );
                    ";

        //更新S07系統使用者的資料到Person表
        sql += @"
                        insert persond (PersonID,SYSTEM_ID,SysPAccount,SysPAccountData,SysPAccountIsUser,SSOKEY,
                        SysBindDT,AllOrgan,CreateDT,CreateUserID,ModifyDT,ModifyUserID,sysPName,sysPMail) 
                        select UID,'S07',ACC_ID,'','Y',null,
                        null,'N',getdate(),1,getdate(),1,NAME,EMAIL 
                        from AP7  
                        where UID is not null and UID <> ' ' 
                        AND UID NOT IN (SELECT SysPAccount FROM PersonD WHERE SysPAccount is not null);
                        ";

        //更新S07系統使用者的異動，使用者的身分證號未啟用或是不存在時，更新使用者登入狀態為不啟用
        sql += @"
                    UPDATE PersonD SET SysPAccountIsUser = 'N'  
                    WHERE SysPAccount NOT IN (SELECT UID FROM AP7 WHERE UID is not null)  
                    AND SYSTEM_ID = 'S07' ;
                    ";

        //更新S07系統使用者的異動(啟用)。(原先停用，之後啟用)
        sql += @"
                    UPDATE PersonD
                    SET SysPAccountIsUser = 'Y'
                    WHERE 1=1
                    AND SysPAccountIsUser = 'N'
                    AND SYSTEM_ID = 'S07'
                    AND SysPAccount IN ( SELECT UID FROM AP7 WHERE UID IS NOT NULL );
                    ";

        //刪除重複的Person主表
        sql += @"
                    delete  from personD
                    where PersonDSNO in (
                    select xx.PersonDSNO
                    from (
                    select ROW_NUMBER() OVER (PARTITION BY PD.PersonID,PD.SYSTEM_ID ORDER BY PD.PersonID,PD.SYSTEM_ID) AS ROW_NO,pd.*
                    from personD PD
                    ) xx
                    where xx.ROW_NO <> 1 )
                    ";

        //更新Person主表，讓帳號同步(PersonD)
        sql += @"
                    insert person (
                    RoleSNO,OrganSNO,PAccount,PName,PPWD,PMail,CreateUserID,ModifyUserID,PersonID
                    ) 
                    select 
                    '1','1',PersonID,sysPname,RIGHT(PersonID,6),sysPmail,1,1,PersonID
                    from Persond  
                    where PersonID is not null and PersonID <> ' ' 
                    AND PersonID NOT IN (SELECT PersonID FROM Person WHERE PersonID is not null);
                    ";

        objDH.executeNonQuery(sql, aDict);
       

        //關閉視窗
        if (Type == "1")
        {
            Response.Write("<script language='javascript'>window.open('', '_self', '');window.close();</" + "script>");
        }
        else
        {
            Response.Write("<script>alert('更新完成!'); location.href='Person.aspx';</script>");
        }
        
    }
}