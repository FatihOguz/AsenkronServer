using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Net.Sockets;
using AsenkronServer.XMessages;
using serverClient.XMessages;

namespace AsenkronClient.XSockets
{
    public delegate void OnMessageReceived(ChatCommand cc);
    class OurSockets
    {
        Socket m_ServerSocket;
        IPEndPoint m_ServerEP;
        byte[] mBuffer = new byte[1024];
        public OnMessageReceived m_OnMessageReceived;
        public OurSockets(IPEndPoint mServerEP)
        {
            this.m_ServerEP = mServerEP;
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }


        public void StartConnect()
        {
            m_ServerSocket.BeginConnect(m_ServerEP, new AsyncCallback(onConnect), null);
        }
        void onConnect(IAsyncResult ar)
        {
            m_ServerSocket.EndConnect(ar);

            m_ServerSocket.BeginReceive(mBuffer, 0, mBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
        void OnReceive(IAsyncResult ar)
        {
            int length = m_ServerSocket.EndReceive(ar);

            if (length <= 0) { return; }

            m_ServerSocket.BeginReceive(mBuffer, 0, mBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }


        private void ExtractBuffer(byte[] mBuffer, int length)
        {
            byte[] sizedBuffer = new byte[length];
            Array.Copy(mBuffer, 0, sizedBuffer, 0, sizedBuffer.Length);
            // array düzeltildi

            if (sizedBuffer[0] == (byte)XMessageProtocols.HEADER)
            {
                XMessageProtocols xmp = (XMessageProtocols)sizedBuffer[1];

                switch (xmp)
                {
                    case XMessageProtocols.CHAT_EVENT:
                        if (m_OnMessageReceived != null)
                        {
                            m_OnMessageReceived(ChatCommand.ParseFrom(GetCommand(sizedBuffer)));
                        }
                        break;
                    case XMessageProtocols.INFO_EVENT:
                        break;
                    default:
                        break;

                }
            }
            else
            {
                Console.WriteLine("Invalid buffer received..");
            }

        }
        byte[] GetCommand(byte[] data)
        {
            byte[] temp = new byte[data.Length - 2];
            Array.Copy(data, 2, temp, 0, temp.Length);
            return temp;
        }

        public void SendChat(string nCommand)
        {
            ChatCommand.Builder ccb = new ChatCommand.Builder();
            ccb.SetSender("My first application");
            ccb.SetCommand(nCommand);
            ccb.SetUserType(UserType.ADMIN);

            byte[] sendData = CreateCommand(ccb.Build().ToByteArray(), XMessageProtocols.CHAT_EVENT);

            SendBytes(sendData);
        }
        private void SendBytes(byte[] sendData)
        {
            m_ServerSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
        }

        void OnSend(IAsyncResult ar)
        {
            int length = m_ServerSocket.EndSend(ar);

            if (length <= 0)
            {
                return;
            }
        }
        private byte[] CreateCommand(byte[] p, XMessageProtocols xMessageProtocols)
        {
            byte[] sendData = new byte[p.Length + 2];

            sendData[0] = (byte)XMessageProtocols.HEADER;
            sendData[1] = (byte)xMessageProtocols;

            Array.Copy(p, 0, sendData, 2, p.Length);
            return sendData;
        }
    }
}
