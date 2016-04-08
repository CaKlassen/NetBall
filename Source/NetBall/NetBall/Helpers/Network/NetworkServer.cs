using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetBall.Helpers.Network
{
    public class NetworkServer
    {
        /** Constants **/
        private static int MESSAGE_SIZE = 1024;

        public static NetworkServer instance;

        private int port;
        private string peerName;

        Socket listener;
        Socket peer;

        public NetworkServer(string peer, int listeningPort)
        {
            instance = this;

            this.peerName = peer;
            this.port = listeningPort;
        }

        public void startServer()
        {
            //Create a reference to a new thread
            ThreadStart networkThreadRef = new ThreadStart(connectedState);

            //create a new thread
            Thread networkThread = new Thread(networkThreadRef);

            //start the thread
            networkThread.Start();
         }

        private void connectedState()
        {
            //convert the peer's IP from a string and create the end point
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // bind the socket to the local endpoint and 
            // listen to the incoming sockets 

            try
            {
                listener.Bind(ipEndPoint);
                listener.Listen(10);

                while (true)
                {
                    // Start listening for connections 
                    peer = listener.Accept();

                    Console.WriteLine("Peer has connected");

                    //once the peer is connected, read for data
                    while (peer.Connected)
                    {
                        string receivedMsg = readData();

                        // show the data on the console 
                        Console.WriteLine("Text Received: {0}", receivedMsg);

                        MessageUtils.parseMessage(receivedMsg);
                    }

                    //close the connection now that the client has disconnected
                    closeConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private string readData()
        {
            string data = null;

            byte[] bytes = new byte[MESSAGE_SIZE];
            int bytesReceived = peer.Receive(bytes);

            data = Encoding.ASCII.GetString(bytes, 0, bytesReceived);
            return data;
        }
         
        public void sendData(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);

            peer.Send(messageBytes);
        }

        private void closeConnection()
        {
            peer.Shutdown(SocketShutdown.Both);
            peer.Close();
        }
    }
}
