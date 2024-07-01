using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ע�����
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
        #region ����

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
    //��ת����¼���
    public void Btn_GoLoginPanel()
    {
        subPLogin.SetActive(true);
    }

    //��ת��ע�����
    public void Btn_GoRegisPanelClick()
    {
        subPLogin.SetActive(false);
    }
    /// <summary>
    /// �˳���¼
    /// </summary>
    public void LoginOut()
    {
        //
        M_CurUserName = "";
        //����ʾ��¼ע��ĸ�����
        UI_MainPageCtr.M_Instance.SubPLoginRegis.gameObject.SetActive(true);
        subPLogin.SetActive(true);
        //���õ�¼״̬Ϊ
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
            //��¼ʧ�ܣ�������룬������֤��
            Btn_VFLoginClick();
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempReturnData.msg, textLoginWarn));
            iFLoginPassword.text = null;
            iFLoginUser.text = null;
            return;
        }
        //����ɹ�
        M_CurUserName = iFLoginUser.text;
        subPLogin.SetActive(false);
        //���õ�¼״̬Ϊtrue
        Global_Manage.IsLogining = true;
        UI_MainPageCtr.M_Instance.SubPBarMenu.gameObject.SetActive(true);
        //����ǰ������Ϣд��xml�ļ�
        Global_XMLCtr.M_Instance.SetElementValue("LastLoginUserName", iFLoginUser.text);
        Global_XMLCtr.M_Instance.SetElementValue("LastLoginPassword", iFLoginPassword.text);
        //������Ȩֵ
        Global_HttpCtr.Instance.LoginChangeAuthorization();
        //��ȡ�û��������ļ�
        Global_Manage.M_UserSetInfo = tempReturnData.data.userSetInfo;
    }
    private void CallBack_GetHospital(byte[] data)
    {
        string tempStr = Encoding.UTF8.GetString(data);
        HtResResult<string[]> tempReturnData = Global_Manage.GetJsonDataByStr<HtResResult<string[]>>(tempStr);
        if (200 != tempReturnData.code)
        {
            string tempError = "��ȡҽԺ�б�ʧ�ܣ�";
            //   StartCoroutine(Exe_ShowWainInfo(tempError, textRegisWarn));
            return;
        }
        //����ɹ��˽���ȡ����ֵ����ҽԺ�����б�
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
    //��¼����֤�밴ť
    public void Btn_VFLoginClick()
    {
        //��ʱȥ��VR�ͻ��˵�¼��ע�����֤�빦��2022/12/12
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
    //ע�����֤��
    public void Btn_VFRegisClick()
    {
        //��ʱȥ��VR�ͻ��˵�¼��ע�����֤�빦��2022/12/12
        //  Global_HttpCtr.Instance.Get(Global_HttpCtr.Instance.URL_VerifyCode, GetRegisVFData);
    }
    public override void Init(bool isActive = true)
    {
        base.Init(isActive);
        subPLogin.SetActive(true);
        Btn_VFLoginClick();
        //��ʼ�������
        iFLoginUser.text = Global_HttpCtr.Instance.M_LastLoginUserName;
        iFLoginPassword.text = Global_HttpCtr.Instance.M_LastLoginPassword;
        // textRegisWarn.gameObject.SetActive(false);
        textLoginWarn.gameObject.SetActive(false);
    }
}
