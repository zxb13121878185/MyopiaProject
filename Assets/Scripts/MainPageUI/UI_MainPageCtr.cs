using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Global_StructClass;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 主页控制
/// </summary>
public class UI_MainPageCtr : GlobalBaseObj
{
    public UI_PanelSetting M_SubPS
    {
        get
        {
            if (null == panelSetObj)
            {
                panelSetObj = gameObject.GetComponentInChildren<UI_PanelSetting>(true);
            }
            return panelSetObj;
        }
    }
    public UI_PLoginRegis SubPLoginRegis;
    public UI_PBarMenu SubPBarMenu;
    public XRController[] xrCtr;//0左手1为右手
    #region 测试

    //public Text text;

    #endregion
    public static UI_MainPageCtr M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<UI_MainPageCtr>(true);
            }
            return _instance;
        }
    }
    private static UI_MainPageCtr _instance;
    private UI_PanelSetting panelSetObj;
    [SerializeField] TrackedDeviceGraphicRaycaster[] subCanvasTDGR;//子菜单的交互开关0为Main、1未Pop-up
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        #region 测试


        #endregion
    }

    public void Open_SetPanel()
    {
        M_SubPS.Show(true);
        subCanvasTDGR[0].enabled = false;

        //string tempFileName = "HtR_Login.json";
        ////   Global_XMLCtr.M_Instance.SetElementValue("LoginUserName", "测试下");
        //HtR_Login tempData = new HtR_Login();
        //tempData.token = "ssssssssssss";
        //tempData.userName = "测试名";
        //  Global_Manage.SaveData_JSON(tempData, tempFileName);
    }

    public void Close_SetPanel()
    {
        M_SubPS.gameObject.SetActive(false);
        subCanvasTDGR[0].enabled = true;
        //string tempStr = null;
        //string tempFileName = Global_Manage.M_URLJSON + "HtR_Login.json";
        //HtP_Login tempData1 = Global_Manage.ReadData_JSON<HtP_Login>(out tempStr, tempFileName);
        //text.text = tempStr +"  Us:"+ tempData1.userName;
        //  text.text = GetAllFileInfos();
    }

    public void GoGameScene(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void GoCalibration()
    {
        openPackage("com.tobii.usercalibration.pico");
        openPackage("com.tobii.usercalibration.neo3");
    }
    public void openPackage(string pkgName)
    {
        using (AndroidJavaClass jcPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject joActivity = jcPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject joPackageManager = joActivity.Call<AndroidJavaObject>("getPackageManager"))
                {
                    using (AndroidJavaObject joIntent = joPackageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", pkgName))
                    {
                        if (null != joIntent)
                        {
                            joActivity.Call("startActivity", joIntent);
                        }
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Time.timeScale = 1;
        M_SubPS.Init(false);

        bool tempIsShowLogin = !Global_Manage.M_Instance.M_IsLocalVersion && !Global_Manage.IsLogining;

        SubPLoginRegis.Init(tempIsShowLogin);//如果是本地版的就不显示登入注册
        SubPBarMenu.Init(!tempIsShowLogin);//如果是本地版的直接显示主菜单

        //  text.text = Global_XMLCtr.M_Instance.GetElementValue("LoginUserName");

        //  Debug.unityLogger.logEnabled = false;
    }
}
