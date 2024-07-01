using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 警告面板，显示面板带回调函数
/// </summary>
public class UI2D_PanelWarn : GlobalBaseObj
{
    [SerializeField] Transform contentObj;
    [SerializeField] AudioSource[] soundsShow;//0是告警，1是提示
    [SerializeField] Text textContent1;
    [SerializeField] Text textContent2;
    private Action<bool> callBackMethod;
    [SerializeField] Button[] BtnCallBack;
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
    /// 简单的显示，不带回调函数，也即没有确认和取消按钮
    /// </summary>
    /// <param name="text1"></param>
    /// <param name="text2"></param>
    public void Show(string text1, string text2 = null, int soundIndex = 0, bool isShake = true)
    {
        gameObject.SetActive(true);
        //textContent1.text = text1;
        //textContent2.text = text2;
        textContent1.text = "告 警";
        textContent2.text = text1;
        for (int i = 0; i < BtnCallBack.Length; i++)
        {
            BtnCallBack[i].gameObject.SetActive(false);
        }
        soundsShow[soundIndex].PlayDelayed(0.35f);
        if (isShake)
        {
            contentObj.DOShakeRotation(1.35f, new Vector3(0, 0, 30), 10, 10);
            contentObj.DOShakePosition(1.35f, new Vector3(0, 0, 8), 10, 5);
        }
    }
    /// <summary>
    /// 带返回方法的弹窗框，带有确认和取消
    /// </summary>
    /// <param name="text1"></param>
    /// <param name="text2"></param>
    /// <param name="callBack"></param>
    public void ShowWithCallBack(string text1, string text2 = null, Action<bool> callBack = null, int soundIndex = 0, bool isShake = true)
    {
        gameObject.SetActive(true);
        textContent1.text = text1;
        textContent2.text = text2;
        callBackMethod = callBack;
        for (int i = 0; i < BtnCallBack.Length; i++)
        {
            BtnCallBack[i].gameObject.SetActive(true);
        }
        soundsShow[soundIndex].PlayDelayed(0.35f);
        if (isShake)
        {
            contentObj.DOShakeRotation(1.35f, new Vector3(0, 0, 30), 10, 10);
            contentObj.DOShakePosition(1.35f, new Vector3(0, 0, 8), 10, 5);
        }
    }
    public void BtnClose_PressDown(AudioSource sound)
    {
        sound.Play();
    }
    public void BtnClose_OnClick()
    {
        gameObject.SetActive(false);
    }
    public void BtnYes_PressDown(AudioSource sound)
    {
        sound.Play();
    }
    public void BtnYes_Onclick()
    {
        gameObject.SetActive(false);
        callBackMethod?.Invoke(true);
    }
    public void BtnNo_PressDown(AudioSource sound)
    {
        sound.Play();
    }
    public void BtnNo_OnClick()
    {
        gameObject.SetActive(false);
        callBackMethod?.Invoke(false);
    }
    public override void Init()
    {
        base.Init();
    }
}
