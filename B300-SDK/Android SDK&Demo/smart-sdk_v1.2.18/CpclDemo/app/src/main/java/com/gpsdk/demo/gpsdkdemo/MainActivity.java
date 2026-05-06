package com.gpsdk.demo.gpsdkdemo;

import android.Manifest;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.TextView;

import com.smart.command.CpclCommand;

import java.util.ArrayList;
import java.util.Vector;

import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_ATTACHED;
import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_DETACHED;
import static com.gpsdk.demo.gpsdkdemo.DeviceConnFactoryManager.ACTION_QUERY_PRINTER_STATE;
import static com.gpsdk.demo.gpsdkdemo.DeviceConnFactoryManager.CONN_STATE_FAILED;

/**
 * Created by Administrator
 *
 * @author 猿史森林
 * Date: 2017/8/2
 * Class description:
 */
public class MainActivity extends AppCompatActivity {
    private static final String TAG = "MainActivity";
    ArrayList<String> per = new ArrayList<>();
    private int counts;
    private static final int REQUEST_CODE = 0x004;

    private Bitmap bitmap = Bitmap.createBitmap(660, 40, Bitmap.Config.ARGB_8888);

    /**
     * Connection status disconnected
     */
    private static final int CONN_STATE_DISCONN = 0x007;
    /**
     * Use printer command error
     */
    private static final int PRINTER_COMMAND_ERROR = 0x008;

    /**
     * Cpcl query printer real time status instruction
     */
    private byte[] cpcl = {0x1b, 0x68};

    private static final int CONN_MOST_DEVICES = 0x11;
    private static final int CONN_PRINTER = 0x12;
    private String[] permissions = {
            Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_COARSE_LOCATION,
            Manifest.permission.BLUETOOTH
    };
    private TextView tvConnState;
    private ThreadPool threadPool;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        checkPermission();
        requestPermission();
        tvConnState = (TextView) findViewById(R.id.tv_connState);
    }

    @Override
    protected void onStart() {
        super.onStart();
        IntentFilter filter = new IntentFilter();
        filter.addAction(ACTION_USB_DEVICE_DETACHED);
        filter.addAction(ACTION_QUERY_PRINTER_STATE);
        filter.addAction(DeviceConnFactoryManager.ACTION_CONN_STATE);
        filter.addAction(ACTION_USB_DEVICE_ATTACHED);
        registerReceiver(receiver, filter);
    }

    private void checkPermission() {
        for (String permission : permissions) {
            if (PackageManager.PERMISSION_GRANTED != ContextCompat.checkSelfPermission(this, permission)) {
                per.add(permission);
            }
        }
    }

    private void requestPermission() {
        if (per.size() > 0) {
            String[] p = new String[per.size()];
            ActivityCompat.requestPermissions(this, per.toArray(p), REQUEST_CODE);
        }
    }

    /**
     * Bluetooth connectivity
     */
    public void btnBluetoothConn(View view) {
        startActivityForResult(new Intent(this, BluetoothDeviceList.class), Constant.BLUETOOTH_REQUEST_CODE);
    }

    /**
     * WIFI connectivity
     */
    public void btnWifiConn(View view) {
        WifiParameterConfigDialog wifiParameterConfigDialog = new WifiParameterConfigDialog(this, new WifiParameterConfigDialog.WifiConfigCallback() {
            @Override
            public void callback(String ip, int port) {
                new DeviceConnFactoryManager.Build()
                        //Set bluetooth mac address
                        .setConnectMethod(DeviceConnFactoryManager.CONN_METHOD.WIFI)
                        .setIp(ip)
                        .setPort(port)
                        .build();
                //Open port
                threadPool = ThreadPool.getInstantiation();
                threadPool.addTask(() -> DeviceConnFactoryManager.getDeviceConnFactoryManager().openPort());
            }
        });
        wifiParameterConfigDialog.show();
    }

    /**
     * Print face sheet
     *
     * @param view
     */
    public void btnCpclPrint(View view) {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManager() == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManager().getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(() -> sendCpcl());
    }

    /**
     * Print face sheet
     */
    private void sendCpcl() {
        //Initialize the cpcl instruction
        CpclCommand cpcl = new CpclCommand();
        //Set sheet height and print copies
        cpcl.addInitializePrinter(1130, 1);
        //Set alignment to center
        cpcl.addJustification(CpclCommand.ALIGNMENT.CENTER);
        //Set font magnification
        cpcl.addSetmag(1, 1);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 30, "Sample");
        //Set font magnification
        cpcl.addSetmag(0, 0);
        //Set alignment to left
        cpcl.addJustification(CpclCommand.ALIGNMENT.LEFT);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 65, "Print text");
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 95, "Welcom to use SMARNET printer!");
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_20, 0, 0, 135, "Welcome to the printer!");
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 195, "Smarnet");
        //Set alignment to left
        cpcl.addJustification(CpclCommand.ALIGNMENT.CENTER);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 195, "Network");
        //Set alignment to right
        cpcl.addJustification(CpclCommand.ALIGNMENT.RIGHT);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 195, "Device");
        //Set alignment to left
        cpcl.addJustification(CpclCommand.ALIGNMENT.LEFT);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 230, "Print bitmap!");
        //Print picture
        Bitmap bitmap = BitmapFactory.decodeResource(getResources(), R.drawable.logo);
        cpcl.addEGraphics(0, 255, 385, 127, bitmap);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 645, "Print code128!");
        //Print barcode
        cpcl.addBarcodeText(5, 2);
        cpcl.addBarcode(CpclCommand.COMMAND.BARCODE, CpclCommand.BARCODETYPE.CODE128, 50, 0, 680, "SMARNET");
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 775, "Print QRcode");
        //Print qrcode
        cpcl.addBQrcode(CpclCommand.QRCODE_LEVEL.M, CpclCommand.MODE.A, 0, 810, "QRcode");
        //Set alignment to center
        cpcl.addJustification(CpclCommand.ALIGNMENT.CENTER);
        //Print text
        cpcl.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 0, 1010, "Completed");
        //Set alignment to left
        cpcl.addJustification(CpclCommand.ALIGNMENT.LEFT);
        //Print
        cpcl.addPrint();
        Vector<Byte> datas = cpcl.getCommand();

        // Send cpcl data
        DeviceConnFactoryManager.getDeviceConnFactoryManager().sendDataImmediately(datas);
    }

    public void btnCpclTextPrint(View view) {
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(() -> {
            CpclCommand cpclCommand = new CpclCommand();
            cpclCommand.addInitializePrinter(0, 1000, 1);
            cpclCommand.addPageWidth(576);
            cpclCommand.addSetmag(1, 1);
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 0, 10, 10, "Font0 12*12 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 1, 10, 30, "Font0 20*12 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 2, 10, 55, "Font0 20*20 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 3, 10, 85, "Font0 32*20 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 4, 10, 115, "Font0 36*20 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 5, 10, 140, "Font0 32*36 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 6, 10, 175, "Font0 42*46 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 55, 10, 215, "Font0 55*55 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_0, 26, 10, 270, "Font0 16*16 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_1, 0, 10, 300, "Font1 24*24 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_2, 0, 10, 330, "Font2 24*24 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_3, 0, 10, 360, "Font3 20*20 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 10, 390, "Font4 32*32 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_4, 3, 10, 420, "Font4 48*48 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_4, 4, 10, 460, "Font4 64*48 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_5, 0, 10, 508, "Font5 24*24 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_7, 0, 10, 538, "Font7 24*24 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_8, 0, 10, 568, "Font8 24*24 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_20, 0, 10, 598, "Font20 16*16 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 10, 620, "Font24 24*24 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 3, 10, 646, "Font24 48*48 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_28, 0, 10, 694, "Font28 28*28 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_55, 0, 10, 728, "Font55 16*16 中文ABCabc");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_55, 3, 10, 750, "Font55 32*32 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_56, 0, 10, 780, "Font56 32*32 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_57, 0, 10, 820, "Font57 12*12 中文ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_88, 0, 10, 845, "Font88 11*11 Je suis désolé");
            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_88, 32, 10, 860, "Font88 32*32 Je suis désolé");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_89, 0, 10, 900, "Font89 24*24 한국어(KS숫자)ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_90, 0, 10, 930, "Font90 24*24 繁體中文(大五碼)ABCabc");

            cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_91, 0, 10, 960, "Font91 24*24 繁體中文(大五碼)ABCabc");
            cpclCommand.addPoPrint();

            DeviceConnFactoryManager.getDeviceConnFactoryManager().sendDataImmediately(cpclCommand.getCommand());
        });
    }

    public void expressDeliverySheetPrinting(View view) {
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(() -> {
            sendExpressDeliverySheet();
        });
    }

    private void sendExpressDeliverySheet() {
        CpclCommand cpclCommand = new CpclCommand();
        cpclCommand.addInitializePrinter(0,1000,1);
        cpclCommand.addPageWidth(576);
        cpclCommand.addLine(10, 6, 572, 6, 2);
        cpclCommand.addLine(10, 110, 572, 110, 1);
        cpclCommand.addLine(10, 192, 572, 192, 2);
        cpclCommand.addLine(10, 251, 572, 251, 1);
        cpclCommand.addLine(10, 407, 462, 407, 2);
        cpclCommand.addLine(10, 526, 462, 526, 1);

        cpclCommand.addLine(84, 563, 462, 563, 2);

        cpclCommand.addLine(10, 600, 572, 600, 1);
        cpclCommand.addLine(10, 734, 572, 734, 2);
        cpclCommand.addLine(10, 868, 572, 868, 1);
        cpclCommand.addLine(10, 979, 572, 979, 2);
        cpclCommand.addLine(10, 6, 10, 979, 2);

        cpclCommand.addLine(84, 251, 84, 600, 2);
        cpclCommand.addLine(210, 526, 210, 600, 2);
        cpclCommand.addLine(292, 192, 292, 251, 2);
        cpclCommand.addLine(336, 526, 336, 600, 2);
        cpclCommand.addLine(462, 251, 462, 600, 2);
        cpclCommand.addLine(572, 6, 572, 979, 2);
        cpclCommand.addSetmag(1, 1);

        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 298, 84, "服务热线：400 820 1666");

        cpclCommand.addSetmag(2, 2);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 160, 14, "快递样张");
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 48, 128, "150 450-00 000");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_4, 0, 116, 206, "中山");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 299, 199, "打印时间：");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 415, 199, "2020-10-06");


        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 415, 225, "10:22:40 1/1");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 36, 300, "收");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 258, "罗小福 13632566635");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 295, "广东省 珠海市香洲区珠海大道");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 332, "自定义速递");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 36, 459, "寄");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 414, "熊小斌 13726299557");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 448, "广东省 中山市坦洲镇七村果子解");

        cpclCommand.addSetmag(5, 5);
        cpclCommand.addSetbold(CpclCommand.BOLD.B1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 91, 436, "450-00");
        cpclCommand.addSetbold(CpclCommand.BOLD.B0);

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 102, 533, "代收货款");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 228, 533, "到付运费");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 367, 570, "0.20KG");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 491, 340, "JT0000543899225");
        cpclCommand.addBarcode(CpclCommand.COMMAND.VBARCODE, CpclCommand.BARCODETYPE.CODE128, 1, CpclCommand.BARCODERATIO.TpointZ1, 55, 499, 585, "JT0000543899225");
        cpclCommand.addBarcode(CpclCommand.COMMAND.BARCODE, CpclCommand.BARCODETYPE.CODE128, 2, CpclCommand.BARCODERATIO.ThpointZ1, 70, 36, 614, "JT0000543899225");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 151, 696, "JT0000543899225");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 513, 696, "1/1");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 20, 750, "备注");
        cpclCommand.addBQrcode(CpclCommand.QRCODE_LEVEL.H, CpclCommand.MODE.A, 24, 882, 2, 2, "http://weixin.qq.com/r/fCk7I112345TrRNA93xu");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_55, 0, 121, 920, "您的签收代表您已验收此包裹，并确认商品信息无误，包裹完好");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_55, 0, 121, 950, "、没有划痕、破损等表面质量问题");

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 330, 830, "打印：2次");

        cpclCommand.addLine(480, 825, 560, 825, 2);
        cpclCommand.addLine(480, 860, 560, 860, 2);
        cpclCommand.addLine(480, 825, 480, 860, 2);
        cpclCommand.addLine(560, 825, 560, 860, 2);

        cpclCommand.addSetmag(1, 1);
        cpclCommand.addText(CpclCommand.TEXT_FONT.FONT_24, 0, 485, 830, "已检视");

        cpclCommand.addPoPrint();

        DeviceConnFactoryManager.getDeviceConnFactoryManager().sendDataImmediately(cpclCommand.getCommand());
    }

    /**
     * Disconnect
     *
     * @param view
     */
    public void btnDisConn(View view) {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManager() == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManager().getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        mHandler.obtainMessage(CONN_STATE_DISCONN).sendToTarget();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK) {
            switch (requestCode) {

                //Bluetooth connect
                case Constant.BLUETOOTH_REQUEST_CODE: {
                    closePort();
                    //Get bluetooth mac address
                    String macAddress = data.getStringExtra(BluetoothDeviceList.EXTRA_DEVICE_ADDRESS);
                    //Init DeviceConnFactoryManager
                    new DeviceConnFactoryManager.Build()
                            .setConnectMethod(DeviceConnFactoryManager.CONN_METHOD.BLUETOOTH)
                            //Set bluetooth mac address
                            .setMacAddress(macAddress)
                            .build();
                    //Open port
                    threadPool = ThreadPool.getInstantiation();
                    threadPool.addTask(() -> DeviceConnFactoryManager.getDeviceConnFactoryManager().openPort());

                    break;
                }
                case CONN_MOST_DEVICES:
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManager() != null &&
                            DeviceConnFactoryManager.getDeviceConnFactoryManager().getConnState()) {
                        tvConnState.setText(getString(R.string.str_conn_state_connected) + "\n" + getConnDeviceInfo());
                    } else {
                        tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /**
     * Disconnect
     */
    private void closePort() {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManager() != null && DeviceConnFactoryManager.getDeviceConnFactoryManager().mPort != null) {
            DeviceConnFactoryManager.getDeviceConnFactoryManager().reader.cancel();
            DeviceConnFactoryManager.getDeviceConnFactoryManager().mPort.closePort();
            DeviceConnFactoryManager.getDeviceConnFactoryManager().mPort = null;
        }
    }

    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            switch (action) {
                case DeviceConnFactoryManager.ACTION_CONN_STATE:
                    int state = intent.getIntExtra(DeviceConnFactoryManager.STATE, -1);
                    switch (state) {
                        case DeviceConnFactoryManager.CONN_STATE_DISCONNECT:
                            tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTING:
                            tvConnState.setText(getString(R.string.str_conn_state_connecting));
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTED:
                            tvConnState.setText(getString(R.string.str_conn_state_connected) + "\n" + getConnDeviceInfo());
                            break;
                        case CONN_STATE_FAILED:
                            Utils.toast(MainActivity.this, getString(R.string.str_conn_fail));
                            tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            break;
                        default:
                            break;
                    }
                    break;
                case ACTION_QUERY_PRINTER_STATE:

                    break;
                default:
                    break;
            }
        }
    };

    private Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case CONN_STATE_DISCONN:
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManager() != null ||
                            !DeviceConnFactoryManager.getDeviceConnFactoryManager().getConnState()) {
                        DeviceConnFactoryManager.getDeviceConnFactoryManager().closePort();
                        Utils.toast(MainActivity.this, getString(R.string.str_disconnect_success));
                    }
                    break;
                case PRINTER_COMMAND_ERROR:
                    Utils.toast(MainActivity.this, getString(R.string.str_choice_printer_command));
                    break;
                case CONN_PRINTER:
                    Utils.toast(MainActivity.this, getString(R.string.str_cann_printer));
                    break;
            }
        }
    };

    @Override
    protected void onStop() {
        super.onStop();
    }

    @Override
    protected void onDestroy() {
        unregisterReceiver(receiver);
        if (DeviceConnFactoryManager.getDeviceConnFactoryManager() != null) {
            DeviceConnFactoryManager.getDeviceConnFactoryManager().closePort();
        }
        if (threadPool != null) {
            threadPool.stopThreadPool();
        }
        super.onDestroy();
    }

    private String getConnDeviceInfo() {
        String str = "";
        DeviceConnFactoryManager deviceConnFactoryManager = DeviceConnFactoryManager.getDeviceConnFactoryManager();
        if (deviceConnFactoryManager != null
                && deviceConnFactoryManager.getConnState()) {
            if (deviceConnFactoryManager.getConnMethod() == DeviceConnFactoryManager.CONN_METHOD.BLUETOOTH) {
                str += "BLUETOOTH\n";
                str += "MacAddress: " + deviceConnFactoryManager.getMacAddress();
            } else if (deviceConnFactoryManager.getConnMethod() == DeviceConnFactoryManager.CONN_METHOD.WIFI) {
                str += "WIFI\n";
                str += "ip: " + deviceConnFactoryManager.getIp() + "\n";
                str += "port: " + deviceConnFactoryManager.getPort();
            }
        }
        return str;
    }
}