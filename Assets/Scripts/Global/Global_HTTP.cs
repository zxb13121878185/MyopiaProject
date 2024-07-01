using Gloabal_EnumCalss;
using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class Global_HTTP
{
    public Action<string> Event_Error;
    public const string METHOD_GET = "GET";
    public const string METHOD_POST = "POST";//Http请求方法名
    public const string CONTENTTYPE_JSON = "application/json";
    public const string TOKENAME = "x-token";//Toke命令的名称不是value，value登入之后保存在xml
    public const string URCMDLSUFFIX_DELETE = "/del";//地址带命令的后缀
    public const string URCMDLSUFFIX_ADD = "/add";//地址带命令的后缀
    public const string URCMDLSUFFIX_MODIFY = "/modify";//地址带命令的后缀



    #region 一些固定地址
    public string URL_Login { get; private set; }
    public string URL_VerifyCode { get; private set; }//验证码地址

    #endregion
    public static Global_HTTP M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new Global_HTTP();
            }
            return _instance;
        }
    }
    private static Global_HTTP _instance;
    private string baseURL;
    private string loginTokeValue;

    private Global_HTTP()
    {
        baseURL = Global_XMLCtr.M_Instance.GetElementValue("HTTPBaseUrl");
        loginTokeValue = Global_XMLCtr.M_Instance.GetElementValue("TokeValue");
        URL_Login = Global_XMLCtr.M_Instance.GetElementValue("URL_Login");
        URL_VerifyCode= Global_XMLCtr.M_Instance.GetElementValue("URL_VerifyCode");
    }
    public HtResResult<T> Command<T>(HtReqParam htRParameter = null, string htURLParams = null)
    {
        HtResResult<T> tempRR = new HtResResult<T>();
        try
        {
            string tempURL = baseURL + htURLParams;
            //Debug.Log("url:" + tempURL);
            string tempStrResult = string.Empty;
            HttpWebRequest tempReq = (HttpWebRequest)WebRequest.Create(tempURL);
          //  tempReq.Headers.Add(TOKENAME, loginTokeValue);
            tempReq.ContentType = CONTENTTYPE_JSON;
            tempReq.Method = METHOD_POST;
            tempReq.KeepAlive = false;
            tempReq.Timeout = 500;
            tempReq.ReadWriteTimeout = 500;
            #region 添加Post 参数
            if (null != htRParameter)
            {
                string tempStrParam = Global_Manage.GetStrByJsonData(htRParameter);
              //  Debug.Log(tempStrParam);
                byte[] data = Encoding.UTF8.GetBytes(tempStrParam);
                tempReq.ContentLength = data.Length;
                using (Stream reqStream = tempReq.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)tempReq.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                tempStrResult = reader.ReadToEnd();
             //   Debug.Log(tempStrResult);
            }
            //将Json数据转换成对应的类
            tempRR = Global_Manage.GetJsonDataByStr<HtResResult<T>>(tempStrResult);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Event_Error?.Invoke(e.Message);
        }
        return tempRR;
    }
    public string PostData(string htURLParams, string dataStr)
    {
        HttpWebRequest tempHttpWebRequest = null;
        HttpWebResponse tempHttpWebResponse = null;
        string tempStrResult = null;
        try
        {
            string tempURL = baseURL + htURLParams;
            #region http web request

            byte[] data = Encoding.UTF8.GetBytes(dataStr);
            tempHttpWebRequest = (HttpWebRequest)WebRequest.Create(tempURL);
            tempHttpWebRequest.Method = METHOD_POST;
            tempHttpWebRequest.KeepAlive = false;
            tempHttpWebRequest.Timeout = 500;
            tempHttpWebRequest.ReadWriteTimeout = 500;
            // tempHttpWebRequest.Headers.Add(TOKENAME, loginTokeValue);
            tempHttpWebRequest.ContentType = CONTENTTYPE_JSON;
            tempHttpWebRequest.ServicePoint.Expect100Continue = false; //当服务器恢复正常时，访问已经是200时，这个线程还是返回操作超时
            tempHttpWebRequest.ContentLength = data.Length;
            using (Stream putStream = tempHttpWebRequest.GetRequestStream())
            {
                putStream.Write(data, 0, data.Length);
                putStream.Close();
            }

            tempHttpWebResponse = tempHttpWebRequest.GetResponse() as HttpWebResponse;
            HttpWebResponse resp = (HttpWebResponse)tempHttpWebRequest.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                tempStrResult = reader.ReadToEnd();
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            Event_Error?.Invoke(e.Message);
        }
        #endregion

        return tempStrResult;
    }
    public byte[] CmdGetImage(string htURLParams = null)
    {
        string tempURL = baseURL + htURLParams;
        //第一步：读取图片到byte数组
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tempURL);
        request.KeepAlive = true;
        request.Timeout = 500;
        request.ReadWriteTimeout = 500;
        byte[] bytes;
        using (Stream stream = request.GetResponse().GetResponseStream())
        {
            using (MemoryStream mstream = new MemoryStream())
            {
                int count = 0;
                byte[] buffer = new byte[1024];
                int readNum = 0;
                while ((readNum = stream.Read(buffer, 0, 1024)) > 0)
                {
                    count = count + readNum;
                    mstream.Write(buffer, 0, readNum);
                }
                mstream.Position = 0;
                using (BinaryReader br = new BinaryReader(mstream))
                {
                    bytes = br.ReadBytes(count);
                }
            }
        }
        return bytes;
    }

}