using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PrinterDEMO
{
    public partial class UserControl_USB_Connect : UserControl
    {
        public UserControl_USB_Connect()
        {
            InitializeComponent();
        }

        POSdllDemo.Form1 fm;//实例化主窗口类
        /// <summary>
        /// 设置本用户控件的父窗口为主窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmarnetDEMO_ParentChanged(object sender, EventArgs e)
        {
            fm = (POSdllDemo.Form1)this.FindForm();
        }
        /// <summary>
        /// ESC文本打印测试，具体指令含义可查找ESC/POS指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_Text_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                int i = 0;
                if (fm.NewUsb.LinkUSB(k))
                {
                    byte[] KanjiMode = { 0x1c, 0x26 };//汉字模式
                    fm.SendData2USB(KanjiMode);

                    #region 打印信息测试
                    byte[] SendData = { 0x1b, 0x40, 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
                    fm.SendData2USB(SendData);

                    string strSendData = "USB联机测试";
                    fm.SendData2USB(strSendData);

                    fm.SendData2USB(new byte[] { 0x0a, 0x0a });
                    fm.SendData2USB(new byte[] { 0x1b, 0x61, 0x00, 0x1b, 0x21, 0x00, 0x1c, 0x57, 0x00 });

                    fm.SendData2USB("技术指标：\n");
                    fm.SendData2USB("*分辨率：203dpi(8dots/mm)\n");
                    fm.SendData2USB("*打印宽度：72mm\n");
                    fm.SendData2USB("*打印速度：120mm/s(Max)\n");
                    fm.SendData2USB("*打印密度：576点/行\n");
                    fm.SendData2USB("*打印浓度：1~6\n");
                    fm.SendData2USB("*使用寿命：50km\n");
                    fm.SendData2USB("*电源要求：DC 5V，2A\n");
                    fm.SendData2USB("*USB接口：USB2.0\n");
                    fm.SendData2USB("*无线接口：Bluetooth4.2，Wifi2.4G/5G\n\n");
                    #endregion

                    #region 字体打印测试
                    fm.SendData2USB(KanjiMode);
                    SendData = new byte[48];
                    int linecount = 3;
                    byte bit = 0xa1, Zone = 0xa1;
                    for (i = 0; i < 48; i += 2)
                    {
                        SendData[i] = Zone;
                        SendData[i + 1] = bit;
                        bit++;
                    }
                    fm.SendData2USB(new byte[] { 0x0a });
                    fm.SendData2USB(SendData);

                    Zone = 0xb0;
                    bit = 0xa1;
                    for (i = 0; i < linecount; i++)
                    {
                        for (int j = 0; j < 48; j += 2)
                        {
                            SendData[j] = Zone;
                            SendData[j + 1] = bit;
                            Zone++;
                        }
                        bit++;
                        fm.SendData2USB(new byte[] { 0x0a });
                        fm.SendData2USB(SendData);
                    }
                    fm.SendData2USB(new byte[] { 0x0a, 0x0a, 0x0a, 0x0a });
                    #endregion

                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// 发送ESC光栅位图
        /// </summary>
        /// <param name="filename"></param>
        public void ESC_sendGSBmpStream(string filename, byte printMod)
        {
            byte[] setHBit = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };    //算法辅助
            byte[] clsLBit = { 0x7F, 0xBF, 0xDF, 0xEF, 0xF7, 0xFB, 0xFD, 0xFE };    //算法辅助

            uint sendWidth = 0;      //实际发送的宽
            uint sendHeight = 0;    //实际发送的高
            byte[] SendBmpData = new byte[] { };

            if (filename != "")
            {//支持4，8位 位图
                StreamReader srReadFile = new StreamReader(filename);

                byte[] byteReaddata = fm.StreamToBytes(srReadFile.BaseStream);//获取读取文件的byte[]数据
                srReadFile.Close();

                Image getimage = Image.FromFile(filename);

                sendWidth = (uint)getimage.Width;      //实际发送的宽
                sendHeight = (uint)getimage.Height;    //实际发送的高

                if (getimage.Height % 8 != 0)
                    sendHeight = sendHeight + 8 - sendHeight % 8;
                if (getimage.Width % 8 != 0)
                    sendWidth = sendWidth + 8 - sendWidth % 8;

                Bitmap getbmp = new Bitmap(getimage);
                //                     Bitmap BmpCopy = new Bitmap(getimage.Width, getimage.Height, PixelFormat.Format32bppArgb);

                SendBmpData = new byte[sendWidth * sendHeight / 8];

                #region 求灰度平均值
                Double redSum = 0, geedSum = 0, blueSum = 0;
                Double total = sendWidth * sendHeight;
                byte[] huiduData = new byte[sendWidth * sendHeight / 8];
                for (int i = 0; i < getimage.Width; i++)
                {
                    int ta = 0, tr = 0, tg = 0, tb = 0;
                    for (int j = 0; j < getimage.Height; j++)
                    {
                        Color getcolor = getbmp.GetPixel(i, j);//取每个点颜色
                        ta = getcolor.A;
                        tr = getcolor.R;
                        tg = getcolor.G;
                        tb = getcolor.B;
                        redSum += tr;
                        geedSum += tg;
                        blueSum += tb;
                    }
                }
                int meanr = (int)(redSum / total);
                int meang = (int)(geedSum / total);
                int meanb = (int)(blueSum / total);
                #endregion 求灰度平均值

                for (int j = 0; j < getimage.Height; j++)
                {
                    for (int i = 0; i < getimage.Width; i++)
                    {
                        Color getcolor = getbmp.GetPixel(i, j);//取每个点颜色
                        if ((getcolor.R * 0.299) + (getcolor.G * 0.587) + (getcolor.B * 0.114) < ((meanr * 0.299) + (meang * 0.587) + (meanb * 0.114)))//颜色转灰度(可调 0-255)
                            SendBmpData[j * sendWidth / 8 + i / 8] |= setHBit[i % 8];
                        //                         if (getcolor.R < meanr)//颜色转灰度(可调 0-255)
                        //                             SendBmpData[i * sendHeight / 8 + j / 8] |= setHBit[j % 8];
                    }
                }
                getimage.Dispose();
                getbmp.Dispose();
            }
            byte[] cmd = new byte[] { 0X1B, 0X40, 0X1D, 0X76, 0X30 };//1B 40--初始化
            fm.SendData2USB(cmd);//开始下载位图 1D 76 30

            //发送 横向点数/8 纵向点数 
            cmd = new byte[5];
            cmd[0] = printMod;
            cmd[1] = (byte)(sendWidth / 8 % 256);
            cmd[2] = (byte)(sendWidth / 8 / 256);
            cmd[3] = (byte)(sendHeight % 256);
            cmd[4] = (byte)(sendHeight / 256);
            fm.SendData2USB(cmd);
            fm.SendData2USB(SendBmpData);
        }
        /// <summary>
        /// ESC图片打印测试，具体指令含义可查找ESC/POS指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_Image_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    ESC_sendGSBmpStream(Application.StartupPath + "\\10.bmp", 0);
                    fm.SendData2USB(new byte[] { 0x0a, 0x0a, 0x0a });
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// ESC一维码打印测试，具体指令含义可查找ESC/POS指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_Barcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("条码类型: UPC-A\n");
                    fm.SendData2USB("HRI字符的打印位置: 条码下方\n");
                    fm.SendData2USB(new byte[] { 0x1D, 0x48, 0x02 });//GS H 选择 HRI 字符的打印位置
                    fm.SendData2USB("HRI使用字体: 标准ASCII字体12*24\n");
                    fm.SendData2USB(new byte[] { 0x1D, 0x66, 0x00 });//GS f 选择 HRI 使用字体
                    fm.SendData2USB("条码高度: 80\n");
                    fm.SendData2USB(new byte[] { 0x1D, 0x68, 0x50 });//GS h 选择条码高度
                    fm.SendData2USB("条码宽度: 0.25\n");
                    fm.SendData2USB(new byte[] { 0x1D, 0x77, 0x02 });//GS w 设置条码宽度
                    fm.SendData2USB("内容: 12345678901\n\n");
                    fm.SendData2USB(new byte[] { 0x1D, 0x6B, 0x00, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x00, 0x0A });
                    fm.SendData2USB(new byte[] { 0x0a, 0x0a, 0x0a });
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// ESC二维码打印测试，具体指令含义可查找ESC/POS指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_QRcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    byte[] ModuleSet = { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, 0x07 };//模块大小（0x00-0x10），默认为0x07
                    byte[] LevelSet = { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, 0x30 };//纠错等级（0x30-0x33），默认为0x30
                    byte[] InputSet = { 0x1D, 0x28, 0x6B, 0x00, 0x00, 0x31, 0x50, 0x30 };//输入内容存储到发送区
                    byte[] PrintQR = { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30 };//

                    fm.SendData2USB("条码类型: QRcode\n");
                    fm.SendData2USB("模块大小: 7\n");
                    fm.SendData2USB(ModuleSet);
                    fm.SendData2USB("纠错等级: L\n");
                    fm.SendData2USB(LevelSet);
                    fm.SendData2USB("内容: TEXT12345678901\n\n");

                    byte[] QR_content = Encoding.UTF8.GetBytes("TEXT12345678901");//要扫描中文二维码必须赢utf8编码
                    InputSet[3] = (byte)((QR_content.Length + 3) % 256);
                    InputSet[4] = (byte)((QR_content.Length + 3) / 256);

                    fm.SendData2USB(InputSet);
                    fm.SendData2USB(QR_content);
                    fm.SendData2USB(PrintQR);

                    fm.SendData2USB(new byte[] { 0x0a, 0x0a, 0x0a });
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL文本打印测试，具体指令含义可查找TSPL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_Text_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("SIZE 80 mm,50 mm\r\n");//标签尺寸
                    fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
                    fm.SendData2USB("CLS\r\n");
                    fm.SendData2USB("TEXT 0,20,\"TSS24.BF2\",0,1,1,\"下面6个文本大小各不相同,能打印中文\"\r\n");
                    fm.SendData2USB("TEXT 0,100,\"TSS16.BF2\",0,1,1,\"TSS16.BF2 简体中文16*16(GB码)\"\r\n");
                    fm.SendData2USB("TEXT 0,140,\"TSS20.BF2\",0,1,1,\"TSS20.BF2 简体中文20*20(GB码)\"\r\n");
                    fm.SendData2USB(Encoding.GetEncoding("big5").GetBytes("TEXT 0,180,\"TST24.BF2\",0,1,1,\"TST24.BF2 繁體中文24*24(大五碼)\"\r\n"));
                    fm.SendData2USB("TEXT 0,220,\"TSS24.BF2\",0,1,1,\"TSS24.BF2 简体中文24*24(GB码)\"\r\n");
                    fm.SendData2USB(Encoding.GetEncoding(949).GetBytes("TEXT 0,260,\"K\",0,1,1,\"K 한국어24*24Font(KS숫자)\"\r\n"));
                    fm.SendData2USB("TEXT 0,300,\"TSS32.BF2\",0,1,1,\"TSS32.BF2 简体中文32*32(GB码)\"\r\n");
                    fm.SendData2USB("PRINT 1\r\n");

                    fm.SendData2USB("SIZE 80 mm,70 mm\r\n");//标签尺寸
                    fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
                    fm.SendData2USB("CLS\r\n");
                    fm.SendData2USB("TEXT 0,20,\"TSS24.BF2\",0,1,1,\"下面10个文本大小各不相同,但不能打印中文\"\r\n");
                    fm.SendData2USB("TEXT 0,100,\"" + "1" + "\",0,1,1,\"" + "1 8*12dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,140,\"" + "2" + "\",0,1,1,\"" + "2 12*20dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,180,\"" + "3" + "\",0,1,1,\"" + "3 16*24dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,220,\"" + "4" + "\",0,1,1,\"" + "4 24*32dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,260,\"" + "5" + "\",0,1,1,\"" + "5 32*48dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,320,\"" + "6" + "\",0,1,1,\"" + "6 14*19dot OCR-B ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,350,\"" + "7" + "\",0,1,1,\"" + "7 21*27dot OCR-B ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,390,\"" + "8" + "\",0,1,1,\"" + "8 14*25dot OCR-A ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,430,\"" + "9" + "\",0,1,1,\"" + "9 9*17dot ABCabc\"\r\n");
                    fm.SendData2USB("TEXT 0,460,\"" + "10" + "\",0,1,1,\"" + "10 12*24dot ABCabc\"\r\n");
                    fm.SendData2USB("PRINT 1\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// 发送TSPL光栅位图
        /// </summary>
        /// <param name="filename"></param>
        public void TSPL_sendGSBmpStream(string filename, byte printMod)
        {
            string strReadFilePath = filename;
            StreamReader srReadFile = new StreamReader(strReadFilePath);

            byte[] b_bmpdata = fm.StreamToBytes(srReadFile.BaseStream);//获取读取文件的byte[]数据
            srReadFile.Close();

            byte bmpBitCount = b_bmpdata[0x1c]; //获取位图 位深度

            uint byteLeght = (uint)b_bmpdata.Length;
            if (byteLeght > 2048000)
            {
                MessageBox.Show("所选文件过大");
                return;
            }

            byte[] SendBmpData;
            uint sendWidth = 0;     //实际发送的宽
            uint sendHeight = 0;    //实际发送的高
            byte[] setHBit = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };    //算法辅助 置1
            byte[] clsLBit = { 0x7F, 0xBF, 0xDF, 0xEF, 0xF7, 0xFB, 0xFD, 0xFE };    //算法辅助 置0

            {
                Stream str1 = new MemoryStream();
                Image getimage = Image.FromFile(strReadFilePath);

                sendWidth = (uint)getimage.Width;      //实际发送的宽
                sendHeight = (uint)getimage.Height;    //实际发送的高

                if (getimage.Height % 8 != 0)
                    sendHeight = sendHeight + 8 - sendHeight % 8;
                if (getimage.Width % 8 != 0)
                    sendWidth = sendWidth + 8 - sendWidth % 8;

                Bitmap getbmp = new Bitmap(getimage);
                //                     Bitmap BmpCopy = new Bitmap(getimage.Width, getimage.Height, PixelFormat.Format32bppArgb);

                SendBmpData = new byte[sendWidth * sendHeight / 8];
                fm.memset(SendBmpData, 0xff, (int)(sendWidth * sendHeight / 8));//0XFF为全白

                #region 求灰度平均值
                Double redSum = 0, geedSum = 0, blueSum = 0;
                Double total = sendWidth * sendHeight;
                byte[] huiduData = new byte[sendWidth * sendHeight / 8];
                for (int i = 0; i < getimage.Width; i++)
                {
                    int ta = 0, tr = 0, tg = 0, tb = 0;
                    for (int j = 0; j < getimage.Height; j++)
                    {
                        Color getcolor = getbmp.GetPixel(i, j);//取每个点颜色
                        ta = getcolor.A;
                        tr = getcolor.R;
                        tg = getcolor.G;
                        tb = getcolor.B;
                        redSum += ta;
                        geedSum += tg;
                        blueSum += tb;
                    }
                }
                int meanr = (int)(redSum / total);
                int meang = (int)(geedSum / total);
                int meanb = (int)(blueSum / total);
                #endregion 求灰度平均值

                for (int j = 0; j < getimage.Height; j++)
                {
                    for (int i = 0; i < getimage.Width; i++)
                    {
                        Color getcolor = getbmp.GetPixel(i, j);//取每个点颜色
                        if ((getcolor.R * 0.299) + (getcolor.G * 0.587) + (getcolor.B * 0.114) < ((meanr * 0.299) + (meang * 0.587) + (meanb * 0.114)))//颜色转灰度(可调 0-255)
                            SendBmpData[j * sendWidth / 8 + i / 8] &= clsLBit[i % 8];
                    }
                }
            }
            fm.SendData2USB("SIZE " + (sendWidth / 8 + 4).ToString() + " mm," + (sendHeight / 8 + 3).ToString() + " mm\n");
            fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
            fm.SendData2USB("CLS\r\n");
            fm.SendData2USB("BITMAP 0,0," + (sendWidth / 8).ToString() + "," + sendHeight.ToString() + ",0,");
            fm.SendData2USB(SendBmpData);
            fm.SendData2USB("PRINT 1\r\n");
        }
        /// <summary>
        /// TSPL图片打印测试，具体指令含义可查找TSPL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_Image_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    TSPL_sendGSBmpStream(Application.StartupPath + "\\10.bmp", 0);
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL条形码打印测试，具体指令含义可查找TSPL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_Barcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("SIZE 80 mm,30 mm\n");
                    fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
                    fm.SendData2USB("CLS\r\n");
                    fm.SendData2USB("TEXT 0,10,\"" + "TSS24.BF2" + "\",0,1,1,\"" + "条码类型：128\"\r\n");
                    fm.SendData2USB("BARCODE 10,50,\"128\",80,1,0,2,4,\"12345678901\"\r\n");
                    fm.SendData2USB("PRINT 1\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL二维码打印测试，具体指令含义可查找TSPL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_QRcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("SIZE 80 mm,40 mm\n");
                    fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
                    fm.SendData2USB("CLS\r\n");
                    fm.SendData2USB("TEXT 0,20,\"" + "TSS24.BF2" + "\",0,1,1,\"" + "打印二维码\"\r\n");
                    fm.SendData2USB("TEXT 0,60,\"" + "TSS24.BF2" + "\",0,1,1,\"" + "下面打印二维码的内容为：\"\r\n");
                    fm.SendData2USB("TEXT 0,100,\"" + "TSS24.BF2" + "\",0,1,1,\"" + "打印测试123456789" + "\"\r\n");
                    fm.SendData2USB("QRCODE 0,150,L,6,A,0,\"打印测试123456789\"\r\n");
                    fm.SendData2USB("PRINT 1\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL文本打印测试，具体指令含义可查找CPCL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Text_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 80 1\r\n");      //提示文本
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 7 0 10 40 下面10个文本大小各不相同，能打印中文\r\n");
                    fm.SendData2USB("PRINT\r\n");

                    fm.SendData2USB("! 0 200 200 500 1\r\n");
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 1 0 10 0 Font1 24dots*24dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 2 0 10 50 Font2 24dots*24dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 3 0 10 100 Font3 20dots*20dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 4 0 10 150 Font4 32dots*32dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 5 0 10 200 Font5 24dots*24dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 7 0 10 250 Font7 24dots*24dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 8 0 10 300 Font8 24dots*24dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 20 0 10 350 Font20 16dots*16dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 55 0 10 400 Font55 16dots*16dots 中文ABCabc\r\n");
                    fm.SendData2USB("TEXT 28 0 10 450 Font28 28dots*28dots 中文ABCabc\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL图片打印测试，具体指令含义可查找CPCL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Image_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 80 1\r\n");      //提示文本
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 24 0 10 10 GRAPHICS命令测试\r\n");
                    fm.SendData2USB("TEXT 24 0 10 40 下面打印GRAPHICS图像,EG命令,心形\r\n");
                    fm.SendData2USB("PRINT\r\n");

                    fm.SendData2USB("! 0 200 200 100 1\r\n");
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("EG 4 24 90 25 00F00F0000F00F0000F00F0000F00F000F0FF0F00F0FF0F00F0FF0F00F0FF0F0F000000FF000000FF000000FF000000F0F0000F00F0000F00F0000F00F0000F000F00F0000F00F0000F00F0000F00F00000FF000000FF000000FF000000FF000\r\n");
                    fm.SendData2USB("PRINT\r\n");

                    fm.SendData2USB("! 0 200 200 50 1\r\n");      //提示文本
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 24 0 10 10 下面打印GRAPHICS图像,CG命令,散件到付\r\n");
                    fm.SendData2USB("PRINT\r\n");

                    fm.SendData2USB(PrinterDEMO.Properties.Resources.CGSend_Data);
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL条形码打印测试，具体指令含义可查找CPCL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Barcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 300 1\r\n");      //提示文本
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 24 0 10 10 BARCODE命令测试，条码类型：UPCA\r\n");
                    fm.SendData2USB("TEXT 24 0 10 40 条码内容：123456789012\r\n");
                    fm.SendData2USB("BARCODE-TEXT 7 0 0\r\n");
                    fm.SendData2USB("BARCODE UPCA 2 1 80 10 100 123456789012\r\n");
                    fm.SendData2USB("BARCODE-TEXT OFF\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL二维码打印测试，具体指令含义可查找CPCL指令手册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_QRcode_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 80 1\r\n");      //提示文本
                    fm.SendData2USB("SETMAG 1 1\r\n");
                    fm.SendData2USB("TEXT 24 0 10 10 QR Code命令测试\r\n");
                    fm.SendData2USB("TEXT 24 0 10 40 下面的二维码内容为：打印测试123456789\r\n");
                    fm.SendData2USB("PRINT\r\n");

                    fm.SendData2USB("! 0 200 200 300 1\r\n");
                    fm.SendData2USB("SETMAG 1 1\n");
                    fm.SendData2USB("BARCODE QR 10 90 M 2 U 6\r\n");
                    fm.SendData2USB("LA,打印测试123456789\r\n");
                    fm.SendData2USB("ENDQR\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// ESC指令DLE EOT 实时状态传送，十六进制 10 04 n
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_State_Click(object sender, EventArgs e)
        {
            byte[] readdata = new byte[] { };

            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB(new byte[] { 0x1B, 0x40 });
                    fm.SendData2USB(new byte[] { 0x10, 0x04, 0x02 });

                    try//尝试执行
                    {
                        fm.NewUsb.ReadDataFmUSB(ref readdata);//打印机返回1个byte的数据，根据返回值，判断打印机当前状态。
                        tb_USB_state.Text += "打印机状态：" + readdata[0].ToString("X").PadLeft(2, '0') + "\r\n";
                    }
                    catch//异常处理
                    {
                        MessageBox.Show("USB查询打印机状态异常！");
                    }
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL指令<ESC>!?查询打印机状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_State_Click(object sender, EventArgs e)
        {
            byte[] readdata = new byte[] { };

            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB(new byte[] { 0x1B, 0x21, 0x3F });
                    try//尝试执行
                    {
                        fm.NewUsb.ReadDataFmUSB(ref readdata);//打印机返回1个byte的数据，根据返回值，判断打印机当前状态。
                        tb_USB_state.Text += "打印机状态：" + readdata[0].ToString("X").PadLeft(2, '0') + "\r\n";
                    }
                    catch//异常处理
                    {
                        MessageBox.Show("USB查询打印机状态异常！");
                    }
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令1B 68 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_State_Click(object sender, EventArgs e)
        {
            byte[] readdata = new byte[] { };

            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB(new byte[] { 0x1B, 0x68 });//发送1B 68查询打印状态
                    try//尝试执行
                    {
                        fm.NewUsb.ReadDataFmUSB(ref readdata);//打印机返回1个byte的数据，根据返回值，判断打印机当前状态。
                        tb_USB_state.Text += "打印机状态：" + readdata[0].ToString("X").PadLeft(2, '0') + "\r\n";
                    }
                    catch//异常处理
                    {
                        MessageBox.Show("USB查询打印机状态异常！");
                    }
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL指令，打印快递样张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_Proof_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    StreamReader srReadFile = new StreamReader(Application.StartupPath + "\\TSPL快递单样张.txt");
                    if (srReadFile == null)
                        return;
                    byte[] byteReaddata = fm.StreamToBytes(srReadFile.BaseStream);//获取读取文件的byte[]数据
                    srReadFile.Close();
                    fm.SendData2USB(byteReaddata);

                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令，打印快递样张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Proof_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    StreamReader srReadFile = new StreamReader(Application.StartupPath + "\\CPCL快递单样张.txt");
                    if (srReadFile == null)
                        return;
                    byte[] byteReaddata = fm.StreamToBytes(srReadFile.BaseStream);//获取读取文件的byte[]数据
                    srReadFile.Close();
                    fm.SendData2USB(byteReaddata);

                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令，打印PDF417二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Pdf417_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 300 1\r\n");
                    fm.SendData2USB("BARCODE PDF-417 10 20 XD 2 YD 6 C 3 S 2\r\n");
                    fm.SendData2USB("PDF Data\r\n");
                    fm.SendData2USB("ABCDE12345\r\n");
                    fm.SendData2USB("ENDPDF\r\n");
                    fm.SendData2USB("BARCODE PDF-417 10 100 XD 3 YD 12 C 3 S 2\r\n");
                    fm.SendData2USB("PDF Data\r\n");
                    fm.SendData2USB("ABCDE12345\r\n");
                    fm.SendData2USB("ENDPDF\r\n");
                    fm.SendData2USB("T 4 0 10 200 PDF Data\r\n");
                    fm.SendData2USB("T 4 0 10 250 ABCDE12345\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令，打印DATAMATRIX二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Datamatrix_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 300 1\r\n");
                    fm.SendData2USB("BARCODE DATAMATRIX 20 40 H 5\r\n");
                    fm.SendData2USB("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n");
                    fm.SendData2USB("qwertyuiopasdfghjklzxcvbnm\r\n");
                    fm.SendData2USB("1234567890\r\n");
                    fm.SendData2USB("QWERTYUIOP123456\r\n");
                    fm.SendData2USB("ENDDATAMATRIX\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// ESC指令，打印日语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_Japanese_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("にほんご\r\n");
                    fm.SendData2USB("がいらいご\r\n");
                    fm.SendData2USB("ひょうじゅんご  きょうつうご\r\n");
                    fm.SendData2USB("こんばんは\r\n");
                    fm.SendData2USB("どうしましたか?\r\n");
                    fm.SendData2USB("\n\n\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// TSPL指令，打印日语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_TSPL_Japanese_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("SIZE 80 mm,24 mm\r\n");//标签尺寸
                    fm.SendData2USB("GAP 0 mm,0 mm\r\n");//间距为0
                    fm.SendData2USB("CLS\r\n");
                    fm.SendData2USB("TEXT 10,0,\"TSS24.BF2\",0,1,1,\"にほんご\"\r\n");
                    fm.SendData2USB("TEXT 10,30,\"TSS24.BF2\",0,1,1,\"がいらいご\"\r\n");
                    fm.SendData2USB("TEXT 10,60,\"TSS24.BF2\",0,1,1,\"ひょうじゅんご  きょうつうご\"\r\n");
                    fm.SendData2USB("TEXT 10,90,\"TSS24.BF2\",0,1,1,\"こんばんは\"\r\n");
                    fm.SendData2USB("TEXT 10,120,\"TSS24.BF2\",0,1,1,\"どうしましたか?\"\r\n");
                    fm.SendData2USB("PRINT 1\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令，打印日语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_Japanese_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB("! 0 200 200 200 1\r\n");
                    fm.SendData2USB("TEXT 24 0 10 0 にほんご\r\n");
                    fm.SendData2USB("TEXT 24 0 10 30 がいらいご\r\n");
                    fm.SendData2USB("TEXT 24 0 10 60 ひょうじゅんご  きょうつうご\r\n");
                    fm.SendData2USB("TEXT 24 0 10 90 こんばんは\r\n");
                    fm.SendData2USB("TEXT 24 0 10 120 どうしましたか?\r\n");
                    fm.SendData2USB("PRINT\r\n");
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// CPCL指令，打印法语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CPCL_France_Click(object sender, EventArgs e)
        {
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    fm.SendData2USB(Encoding.UTF8.GetBytes("! 0 200 200 300 1\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 0 Vénus\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 30 Où sont les toilettes ?\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 60 Je suis désolé\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 90 Je ne comprends pas\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 120 celui-là\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 150 s'il vous pla\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("TEXT 88 24 10 180 Santé!\r\n"));
                    fm.SendData2USB(Encoding.UTF8.GetBytes("PRINT\r\n"));
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        /// <summary>
        /// ESC指令，设置各国代码页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ESC_Codepage_Click(object sender, EventArgs e)
        {
            string[] Codepage_Num = new string[] { "C0 PC437 [美国，欧洲标准]", "C1 片假名", "C2 PC850 [多语言]", "C3 PC860 [葡萄牙语]", 
                                                   "C4 PC863 [加拿大-法语]", "C5 PC865 [北欧]"};
            fm.NewUsb.FindUSBPrinter();
            for (int k = 0; k < fm.NewUsb.USBPortCount; k++)
            {
                if (fm.NewUsb.LinkUSB(k))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        fm.SendData2USB(new byte[] { 0x1B, 0x74, (byte)i });//设置代码页
                        fm.SendData2USB(new byte[] { 0x1C, 0x26 });// 汉字模式
                        fm.SendData2USB(Codepage_Num[i] + "\n");
                        fm.SendData2USB(new byte[] { 0x1C, 0x2E });
                        byte[] data = new byte[256];
                        for (int z = 0; z < 256; z++)
                            data[z] = (byte)z;
                        fm.SendData2USB(data);
                        fm.SendData2USB("\n\n");
                    }
                    fm.SendData2USB(new byte[] { 0x1C, 0x26 });// 汉字模式
                    fm.SendData2USB(new byte[] { 0x1B, 0x74, 0x00, 0x0A, 0x0A });//恢复出厂代码页
                    fm.NewUsb.CloseUSBPort();
                }
            }
        }
        private void button_clean_Click(object sender, EventArgs e)
        {
            tb_USB_state.Text = "";//清空
        }
    }
}
