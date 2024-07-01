using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登入注册面板
/// </summary>
public class UI_PLoginRegis : GlobalBaseObj
{
    public static UI_PLoginRegis M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<UI_PLoginRegis>(true);
            }
            return _instance;
        }
    }
    private static UI_PLoginRegis _instance;
    public string M_CurUserName { get; private set; }

    [Space(10)]
    [SerializeField] GameObject subPLogin;

    [Space(10)]
    [SerializeField] InputField iFLoginUser;
    [SerializeField] InputField iFLoginPassword;
    [SerializeField] InputField iFLoginVerifyCode;
    [SerializeField] RawImage imgLoginVerifyCode;
    [SerializeField] Dropdown DdListHospital;

    [Space(10)]
    [SerializeField] Text textLoginWarn;
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        #region 测试

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Btn_VFLoginClick();
        //}

        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    Btn_LoginClick();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Btn_RegisClick();
        //}

        #endregion
    }
    //跳转到登录面板
    public void Btn_GoLoginPanel()
    {
        subPLogin.SetActive(true);
    }

    //调转到注册面板
    public void Btn_GoRegisPanelClick()
    {
        subPLogin.SetActive(false);
    }
    /// <summary>
    /// 退出登录
    /// </summary>
    public void LoginOut()
    {
        //
        M_CurUserName = "";
        //先显示登录注册的父物体
        UI_MainPageCtr.M_Instance.SubPLoginRegis.gameObject.SetActive(true);
        subPLogin.SetActive(true);
        //设置登录状态为
        Global_Manage.IsLogining = false;
        UI_MainPageCtr.M_Instance.SubPBarMenu.gameObject.SetActive(false);

        HtP_LoginOut tempLoginOut = new HtP_LoginOut();
        tempLoginOut.Authorization = Global_HttpCtr.Instance.LoginReturnData.data.authorization;
        string tempStrData = Global_Manage.GetStrByJsonData(tempLoginOut);
        Global_HttpCtr.Instance.Post(Global_HttpCtr.Instance.URL_LoginOut, tempStrData, null);
    }
    public void Btn_LoginClick()
    {
        HtP_Login tempLogin = new HtP_Login();
        tempLogin.userName = iFLoginUser.text;
        tempLogin.password = Convert.ToBase64String(Encoding.UTF8.GetBytes(iFLoginPassword.text));
        // tempLogin.verifyCode = iFLoginVerifyCode.text;
        string tempStrData = Global_Manage.GetStrByJsonData(tempLogin);
        Global_HttpCtr.Instance.Post(Global_HttpCtr.Instance.URL_Login, tempStrData, CallBack_Login);
    }
    private void CallBack_Login(byte[] data)
    {
        if (null == data) return;
        string tempStr = Encoding.UTF8.GetString(data);
        HtResResult<HtR_Login> tempReturnData = Global_Manage.GetJsonDataByStr<HtResResult<HtR_Login>>(tempStr);
        Global_HttpCtr.Instance.LoginReturnData = tempReturnData;
        if (200 != tempReturnData.code)
        {
            //登录失败，清空密码，更新验证码
            Btn_VFLoginClick();
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempReturnData.msg, textLoginWarn));
            iFLoginPassword.text = null;
            iFLoginUser.text = null;
            return;
        }
        //登入成功
        M_CurUserName = iFLoginUser.text;
        subPLogin.SetActive(false);
        //设置登录状态为true
        Global_Manage.IsLogining = true;
        UI_MainPageCtr.M_Instance.SubPBarMenu.gameObject.SetActive(true);
        //将当前登入信息写入xml文件
        Global_XMLCtr.M_Instance.SetElementValue("LastLoginUserName", iFLoginUser.text);
        Global_XMLCtr.M_Instance.SetElementValue("LastLoginPassword", iFLoginPassword.text);
        //更改授权值
        Global_HttpCtr.Instance.LoginChangeAuthorization();
        //读取用户的配置文件
        Global_Manage.M_UserSetInfo = tempReturnData.data.userSetInfo;
    }
    private void CallBack_GetHospital(byte[] data)
    {
        string tempStr = Encoding.UTF8.GetString(data);
        HtResResult<string[]> tempReturnData = Global_Manage.GetJsonDataByStr<HtResResult<string[]>>(tempStr);
        if (200 != tempReturnData.code)
        {
            string tempError = "读取医院列表失败！";
            //   StartCoroutine(Exe_ShowWainInfo(tempError, textRegisWarn));
            return;
        }
        //如果成功了将获取到的值赋给医院下拉列表
        List<string> tempListHospital = new List<string>();
        for (int i = 0; i < tempReturnData.data.Length; i++)
        {
            tempListHospital.Add(tempReturnData.data[i]);
        }
        DdListHospital.AddOptions(tempListHospital);
    }
    private IEnumerator Exe_ShowWainInfo(string tempStr, Text textWarn)
    {
        textWarn.gameObject.SetActive(true);
        textWarn.text = tempStr;
        yield return new WaitForSeconds(2.0f);
        textWarn.gameObject.SetActive(false);
    }
    //登录的验证码按钮
    public void Btn_VFLoginClick()
    {
        //暂时去掉VR客户端登录、注册的验证码功能2022/12/12
        // Global_HttpCtr.Instance.Get(Global_HttpCtr.Instance.URL_VerifyCode, GetLoginVerifyCodeData);
    }
    private void GetLoginVerifyCodeData(byte[] data)
    {
        int tempW = (int)imgLoginVerifyCode.rectTransform.sizeDelta.x;
        int tempH = (int)imgLoginVerifyCode.rectTransform.sizeDelta.y;
        Texture2D tempTex = new Texture2D(tempW, tempH);
        tempTex.LoadImage(data);
        imgLoginVerifyCode.texture = tempTex;
    }
    //注册的验证码
    public void Btn_VFRegisClick()
    {
        //暂时去掉VR客户端登录、注册的验证码功能2022/12/12
        //  Global_HttpCtr.Instance.Get(Global_HttpCtr.Instance.URL_VerifyCode, GetRegisVFData);
    }
    public override void Init(bool isActive = true)
    {
        base.Init(isActive);
        subPLogin.SetActive(true);
        Btn_VFLoginClick();
        //初始化输入框
        iFLoginUser.text = Global_HttpCtr.Instance.M_LastLoginUserName;
        iFLoginPassword.text = Global_HttpCtr.Instance.M_LastLoginPassword;
        // textRegisWarn.gameObject.SetActive(false);
        textLoginWarn.gameObject.SetActive(false);
    }
}
