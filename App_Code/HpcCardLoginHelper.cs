using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// HpcCardLoginHelper 的摘要描述
/// </summary>
public class HpcCardLoginHelper: CardLoginHelper
{
    public HpcCardLoginHelper(CardLoginModel model) : base(model)
    {
    }

    public override DataTable LoadPerson()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("PersonID", Model.IdNo);
        DataHelper dataHelper = new DataHelper();
        DataTable table = dataHelper.queryData(@"
            Select P.*, R.RoleName, R.RoleOrganType, R.RoleGroup, R.RoleLevel, 
                O.AreaCodeA, O.AreaCodeB, O.OrganName, O.OrganCode, O.OrganLevel, R.IsAdmin 
            From Person P
                INNER JOIN Organ O ON O.OrganSNO = P.OrganSNO
                INNER JOIN Role R ON R.RoleSNO = P.RoleSNO
            Where PersonID=@PersonID 
        ", dictionary);
        return table;
    }

    public override DataRow VerifyPersons(DataTable table)
    {
        DataRow currentRow = base.VerifyPersons(table);
        if (currentRow == null) return null;
        if (!LoginLock(currentRow)) return null;
        ClearLoginError(currentRow);
        return currentRow;
    }
}