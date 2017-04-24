using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Jaylan.Utilities
{
    /// <summary>
    /// 打印机
    /// 热敏打印机
    /// </summary>
    public class NetPOSPrinter
    {
        private readonly string _prienterIp = "127.0.0.1";//打印机Ip
        private readonly int _pinterPort = 9100; //9100打印机指定端口

        private Socket _printSocket;
        private bool _isConnect = false;



        private const string NewLine = "\r\n";
        private readonly Encoding _printEncoding = Encoding.GetEncoding("GB2312");


        public NetPOSPrinter()
        {
        }

        public NetPOSPrinter(string ip)
        {
            _prienterIp = ip;//打印机IP   
        }

        public NetPOSPrinter(string ip, int port)
        {
            _prienterIp = ip;
            _pinterPort = port; //打印机端口   
        }


        /// <summary>
        /// 建立连接
        /// </summary>
        public void Connect()
        {
            var ipa = IPAddress.Parse(_prienterIp);
            var ipe = new IPEndPoint(ipa, _pinterPort);
            _printSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _printSocket.Connect(ipe);
            _isConnect = true;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _printSocket.Dispose();
            _isConnect = false;
        }


        /// <summary>
        /// 打印图片
        /// </summary>
        /// <param name="bmp"></param>
        public void PrintPic(Bitmap bmp)
        {
            //把ip和端口转化为IPEndPoint实例  
            var ipEndpoint = new IPEndPoint(IPAddress.Parse(_prienterIp), _pinterPort);
            //创建一个Socket  
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            //连接到服务器  
            socket.Connect(ipEndpoint);
            //应对同步Connect超时过长的办法，猜测应该是先用异步方式建立以个连接然后，  
            //确认连接是否可用，然后报错或者关闭后，重新建立一个同步连接                      

            //socket.SendTimeout = 1000;  

            //初始化打印机，并打印
            var byteSend = Encoding.GetEncoding("GB2312").GetBytes("\x1b\x40");
            //发送测试信息  
            socket.Send(byteSend, byteSend.Length, 0);


            var data = new byte[] { 0x1B, 0x33, 0x00 };
            socket.Send(data, data.Length, 0);
            data[0] = (byte)'\x00';
            data[1] = (byte)'\x00';
            data[2] = (byte)'\x00';    // Clear to Zero.  

            // ESC * m nL nH 点阵图  
            var escBmp = new byte[] { 0x1B, 0x2A, 0x00, 0x00, 0x00 };

            escBmp[2] = (byte)'\x21';

            //nL, nH  
            escBmp[3] = (byte)(bmp.Width % 256);
            escBmp[4] = (byte)(bmp.Width / 256);
            // data  
            for (var i = 0; i < (bmp.Height / 24) + 1; i++)
            {
                socket.Send(escBmp, escBmp.Length, 0);

                for (var j = 0; j < bmp.Width; j++)
                {
                    for (var k = 0; k < 24; k++)
                    {
                        if (((i * 24) + k) < bmp.Height)   // if within the BMP size  
                        {
                            var pixelColor = bmp.GetPixel(j, (i * 24) + k);
                            if (pixelColor.R == 0)
                            {
                                data[k / 8] += (byte)(128 >> (k % 8));
                            }
                        }
                    }
                    socket.Send(data, 3, 0);
                    data[0] = (byte)'\x00';
                    data[1] = (byte)'\x00';
                    data[2] = (byte)'\x00';    // Clear to Zero.  
                }

                byteSend = Encoding.GetEncoding("GB2312").GetBytes("\n");

                //发送测试信息  
                socket.Send(byteSend, byteSend.Length, 0);
            } // data  

            byteSend = Encoding.GetEncoding("GB2312").GetBytes("\n");

            //发送测试信息  
            socket.Send(byteSend, byteSend.Length, 0);
            socket.Close();
        }


        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="str"></param>
        public void PrintStr(string str)
        {
            if (!_isConnect)
            {
                Connect();
            }
            var strBtyes = _printEncoding.GetBytes(str + NewLine);
            _printSocket.Send(strBtyes);
        }

        /// <summary>
        /// 打印空白行
        /// </summary>
        public void PrintBlank(int rowNum = 1)
        {
            if (!_isConnect)
            {
                Connect();
            }
            for (var i = 0; i < rowNum; i++)
            {
                var strBtyes = _printEncoding.GetBytes("  " + NewLine);
                _printSocket.Send(strBtyes);
            }
        }



        public void SetFontSize(byte size = 20)
        {
            if (!_isConnect)
            {
                Connect();
            }
            var fontSize = _printEncoding.GetBytes("" + (char)(27) + (char)(33) + (char)(size));
            _printSocket.Send(fontSize);
        }

        public void InitPrint()
        {
            if (!_isConnect)
            {
                Connect();
            }
            var init = "" + (char)(27) + (char)(64);
            var initBytes = _printEncoding.GetBytes(init);
            _printSocket.Send(initBytes);
        }

        /// <summary>
        /// 切纸
        /// </summary>
        public void Cut()
        {
            if (!_isConnect)
            {
                Connect();
            }
            //var cutBytes = new byte[] { 0x1b, 0x69 };
            var cutBytes = new byte[] { 0x1d, 0x56, 0x42, 0x00 };
            _printSocket.Send(cutBytes);
        }

    }
}