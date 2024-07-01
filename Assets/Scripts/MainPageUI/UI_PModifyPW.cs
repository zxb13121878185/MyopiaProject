using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 修改密码面板
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
    //修改密码
    public void Btn_Ok()
    {
        //先现在新密码和确认密码是同一个
        if (string.IsNullOrEmpty(iFOldPW.text) || string.IsNullOrEmpty(iFNewPW.text) || string.IsNullOrEmpty(iFNewPWConfirm.text))
        {
            string tempStr = "密码框不能为空！";
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempStr, textWarn));
            return;
        }
        if (iFNewPW.text != iFNewPWConfirm.text)
        {
            string tempStr = "两次输入的密码不一样";
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
            //登录失败，清空密码，更新验证码
            StopAllCoroutines();
            StartCoroutine(Exe_ShowWainInfo(tempReturnData.msg, textWarn));
        }
        else
        {
            string tempStr2 = "密码修改成功";
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
