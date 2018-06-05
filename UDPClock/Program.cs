using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDPClock
{
    class Program
    {

        static int localPort = 1024;
        static IPAddress remoteAddress;

        static void Main(string[] args)
        {
            try
            {
                remoteAddress = IPAddress.Parse("235.5.5.11");
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            receiver.JoinMulticastGroup(remoteAddress, 50);
            IPEndPoint remoteIp = null; // адрес входящего подключения
            string localAddress = LocalIPAddress();
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string str_time = Encoding.Unicode.GetString(data);
                    var time = TimeSpan.Parse(str_time);
                    Console.Clear();
                    Console.WriteLine(time);                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }

        private static string LocalIPAddress()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

    }
}
