using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �޸��������
/// </summary>
public class UI_PModifyPW : GlobalBaseObj
{
    [SerializeField] InputField iFOldPW;
    [SerializeField] InputField iFNewPW;
    [SerializeField] InputField iFNewPWConfirm;
    [SerializeField] Text textWarn;
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }
    private IEnumerator Exe_ShowWainInfo(string tempStr, Text textWarn)
    {
        textWarn.gameObject.SetActive(true);
        textWarn.text = tempStr;
        yield return new WaitForSeconds(2.0f);
        textWarn.gameObject.SetActive(false);
    }
    public void Set_Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    //�޸�����
    public void Btn_Ok()
    {
        //�������������ȷ��������ͬһ��
        if (string.IsNullOrEmpty(iFOldPW.text) || string.IsNullOrEmpty(iFNewPW.text) || string.IsNullOrEmpty(iFNewPWConfirm.text))
        {
            string tempStr = "�������Ϊ�գ�";
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempStr, textWarn));
            return;
        }
        if (iFNewPW.text != iFNewPWConfirm.text)
        {
            string tempStr = "������������벻һ��";
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempStr, textWarn));
            return;
        }
        HtP_UpdatePW tempData = new HtP_UpdatePW();
        tempData.curpwd = iFOldPW.text;
        tempData.pwd = iFNewPW.text;
        string tempStrData = Global_Manage.GetStrByJsonData(tempData);
        //  Global_HttpCtr.Instance.Post(Global_HttpCtr.Instance.URL_ModifyPW, tempStrData, CallBack_UpdatePW);
    }
    private void CallBack_UpdatePW(byte[] data)
    {
        if (null == data) return;
        string tempStr = Encoding.UTF8.GetString(data);
        HtResResult<HtR_PModifyPW> tempReturnData = Global_Manage.GetJsonDataByStr<HtResResult<HtR_PModifyPW>>(tempStr);
        if (200 != tempReturnData.code)
        {
            //��¼ʧ�ܣ�������룬������֤��
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempReturnData.msg, textWarn));
        }
        else
        {
            string tempStr2 = "�����޸ĳɹ�";
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempStr2, textWarn));
        }
        iFNewPW.text = null;
        iFOldPW.text = null;
        iFNewPWConfirm.text = null;
    }
    public override void Init(bool isActive = true)
    {
        base.Init(isActive);
        textWarn.gameObject.SetActive(false);
    }
}
