using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace MRLessonrender
{
    /// <summary>
    /// 发送给两个组网、分别是外部控制的组网，和内部控制的组网
    /// </summary>
    public class UDPMulticast_Send : MonoBehaviour
    {

        public static UDPMulticast_Send M_Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType<UDPMulticast_Send>();
                }
                return _instance;
            }
        }

        #region 发送给系统

        private static UdpClient udpSendSys;
        private static IPEndPoint ipe_sendSys;
        private IPAddress GroupAddressSys;
        private int port_sendToGroupSys;
        /// <summary>
        /// 发送次数
        /// </summary>
        private int sendFrequency_NumberSys;
        /// <summary>
        /// 发送一次的时间间隔
        /// </summary>
        private float sendFrequency_OnceTimeSys;
        private int curSendDataNumberSys;
        private bool isStartBrustSendDataSys = false;
        private Queue<byte[]> queueSendMsgSys = new Queue<byte[]>();
        private float timeConuterSys = 0;

        #endregion

        #region 发送给外部

        private static UdpClient udpSendExt;
        private static IPEndPoint ipe_sendExt;
        private IPAddress GroupAddressExt;
        private int port_sendToGroupExt;

        #endregion



        private static UDPMulticast_Send _instance;
        private bool isInitSucced = false;

        // Use this for initialization
        void Start()
        {

            sendFrequency_NumberSys = int.Parse(Global_XMLCtr.M_Instance.GetElementValue("SendDataFrequency"));
            sendFrequency_OnceTimeSys = float.Parse(Global_XMLCtr.M_Instance.GetElementValue("SendDataOnceTime"));
            GroupAddressSys = IPAddress.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPGroupIPSys"));
            GroupAddressExt = IPAddress.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPGroupIPExt"));
            port_sendToGroupSys = int.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPGroupPortSys"));
            port_sendToGroupExt = int.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPGroupPortExt"));
        }

        // Update is called once per frame
        void Update()
        {
            if (!isInitSucced) return;

            if (isStartBrustSendDataSys)
            {
                timeConuterSys += Time.deltaTime;
                if (timeConuterSys >= sendFrequency_OnceTimeSys)
                {
                    timeConuterSys = 0;
                    curSendDataNumberSys += 1;
                    if (queueSendMsgSys.Count > 0)
                    {
                        byte[] tempData = queueSendMsgSys.Dequeue();
                        // Debug.Log("remove:");
                        udpSendSys.Send(tempData, tempData.Length, ipe_sendSys);
                        udpSendExt.Send(tempData, tempData.Length, ipe_sendExt);
                    }
                }
                if (curSendDataNumberSys >= sendFrequency_NumberSys)
                {
                    curSendDataNumberSys = 0;
                    if (queueSendMsgSys.Count == 0)
                    {
                        // Debug.Log("over");
                        isStartBrustSendDataSys = false;
                    }
                }
            }
        }
        private void OnApplicationQuit()
        {
            if (null != udpSendSys)
            {
                udpSendSys.Close();
            }
            if (null != udpSendExt)
            {
                udpSendExt.Close();
            }
        }
        public void Init()
        {
            ////发送到系统组播网
            udpSendSys = new UdpClient();
            ipe_sendSys = new IPEndPoint(GroupAddressSys, port_sendToGroupSys);

            //发送到外部组网
            udpSendExt = new UdpClient();
            ipe_sendExt = new IPEndPoint(GroupAddressExt, port_sendToGroupExt);

            isInitSucced = true;

        }
        /// <summary>
        /// 向系统和外部都发送
        /// </summary>
        public void SendMulNum_Data(params object[] sendData)
        {
            for (int i = 0; i < sendData.Length; i++)
            {
                byte[] tempData = Global_Manage.StructToBytes(sendData[i]);
                queueSendMsgSys.Enqueue(tempData);
            }
            //  Debug.Log("add count:"+queueSendMsgSys.Count);
            isStartBrustSendDataSys = true;
            curSendDataNumberSys = 0;
            timeConuterSys = 0;
        }
        /// <summary>
        /// 向内部和外部同时发送
        /// </summary>
        /// <param name="sendData"></param>
        public static void Send_Data(object sendData)
        {
            byte[] tempData = Global_Manage.StructToBytes(sendData);
            udpSendSys.Send(tempData, tempData.Length, ipe_sendSys);
            udpSendExt.Send(tempData, tempData.Length, ipe_sendExt);
        }
        public static void Send_Data_Ext(byte[] tempData)
        {
            udpSendExt.Send(tempData, tempData.Length, ipe_sendExt);
        }
        public static void Send_Data_Sys(byte[] tempData)
        {
            udpSendSys.Send(tempData, tempData.Length, ipe_sendSys);
        }
        public void Send_Data(string sendData)
        {
            byte[] tempSendMsg = Encoding.Unicode.GetBytes(sendData);
            udpSendSys.Send(tempSendMsg, tempSendMsg.Length, ipe_sendSys);

        }
    }
}
