package cc.smarnet.tscdemo;

/**
 * Created by Administrator
 *
 * @author 猿史森林
 *         Date: 2017/10/14
 *         Class description:
 */
public class Constant {
    public static final String SERIALPORTPATH = "SerialPortPath";
    public static final String SERIALPORTBAUDRATE = "SerialPortBaudrate";
    public static final String WIFI_CONFIG_IP = "wifi config ip";
    public static final String WIFI_CONFIG_PORT = "wifi config port";
    public static final String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
    public static final int BLUETOOTH_REQUEST_CODE = 0x001;
    public static final int USB_REQUEST_CODE = 0x002;
    public static final int WIFI_REQUEST_CODE = 0x003;
    public static final int SERIALPORT_REQUEST_CODE = 0x006;
    public static final int CONN_STATE_DISCONN = 0x007;
    public static final int MESSAGE_UPDATE_PARAMETER = 0x009;

    /**
     * wifi 默认ip
     */
    public static final String WIFI_DEFAULT_IP = "192.168.123.100";

    /**
     * wifi 默认端口号
     */
    public static final int WIFI_DEFAULT_PORT = 9100;

    /**
     * 打印机状态
     */
    public static final class PrinterInfo {
        public static final int NORMAL = 200;//正常
        public static final int PRINT_HEAD_OVERHEATING = 201;//過熱
        public static final int LOW_POWER = 202;//低电量
        public static final int PRINTER_OUT_OF_PAPER = 203;//缺纸
        public static final int NO_GAP_FOUND = 204;//未检到间隙
        public static final int POWER = 205;//充电
        public static final int PRINT_OPEN_COVER = 206;//開蓋
        public static final int BLACK_MARK_ERROR = 207;//黑标错误
        public static final int OTHER = 208;//其它错误（如：过热）
    }
}