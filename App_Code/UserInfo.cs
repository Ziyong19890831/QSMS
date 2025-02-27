using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// UserInfo 的摘要描述
/// </summary>
[Serializable]
public class UserInfo
{
    public String PersonSNO { get; set; }       //使用者ID
    public String RoleSNO { get; set; }         //使用者角色ID
    public String RoleName { get; set; }        //使用者角色名稱
    public String RoleOrganType { get; set; }   //使用者角色單位分類
    public String RoleLevel { get; set; }       //使用者角色層級
    public String RoleGroup { get; set; }       //使用者角色群組
    public bool IsAdmin { get; set; }           //是否為管理者
    public String PersonID { get; set; }        //使用者身分證
    public String AreaCodeA { get; set; }       //使用者單位行政區碼2
    public String AreaCodeB { get; set; }       //使用者單位行政區碼4
    public String OrganSNO { get; set; }        //使用者單位ID
    public String OrganCode { get; set; }       //使用者單位代碼
    public String OrganName { get; set; }       //使用者單位名稱
    public String OrganLevel { get; set; }      //使用者單位角色層級
    public String UserAccount { get; set; }     //使用者帳號
    public String UserPWD { get; set; }         //使用者密碼
    public String UserTel { get; set; }         //使用者聯絡電話
    public String UserPhone { get; set; }       //使用者手機
    public String UserMail { get; set; }        //使用者電子信箱
    public String UserName { get; set; }        //使用者姓名
    public String Birthday { get; set; }        //使用者姓名
    public String Address { get; set; }        //使用者姓名
    public String verification { get; set; }    //帳號申請驗證碼
    public String TsType { get; set; }          //服務專科    
    public String TsTypeNote { get; set; }        //服務專科備註
    //管理權限
    public bool AdminIsInsert { get; set; }     //是否可新增
    public bool AdminIsDelete { get; set; }     //是否可刪除
    public bool AdminIsUpdate { get; set; }     //是否可修改

}