using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;

public class CardLoginHelper
{
    public CardLoginModel Model { get; set; }
    public CardLoginHelper(CardLoginModel model)
    {
        Model = model;
    }
    public virtual bool VerifyInput()
    {
        if (Model.VerificationCode != Model.CheckCode)
        {
            WriteLoginLog("4001", "認證碼錯誤");
            Utility.showMessage(Model.Page, "ErrorMessage", "認證碼錯誤");
            return false;
        }
        return true;
    }

    public void WriteLoginLog(string status, string message)
    {
        var dataHelper = new DataHelper();
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("PAccount", Model.IdNo);
        dictionary.Add("LoginIP", Model.IP);
        dictionary.Add("LoginStatus", status);
        dictionary.Add("LoginInfo", message);
        dataHelper.executeNonQuery(@"
                INSERT INTO [LoginLog](PAccount,LoginIP,LoginStatus, LoginInfo) 
                VALUES (@PAccount, @LoginIP, @LoginStatus, @LoginInfo)", dictionary);
    }

    public virtual DataTable LoadPerson()
    {
        return new DataTable();
    }

    public virtual DataRow VerifyPersons(DataTable table)
    {
        if (table.Rows.Count == 0)
        {
            WriteLoginLog("4002", "帳號不存在");
            Utility.showMessage(Model.Page, "ErrorMessage", "帳號不存在");
            return null;
        }
        return table.Rows[0];
    }

    protected void UpdateLoginError(DataRow currentRow)
    {
        var loginError = currentRow.IsNull("LoginError") ? 0 : (int)currentRow["LoginError"];
        var personSNO = (int)currentRow["PersonSNO"];
        var dataHelper = new DataHelper();
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("LoginError", loginError + 1);
        dictionary.Add("LoginErrorTime", DateTime.Now);
        dictionary.Add("PersonSNO", personSNO);
        dataHelper.executeNonQuery(@"
            UPDATE [Person] 
            SET LoginError = @LoginError, LoginErrorTime = @LoginErrorTime 
            WHERE PersonSNO = @PersonSNO", dictionary);
    }
    protected void ClearLoginError(DataRow currentRow)
    {
        var loginError = currentRow.IsNull("LoginError") ? 0 : (int)currentRow["LoginError"];
        var personSNO = (int)currentRow["PersonSNO"];
        var dataHelper = new DataHelper();
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("LoginError", 0);
        dictionary.Add("LoginErrorTime", DBNull.Value);
        dictionary.Add("PersonSNO", personSNO);
        dataHelper.executeNonQuery(@"
            UPDATE [Person] 
            SET LoginError = @LoginError, LoginErrorTime = @LoginErrorTime 
            WHERE PersonSNO = @PersonSNO", dictionary);
    }
    protected bool LoginLock(DataRow currentRow)
    {
        var loginError = currentRow.IsNull("LoginError") ? 0 : (int)currentRow["LoginError"];
        var loginErrorTime = currentRow.IsNull("LoginErrorTime") ? DateTime.MinValue : (DateTime)currentRow["LoginErrorTime"];
        if ((DateTime.Now - loginErrorTime).TotalMinutes <= 30 && loginError >= 3)
        {
            Utility.showMessage(Model.Page, "ErrorMessage", "多次登入失敗，帳號已鎖定! 請聯繫管理者進行解鎖 或 三十分鐘再嘗試登入。");
            return false;
        }
        return true;
    }

    public DataRow LoadLastLogin(DataRow person)
    {
        var dictionary = new Dictionary<string, object>();
        var personSNO = (int)person["PersonSNO"];
        var dataHelper = new DataHelper();

        dictionary.Add("PAccount", Model.IdNo);
        var table = dataHelper.queryData(@"
            SELECT TOP 1 * 
            FROM [LoginLog]
            WHERE PAccount = @PAccount
            ORDER BY LoginTime DESC
        ", dictionary);
        if (table.Rows.Count == 0)
            return null;
        else
            return table.Rows[0];
    }

    //卡片不需三個月定期改密碼
    //public bool ModifyPassword(DataRow person)
    //{
    //    const string key = "exampleModal";
    //    const string js = "$('#exampleModal').modal({ backdrop: 'static', keyboard: false });";
    //    if (person.IsNull("PasswordModilyDT"))
    //    {
    //        if (person.IsNull("CreateDT") || DateTime.Now.Subtract((DateTime)person["CreateDT"]) > TimeSpan.FromDays(90))
    //        {
    //            ScriptManager.RegisterStartupScript(Model.Page, Model.Page.GetType(), key, js, true);
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        var passwordModilyDT = (DateTime)person["PasswordModilyDT"];
    //        if (DateTime.Now.Subtract(passwordModilyDT) > TimeSpan.FromDays(90))
    //        {
    //            ScriptManager.RegisterStartupScript(Model.Page, Model.Page.GetType(), key, js, true);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public UserInfo ToUserInfo(DataRow person, DataRow lastLog)
    {
        var result = new UserInfo()
        {
            //PersonSNO = Convert.ToString(person["PersonSNO"]),
            ////RoleSNO = Convert.ToString(person["RoleSNO"]),
            ////RoleName = Convert.ToString(person["RoleName"]),
            //MainRoleLevel = Convert.ToString(person["RoleLevel"]),
            //SysRoleLevel = new Dictionary<string, string>(),
            //AdminRole = new List<string>(),
            //AreaCodeA = Convert.ToString(person["AreaCodeA"]),
            //AreaCodeB = Convert.ToString(person["AreaCodeB"]),
            //OrganLevel = Convert.ToString(person["OrganLevel"]),
            //OrganSNO = Convert.ToString(person["OrganSNO"]),
            //OrganCode = Convert.ToString(person["OrganCode"]),
            //OrganName = Convert.ToString(person["OrganName"]),
            //UserAccount = Convert.ToString(person["PAccount"]),
            //UserPWD = Convert.ToString(person["PPWD"]),
            //UserTel = Convert.ToString(person["PTel"]),
            //UserMail = Convert.ToString(person["PMail"]),
            //UserName = Convert.ToString(person["PName"]),
            //PersonID = Convert.ToString(person["PersonID"]),
            PersonSNO = Convert.ToString(person["PersonSNO"]),
            RoleSNO = Convert.ToString(person["RoleSNO"]),
            RoleName = Convert.ToString(person["RoleName"]),
            RoleOrganType = Convert.ToString(person["RoleOrganType"]),
            RoleGroup = Convert.ToString(person["RoleGroup"]),
            RoleLevel = Convert.ToString(person["RoleLevel"]),
            IsAdmin = (bool)person["IsAdmin"],
            AreaCodeA = Convert.ToString(person["AreaCodeA"]),
            AreaCodeB = Convert.ToString(person["AreaCodeB"]),
            OrganSNO = Convert.ToString(person["OrganSNO"]),
            OrganCode = Convert.ToString(person["OrganCode"]),
            OrganName = Convert.ToString(person["OrganName"]),
            OrganLevel = Convert.ToString(person["OrganLevel"]),
            UserAccount = Convert.ToString(person["PAccount"]),
            UserPWD = Convert.ToString(person["PPWD"]),
            UserTel = Convert.ToString(person["PTel"]),
            UserPhone = Convert.ToString(person["PPhone"]),
            UserMail = Convert.ToString(person["PMail"]),
            UserName = Convert.ToString(person["PName"]),
            PersonID = Convert.ToString(person["PersonID"]),
            Address = Convert.ToString(person["PAddr"]),
            Birthday = Convert.ToString(person["PBirthdate"]),
         
        };


        return result;
    }

    public bool VerifyCard()
    {
        return true;
    }

}

