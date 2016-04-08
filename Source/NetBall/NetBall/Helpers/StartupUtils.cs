using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public struct FileData
    {
        public FileData(string peer, int port, bool isHost)
        {
            this.peer = peer;
            this.port = port;
            this.isHost = isHost;
        }

        public string peer;
        public int port;
        public bool isHost;

    }
    public static class StartupUtils
    {
        public static FileData readfileData()
        {
            StreamReader sr = new StreamReader("Content/Config/NetBall.config");
           
            //Fetch each line of the file starting with the peer data
            String peer = sr.ReadLine();
            int port = int.Parse(sr.ReadLine());
            bool hostStatus = bool.Parse(sr.ReadLine());

            return new FileData(peer, port, hostStatus);
        }
    }
}
