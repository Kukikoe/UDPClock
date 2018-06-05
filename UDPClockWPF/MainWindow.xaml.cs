using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;


namespace UDPClockWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static int localPort = 1024;
        static IPAddress remoteAddress;
        DateTime time;

        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            remoteAddress = IPAddress.Parse("235.5.5.11");
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start();
        }

        private void CompositionTarget_Rendering(object sender, object args)
        {
                rotateSecond.Angle = 6 * (time.Second + time.Millisecond / 1000.0);
                rotateMinute.Angle = 6 * time.Minute + rotateSecond.Angle / 60;
                rotateHour.Angle = 30 * (time.Hour % 12) + rotateMinute.Angle / 12;  
        }

        private void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            receiver.JoinMulticastGroup(remoteAddress, 20);
            IPEndPoint remoteIp = null; // адрес входящего подключения
            string localAddress = LocalIPAddress();
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string str_time = Encoding.Unicode.GetString(data);
                    time = DateTime.Parse(str_time);
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
