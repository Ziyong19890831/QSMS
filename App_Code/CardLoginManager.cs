using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CardLoginManager 的摘要描述
/// </summary>
public static class CardLoginManager
{


    public static CardLoginHelper CreateHelper(CardLoginModel model)
    {
        if (string.Compare(model.CardType, "1") == 0)   //醫事人員卡
            return new HpcCardLoginHelper(model);
        //else if (string.Compare(model.CardType, "2") == 0)
        //    return new HscCardLoginHelper(model);
        //else if (string.Compare(model.CardType, "3") == 0)  //健保卡
        //    return new HcCardLoginHelper(model);
        else
            throw new Exception(string.Format("不支援的卡片種類 {0}", model.CardType));
    }

}