using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace MRLessonrender
{
    /// <summary>
    /// UDP单播发送
    /// </summary>
    public class UDPUnicast_Send : MonoBehaviour
    {

        public static UDPUnicast_Send M_Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType<UDPUnicast_Send>();
                }
                return _instance;
            }
        }

        private static UdpClient udpSend;
        private static List<IPEndPoint> listIpes_send = new List<IPEndPoint>();
        private IPAddress UnicastAddress;

        private static UDPUnicast_Send _instance;
        private bool isInitSucced = false;
        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isInitSucced) return;

            #region 测试
            if (Input.GetKeyDown(KeyCode.S))
            {
                Send_Data("测试消息！");
            }
            #endregion
        }
        private void OnApplicationQuit()
        {
            if (null != udpSend)
            {
                udpSend.Close();
            }
        }
        public void Init()
        {
            udpSend = new UdpClient();
            UnicastAddress = IPAddress.Parse(Global_XMLCtr.M_Instance.GetElementValue("UDPUnicastAddress"));
            string[] tempStr = Global_XMLCtr.M_Instance.GetElementValue("UDPUnicasPorts").Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                int tempPort = int.Parse(tempStr[i]);
                IPEndPoint tempIpe_send = new IPEndPoint(UnicastAddress, tempPort);
                listIpes_send.Add(tempIpe_send);

            }


            isInitSucced = true;
        }
        public static void Send_Data(object sendData)
        {
            byte[] tempData = Global_Manage.StructToBytes(sendData);
            for (int i = 0; i < listIpes_send.Count; i++)
            {
                udpSend.Send(tempData, tempData.Length, listIpes_send[i]);

            }
        }
        public static void Send_Data(byte[] sendData, int length)
        {
            for (int i = 0; i < listIpes_send.Count; i++)
            {
                udpSend.Send(sendData, sendData.Length, listIpes_send[i]);

            }
        }
        public void Send_Data(string sendData)
        {
            byte[] tempSendMsg = Encoding.Unicode.GetBytes(sendData);
            for (int i = 0; i < listIpes_send.Count; i++)
            {
                udpSend.Send(tempSendMsg, tempSendMsg.Length, listIpes_send[i]);
            }
        }
    }
}
