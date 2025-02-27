using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// CardLoginModel 的摘要描述
/// </summary>
public class CardLoginModel
{
    public string CardType { get; set; }
    public string IdNo { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }
    public string CheckCode { get; set; }
    public string IP { get; set; }
    public Page Page { get; set; }
    public string Now { get; set; }
    public string Sign { get; set; }
    public string UserPhone { get; set; }
    //public string Address { get; set; }        //使用者姓名
    //public string Birthday { get; set; }
    //public string UserMail { get; set; }
    
}