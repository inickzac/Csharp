using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverSeaBattle
{
    public partial class Form1 : Form
    {
       static Form1 form;
        public Form1()
        {
            InitializeComponent();
            Server server;
            Task.Run(new Action(() => server = new Server(8888)));
            form = this;          
        }

        public static void writeQuantityOfPlayersFree(string mess)
        {
            if (form != null)
            {
                form.Invoke(new Action(() => form.label2.Text ="Количество свободных игроков "+ mess));
            }
        }
        public static void writeQuantityOfPlayers(string mess)
        {
            if (form != null)
            {
                form.Invoke(new Action(() => form.label1.Text="Общее количество игроков "+mess));
            }
        }

        public static void writeToLog(string mess)
        {
            if(form!=null)
            {
                form.Invoke(new Action(() => form.listBox1.Items.Add(mess)));
            }
        }
    }

    class Server
    {
        TcpListener Listener;
        public Server(int Port)
        {
            Listener = new TcpListener(IPAddress.Any, Port); 
            Listener.Start();
           Form1.writeQuantityOfPlayersFree("0");
           Form1.writeQuantityOfPlayers("0");

            while (true)
            {
                TcpClient Client = Listener.AcceptTcpClient();
                Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                thread.Start(Client);
            }
        }

        static void ClientThread(Object StateInfo)
        {
            new Connection((TcpClient)StateInfo);
        }
        ~Server()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }

    }

    class Connection
    {
        bool isBusy = false;
        Connection pair { get; set; }
        TcpClient tcpClient;
        NetworkStream NetworkStream { get; set; }
        public static List<Connection> clientPlayers = new List<Connection>();
        public string Name { get; set; }
        public Connection(TcpClient client)
        {
            this.tcpClient = client;
            this.NetworkStream = tcpClient.GetStream();
            clientPlayers.Add(this);                     
            communication();
        }
        public void WriteToPlayer(string message)
        {
            if (message != "")
            {
                Form1.writeToLog("отправляемое от "+tcpClient.Client.RemoteEndPoint.ToString() +" "+ message);
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                NetworkStream.Write(buffer, 0, buffer.Length);
            }
            }
        public List<string> GetPlayerMessage()
        {
            List<string> reciveMessage = new List<string>();
            try
            {
                if (tcpClient.Connected)
                {
                    byte[] data = new byte[1024];
                    int bytes = tcpClient.GetStream().Read(data, 0, data.Length); // получаем количество считанных байтов
                    string message = Encoding.UTF8.GetString(data, 0, bytes);
                    Form1.writeToLog("принимаемое "+ "от " + tcpClient.Client.RemoteEndPoint.ToString() +" "+ message);
                    reciveMessage.AddRange(message.Split(','));
                }
            }
            catch (SocketException)
            {
                connectionExc();
            }
            return reciveMessage;

        }
        public void sendPlayersList()
        {
            try
            {
                WriteToPlayer(getPlayersList());
            }
            catch (SocketException)
            {
                connectionExc();
            }
        }
        public  void sendPlayersListAllPlayers()
        {
            clientPlayers.ToList().ForEach(o => o.sendPlayersList());
        }
        public string getPlayersList()
        {
            string mes = "playersList";
            foreach (var i in clientPlayers)
            {
                if(!isBusy)mes += "," + i.Name;
            }
            return mes;
        }
        public void communication()
        {
            try
            {
                List<string> listMessage;
                while (true)
                {
                    listMessage = GetPlayerMessage();
                    if (listMessage[0] == "name")
                    {
                        if(clientPlayers.All(o=> o.Name!= listMessage[1]))
                        {
                            Name = listMessage[1];
                            if (clientPlayers.Count > 1) {
                                sendPlayersListAllPlayers();
                            }
                        }                      
                    }

                    if (listMessage[0] == "disconect")
                    {
                        clientPlayers.Remove(this);
                        sendPlayersListAllPlayers();
                    }

                    if (listMessage[0] == "choice")
                    {
                        if (clientPlayers.Any(o => o.Name == listMessage[1]))
                        {
                            pair = clientPlayers.Where(o => o.Name == listMessage[1]).First();
                            pair.pair = this;
                            isBusy = true;
                            pair.isBusy = true;
                            this.WriteToPlayer("connect");
                            pair.WriteToPlayer("connect");
                        }
                    }

                    if(listMessage[0]!= "close" && listMessage[0] != "choice" && listMessage[0] != "name")
                    {
                      if(pair!=null)  pair.WriteToPlayer(getStringWithSplit(listMessage));
                    }

                    if (listMessage[0] == "close")
                    {
                       if(pair!=null) pair.WriteToPlayer("disconect");
                        clientPlayers.Remove(pair);
                        clientPlayers.Remove(this);
                        sendPlayersListAllPlayers();
                    }
                    Form1.writeQuantityOfPlayers(clientPlayers.Count().ToString());
                    Form1.writeQuantityOfPlayersFree(clientPlayers.Where(o => o.isBusy == false).Count().ToString());
                }

               
            }
            catch(SocketException)
            {
                connectionExc();
            }
        }
       public string getStringWithSplit(List<string> mess)
        {
            string ms = "";
            foreach(var i in mess)
            {
                ms += i + ",";
            }
            ms=ms.Remove(ms.Length - 1);
            return ms;
        }

        public void connectionExc()
        {
            clientPlayers.Remove(this);
            if (pair != null) pair.WriteToPlayer("disconect");
            sendPlayersListAllPlayers();
        }

    }

  
   
}
