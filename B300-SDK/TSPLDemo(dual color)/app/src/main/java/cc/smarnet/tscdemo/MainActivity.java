package cc.smarnet.tscdemo;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.smart.command.CpclCommand;
import com.smart.command.EscCommand;
import com.smart.command.LabelCommand;
import com.smart.io.WriterResult;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Vector;

import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_ATTACHED;
import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_DETACHED;
import static cc.smarnet.tscdemo.DeviceConnFactoryManager.ACTION_QUERY_PRINTER_STATE;
import static cc.smarnet.tscdemo.DeviceConnFactoryManager.CONN_STATE_FAILED;

public class MainActivity extends AppCompatActivity {
    private static final String TAG = "MainActivity";
    ArrayList<String> per = new ArrayList<>();
    private UsbManager usbManager;
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

    private static final int CONN_MOST_DEVICES = 0x11;
    private static final int CONN_PRINTER = 0x12;
    private PendingIntent mPermissionIntent;
    private String[] permissions = {
            Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_COARSE_LOCATION,
            Manifest.permission.BLUETOOTH
    };
    private String usbName;
    private TextView tvConnState;
    private ThreadPool threadPool;

    private int id = 0;
    private int printcount = 0;
    private boolean continuityprint = false;
    private DeviceConnFactoryManager deviceConnFactoryManager;

    // private KeepConn keepConn;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.e(TAG, "onCreate()");
        setContentView(R.layout.activity_main);
        usbManager = (UsbManager) getSystemService(Context.USB_SERVICE);
        checkPermission();
        requestPermission();
        tvConnState = (TextView) findViewById(R.id.tv_connState);
    }

    @Override
    protected void onStart() {
        super.onStart();
        IntentFilter filter = new IntentFilter();
        filter.addAction(ACTION_QUERY_PRINTER_STATE);
        filter.addAction(DeviceConnFactoryManager.ACTION_CONN_STATE);
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
     * Bluetooth connect
     */
    public void btnBluetoothConn(View view) {
        startActivityForResult(new Intent(this, BluetoothDeviceList.class), Constant.BLUETOOTH_REQUEST_CODE);
    }

    /**
     * Print label
     *
     * @param view
     */
    public void btnLabelPrint(View view) {
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(() -> {
            if (!isConnected()) {
                mHandler.obtainMessage(CONN_PRINTER).sendToTarget();
                return;
            }
            sendLabel();
        });
    }

    /**
     * Disconnect
     *
     * @param view
     */
    public void btnDisConn(View view) {
        if (!isConnected()) {
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

                /*蓝牙连接*/
                case Constant.BLUETOOTH_REQUEST_CODE: {
                    closeport();
                    /*获取蓝牙mac地址*/
                    String macAddress = data.getStringExtra(BluetoothDeviceList.EXTRA_DEVICE_ADDRESS);
                    //初始化话DeviceConnFactoryManager
                    deviceConnFactoryManager = new DeviceConnFactoryManager.Build()
                            //设置连接的蓝牙mac地址
                            .setMacAddress(macAddress)
                            .build(MainActivity.this);
                    //打开端口
                    threadPool = ThreadPool.getInstantiation();
                    threadPool.addTask(() -> deviceConnFactoryManager.openPort());

                    break;
                }
                default:
                    break;
            }
        }
    }

    /**
     * Reconnection recycles the last connected object to avoid memory leakage
     */
    private void closeport() {
        if (isConnected()) {
//            DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].reader.cancel();
            deviceConnFactoryManager.closePort();
            deviceConnFactoryManager = null;
        }
    }

    /**
     * Print label
     */
    void sendLabel() {
        LabelCommand tsc = new LabelCommand();
        // Set the label size according to the actual size
        tsc.addSize(60, 40);
        // Set the label gap according to the actual size. If it is no gap paper, set it to 0
        tsc.addGap(0);
        // Set print direction
        tsc.addDirection(LabelCommand.DIRECTION.BACKWARD, LabelCommand.MIRROR.NORMAL);
        // Turn on Printing with response for continuous printing
        tsc.addQueryPrinterStatus(LabelCommand.RESPONSE_MODE.ON);
        // Set origin coordinates
        tsc.addReference(0, 0);
        // Tearing mode on
        tsc.addTear(EscCommand.ENABLE.ON);
        // Clear print buffer
        tsc.addCls();
        // Draw simplified Chinese
        tsc.addStrToCommand("SETCOLOR RED 3\r\n");
        tsc.addText(10, 0, LabelCommand.FONTTYPE.SIMPLIFIED_CHINESE, LabelCommand.ROTATION.ROTATION_0, LabelCommand.FONTMUL.MUL_1, LabelCommand.FONTMUL.MUL_1,
                "红色打印测试");
        tsc.addStrToCommand("SETCOLOR BLACK\r\n");
        tsc.addText(10, 30, LabelCommand.FONTTYPE.SIMPLIFIED_CHINESE, LabelCommand.ROTATION.ROTATION_0, LabelCommand.FONTMUL.MUL_1, LabelCommand.FONTMUL.MUL_1,
                "黑色打印测试");
        tsc.addQRCode(10, 80, LabelCommand.EEC.LEVEL_L, 5, LabelCommand.ROTATION.ROTATION_0, "www.smarnet.cc");
        // Draw a picture
//        Bitmap b = BitmapFactory.decodeResource(getResources(), R.drawable.picture);
//        tsc.addBitmap(60, 20, -1, 3.7f, LabelCommand.BITMAP_MODE.OVERWRITE, 200, b);
        tsc.addStrToCommand("SETCOLOR RED 3\r\n");
        tsc.addQRCode(180, 80, LabelCommand.EEC.LEVEL_L, 5, LabelCommand.ROTATION.ROTATION_0, "www.smarnet.cc");
        // print label
        tsc.addPrint(1, 1);
        // Buzzer sounds after label printing
        tsc.addCashdrwer(LabelCommand.FOOT.F5, 255, 255);
        Vector<Byte> datas = tsc.getCommand();
        // send data
        deviceConnFactoryManager.sendDataImmediately(datas);
    }

    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            switch (action) {
                case DeviceConnFactoryManager.ACTION_CONN_STATE:
                    int state = intent.getIntExtra(DeviceConnFactoryManager.STATE, -1);
                    int deviceId = intent.getIntExtra(DeviceConnFactoryManager.DEVICE_ID, -1);
                    switch (state) {
                        case DeviceConnFactoryManager.CONN_STATE_DISCONNECT:
                            if (id == deviceId) {
                                tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            }
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTING:
                            tvConnState.setText(getString(R.string.str_conn_state_connecting));
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTED:
                            tvConnState.setText(getString(R.string.str_conn_state_connected) + "\n" + getConnDeviceInfo());
                            break;
                        case CONN_STATE_FAILED:
                            Utils.toast(MainActivity.this, getString(R.string.str_conn_fail));
                            //wificonn=false;
                            tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            break;
                        default:
                            break;
                    }
                    break;
                case ACTION_QUERY_PRINTER_STATE:
                    if (counts >= 0) {
                        if (continuityprint) {
                            printcount++;
                            Utils.toast(MainActivity.this, getString(R.string.str_continuityprinter) + " " + printcount);
                        }
                        if (counts != 0) {
//                            sendContinuityPrint();
                        } else {
                            continuityprint = false;
                        }
                    }
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
                    if (isConnected()) {
                        deviceConnFactoryManager.closePort();
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
        unregisterReceiver(receiver);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.e(TAG, "onDestroy()");
        if (isConnected()) {
            deviceConnFactoryManager.closePort();
        }
        if (threadPool != null) {
            threadPool.stopThreadPool();
        }
    }

    private String getConnDeviceInfo() {
        String str = "";
        if (isConnected()) {
            str += "BLUETOOTH\n";
            str += "MacAddress: " + deviceConnFactoryManager.getMacAddress();
        }
        return str;
    }

    private boolean isConnected() {
        return deviceConnFactoryManager != null && deviceConnFactoryManager.getConnState();
    }

}