using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 读取Text数据的类
/// </summary>
public class Global_TextCtrl
{
    public enum EI_TextType
    {
        UI = 0,
        ModelName,
        QA,
    }
    #region 私有变量
    /// <summary>
    /// 从txt中读取的文字数据
    /// </summary>
    private Dictionary<string, string> DicTextUI { get; set; }
    private const char SplitChar2 = '/';//二级分隔符
    /// <summary>
    /// 单例
    /// </summary>
    private static Global_TextCtrl _instance = new Global_TextCtrl();
    private EI_TextType curTextType;
    #endregion

    #region 属性
    /// <summary>
    /// 单例属性
    /// </summary>
    public static Global_TextCtrl M_Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Global_TextCtrl();
            return _instance;
        }
    }
    #endregion
    public string GetUIStr(string index)
    {
        if (DicTextUI.ContainsKey(index))
        {
            return DicTextUI[index];
        }
        else
        {
            //       Debug.LogError("未找到索引为：" + index + "的文本内容");
            return string.Empty;
        }
    }
    /// <summary>
    /// 获取需要插入动态变量的字符，分隔符为'/'
    /// </summary>
    /// <param name="index">txt里面的模板字符索引</param>
    /// <param name="insertStr">需要插入的动态字符</param>
    /// <returns></returns>
    public string GetUIStr(string index, params string[] insertStr)
    {
        string tempString = string.Empty;

        tempString = GetUIStr(index);
        //提取分割的字符数组，分隔符为‘/’
        string[] tempStrs = tempString.Split(SplitChar2);
        //插入的字符串数应该总是小于等于分割后的字符串长度-1
        if (null == insertStr || null == tempStrs || (tempString.Length - 1) < insertStr.Length)
        {
            Debug.LogError("插入字符" + tempString + "错误");
            return tempString;
        }
        tempString = string.Empty;
        int i = 0;
        //间隔插入所需的字符
        for (; i < insertStr.Length; i++)
        {
            tempString += tempStrs[i] + insertStr[i];
        }
        //填补后续的字符
        for (; i < tempStrs.Length; i++)
        {
            tempString += tempStrs[i];
        }
        return tempString;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public Global_TextCtrl()
    {
        if (DicTextUI != null)
        {
            DicTextUI.Clear();
        }
        else
        {
            DicTextUI = new Dictionary<string, string>();
        }

        //获取txt中UI的文字信息,一个Text文件包含多类数据的读取
        string TipText;
      //  StreamReader srUI = new StreamReader(Global_Manage.M_CurProjectAssetPath + @"/ServerData/TextData.txt", Encoding.Default);
          StreamReader srUI = new StreamReader(Application.streamingAssetsPath + @"/TextData.txt", Encoding.Default);
        while ((TipText = srUI.ReadLine()) != null)
        {
            if (TipText.StartsWith("/*UI"))
            {
                curTextType = EI_TextType.UI;
                continue;
            }
            if (TipText.Contains(":"))
            {
                int keyPos = TipText.IndexOf(":");
                string TextKey = TipText.Substring(0, keyPos);//不允许用Split，不然信息中的其余冒号也会被抹杀
                string TextInfo = TipText.Substring(keyPos + 1);
                switch (curTextType)
                {
                    case EI_TextType.UI:
                        DicTextUI.Add(TextKey, TextInfo);
                        break;
                    case EI_TextType.ModelName:

                        break;
                    case EI_TextType.QA:

                        break;
                    default:
                        break;
                }
            }
        }
        srUI.Close();

        /*
        //从Text中获取模型名称键值对
        string tempStrModleName;
        StreamReader srModleName = new StreamReader(Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\XML\TextModelName.txt", Encoding.Default);
        while ((tempStrModleName = srModleName.ReadLine()) != null)
        {
            if (tempStrModleName.Contains(":"))
            {
                int keyPos = tempStrModleName.IndexOf(":");
                string TextKey = tempStrModleName.Substring(0, keyPos);//不允许用Split，不然信息中的其余冒号也会被抹杀
                string TextInfo = tempStrModleName.Substring(keyPos + 1);
                DicTextModelName.Add(TextKey, TextInfo);
            }
        }
        srModleName.Close();
        */
    }

}

