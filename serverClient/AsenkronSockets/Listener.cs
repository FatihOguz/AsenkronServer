using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using AsenkronServer.XMessages;

namespace AsenkronServer.AsenkronSockets
{
    class Listener
    {
        Socket m_ListenerSocket;
        int m_Port;
        private String message;
        List<XClient> m_ClientList = new List<XClient>();
        public String  getMessage()
        {
            return this.message;
        }
        public Listener(int m_Port)
        {
            this.m_Port = m_Port;
            m_ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartListen()
        {
            m_ListenerSocket.Bind(new IPEndPoint(IPAddress.Any, this.m_Port));
            m_ListenerSocket.Listen(10); // 10 kişi katılabilir

            m_ListenerSocket.BeginAccept(new AsyncCallback(OnAccept), null);
        }
        private void OnAccept(IAsyncResult ar)
        {
            Socket temp = m_ListenerSocket.EndAccept(ar);
            XClient xc = new XClient(temp);
            xc.m_onMessageReceived += new XClient.OnMessageReceived(OnMessageReceived);
            xc.StartRelay();
            


            m_ClientList.Add(xc);

        }

        void OnMessageReceived(ChatCommand cc)
        {
            Console.WriteLine("a CHAT command received form " + cc.UserType + " client " + cc.Sender);
            Console.WriteLine("Message: " + cc.Command);

            this.message = "a CHAT command received form " + cc.UserType.ToString() + " client " + cc.Sender.ToString()  + "Message: " + cc.Command.ToString();
        }
    }
}
