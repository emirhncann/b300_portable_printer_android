using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using SmarnetTest;
using System.Threading;

namespace POSdllDemo
{
    public partial class Form1 : Form
    {
        private IntPtr Gp_IntPtr;                   //驱动打印句柄
        public libUsbContorl.UsbOperation NewUsb=new libUsbContorl.UsbOperation();
        PrinterDEMO.UserControl_COM_Connect Ctrl_COM_Connect = new PrinterDEMO.UserControl_COM_Connect();//自定义用户控件，COM口发送指令测试，实例化
        PrinterDEMO.UserControl_USB_Connect Ctrl_USB_Connect = new PrinterDEMO.UserControl_USB_Connect();//自定义用户控件，USB口发送指令测试，实例化

        public Form1()
        {
            InitializeComponent();
            tabPage_COM.Controls.Add(Ctrl_COM_Connect);
            tabPage_USB.Controls.Add(Ctrl_USB_Connect);
        }

        /// <summary>
        /// 网口打印测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoadPOSDll PosPrint = new LoadPOSDll();
            
						//POS_COM_DTR_DSR 0x00 流控制为DTR/DST  
						//POS_COM_RTS_CTS 0x01 流控制为RTS/CTS 
						//POS_COM_XON_XOFF 0x02 流控制为XON/OFF 
						//POS_COM_NO_HANDSHAKE 0x03 无握手 
						//POS_OPEN_PARALLEL_PORT 0x12 打开并口通讯端口 
						//POS_OPEN_BYUSB_PORT 0x13 打开USB通讯端口 
						//POS_OPEN_PRINTNAME 0X14 打开打印机驱动程序 
						//POS_OPEN_NETPORT 0x15 打开网络接口 

            if (PosPrint.OpenNetPort("192.168.0.123"))//当参数nParam的值为POS_OPEN_NETPORT时，表示打开指定的网络接口，如“192.168.10.251”表示网络接口IP地址，打印时参考
            {
                Gp_IntPtr = PosPrint.POS_IntPtr;
            }
            if (LoadPOSDll.POS_StartDoc())
            {
                byte[] by_SendData = System.Text.Encoding.Default.GetBytes("test print\r\n");
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, by_SendData, (uint)by_SendData.Length);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x0a }, 1);
                LoadPOSDll.POS_EndDoc();
            }
        }
        
        public void SendData2USB(byte[] str)
        {
        	NewUsb.SendData2USB(str,str.Length);
        }
        public void SendData2USB(string str)
        {
        	byte[] by_SendData=System.Text.Encoding.GetEncoding(54936).GetBytes(str);
        	SendData2USB(by_SendData);
        }
        /// <summary>
        /// 重写metset
        /// </summary>
        /// <param name="buf">设置的数组</param>
        /// <param name="val">设置的数据</param>
        /// <param name="size">数据长度</param>
        /// <returns>void</returns>     
        public void memset(byte[] buf, byte val, int size)
        {
            int i;
            for (i = 0; i < size; i++)
                buf[i] = val;
        }
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 获取软件版本号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Version ApplicationVersion = new Version(Application.ProductVersion);
            string AssmblyVersion = ApplicationVersion.ToString();//获取主版本号  
            this.Text += " V" + AssmblyVersion;
        }

        private void tabPage_USB_Click(object sender, EventArgs e)
        {

        }
    }
}
