﻿using Gloabal_EnumCalss;
using Global_StructClass;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class Global_Manage : MonoBehaviour
{
    public AudioClip[] ScoreAudioClips;

    #region 属性
    /// <summary>
    /// 摧毁目标物体的数量
    /// </summary>
    public int M_NumDestroyObj
    {
        get
        {
            return numDestroyObj;
        }
    }
    public static Global_Manage M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<Global_Manage>();
            }
            return _instance;
        }

    }

    /// <summary>
    /// 获取跟工程相同目录的路径，如“E:/ZXB/MyProject/Asset/",去掉后面两个就得到和工程同级的目录为：“E:/ZXB”
    /// </summary>
    /// <returns></returns>
    public static string M_CurProjectAssetPath
    {
        get
        {
            if (string.Empty == curProjectAssetPath)
            {
                curProjectAssetPath = GetCurProjectFilePath();
            }
            return curProjectAssetPath;
        }
    }
    /// <summary>
    /// 当前JSON文件夹的地址
    /// </summary>
    public static string M_CurResourcesDataURL_JSON
    {
        get
        {
            if (string.Empty == curJSONRUL)
            {
                curJSONRUL = M_CurProjectAssetPath + @"\ResourcesData\JSON\";
            }
            return curJSONRUL;
        }
    }
    #endregion

    #region 私有变量
    private int numDestroyObj;
    private static string curProjectAssetPath = string.Empty;
    private static Global_Manage _instance;
    private static string curJSONRUL = string.Empty;
    private AudioSource curAudioSource;
    #endregion

    #region 系统方法
    // Start is called before the first frame update
    void Start()
    {
        curAudioSource = GetComponent<AudioSource>();

        #region 初始化各项物体

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region 测试


        if (Input.GetKeyDown(KeyCode.T))
        {
            /*
            Data_ListModule tempData = new Data_ListModule();
            tempData.Init(1);
            Data_Module data1 = new Data_Module();
            data1.Init(Enum_MODULE_TYPE.BOE, "Boe");
            tempData.ListDataModule.Add(data1);
            Data_Module data2 = new Data_Module();
            data2.Init(Enum_MODULE_TYPE.BOE, "Boe");
            tempData.ListDataModule.Add(data2);
            SaveData_JSON(tempData);
            */
            Data_XvisioConfig tempData1 = new Data_XvisioConfig();
            SaveData_JSON(tempData1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // tempDataList = ReadData_JSON<JSONData_ListModule>();
        }

        #endregion
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion

    #region 私有方法

    /// <summary>
    /// 获取跟工程相同目录的路径，如“E:/ZXB/MyProject/Asset/",去掉后面两个就得到和工程同级的目录为：“E:/ZXB”
    /// </summary>
    /// <returns></returns>
    private static string GetCurProjectFilePath()
    {
        string[] strTempPath = Application.dataPath.Split('/');
        string strPath = string.Empty;
        //去掉后面两个，获取跟工程相同目录的路径，如“E:/ZXB/MyProject/Asset/",去掉后面两个就得到和工程同级的目录为：“E:/ZXB”
        for (int i = 0; i < strTempPath.Length - 2; i++)
        {
            strPath += strTempPath[i] + "/";
        }
        return strPath;
    }
    #endregion

    #region 公有方法
    public void PlaySound_Score()
    {
        int tempIndex = UnityEngine.Random.Range(0, ScoreAudioClips.Length + 1);
        curAudioSource.Stop();
        curAudioSource.PlayOneShot(ScoreAudioClips[tempIndex]);
    }
    public void AddDestroy()
    {

    }
    /// <summary>
    /// 将结构体数据保存成JSON文件
    /// </summary>
    /// <param name="JSONFilePath"></param>
    /// <param name="data"></param>
    public static bool SaveData_JSON(object data)
    {
        bool isSaveSucced = true;
        string JSONFilePath = Global_Windows.M_Instance.Open_WindowFile("json", true);
        #region 将结构体添加数据转换成JSON文件并存储
        try
        {
            FileInfo file = new FileInfo(JSONFilePath);
            //判断有没有文件，有则打开文件，，没有创建后打开文件
            StreamWriter sw = file.CreateText();
            string json = string.Empty;
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            json = JsonConvert.SerializeObject(data, settings);
            //   Debug.Log(json);
            //将转换好的字符串存进文件，
            sw.WriteLine(json);
            //注意释放资源
            sw.Close();
            sw.Dispose();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            isSaveSucced = false;
        }
        return isSaveSucced;
        #endregion
    }
    /// <summary>
    /// 读取文件夹里的JSON文件，并转换成对应的结构体,
    /// 默认没有路径的会自动打开文件夹选择窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadData_JSON<T>(string jsonURL = null)
    {
        T tempT = default(T);
        string JSONFilePath = jsonURL;
        if (null == JSONFilePath)
        {
            JSONFilePath = Global_Windows.M_Instance.Open_WindowFile("json", false);
        }
        try
        {
            StreamReader sr = new StreamReader(JSONFilePath, Encoding.UTF8);
            string tempStrData = sr.ReadToEnd();
            if (tempStrData.Length == 0)
            {
                Global_Windows.MessageBox(IntPtr.Zero, "JSON文件内容为空!", "确认", 0);
                return tempT;
            }
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            tempT = JsonConvert.DeserializeObject<T>(tempStrData, settings);
            //   Debug.Log(tempStrData);
        }
        catch (Exception e)
        {
            Global_Windows.MessageBox(IntPtr.Zero, "读取JSON异常" + e.Message, "确认", 0);
            return tempT;
        }
        //  Global_Windows.MessageBox(IntPtr.Zero, "读取JSON数据成功", "确认", 0);
        return tempT;
    }
    #endregion
}
