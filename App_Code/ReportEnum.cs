using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ReportEnum 的摘要描述
/// </summary>
public enum ReportEnum
{
    /// <summary>
    /// 人員登入紀錄
    /// </summary>
    LoginLog,
    /// <summary>
    /// 學員名冊
    /// </summary>
    ReportMember,
    ReportMember1,
    ReportMember2,
    ReportMember3,
    ReportMember4,
    ReportMember5,
    /// <summary>
    /// 證書名冊
    /// </summary>
    ReportCertificate,
    /// <summary>
    /// 線上課程統計報表
    /// </summary>
    ReportCourseOnline,
    /// <summary>
    /// 線上課程記錄報表
    /// </summary>
    ReportLearning,
    /// <summary>
    /// 線上課程記錄報表
    /// </summary>
    ReportExamOnline,
    /// <summary>
    /// 實體課程記錄報表
    /// </summary>
    ReportTest,
    /// <summary>
    /// 換證通知&&延期審核
    /// </summary>
    CertificateExpire,
    /// <summary>
    /// 滿意度調查報表
    /// </summary>
    Questionnaire,
    /// <summary>
    /// 積分記錄列印
    /// </summary>
    RecordLog,
    /// <summary>
    /// 報名管理
    /// </summary>
    EventAudit,
    /// <summary>
    /// 報名管理
    /// </summary>
    EventManager,
    /// <summary>
    /// 繼續教育學分管理
    /// </summary>
    QS_EManager,
    /// <summary>
    /// 繼續教育線上學分管理
    /// </summary>
    Elearning,
    /// <summary>
    /// 繼續教育線上學分管理
    /// </summary>
    Toolkits,
    /// <summary>
    /// 證書領證
    /// </summary>
    CertificateChange,
    /// <summary>
    /// 繼續教育領證
    /// </summary>
    EcertificateChange,

    QS_Manager,
    /// <summary>
    /// 自動發送信件
    /// </summary>
    AutoSendMail,
    /// <summary>
    /// 各縣市舊制證書取得人數統計(原「培訓認證人數統計」)
    /// </summary>
    OldCertificate,
    /// <summary>
    /// 新訓取得證書人數
    /// </summary>
    NewCertificate,
    /// <summary>
    /// 具戒菸服務資格人數
    /// </summary>
    GetQuictSmok,
    /// <summary>
    /// 各縣市舊制證書取得人數統計(原「培訓認證人數統計」)匯出EXCEL
    /// </summary>
    ExportCTable,
    /// <summary>
    /// 各縣市舊制證書取得人數統計(原「培訓認證人數統計」)匯出ODS
    /// </summary>
    ExportCTableODS
}