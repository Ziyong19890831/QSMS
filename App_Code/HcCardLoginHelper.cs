using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// HcCardLoginHelper 的摘要描述
/// </summary>
public class HcCardLoginHelper : CardLoginHelper
{
    public HcCardLoginHelper(CardLoginModel model) : base(model)
    {
    }

    public override bool VerifyInput()
    {
        if (!base.VerifyInput()) return false;
        else if (Model.IdNo == "")
        {
            //讀取不到健保卡ID(空白)，跳出錯誤訊息。
            Utility.showMessage(Model.Page, "ErrorMessage", "請插入健保卡");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(Model.Password))
        {
            Utility.showMessage(Model.Page, "ErrorMessage", "請輸入密碼");
            return false;
        }
        return true;
    }

    public override DataTable LoadPerson()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("PersonID", Model.IdNo);
        DataHelper dataHelper = new DataHelper();
        DataTable table = dataHelper.queryData(@"
            SELECT
                P.*, O.OrganLevel, O.AreaCodeA, O.AreaCodeB, O.OrganName, O.OrganCode 
            FROM Person AS P 
            INNER JOIN Organ AS O ON O.OrganSNO = P.OrganSNO
            WHERE PersonID = @PersonID
        ", dictionary);
        return table;
    }

    public override DataRow VerifyPersons(DataTable table)
    {
        if (base.VerifyPersons(table) == null) return null;
        DataRow currentRow = null;
        foreach (DataRow row in table.Rows)
        {
            if (row["PPWD"].Equals(Model.Password))
            {
                currentRow = row;
                break;
            }
        }
        if (currentRow == null)
        {
            WriteLoginLog("4003", "密碼錯誤");
            currentRow = table.Rows[0];
            UpdateLoginError(currentRow);
            Utility.showMessage(Model.Page, "ErrorMessage", "密碼錯誤");
            return null;
        }
        if (!LoginLock(currentRow)) return null;
        ClearLoginError(currentRow);
        return currentRow;
    }
}