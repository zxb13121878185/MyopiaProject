using Gloabal_EnumCalss;
using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelSetting : GlobalBaseObj
{

    #region 左右眼设置

    [SerializeField] CamBSC[] camBSC;//0为左眼、1为右眼
    [SerializeField] Text[] textLeft;//从0~2分别为对比度、亮度、饱和度，下面的数组一样
    [SerializeField] Text[] textRight;
    [SerializeField] Slider[] sliderLeft;
    [SerializeField] Slider[] sliderRight;

    #endregion


    public void LeftEyeChangeValue(int index)
    {
        float tempValue = sliderLeft[index].value;
        textLeft[index].text = tempValue.ToString("f1");
        switch (index)
        {
            case 0:
                camBSC[0].contrast = tempValue;
                break;
            case 1:
                camBSC[0].brightness = tempValue;
                break;
            case 2:
                camBSC[0].saturation = tempValue;
                break;
            default:
                break;
        }
    }
    public void Btn_ResetLeftEye()
    {
        for (int i = 0; i < sliderLeft.Length; i++)
        {
            sliderLeft[i].value = 1;
        }
    }
    public void Btn_ResetRightEye()
    {
        for (int i = 0; i < sliderRight.Length; i++)
        {
            sliderRight[i].value = 1;
        }
    }
    public void RightEyeChangeVaule(int index)
    {
        float tempValue = sliderRight[index].value;
        textRight[index].text = tempValue.ToString("f1");
        switch (index)
        {
            case 0:
                camBSC[1].contrast = tempValue;
                break;
            case 1:
                camBSC[1].brightness = tempValue;
                break;
            case 2:
                camBSC[1].saturation = tempValue;
                break;
            default:
                break;
        }
    }
    public void Show(bool isShow)
    {
        UI_MainPageCtr.M_Instance.M_SubPS.gameObject.SetActive(isShow);

        if (Global_Manage.M_Instance.M_IsLocalVersion)
        {
            float tempL1 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Left_Contrast"));
            float tempL2 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Left_Brightness"));
            float tempL3 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Left_Saturation"));
            float tempR1 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Right_Contrast"));
            float tempR2 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Right_Brightness"));
            float tempR3 = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("Right_Saturation"));

            sliderLeft[0].value = tempL1;
            sliderLeft[1].value = tempL2;
            sliderLeft[2].value = tempL3;
            sliderRight[0].value = tempR1;
            sliderRight[1].value = tempR2;
            sliderRight[2].value = tempR3;
        }
        else
        {
            //从登录信息里获取用户设置的参数
            UserSetInfo tempData = Global_Manage.M_UserSetInfo;
            sliderLeft[0].value = tempData.contrastLeft;
            sliderLeft[1].value = tempData.brightLeft;
            sliderLeft[2].value = tempData.saturationLeft;
            sliderRight[0].value = tempData.contrastRight;
            sliderRight[1].value = tempData.brightRight;
            sliderRight[2].value = tempData.saturationRight;
        }
    }
    public void Btn_SaveConfig()
    {
        //默认无论哪个模式（本地还是网络版本）都会存储到XML文件中
        Global_XMLCtr.M_Instance.SetElementValue("Left_Contrast", sliderLeft[0].value.ToString("f1"));
        Global_XMLCtr.M_Instance.SetElementValue("Left_Brightness", sliderLeft[1].value.ToString("f1"));
        Global_XMLCtr.M_Instance.SetElementValue("Left_Saturation", sliderLeft[2].value.ToString("f1"));
        Global_XMLCtr.M_Instance.SetElementValue("Right_Contrast", sliderRight[0].value.ToString("f1"));
        Global_XMLCtr.M_Instance.SetElementValue("Right_Brightness", sliderRight[1].value.ToString("f1"));
        Global_XMLCtr.M_Instance.SetElementValue("Right_Saturation", sliderRight[2].value.ToString("f1"));

        //如果不是单机版，存储到数据库中
        if (!Global_Manage.M_Instance.M_IsLocalVersion)
        {
            UserSetInfo tempData = new UserSetInfo();
            tempData.brightLeft = sliderLeft[1].value;
            tempData.brightRight = sliderRight[1].value;
            tempData.contrastLeft = sliderLeft[0].value;
            tempData.contrastRight = sliderRight[0].value;
            tempData.saturationLeft = sliderLeft[2].value;
            tempData.saturationRight = sliderRight[2].value;

            Global_Manage.M_UserSetInfo = tempData;
            //  tempData.userId = 10086;
            //  tempData.versionId = 0;
            string tempStr = Global_Manage.GetStrByJsonData(tempData);
            // Debug.Log(tempStr);
            Global_HttpCtr.Instance.Post(Global_HttpCtr.Instance.URL_UpLoadSysSetting, tempStr, CallBack_UpLoadSysSet);
        }

        Btn_Close();


    }
    private void CallBack_UpLoadSysSet(byte[] data)
    {
        if (null == data) return;
        //string tempStr = Encoding.UTF8.GetString(data);
        ////  Debug.Log(tempStr);
    }
    public void Btn_Close()
    {
        UI_MainPageCtr.M_Instance.Close_SetPanel();
    }
    public void RefreshConfig()
    {
        for (int i = 0; i < 3; i++)
        {
            LeftEyeChangeValue(i);
            RightEyeChangeVaule(i);
        }
    }
    public void Toggle_IsLocalVersion()
    {

    }
    protected override void Update_State()
    {
        #region 测试

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Btn_SaveConfig();
        //}

        #endregion
    }

    protected override void LateUpdate_State()
    {

    }

    protected override void FixedUpdate_State()
    {

    }
    public override void Init(bool isActive)
    {
        base.Init(isActive);

        RefreshConfig();

    }
}
