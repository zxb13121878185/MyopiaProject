using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.XR.Interaction.Toolkit;
using Global_StructClass;

/// <summary>
/// Http Request SDK 
/// </summary>
public class Global_HttpCtr : MonoBehaviour
{
    /// <summary>
    /// ����ĺ�׺��ַ
    /// </summary>
    public string URL_Login { get; private set; }
    public string URL_LoginOut { get; private set; }
    /// <summary>
    /// ��֤���׺��ַ
    /// </summary>
    public string URL_VerifyCode { get; private set; }//��֤���ַ
    /// <summary>
    ///ע���ַ
    /// </summary>
    public string URL_Regis { get; private set; }
    public string URL_DataFruits { get; private set; }
    public string URL_DataForest { get; private set; }
    public string URL_DataForest2 { get; private set; }//ɭ�ֻ���2��Ϸ����
    public string URL_DataBalloon { get; private set; }
    public string URL_DataBalloon2 { get; private set; }//����2��Ϸ����
    public string URL_UpLoadSysSetting { get; private set; }
    //public string URL_ModifyPW { get; private set; }
    public string URL_SetGameLevel { get; private set; }
    public string M_LastLoginUserName { get; private set; }
    public string M_LastLoginPassword { get; private set; }
    public static Global_HttpCtr Instance
    {
        get
        {
            if (_instacne == null)
            {
                _instacne = FindObjectOfType<Global_HttpCtr>(true);
            }
            return _instacne;
        }
    }
    private static Global_HttpCtr _instacne = null;
    private string baseURL = string.Empty;
    Dictionary<string, string> requestHeader = new Dictionary<string, string>();  //  header

    [HideInInspector]

    public HtResResult<HtR_Login> LoginReturnData;//���뷵�ص�����

    void Awake()
    {
        baseURL = Global_XMLCtr.M_Instance.GetElementValue("HTTPBaseUrl");
        URL_Login = Global_XMLCtr.M_Instance.GetElementValue("URL_Login");
        URL_LoginOut = Global_XMLCtr.M_Instance.GetElementValue("URL_LoginOut");
        //   URL_VerifyCode = Global_XMLCtr.M_Instance.GetElementValue("URL_VerifyCode");
        //   URL_Regis = Global_XMLCtr.M_Instance.GetElementValue("URL_Regis");
        URL_UpLoadSysSetting = Global_XMLCtr.M_Instance.GetElementValue("URL_UpLoadSysSetting");
        URL_SetGameLevel = Global_XMLCtr.M_Instance.GetElementValue("URL_SetGameLevel");
        URL_DataFruits = Global_XMLCtr.M_Instance.GetElementValue("URL_DataFruits");
        URL_DataForest = Global_XMLCtr.M_Instance.GetElementValue("URL_DataForest");
        URL_DataForest2 = Global_XMLCtr.M_Instance.GetElementValue("URL_DataForest2");

        URL_DataBalloon = Global_XMLCtr.M_Instance.GetElementValue("URL_DataBalloon");
        URL_DataBalloon2 = Global_XMLCtr.M_Instance.GetElementValue("URL_DataBalloon2");
        //   URL_ModifyPW = Global_XMLCtr.M_Instance.GetElementValue("URL_ModifyPW");
        M_LastLoginUserName = Global_XMLCtr.M_Instance.GetElementValue("LastLoginUserName");
        M_LastLoginPassword = Global_XMLCtr.M_Instance.GetElementValue("LastLoginPassword");

        DontDestroyOnLoad(gameObject);
        //http header ������
        requestHeader.Add("Content-Type", "application/json");
        requestHeader.Add("Authorization", "");

    }

    public void Get(string otherURLName, Action<byte[]> callback)
    {
        StartCoroutine(GetRequest(otherURLName, callback));
    }
    public IEnumerator GetRequest(string otherURLName, Action<byte[]> callback)
    {
        string url = baseURL + otherURLName;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            //����header
            foreach (var v in requestHeader)
            {
                webRequest.SetRequestHeader(v.Key, v.Value);
            }
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(webRequest.downloadHandler.data);
                }
            }
        }
    }
    /// <summary>
    /// ����ɹ����޸�Authorizationֵ
    /// </summary>
    public void LoginChangeAuthorization()
    {
        if (null == LoginReturnData) return;
        requestHeader["Authorization"] = LoginReturnData.data.authorization;
    }
    //jsonString Ϊjson�ַ�����post�ύ�����ݰ�Ϊjson
    public void Post(string otherURLName, string jsonString, Action<byte[]> callback)
    {
        StartCoroutine(PostRequest(otherURLName, jsonString, callback));
    }
    public IEnumerator PostRequest(string otherURLName, string jsonString, Action<byte[]> callback)
    {
        string tempUrl = baseURL + otherURLName;
        // Debug.Log(string.Format("url:{0} postData:{1}",url,jsonString));
        using (UnityWebRequest webRequest = new UnityWebRequest(tempUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            foreach (var v in requestHeader)
            {
                webRequest.SetRequestHeader(v.Key, v.Value);
            }
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(webRequest.downloadHandler.data);
                }
            }
        }
    }
}

