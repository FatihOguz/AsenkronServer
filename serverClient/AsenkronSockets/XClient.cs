using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using serverClient.XMessages;
using AsenkronServer.XMessages;

namespace AsenkronServer.AsenkronSockets
{
    class XClient
    {
        public delegate void OnMessageReceived(ChatCommand cc);
        Socket m_ClientSockets;
        byte[] Buffer = new byte[1024];
        public OnMessageReceived m_onMessageReceived;

        public XClient(Socket nClientSocket)
        {
            this.m_ClientSockets = nClientSocket;


        }
        public void StartRelay()
        {
            m_ClientSockets.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
        private void OnReceive(IAsyncResult ar)
        {
            int length = m_ClientSockets.EndReceive(ar);
            if (length <= 0)
            {
                return;

            }
            ExtractBuffer(this.Buffer, length);

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
                        if (m_onMessageReceived != null)
                        {
                            m_onMessageReceived(ChatCommand.ParseFrom(GetCommand(sizedBuffer)));
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
            m_ClientSockets.BeginReceive(mBuffer, 0, mBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
        byte[] GetCommand(byte[] data)
        {
            byte[] temp = new byte[data.Length - 2];
            Array.Copy(data, 2, temp, 0, temp.Length);
            return temp;
        }
    }
}
