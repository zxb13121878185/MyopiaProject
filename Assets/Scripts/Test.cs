using Gloabal_EnumCalss;
using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Transform[] camObjs;//0是左摄像头，1是右摄像头
    // Start is called before the first frame update
    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LeftTrigger()
    {
        camObjs[0].position += new Vector3(0, 0, 0.1f);
    }
}
