using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace newEx3.Models
{
        class Instructions
        {
            private TcpClient client;
            private NetworkStream stream;
            private BinaryReader bReader;
            private BinaryWriter bWriter;
            private readonly static object mut = new object();
            private static Instructions instructions = null;
            private Dictionary<string, string> commandsToPath;

            /**
             * Private CTOR - SingleTon.
             **/
            private Instructions()
            {
                client = null;
                stream = null;
                commandsToPath = new Dictionary<string, string>();
                InitPathMapValues();
                
            }
            public static Instructions getInstance
            {
                get
                {
                    // if exists return instance, else create it and return.
                    lock (mut)
                    {
                        return instructions ?? (instructions = new Instructions());
                    }
                }
            }
            /// <summary>
            /// send the command after set like it should be.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="val"></param>
            public void ManualSendCommand(string command, double val)
            {
                if (client == null)
                {
                    return;
                }
                string line = "set " + commandsToPath[command] + " ";
                line += val.ToString("N5") + "\r\n";
                lock (mut)
                {
                    byte[] buffer = System.Text.Encoding.ASCII.GetBytes(line.ToString());
                    stream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("command: " + line + " sent");
                    stream.Flush();
                }
            }
        public double GetParamValue(string command)
        {
            if (string.IsNullOrEmpty(command)) return 0;
            string buffer = "get " + commandsToPath[command] + "\r\n";
            bWriter.Write(Encoding.ASCII.GetBytes(buffer));
            char c;
            string line = "";
            while((c= bReader.ReadChar())!='\n')  line += c;
            return ConvertToDouble(line);
        }
        public double ConvertToDouble(string line)
        {
            string afterParsed = "";
            string[] values = line.Split('\'');
            afterParsed = values[1];
            return double.Parse(afterParsed);
        }
            /**
             * Init all map path values by command name.
             * */
            private void InitPathMapValues()
            {
                commandsToPath.Add("rudder", "controls/flight/rudder");
                commandsToPath.Add("throttle", "controls/engines/current-engine/throttle");
                commandsToPath.Add("aileron", "controls/flight/aileron");
                commandsToPath.Add("elevator", "controls/flight/elevator");
                commandsToPath.Add("lon", "position/longitude-deg");
                commandsToPath.Add("lat", "position/latitude-deg");
        }
            public void open(string ip, int port)
            {
                if (isOpen())
                {
                    return;
                }
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                stream.Flush();
                Console.WriteLine("connected, " + ip + " " + port.ToString());
                bReader = new BinaryReader(stream);
                bWriter = new BinaryWriter(stream);
        }
            public void close()
            {
                stream.Close();
                client.Close();
            }
            public void send(List<string> commands)
            {
                if (null == client) return;
                Thread t = new Thread(() =>
                {
                    foreach (string command in commands)
                    {
                        string cmd = command + "\r\n";
                        lock (mut)
                        {
                            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(cmd.ToString());
                            stream.Write(buffer, 0, buffer.Length);
                           
                            Console.WriteLine("command: " + cmd + " sent");
                            stream.Flush();
                        }
                        Thread.Sleep(2000);
                    }
                });
                t.Start();
            }
            public bool isOpen()
            {
                return null != this.client;
            }
        }
    }