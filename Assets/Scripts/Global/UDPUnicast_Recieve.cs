using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace MRLessonrender
{
    /// <summary>
    /// UDP单个接收数据
    /// </summary>
    public class UDPUnicast_Recieve : MonoBehaviour
    {

        #region 公有变量

        #endregion

        #region 属性
        public static UDPUnicast_Recieve M_Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType<UDPUnicast_Recieve>();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 接收的IP数据和信息
        /// </summary>
        public string M_RecDesStr
        {
            get
            {
                return DebugStr_CurReciveData;
            }
        }
        #endregion

        #region 私有变量

        private static int curReceAllDataNum = 0;

        #region 接收外部的

        private UdpClient udpRecieveExt;
        private IPEndPoint ipe_recieveExt;
        private int port_recieveOtherClient;  //接收其他客户端发送数据的端口
        private Thread thread_recieveExt;
        private static bool IsDebugUDPReceiveData = false;
        private static string DebugStr_UDPException = "没有数据！";//异常信息
        private static string DebugStr_CurReciveData = "开始接受数据！";//当前接收的数据

        #endregion

        private static UDPUnicast_Recieve _instance;
        private bool isInitSucced = false;
        private int ClientNums;//客户端数量

        #endregion

        #region 系统方法

        // Use this for initialization
        void Start()
        {
            //  IsDebugUDPFighting_CurReceiveDataExt = Global_XMLCtr.M_Instance.GetElementValue("IsDebugUDP") == "1";
            port_recieveOtherClient = int.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPUnicasPorts"));
            //  ClientNums= int.Parse(Global_XMLCtr.M_Instance.GetElementValue("ClientNums"));
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isInitSucced) return;
            if (IsDebugUDPReceiveData)
            {
                Debug.Log("接收外部数据：" + DebugStr_CurReciveData);
            }
        }
        private void LateUpdate()
        {

        }
        private void OnApplicationQuit()
        {
            if (null != thread_recieveExt)
            {
                thread_recieveExt.Interrupt();
                thread_recieveExt.Abort();
            }
            if (null != udpRecieveExt)
            {
                udpRecieveExt.Close();
            }
        }
        #endregion

        #region 私有方法

        //private IEnumerator ClearAllCacheData()
        //{
        //    int maxNumCacheData = 10;
        //    while (true)
        //    {
        //        for (int i = 0; i < ListCurRecvCMDFeedback.Count; i++)
        //        {
        //            int maxNo = ListCurRecvCMDFeedback[ListCurRecvCMDFeedback.Count - 1].Number;
        //            int minNo = ListCurRecvCMDFeedback[i].Number;
        //            int offsetNo = maxNo - minNo;
        //            if (offsetNo > maxNumCacheData && ListCurRecvCMDFeedback[i].IsUsed)
        //            {
        //                ListCurRecvCMDFeedback.RemoveAt(i);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        yield return null;
        //    }
        //}
        private void RecvThread_Ext()
        {
            DebugStr_CurReciveData = "开始接收外部数据！";
            while (true)
            {
                try
                {
                    byte[] tempRecvData = udpRecieveExt.Receive(ref ipe_recieveExt);
                    string tempStr = Encoding.Unicode.GetString(tempRecvData);
                    Debug.Log(tempStr);
                    //   Data_Header tempDataHeader = (Data_Header)Global_Manage.BytesToStruct(tempRecvData, typeof(Data_Header));

                    //   NET_MSG_TYPE curMsgType = (NET_MSG_TYPE)tempDataHeader.msgType;
                    //switch (curMsgType)
                    //{

                    //}

                    DebugStr_CurReciveData = "msg from:" + ipe_recieveExt.Address.ToString() + " " + curReceAllDataNum++;
                    if (IsDebugUDPReceiveData)
                    {
                        print("msg from:" + ipe_recieveExt.ToString());
                    }
                }
                catch (Exception e)
                {
                    if (IsDebugUDPReceiveData)
                    {
                        DebugStr_UDPException = "异常:" + e.Message;
                        print(DebugStr_UDPException);
                    }
                }
            }
        }
        #endregion

        #region 公有方法
        public void Init()
        {

            #region 接收外部其他客户端

            udpRecieveExt = new UdpClient(port_recieveOtherClient);
            ipe_recieveExt = new IPEndPoint(IPAddress.Any, 0);
            thread_recieveExt = new Thread(RecvThread_Ext);
            thread_recieveExt.IsBackground = true;
            thread_recieveExt.Start();

            #endregion

            // StartCoroutine(ClearAllCacheData());

            isInitSucced = true;
        }

        #endregion
    }
}
