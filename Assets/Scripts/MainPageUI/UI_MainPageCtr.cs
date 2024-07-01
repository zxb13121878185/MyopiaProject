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
/// ��ҳ����
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
    public XRController[] xrCtr;//0����1Ϊ����
    #region ����

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
    [SerializeField] TrackedDeviceGraphicRaycaster[] subCanvasTDGR;//�Ӳ˵��Ľ�������0ΪMain��1δPop-up
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        #region ����


        #endregion
    }

    public void Open_SetPanel()
    {
        M_SubPS.Show(true);
        subCanvasTDGR[0].enabled = false;

        //string tempFileName = "HtR_Login.json";
        ////   Global_XMLCtr.M_Instance.SetElementValue("LoginUserName", "������");
        //HtR_Login tempData = new HtR_Login();
        //tempData.token = "ssssssssssss";
        //tempData.userName = "������";
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

        SubPLoginRegis.Init(tempIsShowLogin);//����Ǳ��ذ�ľͲ���ʾ����ע��
        SubPBarMenu.Init(!tempIsShowLogin);//����Ǳ��ذ��ֱ����ʾ���˵�

        //  text.text = Global_XMLCtr.M_Instance.GetElementValue("LoginUserName");

        //  Debug.unityLogger.logEnabled = false;
    }
}
