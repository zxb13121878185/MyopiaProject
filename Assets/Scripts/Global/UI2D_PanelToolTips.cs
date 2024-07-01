using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 文字提示工具
/// </summary>
public class UI2D_PanelToolTips : GlobalBaseObj
{
    public static UI2D_PanelToolTips M_Instance { get; private set; }
    public Vector3 OffsetPOs;
    private Text textTips;
    private void Awake()
    {
        M_Instance = this;
        Init(false);
    }
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strIndex">文本在Text文件中的索引</param>
    /// <param name="parentPos"></param>
    public void Show(string strIndex, Vector3 parentPos)
    {
        gameObject.SetActive(true);
        transform.position = parentPos + OffsetPOs;
        strIndex = Global_TextCtrl.M_Instance.GetUIStr(strIndex);
        textTips.text = strIndex;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public override void Init(bool isActive = true)
    {
        base.Init(isActive);
        textTips = GetComponentInChildren<Text>(true);
        transform.SetAsLastSibling();
    }
}

