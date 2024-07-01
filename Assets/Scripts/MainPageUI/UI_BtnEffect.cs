using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮的交互效果
/// </summary>
public class UI_BtnEffect : MonoBehaviour
{
    private Vector3 normalPosition = Vector3.zero;
    public float targetPositionZ = -50f;
    private float time = 0.15f;
    private Tween hoverTween;
    bool isSelected;
    private Matrix4x4 matrix;
    private RectTransform rectTrans;
    void OnEnable()
    {
        rectTrans = GetComponent<RectTransform>();
        normalPosition = rectTrans.anchoredPosition3D;
      //  normalPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(normalPosition==Vector3.zero)
        {
            normalPosition = rectTrans.anchoredPosition3D;
        }
    }
    public void OnHover(bool isHover)
    {
        if (!this.enabled)
            return;
        if (isHover)
        {
            if (rectTrans.anchoredPosition3D.z != targetPositionZ)
            {
                isSelected = true;
                hoverTween.Kill();
                hoverTween = rectTrans.DOAnchorPos3DZ(targetPositionZ, time);
            }

        }
        else
        {
            if (rectTrans.anchoredPosition3D.z != normalPosition.z)
            {
                isSelected = false;
                hoverTween.Kill();
                hoverTween = rectTrans.DOAnchorPos3DZ(normalPosition.z, time);
            }
        }
    }
    public void Press()
    {
        if (!this.enabled)
            return;
        hoverTween.Kill();
        rectTrans.anchoredPosition3D = normalPosition;
    }
}
