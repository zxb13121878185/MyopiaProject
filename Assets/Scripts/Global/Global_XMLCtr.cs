using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class Global_XMLCtr
{

    private static Global_XMLCtr _Instance;


    /// <summary>
    /// 安卓的SD卡绝对路径，另需要设置发布时候SD卡壳读写
    /// </summary>
    private static string xmlpath = String.Empty;
    public static Global_XMLCtr M_Instance
    {
        get
        {
            if (null == _Instance)
            {
                //#if UNITY_ANDROID
                //                xmlpath= "file:///storage/emulated/0" + "/ResourcesData/XML/GlobalConfig.xml";

                //#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                //                    xmlpath = Global_Manage.M_CurProjectAssetPath + "ResourcesData/XML/GlobalConfig.xml";
                //#endif

                ////在移动端不能修改Application.streamingAssetsPath文件内容
                ////因此需将读取的xml文件写入到Application.persistentDataPath 路径，因为这个路径可以修改
                // string tempStrStreamURL = Application.streamingAssetsPath + "/GlobalConfig.xml";
                string tempResURL = Application.dataPath + "/Resources/GlobalConfig.xml";
                string tempStrPersistPath = Application.persistentDataPath + "/GlobalConfig.xml";

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

                xmlpath = tempResURL;
                root = XElement.Load(xmlpath);

#elif UNITY_ANDROID || UNITY_IOS

              TextAsset textAsset = (TextAsset)Resources.Load("GlobalConfig", typeof(TextAsset));
                if (!File.Exists(tempStrPersistPath))
                {
                    // root = XElement.Load(tempResURL);
                    root = XElement.Parse(textAsset.text);
                    root.Save(tempStrPersistPath);
                }
                root = XElement.Load(tempStrPersistPath);
                xmlpath = tempStrPersistPath;
#endif
                _Instance = new Global_XMLCtr();
            }
            return _Instance;
        }
    }
    private static XElement root;
    public void CreateXMLDocument()
    {
        XElement root = new XElement("XMLContent");
        root.Save(xmlpath);
    }
    public bool CheckXMLISNull()
    {
        XElement root = null;
        try
        {
            root = XElement.Load(xmlpath);
        }
        catch (Exception ex)
        {
            if (ex != null)
                return true;
        }
        return false;
    }
    public void SetElementValue(string name, string value)
    {
        if (CheckElementIsNull(name))
        {
            Debug.LogError("元素" + name + "不存在");
            return;
        }
        root.Element(name).SetValue(value);
        root.Save(xmlpath);
    }
    /// <summary>
    /// 在根节点元素之前添加新的元素
    /// </summary>
    /// <param name="name">元素名字</param>
    /// <param name="value">元素的值</param>
    public void AddElement(string name, string value)
    {
        if (null != root.Element(name)) return;
        XElement newElement = new XElement(name, value);
        root.Add(newElement);
        root.Save(xmlpath);
    }
    /// <summary>
    /// 删除指定的元素
    /// </summary>
    /// <param name="name">要删除的元素名称</param>
    public void RemoveElement(string name)
    {
        if (null == root.Element(name)) return;
        root.Element(name).Remove();
        root.Save(xmlpath);
    }
    /// <summary>
    /// 根据元素名查找元素对应的值
    /// </summary>
    /// <param name="name">元素名</param>
    /// <returns></returns>
    public string GetElementValue(string name)
    {
        if (CheckElementIsNull(name))
        {
            Debug.LogError("元素" + name + "不存在");
            return string.Empty;
        }
        //     XAttribute xattr = root.Element(name.Trim()).Attribute("MyVaule");
        XElement curElement = root.Element(name.Trim());
        string s = curElement.Value;
        return s;
    }
    /// <summary>
    /// 检测这个元素是否存在，不存在为True
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckElementIsNull(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        XElement xElement = root.Element(name);//XMl的元素名不能以数字开头
        return xElement == null ? true : false;
    }
}
