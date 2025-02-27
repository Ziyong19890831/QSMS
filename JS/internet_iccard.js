function IC_CARD() {
    var self = this;
    function GetMessage(statusCode) {
        switch (statusCode) {
            case 5001: return "一般錯誤, 可能為其他導致無法正常運作的錯誤，請排除不正常的使用流程或無法使用的的網路環境或參數。";
            case 5002: return "配置記憶體發生錯誤";
            case 5003: return "記憶體緩衝區太小";
            case 5004: return "未支援功能";
            case 5005: return "錯誤的參數";
            case 5006: return "不合法的Handle";
            case 5007: return "試用版期限已過";
            case 5008: return "Base64編碼錯誤";
            case 5010: return "無法在MS CryptoAPI Database中找到指定憑證";
            case 5011: return "憑證已過期";
            case 5012: return "憑證尚未合法, 無法使用";
            case 5013: return "憑證可能過期或無法使用";
            case 5014: return "憑證主旨錯誤";
            case 5015: return "無法找到憑證發行者";
            case 5016: return "不合法的憑證簽章";
            case 5017: return "憑證用途(加解密, 簽驗章)不合適";
            case 5020: return "憑證已撤銷";
            case 5021: return "憑證已撤銷(金鑰洩露)";
            case 5022: return "憑證已撤銷(CA compromised)";
            case 5023: return "憑證已撤銷(聯盟已變更)";
            case 5024: return "憑證已撤銷(已取代)";
            case 5025: return "憑證已撤銷(已停止)";
            case 5026: return "憑證保留或暫禁";
            case 5028: return "憑證己撤銷（凍結）";
            case 5030: return "CRL已過期";
            case 5031: return "不合法的CRL";
            case 5032: return "無法找到CRL";
            case 5035: return "Digest錯誤";
            case 5036: return "不合法的簽章";
            case 5037: return "內容錯誤";
            case 5040: return "憑證格式錯誤";
            case 5041: return "CRL格式錯誤";
            case 5042: return "錯誤的PKCS7格式";
            case 5043: return "Key的格式錯誤";
            case 5044: return "不合法的PKCS10格式";
            case 5045: return "不合適的格式";
            case 5050: return "找不到指定物件";
            case 5051: return "簽章值中無原文";
            case 5052: return "簽章值中無憑證";
            case 5053: return "簽章值中無SignerInfo";
            case 5060: return "錯誤的憑證或金鑰";
            case 5061: return "簽章失敗";
            case 5062: return "驗章失敗";
            case 5063: return "加密失敗";
            case 5064: return "解密失敗";
            case 5065: return "產生金鑰失敗";
            case 5070: return "操作中止";
            case 5071: return "密碼錯誤";
            case 5080: return "無法剖析XML文件";
            case 5081: return "無法在XML中, 找到指定的標籤名稱";
            case 5401: return "CRL沒有發行者";
            case 5410: return "OCSP沒有簽章值";
            case 5411: return "OCSP沒有簽章憑證";
            case 5412: return "OCSP沒有原始資料";
            case 5413: return "OCSP回應不支援";
            case 5414: return "OCSP無效的回應";
            case 5415: return "OCSP沒有對應的憑證編號";
            case 5416: return "OCSP未知的憑證狀態";
            case 5417: return "OCSP無效的回應";
            case 5418: return "OCSP無效的回應";
            case 5420: return "OCSP無效的請求";
            case 5421: return "OCSP內部錯誤";
            case 5422: return "OCSP忙碌中請稍後再試";
            case 5423: return "OCSP資料不完整";
            case 5424: return "OCSP未授權";
            case 5425: return "OCSP未知的錯誤";
            case 5500: return "網路連線錯誤";
            case 5501: return "未知的主機";
            case 5502: return "網路連線錯誤";
            case 5503: return "網路傳送錯誤";
            case 5504: return "網路接收錯誤";
            case 5505: return "網路連線關閉";
            case 5509: return "無效狀態";
            case 5510: return "網路連線逾時";
            case 5901: return "Unicode錯誤"; 
            case 5902: return "找不到指定檔案或憑證載具";
            case 9001: return "中斷";
            case 9002: return "記憶體錯誤";
            case 9003: return "Slot Id不存在";
            case 9004: return "一般錯誤";
            case 9005: return "函數失敗";
            case 9006: return "錯誤的參數";
            case 9007: return "無事件";
            case 9008: return "需要建立Threads";
            case 9009: return "無法Lock";
            case 9010: return "唯讀屬性";
            case 9011: return "機密屬性";
            case 9012: return "屬性型態不正確";
            case 9013: return "屬性值不正確";
            case 9014: return "資料不正確";
            case 9015: return "資料長度不正確";
            case 9016: return "裝置不存在";
            case 9017: return "裝置記憶體不足";
            case 9018: return "裝置已拔除";
            case 9019: return "加密資料不正確";
            case 9020: return "被加密資料長度錯誤";
            case 9021: return "功能已取消";
            case 9022: return "功能無法並行";
            case 9023: return "功能不支援"; 
            case 9024: return "指定的金鑰不存在";
            case 9025: return "金鑰長度不正確";
            case 9026: return "金鑰種類不一致";
            case 9027: return "金鑰已不需要";
            case 9028: return "金鑰已變更";
            case 9029: return "需要金鑰";
            case 9030: return "無法接受的金鑰";
            case 9031: return "金鑰功能不被允許";
            case 9032: return "金鑰無法包裹";
            case 9033: return "金鑰無法匯出";
            case 9034: return "指定的機制不存在";
            case 9035: return "機制參數不正確";
            case 9036: return "指定的物件不存在";
            case 9037: return "操作運作中";
            case 9038: return "操作未初始化";
            case 9039: return "PIN碼錯誤";
            case 9040: return "PKCS#11 PIN碼未設定";
            case 9041: return "PIN碼錯誤";
            case 9042: return "密碼已過期";
            case 9043: return "PIN碼錯誤次數超過裝置設定次數";
            case 9044: return "與裝置的連線結束"; 
            case 9045: return "與裝置的連線次數";
            case 9046: return "指定的連線不存在";
            case 9047: return "Session不支援並行";
            case 9048: return "Session為唯讀";
            case 9049: return "指定的連線已存在";
            case 9050: return "Session為唯讀已存在";
            case 9051: return "Session可讀寫已存在";
            case 9052: return "簽章值不合法";
            case 9053: return "簽章值長度不正確";
            case 9054: return "樣板不完整";
            case 9055: return "樣板不一致";
            case 9056: return "裝置不存在(偵 測不到憑證載具 or 晶片接觸不良)";
            case 9057: return "卡片無法辨識(卡片或讀卡機接觸不良)";
            case 9058: return "對裝置作寫的動作時所使用的權限錯誤，可能是SO Pin錯誤或指定的裝置不可寫入";
            case 9059: return "解包裹的金鑰Handle不合法";
            case 9060: return "解包裹的金鑰長度不正確";
            case 9061: return "解包裹的金鑰種類不一致";
            case 9062: return "已登入裝置";
            case 9063: return "未登入裝置";
            case 9064: return "密碼未初始化";
            case 9065: return "使用者種類不正確";
            case 9066: return "另一個使用者已登入";
            case 9067: return "太多使用者種類";
            case 9068: return "包裹的金鑰不合法";
            case 9069: return "包裹的金鑰長度不正確";
            case 9070: return "包裹的金鑰Handle不合法";
            case 9071: return "包裹中的金鑰長度不正確";
            case 9072: return "包裹中的金鑰種類不一致";
            case 9073: return "不支援Random Seed";
            case 9074: return "無Random RNG";
            case 9075: return "記憶體緩衝區太小";
            case 9076: return "儲存狀態失敗";
            case 9077: return "資訊為機密";
            case 9078: return "狀態無法儲存";
            case 9079: return "函式庫尚未初始化過";
            case 9080: return "函式庫已經初始化過";
            case 9081: return "MUTEX壞了";
            case 9082: return "MUTEX無鎖住";
            case 9083: return "廠商已定義";
            case 9100: return "指定的物件不存在";
            case 9101: return "指定的物件已存在";
            case 9102: return "裝置中有兩個相同的物件";
            case 9110: return "載入函式庫失敗";
            case 9111: return "函式庫尚未載入";
            case 9999: return "未知的錯誤";
            default:
                return "(" + statusCode + ") 其它錯誤";
        }
    }
    var HCAPKCS11_MODULE = "HCAPKCS11.dll"
    var CKF_RW_SESSION = 0x00000002;
    var CKF_SERIAL_SESSION = 0x00000004;
    var CARD_TYPE_HPC = 1;
    var CKM_RSA_PKCS = 0x00000001;
    var CKM_SHA1_RSA_PKCS = 0x00000006;

    var CKM_SHA256_RSA_PKCS = 0x00000040
    var CKM_SHA384_RSA_PKCS = 0x00000041
    var CKM_SHA512_RSA_PKCS = 0x00000042
    var CKM_SHA224_RSA_PKCS = 0x00000046

    function ExecuteError(errFunc) {
        var errorCode = hca.ATL_GetErrorCode();
        if (errorCode != 0) {
            var data = {
                Result: errorCode,
                Message: GetMessage(errorCode) + " "
            };
            if (errFunc) errFunc(data);
            return true;
        }
        return false;
    }
    function getBWver() {
        var Sys = {};
        var ua = navigator.userAgent.toLowerCase();
        var s;
        //        alert(ua);
        (s = ua.match(/rv:([\d.]+)\) like gecko/)) ? Sys.ie = s[1] :
            (s = ua.match(/edge\/([\d.]+)/)) ? Sys.ie = s[1] :
                (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
                    (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
                        (s = ua.match(/opr\/([\d.]+)/)) ? Sys.opera = s[1] :
                            (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
                                //       (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
                                (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

        if (Sys.ie) return 'MSIE:' + Sys.ie;     //document.write('IE: ' + Sys.ie);
        if (Sys.firefox) return 'Firefox:' + Sys.firefox;//document.write('Firefox: ' + Sys.firefox);
        if (Sys.chrome) return 'Chrome:' + Sys.chrome; //document.write('Chrome: ' + Sys.chrome);
        if (Sys.opera) return 'Opera:' + Sys.opera;  //document.write('Opera: ' + Sys.opera);
        if (Sys.safari) return 'Safari:' + Sys.safari; //document.write('Safari: ' + Sys.safari);
    }

    this.BeginAuthHPC = function (pin, okFunc, errFunc) {
        var m_hModule = null;
        var m_hSession = null;
        var hPriKey = null;
        try {
            var bwstr = getBWver().split(':');
            if (bwstr[0] != "MSIE") {
                var data = {
                    Result: 0,
                    Message: '憑證登入只支援 IE 瀏覽器 ',
                };
                if (errFunc) errFunc(data);
                return;
            }
            if (!hca) {
                var data = {
                    Result: 0,
                    Message: 'HCA 元件尚未安裝或啟用',
                };
                if (errFunc) errFunc(data);
                return;
            }
            m_hModule = hca.ATL_InitModule(HCAPKCS11_MODULE, "");
            if (ExecuteError(errFunc)) {
                return;
            }
            m_hSession = hca.ATL_InitSession(m_hModule, CKF_SERIAL_SESSION, pin);
            if (ExecuteError(errFunc)) {
                hca.ATL_CloseModule(m_hModule);
                return;
            }

            var card_type = hca.ATL_GetHCACardType(m_hModule);
            if (ExecuteError(errFunc)) {
                hca.ATL_CloseSession(m_hModule, m_hSession);
                hca.ATL_CloseModule(m_hModule);
                return;
            }

            var data = hca.ATL_GetHCABasicData(m_hModule);
            if (ExecuteError(errFunc)) {
                hca.ATL_CloseSession(m_hModule, m_hSession);
                hca.ATL_CloseModule(m_hModule);
                return;
            }

            var now = new Date();
            var nowString = now.toISOString();
            hPriKey = hca.ATL_GetKeyObjectHandle(m_hModule, m_hSession, 0, "", "1");
            if (ExecuteError(errFunc)) {
                hca.ATL_CloseSession(m_hModule, m_hSession);
                hca.ATL_CloseModule(m_hModule);
                return;
            }

            var sign = hca.ATL_MakeSignatureEx2(m_hModule, m_hSession, CKM_SHA256_RSA_PKCS, 0, nowString, hPriKey);
            if (ExecuteError(errFunc)) {
                hca.ATL_DeleteKeyObject(m_hModule, m_hSession, hPriKey);
                hca.ATL_CloseSession(m_hModule, m_hSession);
                hca.ATL_CloseModule(m_hModule);
                return;
            }

            hca.ATL_DeleteKeyObject(m_hModule, m_hSession, hPriKey);
            hca.ATL_CloseSession(m_hModule, m_hSession);
            hca.ATL_CloseModule(m_hModule);

            if (card_type != CARD_TYPE_HPC) {
                var data = {
                    Result: 0,
                    Message: '卡片種類非醫事人員卡'
                };
                if (errFunc) errFunc(data);
                return;
            }

            var datas = data.toArray();
            var data = {
                idno: datas[4],
                now: nowString, 
                sign: sign,
            };

            if (okFunc) okFunc(data);
        } catch (error) {
            hca.ATL_DeleteKeyObject(m_hModule, m_hSession, hPriKey);
            hca.ATL_CloseSession(m_hModule, m_hSession);
            hca.ATL_CloseModule(m_hModule);
            var data = {
                Result: error,
                Message: GetMessage(error.data)
            };
            if (errFunc) errFunc(data);
        }        
    }
    this.BeginAuthHC = function (okFunc, errFunc) {
        try {
            var bwstr = getBWver().split(':');
            if (bwstr[0] != "MSIE") {
                var data = {
                    Result: 0,
                    Message: '憑證登入只支援 IE 瀏覽器 ',
                };
                if (errFunc) errFunc(data);
                return;
            }
            if (!twNHIICC) {
                var data = {
                    Result: 0,
                    Message: 'NHIICC 元件尚未安裝或啟用 ',
                };
                if (errFunc) errFunc(data);
                return;
            }

            var ver = twNHIICC.GetVersion();
            var basic = twNHIICC.GetBasic();
            var datas = basic.split(',');
            var data = {
                idno: datas[1]
            };
            if (okFunc) okFunc(data);
        } catch (error) {
            var data = {
                Result: 0,
                Message: 'NHIICC 發生異常錯誤 ',
            };
            if (errFunc) errFunc(data);
            return;
        }
        
    }
}