using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace imap
{
  
    class RequestInServer
    {
      static  Random rand = new Random();
        public string Label { get; private set; }
        List<string> answer;
        public List<string> Answer
        {
            get { return answer; }
            set
            {
                answer = value;
                if (answer.Count != 0)
                {
                    if (callBackAfterAnswer != null) callBackAfterAnswer.Invoke(answer);
                    state = RequestInServer.stateRequest.compleate;
                }
            }
        }

        public Action<List<string>> callBackAfterAnswer { get; set; }

        public string Request { get; private set; }


        public stateRequest state { get; private set; }
        public RequestInServer(string Request)
        {
            this.Request = Request;
            state = stateRequest.newRequest;
            Label = GetLabelIMAP();
            Answer = new List<string>();
        }



        public enum stateRequest { newRequest, shiped, compleate, error }
        static string GetLabelIMAP()
        {
            
            string letters = "ABCD";
            string label = letters.Substring(rand.Next(0, letters.Length - 1), 1) + rand.Next(1000, 9999);
            return label;
        }
        public void SendThisRequest(SslStream sslStream)
        {
            if (this.Request != "")
            {
               
               string request = Label + " " + this.Request + "\r\n";
               sslStream.Write(Encoding.UTF8.GetBytes(request));

                state = RequestInServer.stateRequest.shiped;
            }
            else throw new Exception("Попытка отправки запросса без команды");
        }
        public RequestInServer returnThisRequest()
        {
            return this;
        }

    }

    public class CoreImap
    {
        public static event Action<string> SendInServer;
        public static event Action<string> AnswerFromServer;
        TcpClient mail;
        SslStream sslStream;
        List<RequestInServer> requestInServersList;
        List<string> Labellist;
        static object locker = new object();
        public connectStatusEnum connectStatus { get; private set; }
        CancellationTokenSource canToken;
        CancellationToken token;


        public CoreImap(string domain, int port)
        {
            connect(domain, port);
            requestInServersList = new List<RequestInServer>();
            Labellist = new List<string>();

            canToken = new CancellationTokenSource();
            token = canToken.Token;
            Task.Factory.StartNew(() => CheckListRequestAsync(),token);
            Task.Factory.StartNew(() => IncomingRequestProcessAsync(),token);
        }

        public enum connectStatusEnum { noConnect, connect, error }


        public void Disconnect()
        {

            canToken.Cancel();
            Thread.Sleep(100);
        }


        void connect(string domain, int port)
        {

            mail = new TcpClient();
            mail.Connect(domain, port);
           // mail.Connect(System.Net.IPAddress.Parse("217.69.139.90"), port);
            sslStream = new SslStream(mail.GetStream());
            sslStream.AuthenticateAsClient(domain);
            string HelloAnswer = ReadOneResponse(sslStream);
            if (HelloAnswer.Contains("OK")) connectStatus = CoreImap.connectStatusEnum.connect;
            else connectStatus = CoreImap.connectStatusEnum.error;           
        }

        string ReadOneResponse(SslStream sslStream)
        {
            string answer;
            int bytes = -1;
            byte[] buffer = new byte[2048];
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            answer = Encoding.UTF8.GetString(buffer, 0, bytes);
            AnswerFromServer(answer);
            return answer;
        }

        void CheckListRequestAsync()
        {
            while (true)
            {
                lock (locker)
                {
                    foreach (RequestInServer requestInServer in requestInServersList.ToArray())
                    {
                        if (requestInServer.state == RequestInServer.stateRequest.newRequest)
                        {
                            requestInServer.SendThisRequest(sslStream);
                        }
                        if (requestInServer.state == RequestInServer.stateRequest.compleate)
                        {
                            //requestInServersList.Remove(requestInServer);
                        }
                        if(token.IsCancellationRequested)
                        {
                            return;
                        }

                    }
                }
            }
        }

        void IncomingRequestProcessAsync()
        {

            while (true)
            {
                Thread.Sleep(10);
                List<string> AnswerStringMass = new List<string>(ReadOneResponse(sslStream).Split(("\r\n").ToArray(), StringSplitOptions.RemoveEmptyEntries));
                while (AnswerStringMass[AnswerStringMass.Count - 1].Length < 9 || (AnswerStringMass[AnswerStringMass.Count - 1].Substring(6, 2) != "OK" && AnswerStringMass[AnswerStringMass.Count - 1].Substring(6, 3) != "BAD"
                    && AnswerStringMass[AnswerStringMass.Count - 1].Substring(6, 2) != "NO"))
                {
                    AnswerStringMass.AddRange(ReadOneResponse(sslStream).Split(("\r\n").ToArray(), StringSplitOptions.RemoveEmptyEntries));
                }

                for (int i = 0; i < requestInServersList.Count; i++)
                {
                    for (int j = 0; j < AnswerStringMass.Count; j++)

                    {
                        if (AnswerStringMass[j].Contains(requestInServersList[i].Label))
                        {
                            requestInServersList[i].Answer = AnswerStringMass;
                        }
                    }

                }
                if(token.IsCancellationRequested)
                { return; }
            }

        }

      public List<string> AddRequestAndWaitAnswer(string request)
        {
            SendInServer(request);
            List<string> Answer= new List<string>();
            var AnswerBlock = new Action<List<string>>((lstsr) => { Answer = lstsr; });
            AddReaquest(request, AnswerBlock);

            while (Answer.Count <= 0)
            {
                Thread.Sleep(50);
            }
            return Answer;
        }



        public void AddReaquest(string request, Action<List<string>> callBackAfterRequest)
        {
            var Request = new RequestInServer(request);          
            Request.callBackAfterAnswer = callBackAfterRequest;
            lock (locker)
            {
                requestInServersList.Add(Request);
            }
        }



    }
}
