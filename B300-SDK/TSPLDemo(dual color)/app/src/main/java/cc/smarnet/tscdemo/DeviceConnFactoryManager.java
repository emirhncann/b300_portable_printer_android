package cc.smarnet.tscdemo;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.hardware.usb.UsbDevice;
import android.util.Log;

import com.smart.io.BluetoothPort;
import com.smart.io.EthernetPort;
import com.smart.io.PortManager;
import com.smart.io.SerialPort;
import com.smart.io.UsbPort;

import java.io.IOException;
import java.util.Vector;

import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_DETACHED;

/**
 * Created by Administrator
 *
 * @author 猿史森林
 * Time 2017/8/2
 */
public class DeviceConnFactoryManager {

    public PortManager mPort;

    private static final String TAG = DeviceConnFactoryManager.class.getSimpleName();

    private String macAddress;

    private Context mContext;

    private boolean isOpenPort;
    /**
     * ESC查询打印机实时状态指令
     */
    private byte[] esc = {0x10, 0x04, 0x02};

    /**
     * ESC查询打印机实时状态 缺纸状态
     */
    private static final int ESC_STATE_PAPER_ERR = 0x20;

    /**
     * ESC指令查询打印机实时状态 打印机开盖状态
     */
    private static final int ESC_STATE_COVER_OPEN = 0x04;

    /**
     * ESC指令查询打印机实时状态 打印机报错状态
     */
    private static final int ESC_STATE_ERR_OCCURS = 0x40;

    /**
     * TSC查询打印机状态指令
     */
    private byte[] tsc = {0x1b, '!', '?'};

    /**
     * TSC指令查询打印机实时状态 打印机缺纸状态
     */
    private static final int TSC_STATE_PAPER_ERR = 0x04;

    /**
     * TSC指令查询打印机实时状态 打印机开盖状态
     */
    private static final int TSC_STATE_COVER_OPEN = 0x01;

    /**
     * TSC指令查询打印机实时状态 打印机出错状态
     */
    private static final int TSC_STATE_ERR_OCCURS = 0x80;

    private byte[] cpcl = {0x1b, 0x68};

    /**
     * CPCL指令查询打印机实时状态 打印机缺纸状态
     */
    private static final int CPCL_STATE_PAPER_ERR = 0x01;
    /**
     * CPCL指令查询打印机实时状态 打印机开盖状态
     */
    private static final int CPCL_STATE_COVER_OPEN = 0x02;

    private byte[] sendCommand = cpcl;
    /**
     * 判断打印机所使用指令是否是ESC指令
     */
    public static final byte FLAG = 0x10;
    private static final int READ_DATA = 10000;
    private static final String READ_DATA_CNT = "read_data_cnt";
    private static final String READ_BUFFER_ARRAY = "read_buffer_array";
    public static final String ACTION_CONN_STATE = "action_connect_state";
    public static final String ACTION_QUERY_PRINTER_STATE = "action_query_printer_state";
    public static final String ACTION_PRINT_FINISH = "action_print_finish";
    public static final String ACTION_PRINT_ERROR = "action_print_error";
    public static final String STATE = "state";
    public static final String DEVICE_ID = "id";
    public static final int CONN_STATE_DISCONNECT = 0x90;
    public static final int CONN_STATE_CONNECTING = CONN_STATE_DISCONNECT << 1;
    public static final int CONN_STATE_FAILED = CONN_STATE_DISCONNECT << 2;
    public static final int CONN_STATE_CONNECTED = CONN_STATE_DISCONNECT << 3;

    /**
     * 打开端口
     *
     * @return
     */
    public void openPort() {
        isOpenPort = false;
        sendStateBroadcast(CONN_STATE_CONNECTING);
        mPort = new BluetoothPort(macAddress);
        isOpenPort = mPort.openPort();

        //端口打开成功后，检查连接打印机所使用的打印机指令ESC、TSC
        if (isOpenPort) {
            sendStateBroadcast(CONN_STATE_CONNECTED);
        } else {
            if (this.mPort != null) {
                this.mPort = null;
            }
            sendStateBroadcast(CONN_STATE_FAILED);
        }
    }

    /**
     * 获取端口打开状态（true 打开，false 未打开）
     *
     * @return
     */
    public boolean getConnState() {
        return isOpenPort;
    }

    /**
     * 获取连接蓝牙的物理地址
     *
     * @return
     */
    public String getMacAddress() {
        return macAddress;
    }

    /**
     * 关闭端口
     */
    public void closePort() {
        if (this.mPort != null) {
            boolean b = this.mPort.closePort();
            if (b) {
                this.mPort = null;
                isOpenPort = false;
            }
        }
        sendStateBroadcast(CONN_STATE_DISCONNECT);
    }

    private DeviceConnFactoryManager(Build build) {
        this.macAddress = build.macAddress;
        this.mContext = build.context;
    }

    public static final class Build {
        private String ip;
        private String macAddress;
        private UsbDevice usbDevice;
        private int port;
        private Context context;

        public Build setIp(String ip) {
            this.ip = ip;
            return this;
        }

        public Build setMacAddress(String macAddress) {
            this.macAddress = macAddress;
            return this;
        }

        public Build setUsbDevice(UsbDevice usbDevice) {
            this.usbDevice = usbDevice;
            return this;
        }

        public Build setPort(int port) {
            this.port = port;
            return this;
        }

        public DeviceConnFactoryManager build(Context context) {
            this.context = context;
            return new DeviceConnFactoryManager(this);
        }
    }

    public void sendDataImmediately(final Vector<Byte> data) {
        if (this.mPort == null) {
            return;
        }
        try {
            this.mPort.writeDataImmediately(data, 0, data.size());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public boolean readDataImmediately(byte[] buffer,int len,long timeout) throws IOException {
        return this.mPort.readData(buffer, len, timeout);
    }

    private void sendStateBroadcast(int state) {
        Intent intent = new Intent(ACTION_CONN_STATE);
        intent.putExtra(STATE, state);
        mContext.sendBroadcast(intent);
    }

}