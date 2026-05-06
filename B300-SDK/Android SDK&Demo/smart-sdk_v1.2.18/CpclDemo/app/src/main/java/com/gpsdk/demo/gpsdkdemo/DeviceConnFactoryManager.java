package com.gpsdk.demo.gpsdkdemo;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.os.Message;
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
 *         Time 2017/8/2
 */
public class DeviceConnFactoryManager {

    public PortManager mPort;

    private static final String TAG = DeviceConnFactoryManager.class.getSimpleName();

    private String macAddress;


    private Context mContext;

    private static DeviceConnFactoryManager deviceConnFactoryManager;

    private boolean isOpenPort;
    private CONN_METHOD connMethod;
    public static final byte FLAG = 0x10;
    private static final int READ_DATA = 10000;
    private static final String READ_DATA_CNT = "read_data_cnt";
    private static final String READ_BUFFER_ARRAY = "read_buffer_array";
    public static final String ACTION_CONN_STATE = "action_connect_state";
    public static final String ACTION_QUERY_PRINTER_STATE = "action_query_printer_state";
    public static final String STATE = "state";
    public static final String DEVICE_ID = "id";
    public static final int CONN_STATE_DISCONNECT = 0x90;
    public static final int CONN_STATE_CONNECTING = CONN_STATE_DISCONNECT << 1;
    public static final int CONN_STATE_FAILED = CONN_STATE_DISCONNECT << 2;
    public static final int CONN_STATE_CONNECTED = CONN_STATE_DISCONNECT << 3;
    public PrinterReader reader;
    private String ip;
    private int port;

    public enum CONN_METHOD {
        //Bluetooth
        BLUETOOTH("BLUETOOTH"),
        //Wifi
        WIFI("WIFI");

        private String name;

        private CONN_METHOD(String name) {
            this.name = name;
        }

        @Override
        public String toString() {
            return this.name;
        }
    }

    public static DeviceConnFactoryManager getDeviceConnFactoryManager() {
        return deviceConnFactoryManager;
    }

    /**
     * Open port
     */
    public void openPort() {
        deviceConnFactoryManager.isOpenPort = false;
        sendStateBroadcast(CONN_STATE_CONNECTING);
        //Open port
        switch (connMethod) {
            case BLUETOOTH:
                //Init bluetooth port
                mPort = new BluetoothPort(macAddress);
                isOpenPort = mPort.openPort();
                break;

            case WIFI:
                //Init ethernet port
                mPort = new EthernetPort(ip, port);
                isOpenPort = mPort.openPort();
                break;
            default:
                break;
        }

        if (isOpenPort) {//Connect success
            sendStateBroadcast(CONN_STATE_CONNECTED);
            startPrinterReader();
        } else {//Connect fail
            if (this.mPort != null) {
                    this.mPort=null;
            }
            sendStateBroadcast(CONN_STATE_FAILED);
        }
    }

    /**
     * Start reader thread
     */
    private void startPrinterReader() {
        //Turn on the read printer to return the data thread
        reader = new PrinterReader();
        reader.start(); //Read data thread
    }

    /**
     * Get port open status (true open, false not open)
     */
    public boolean getConnState() {
        return isOpenPort;
    }

    public CONN_METHOD getConnMethod() {
        return connMethod;
    }

    public String getIp() {
        return ip;
    }

    public int getPort() {
        return port;
    }

    /**
     * Get the physical address to connect Bluetooth
     */
    public String getMacAddress() {
        return macAddress;
    }


    /**
     * Close the port
     */
    public void closePort() {
        if (this.mPort != null) {
            reader.cancel();
           boolean b= this.mPort.closePort();
            if(b) {
                this.mPort=null;
                isOpenPort = false;
            }
        }
        sendStateBroadcast(CONN_STATE_DISCONNECT);
    }

    private DeviceConnFactoryManager(Build build) {
        this.macAddress = build.macAddress;
        this.mContext = build.context;
        this.ip = build.ip;
        this.port = build.port;
        this.connMethod = build.connMethod;
        deviceConnFactoryManager = this;
    }

    public static final class Build {
        private String macAddress;
        private Context context;
        private CONN_METHOD connMethod;
        private String ip;
        private int port;

        public DeviceConnFactoryManager.Build setMacAddress(String macAddress) {
            this.macAddress = macAddress;
            return this;
        }

        public DeviceConnFactoryManager.Build setContext(Context context) {
            this.context = context;
            return this;
        }

        public DeviceConnFactoryManager.Build setConnectMethod(CONN_METHOD connectMethod) {
            this.connMethod = connectMethod;
            return this;
        }

        public DeviceConnFactoryManager.Build setIp(String ip) {
            this.ip = ip;
            return this;
        }

        public DeviceConnFactoryManager.Build setPort(int port) {
            this.port = port;
            return this;
        }

        public DeviceConnFactoryManager build() {
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

    public boolean readDataImmediately(byte[] buffer, int len, long timeout) throws IOException {
        return this.mPort.readData(buffer, len, timeout);
    }

    class PrinterReader extends Thread {
        private volatile boolean isRun = false;

        private byte[] buffer = new byte[100];

        public PrinterReader() {
            isRun = true;
        }

        @Override
        public void run() {
            try {
                while (isRun) {
                    //Read printer return information
                    if (mPort != null && mPort.getInputStream() != null) {
                        int len = mPort.getInputStream().read(buffer);
                        Log.e(TAG, "len - " + len);
                        if (len > 0) {
                            Message message = Message.obtain();
                            message.what = READ_DATA;
                            Bundle bundle = new Bundle();
                            bundle.putInt(READ_DATA_CNT, len); //Data length
                            bundle.putByteArray(READ_BUFFER_ARRAY, buffer); //Data
                            message.setData(bundle);
                        }
                    }
                }
            } catch (Exception e) {
                e.printStackTrace();
                if (deviceConnFactoryManager != null) {
                    closePort();
                }
            }
        }

        public void cancel() {
            isRun = false;
        }
    }

    private void sendStateBroadcast(int state) {
        Intent intent = new Intent(ACTION_CONN_STATE);
        intent.putExtra(STATE, state);
        App.getContext().sendBroadcast(intent);
    }

}