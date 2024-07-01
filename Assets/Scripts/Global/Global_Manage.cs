using Gloabal_EnumCalss;
using Global_StructClass;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 全局管理类
/// </summary>
public class Global_Manage
{
    public const string TIMEFORMAT = "yyyy-MM-dd HH:mm:ss";
    public static bool IsLogining = false;
    public static UserSetInfo M_UserSetInfo = new UserSetInfo();
    #region 属性
    /// <summary>
    /// 课件名分隔符
    /// </summary>
    public const string COURSEWARENAMESPLIT = "-";
    /// <summary>
    /// 课件后缀
    /// </summary>
    public const string COURSEWARESUFFIX = ".xrcw";
    /// <summary>
    /// 双击的间隔时间
    /// </summary>
    public static float M_TimeDoubleClickInterval => 0.25f;
    public static Global_Manage M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new Global_Manage();
                bool.TryParse(Global_XMLCtr.M_Instance.GetElementValue("IsLocalVersion"), out isLocalVersion);
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
    /// AssetBundle地址
    /// </summary>
    public static string M_URLAB
    {
        get
        {
            if (string.Empty == curURLAB)
            {
                curURLAB = M_CurProjectAssetPath + @"/AssetBundle/";
                if(!Directory.Exists(curURLAB))
                {
                    Directory.CreateDirectory(curURLAB);
                }
            }
            return curURLAB;
        }
    }
    /// <summary>
    /// 当前JSON文件夹的地址
    /// </summary>
    public static string M_URLJSON
    {
        get
        {
            if (string.Empty == curJSONRUL)
            {
                curJSONRUL = M_CurProjectAssetPath + "/JSON/";
                if (!Directory.Exists(curJSONRUL))
                {
                    Directory.CreateDirectory(curJSONRUL);
                }
            }
            return curJSONRUL;
        }
    }
    public static string M_DbName
    {
        get
        {
            if(string.IsNullOrEmpty(DbName))
            {
                DbName = Global_XMLCtr.M_Instance.GetElementValue("DbName");
            }
            return DbName;
        }
    }
    public static string M_CurAdviceID
    {
        get
        {
            if(string.IsNullOrEmpty(curAdviceID))
            {
                curAdviceID = Global_XMLCtr.M_Instance.GetElementValue("CurAdviceID");
            }
            return curAdviceID;
        }
    }
    /// <summary>
    /// 虚拟3D搜狗输入法键盘的偏离高度
    /// </summary>
    public static float M_HeigitKeyboard
    {
        get
        {
            if (float.IsNaN(heightKeyboard))
            {
                heightKeyboard = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("heightKeyboard"));
            }
            return heightKeyboard;
        }
    }
    public bool M_IsLocalVersion
    {
        get
        {
            return isLocalVersion;
        }
    }
    #endregion

    #region 私有变量

    /// <summary>
    /// 网络二进制数据长度
    /// </summary>
    private static int ReceiveByteLength;
    /// <summary>
    /// 网络二进制数据转换为结构体的长度
    /// </summary>
    private static int StructTypeLength;
    private static string curProjectAssetPath = string.Empty;
    private static Global_Manage _instance;
    private static string curJSONRUL = string.Empty;
    private static string curURLAB = string.Empty;
    private static string DbName = string.Empty;
    private static float heightKeyboard = float.NaN;
    private static bool isLocalVersion = false;//是否为本地不联网版本的游戏
    private static string curAdviceID = string.Empty;
    #endregion

    #region 私有方法

    /// <summary>
    /// 获取跟工程相同目录的路径，如“E:/ZXB/MyProject/Asset/",去掉后面两个就得到和工程同级的目录为：“E:/ZXB/”
    /// </summary>
    /// <returns>返回字符串</returns>
    private static string GetCurProjectFilePath()
    {
        string strPath = string.Empty;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        string[] strTempPath = Application.dataPath.Split('/');
        //去掉后面两个，获取跟工程相同目录的路径，如“E:/ZXB/MyProject/Asset/",去掉后面两个就得到和工程同级的目录为：“E:/ZXB”
        for (int i = 0; i < strTempPath.Length - 2; i++)
        {
            strPath += strTempPath[i] + "/";
        }
#elif UNITY_ANDROID
        // strPath = "file:///storage/emulated/0/";
        strPath = Application.persistentDataPath;

#endif
        return strPath;
    }
    #endregion

    #region 公有方法

    public static string GetAndroidID()
    {
        string _strAndroidID =string.Empty;
        if (string.IsNullOrEmpty(_strAndroidID))
        {
            _strAndroidID = "none";
#if (UNITY_ANDROID && !UNITY_EDITOR) || ANDROID_CODE_VIEW
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver"))
                        {
                            using (AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure"))
                            {
                                _strAndroidID = secure.CallStatic<string>("getString", contentResolver, "android_id");
                                if (string.IsNullOrEmpty(_strAndroidID))
                                {
                                    _strAndroidID = "none";
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
            }
#endif
            return _strAndroidID;
        }

        return _strAndroidID;
    }


    /// <summary>
    /// 获取请求ID：时间戳+6位随机数
    /// </summary>
    /// <returns></returns>
    public static string GetRequestId()
    {
        string tempStr = string.Empty;

        int tempRandValue = UnityEngine.Random.Range(100000, 999999);
        tempStr = DateTime.Now.ToString("yyyyMMddHHmmss") + tempRandValue;
        return tempStr;
    }
    /// <summary>
    /// 根据当前物体的子物体网格组合后添加合适的碰撞体
    /// </summary>
    public static MeshCollider AddCollider(GameObject obj)
    {
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Matrix4x4 matrix = obj.transform.worldToLocalMatrix;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            MeshFilter mf = meshFilters[i];
            combine[i].mesh = mf.sharedMesh;
            combine[i].transform = matrix * mf.transform.localToWorldMatrix;
        }
        Mesh tempMesh = new Mesh();
        tempMesh.name = obj.name;
        tempMesh.CombineMeshes(combine, false);

        MeshCollider tempMeshCollider = obj.gameObject.AddComponent<MeshCollider>();
        tempMeshCollider.sharedMesh = tempMesh;
        return tempMeshCollider;
    }

    /// <summary>
    /// 将结构体数据保存成JSON文件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fileName">存储的文件名，编辑器和PC状态下如果为空则打开文件管理器选择路径，反之则直接存再Json文件夹</param>
    /// <param name="headType">是否处理Json数据的头，如果为None则没有头</param>
    /// <returns></returns>
    public static bool SaveData_JSON(object data, string fileName, TypeNameHandling headType = TypeNameHandling.None)
    {
        bool isSaveSucced = true;
        string JSONFilePath = null;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (string.IsNullOrEmpty(fileName))
        {
             JSONFilePath = Global_Windows.M_Instance.Open_WindowFile("json", true);
        }
        else
        {
            JSONFilePath = M_URLJSON + fileName;
        }

#elif UNITY_ANDROID

         JSONFilePath=M_URLJSON+fileName;

#endif
        #region 将结构体添加数据转换成JSON文件并存储
        try
        {
            FileInfo file = new FileInfo(JSONFilePath);
            //判断有没有文件，有则打开文件，，没有创建后打开文件
            StreamWriter sw = file.CreateText();
            string json = string.Empty;
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = headType
            };
            json = JsonConvert.SerializeObject(data, settings);
            //   Debug.Log(json);
            sw.WriteLine(json);
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
    public static string GetStrByJsonData<T>(T t, TypeNameHandling headType = TypeNameHandling.None)
    {
        string tempJson = string.Empty;
        try
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = headType
            };
            tempJson = JsonConvert.SerializeObject(t, settings);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return tempJson;
    }

    /// <summary>
    /// 只读取JSON数据的字符串内容
    /// </summary>
    /// <param name="jsonURL"></param>
    /// <returns></returns>
    public static string GetJSONDataStr(string jsonURL = null)
    {
        string tempJSONData = string.Empty;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (null == jsonURL)
        {
            jsonURL = Global_Windows.M_Instance.Open_WindowFile("json", false);
        }
#endif
        try
        {
            WWW www = new WWW(jsonURL);
            while (!www.isDone)
            {
                // Debug.Log("正在读取！");
            }
            tempJSONData = www.text;
            if (tempJSONData.Length == 0 || string.Empty == tempJSONData || null == tempJSONData)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                Global_Windows.MessageBox(IntPtr.Zero, "JSON文件内容为空!", "确认", 0);
#endif
                Debug.Log("JSON文件内容为空!");
                return tempJSONData;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Global_Windows.MessageBox(IntPtr.Zero, "读取JSON异常" + e.Message, "确认", 0);
#endif
            return tempJSONData;
        }
        //  Global_Windows.MessageBox(IntPtr.Zero, "读取JSON数据成功", "确认", 0);
        return tempJSONData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataStr"></param>
    /// <param name="headType">是否处理Json数据的头，如果为None则没有头</param>
    /// <returns></returns>
    public static T GetJsonDataByStr<T>(string dataStr, TypeNameHandling headType = TypeNameHandling.None)
    {
        T tempT = default(T);
        try
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = headType
            };
            tempT = JsonConvert.DeserializeObject<T>(dataStr, settings);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return tempT;
        }
        return tempT;
    }

    /// <summary>
    /// 读取文件夹里的JSON文件，并转换成对应的结构体,
    /// 默认没有路径的会自动打开文件夹选择窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tempStrData"></param>
    /// <param name="jsonURL"></param>
    /// <param name="headType">是否处理Json数据的头，如果为None则没有头</param>
    /// <returns></returns>
    public static T ReadData_JSON<T>(out string tempStrData, string jsonURL = null, TypeNameHandling headType = TypeNameHandling.None)
    {
        tempStrData = string.Empty;
        T tempT = default(T);
        string JSONFilePath = jsonURL;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (null == JSONFilePath)
        {
            JSONFilePath = Global_Windows.M_Instance.Open_WindowFile("json", false);
        }
#endif
        try
        {
            tempStrData = File.ReadAllText(JSONFilePath, Encoding.UTF8);
            if (tempStrData.Length == 0 || string.Empty == tempStrData || null == tempStrData)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                Global_Windows.MessageBox(IntPtr.Zero, "JSON文件内容为空!", "确认", 0);
#endif
                Debug.Log("JSON文件内容为空!");
                return tempT;
            }
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = headType
            };
            tempT = JsonConvert.DeserializeObject<T>(tempStrData, settings);

            //   Debug.Log(tempStrData);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Global_Windows.MessageBox(IntPtr.Zero, "读取JSON异常" + e.Message, "确认", 0);
#endif
            return tempT;
        }
        //  Global_Windows.MessageBox(IntPtr.Zero, "读取JSON数据成功", "确认", 0);
        return tempT;
    }

    /// <summary>
    /// 获取时间戳Timestamp  
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public long GetTimeStamp(DateTime dt)
    {
        DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        long timeStamp = Convert.ToInt64((dt - dateStart).TotalMilliseconds);
        return timeStamp;
    }

    /// <summary>
    /// 将二进制转换成对应的结构体
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="structType"></param>
    /// <returns></returns>
    public static object BytesToStruct(byte[] bytes, System.Type structType)
    {
        object obj;
        try
        {
            ReceiveByteLength = bytes.Length;
            // 获取结构体大小
            int size = Marshal.SizeOf(structType);
            StructTypeLength = size;
            // byte数据长度小于结构体的大小
            if (size > bytes.Length)
            {
                Debug.LogError("转换的结构体" + structType.ToString() + "大于字节长度");
                return null;
            }
            // 分配结构体大小的内存空间
            System.IntPtr structPtr = Marshal.AllocHGlobal(size);
            // 将byte数组拷贝到分配好的的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            // 将内存空间转换为目标结构体
            obj = Marshal.PtrToStructure(structPtr, structType);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    /// <summary>
    /// 结构体转换成字节数组
    /// </summary>
    /// <param name="structObj"></param>
    /// <returns></returns>
    public static byte[] StructToBytes(object structObj)
    {
        int size = Marshal.SizeOf(structObj);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structObj, buffer, false);
            byte[] bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    /// <summary>
    ///获取本地的IP地址
    /// </summary>
    /// <returns></returns>
    public IPAddress GetAddressIP()
    {
        IPAddress tempIPA = null;
        string tempName = Dns.GetHostName();
        IPAddress[] ipadrlist = Dns.GetHostAddresses(tempName);
        foreach (IPAddress _IPAddress in ipadrlist)
        {
            if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                tempIPA = _IPAddress;
            }
        }
        return tempIPA;
    }

    public void SetLocalVersion(bool isLocal)
    {
        Global_XMLCtr.M_Instance.SetElementValue("IsLocalVersion", isLocal.ToString());
        isLocalVersion = isLocal;
    }
    #endregion
}


