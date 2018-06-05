using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SendMessageUDP
{
    class Program
    {
        static string remoteAddress = "235.5.5.11";
        static int remotePort = 1024;

        static void Main(string[] args)
        {         
                SendMessage();
        }

        private static void SendMessage()
        {
            UdpClient sender = new UdpClient(); // создаем UdpClient для отправки данных
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    byte[] data = Encoding.Unicode.GetBytes(DateTime.Now.ToString("HH:mm:ss"));
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
