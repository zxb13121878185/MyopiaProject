using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获取unity物体的摄像机，该摄像机将图像信息生成RenderTexure
/// </summary>
public class Camera_ObtainRT : BaseGameObj
{
    public Camera M_CurCamLeft
    {
        get
        {
            return curCamLeft;
        }
    }
    public Camera M_CurCamRight
    {
        get
        {
            return curCamRight;
        }
    }

    [SerializeField] Camera curCamLeft;
    [SerializeField] Camera curCamRight;
    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }
    public override void Init()
    {
        base.Init();
        //设置摄像机的各项参数
        Data_Module tempdataM = Camera_UnBoundedManage.M_Instance.M_CurModule;

        float tempAspect =(float) tempdataM.ResolutionX / (tempdataM.ResolutionY*2.0f);
        curCamLeft.fieldOfView = curCamRight.fieldOfView = tempdataM.FieldOfView;
        curCamLeft.aspect = curCamRight.aspect = tempAspect;

        curCamLeft.targetTexture.width = curCamRight.targetTexture.width = tempdataM.ResolutionX;
        curCamLeft.targetTexture.height = curCamRight.targetTexture .height= tempdataM.ResolutionY;
        curCamLeft.targetTexture.depth = curCamRight.targetTexture .depth= 24;
        curCamLeft.targetTexture.format = curCamRight.targetTexture .format= RenderTextureFormat.ARGB32;

        //设置摄像机直接的距离，以保证跟瞳距一样
        float tempDis = tempdataM.InterPupilDistance / 2.0f;
        curCamLeft.transform.localPosition = new Vector3(-tempDis, 0, 0);
        curCamRight.transform.localPosition = new Vector3(tempDis, 0, 0);

    }
}
