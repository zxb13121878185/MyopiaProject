using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ImeManager : MonoBehaviour
{
    public ImeDelegateBase mDelegate;
    private ImeBase mIme;
    private Vector2 mSize;

    private SGImeInputType imeInputType;
    public SGImeInputType ImeInputType => imeInputType;

    private SGImeTextType imeTextType;
    public SGImeTextType ImeTextType => imeTextType;

    void Start()
    {
        if (null != mDelegate)
        {
#if UNITY_EDITOR
            mIme = new DummyIme();
#else
            mIme = new SGIme();
#endif
            mIme.Create(mDelegate);
        }
    }

    void Update()
    {
        if (null == mIme)
        {
            return;
        }
        mIme.UpdateData();

        #region 测试


        //// Transform tempXRCtr = UI_MainPageCtr.M_Instance.xrCtr[0].transform;
        // //从手柄处发送一条射线
        // Ray ray = new Ray(tempXRCtr.position, tempXRCtr.forward);
        // RaycastHit hit;
        // bool tempIsHide = true;
        // if (Physics.Raycast(ray, out hit, 1000))
        // {
        //     //碰到了键盘物体
        //     string tempKeyboardName = "polySurface";
        //  //   Debug.Log(hit.transform.name);
        //     if (hit.transform.name.Contains(tempKeyboardName))
        //     {
        //         tempIsHide = false;
        //     }
        // }

        //if (tempIsHide && !TextHandler.FocusOn)
        //{
        //    Hide();
        //}


        #endregion

    }

    //export
    public void Show(SGImeInputType typeInput, SGImeTextType typeText)
    {
        imeInputType = typeInput;
        imeTextType = typeText;

        //DebugHelper.instance.Log("Imemanager.show()"); check right
        mIme.Show(typeInput, typeText);
        mIme.GetSize(ref mSize);
        //mDelegate.OnIMEShow(mSize);
    }

    public void Hide()
    {
        mIme.Hide();
        mDelegate.OnIMEHide();
    }

    public void Draw(Texture2D tex)
    {
        mIme.Draw(tex);
    }

    public void OnTouch(float x, float y, SGImeMotionEventType type)
    {
        mIme.OnTouch(x, y, type);
    }
    /// <summary>
    /// 当没有点击到输入框组建时自动隐藏键盘
    /// </summary>
    public void HideWhenNoInput(int index)
    {
        //Transform tempXRCtr = UI_MainPageCtr.M_Instance.xrCtr[index].transform;
        //string tempStr = tempXRCtr.forward.ToString();
        ////从手柄处发送一条射线
        //Ray ray = new Ray(tempXRCtr.position, tempXRCtr.forward);
        //RaycastHit hit;
        //bool tempIsHide = true;
        //if (Physics.Raycast(ray, out hit, 1000))
        //{
        //    //碰到了键盘物体
        //    string tempKeyboardName = "polySurface";
        //    tempStr += hit.transform.name;
        //    if (hit.transform.name.Contains(tempKeyboardName))
        //    {
        //        tempIsHide = false;
        //    }
        //}
        //textMesh.text = tempStr;
        if (!ImeDelegateImpl_kbd.isFocusOn && !TextHandler.IsFocusOn)
        {
            Hide();
        }
    }
}
