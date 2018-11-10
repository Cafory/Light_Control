using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Light_Control_Test
{
    public static class Instruction
    {
        public static byte[] backByte = null;

        /// <summary>
        /// 1.主机对配电箱：寻配电箱码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_FindpeiCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x01, 0, 0 });
        }


        /// <summary>
        /// 2.主机对所有配电箱：所有配电箱寻灯码，PS：这里有个D2*100还没有写
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool All_P_FindLightCode(string data)  //
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x02, 00, 00 });
        }

        /// <summary>
        /// 3.主机对单个配电箱：询问配电箱下的灯的数量（判断配电箱寻灯是否完毕）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_AskLightAmount(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x03, 00, 00 });
        }

        /// <summary>
        /// 4.主机对单个配电箱：询问指定配电箱下灯码  PS：这里有两个配电箱不知道怎么写
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_AskSelectedLightCode(string data, string pei)
        {
            byte[] databyte = ProcessString(pei);
            return L_ReceiveMethod(data, pei, new byte[] { 0x66, 0x04, databyte[1], databyte[0] });
        }

        /// <summary>
        /// 主机对单个配电箱：屏蔽指定配电箱下指定灯码 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_BanSelectedLightCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0E, 00, 00 });
        }

        /// <summary>
        /// 5.主机对单个灯：询问指定灯码 寻灯 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool L_AskSelectedLight(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x05, 00, 00 });
        }

        /// <summary>
        /// 6.主机对单个灯：设定指定灯的状态    PS：这里由灯状态控制字，应该根据不同的状态设定不同的方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool L_SetSelectedLightStatus(string data, byte[] conByte)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x05, conByte[0], conByte[1] });
        }

        /// <summary>
        /// 7.主机对单个配电箱：询问指定配电箱下灯码和灯的状态  PS：遇到了选地址的问题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_AskLightCodeAndStatus(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x06, 00, 00 });
        }

        /// <summary>
        /// 8.1主机对单个配电箱：指定配电箱下的灯全闪
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_LightShine(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0D, 0x0C, 0x77 });
        }

        /// <summary>
        /// 8.2主机对单个配电箱：指定配电箱下的灯恢复闪前状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_LightBeforeShine(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x08, 00, 00 });
        }



        /// <summary>
        /// 9.主机对所有灯：灯恢复初始状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool L_IniStatus(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x09, 00, 00 });
        }

        /// <summary>
        /// 10.主机对单个配电箱：指定配电箱下灯复位态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_SelectedPeiLightReset(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0A, 00, 00 });
        }


        /// <summary>
        /// 11.主机对所有配电箱：应急
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool All_P_ForEmergency(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0B, 00, 00 });
        }


        /// <summary>
        /// 12.主机对所有配电箱：主电
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool All_P_MainE(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0C, 00, 00 });
        }




        /// <summary>
        /// 13.主机对单个配电箱：控制指定配电箱下所有灯的状态   PS：对于这种只有一位不同的可以加参数啊，然后利用Switch语句记一下
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_AllLightsStatues(string data, byte[] conByte)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0D, conByte[0], conByte[1] });
        }


        /// <summary>
        /// 14.配电箱查询灯的状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_AskLightStatus(string data)
        {
            return ReceiveMethod(data, new byte[] { 0xAA, 0x10, 00, 00 });
        }

        /// <summary>
        /// 15.1 编码器对配电箱：写配电箱的码  PS :???这句话干啥用的
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_WritePeiCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x0F, 00, 00 });
        }

        /// <summary>
        /// 15.2 读配电箱的码  PS :???这句话干啥用的
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool P_ReadPeiCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0x66, 0x14, 00, 00 });
        }


        /// <summary>
        /// 16.1 编码器对灯：写灯码  PS :???这句话干啥用的
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool L_WriteLightCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0xAA, 0x0F, 00, 00 });
        }

        /// <summary>
        /// 16.2 读配电箱的码  PS :???这句话干啥用的
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool L_ReadLighteCode(string data)
        {
            return ReceiveMethod(data, new byte[] { 0xAA, 0x14, 00, 00 });
        }

        /// <summary>
        /// 22.1上位机控制灯年/月检
        /// </summary>
        /// <returns></returns>
        public static bool C_ControlYearCheck()   //主电指令
        {
            return ReceiveMethod("00000000", new byte[] { 0x66, 0x0C, 0x00, 00 });
        }

        /// <summary>
        /// 22.2上位机控制灯月检 //应急指令
        /// </summary>
        /// <returns></returns>
        public static bool C_ControlMonthCheck()
        {
            return ReceiveMethod("00000000", new byte[] { 0x66, 0x0B, 0x00, 00 });
        }

        public static bool ReceiveMethod(string data, byte[] commandData)   //对于接收数据的处理
        {
            PortHelper.receiver = null;
            bool flag = false;
            byte[] dataToByte = ProcessString(data);
            PortHelper.SendData(new byte[] { commandData[0], commandData[1], dataToByte[3], dataToByte[2], dataToByte[1], dataToByte[0], commandData[2], commandData[3] });
            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < 500000000; i++)
            {
                if (PortHelper.receiver != null)
                {
                    byte[] receiveData = PortHelper.receiver;                 
                    backByte = receiveData;
                    flag = true;
                    break;
                }
            }
          
           // MessageBox.Show(sw.Elapsed.ToString());
            PortHelper.receiver = null;
            return flag;
        }


        public static bool L_ReceiveMethod(string Ldata, string Pdata, byte[] commandData)   //对于接收数据的处理
        {
            PortHelper.receiver = null;
            bool flag = false;
            byte[] LdataToByte = ProcessString(Ldata);
            byte[] PdataToByte = ProcessString(Pdata);
            byte[] receiveData = PortHelper.receiver;
            PortHelper.SendData(new byte[] { 0x66, 0x05, LdataToByte[3], LdataToByte[2], LdataToByte[1], LdataToByte[0], 00, 00 });
            //Thread.Sleep(3000);
            Stopwatch sw = new Stopwatch(); 
           
            sw.Start();
            for (int i = 0; i < 70000000; i++)   //5000万次循环是  500ms左右
            {
                receiveData = PortHelper.receiver;
                if (receiveData != null)
                {
                    if (receiveData.Length==5)
                    {
                        if (receiveData[2] == PdataToByte[0])
                        {
                            backByte = receiveData;
                            flag = true;
                            break;
                        }
                    }
                }
             
            }

            sw.Stop();
        //    MessageBox.Show(sw.ElapsedMilliseconds.ToString());
            PortHelper.receiver = null;
            return flag;
        }



        private static byte[] ProcessString(string data)     //对输入的码进行处理，转化为byte
        {


            byte[] dataToByte = new byte[4];
            dataToByte[3] = (byte)(Convert.ToInt32(data) / 0x1000000);
            dataToByte[2] = (byte)((Convert.ToInt32(data) - dataToByte[3] * 0x1000000) / 0x10000);
            dataToByte[1] = (byte)((Convert.ToInt32(data) - dataToByte[3] * 0x1000000 - dataToByte[2] * 0x10000) / 0x100); ;
            dataToByte[0] = (byte)(Convert.ToInt32(data));

            return dataToByte;
        }
    }
}
