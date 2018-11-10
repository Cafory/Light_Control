using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Threading;

namespace Light_Control_Test
{
    class PortHelper
    {
        public static byte [] receiver = null;
        public static string[] portNames;
        public static SerialPort serialPort = new SerialPort();
        public  static bool isOpen = false;
        private static bool allowSend = true;
        public static void OpenPort()   //打开端口
        {
            try
            {
                Configuration ca = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (isOpen == false)
                {
                    portNames = SerialPort.GetPortNames();    //获得端口名字集合
                    if (portNames.Length > 0)
                    {
                        serialPort.PortName =portNames[0];
                        serialPort.BaudRate = 9600;             //设置波特率
                        serialPort.Parity = Parity.None;
                        //serialPort.StopBits = StopBits.None;  
                        serialPort.DataBits = 8;                //设置数据位数
                        serialPort.Handshake = Handshake.None;   //握手协议
                        serialPort.DataReceived += SerialPort_DataReceived;   //接收数据的事件委托
                        serialPort.Open();                                    //打开端口

                    }
                    isOpen = true;
                }
            }
            catch
            {
                MessageBox.Show("打开串口失败！");
            }
        }
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] reciervbyte = new byte[serialPort.BytesToRead];
            serialPort.Read(reciervbyte, 0, reciervbyte.Length);
            receiver = reciervbyte;
            allowSend = true;
        }

        public static int  checkSum=0;
        public static void SendData(byte[] datas)
        {
            checkSum = 0;
            try                               //try语句异常提示
            {

                //if (allowSend)
                //{
                    allowSend = false;       //防止发送的时候二次发送
                    byte[] dataToSend = new byte[datas.Length + 1];
                    for (int i = 0; i < datas.Length; i++)
                    {
                        dataToSend[i] = datas[i];
                    }
                    checkSum= Convert.ToByte((dataToSend[1] + dataToSend[2] + dataToSend[3] + dataToSend[4] + dataToSend[5] + dataToSend[6] + dataToSend[7]) % 256);
                dataToSend[datas.Length] = Convert.ToByte((dataToSend[1] + dataToSend[2] + dataToSend[3] + dataToSend[4] + dataToSend[5] + dataToSend[6] + dataToSend[7]) % 256);  //校验和位
                    serialPort.Write(dataToSend, 0, dataToSend.Length);

                  //  MessageBox.Show("发送成功");
                //}
                //else
                //{
                //    MessageBox.Show("请等待当前数据接收完毕!", "提示");
                
                //}
            }
            catch
            {
                MessageBox.Show("串口设置错误或串口被关闭！", "提示！");
            }
        }
        public static void  HSendData(byte[] datas)
        {
            if (datas != null)
            {
                serialPort.Write(datas, 0, datas.Length);
            }
        }
    }
}
