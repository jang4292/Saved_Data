package com.mamp.testble.testble;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ListActivity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothManager;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Arrays;

/**
 * Created by jangyunhwan on 2017-11-09.
 */

public class BleActivity extends ListActivity {
    //private final static String TAG = BluetoothLeService.class.getSimpleName();
    private final static String TAG = "Test_BLE_Activity";

    boolean mScanning_run=false; //�˻� ������

    private Handler mHandler;
    BLE_Device mBLE_Device;

    private BluetoothAdapter mBluetoothAdapter;//������� ����

    private Button button;

    boolean Search_state=true;

    private LeDeviceListAdapter mLeDeviceListAdapter; //����̽� ����

    private static final long SCAN_PERIOD = 10000;

    private boolean mScanning;//BLE ��ĵ

    int user_data_count=0;

    private static final int REQUEST_ENABLE_BT = 1;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.ble_device_list_activity);


        Display_loop();
        mHandler = new Handler();
        mBLE_Device=new BLE_Device();

        // Use this check to determine whether BLE is supported on the device.  Then you can
        // selectively disable BLE-related features.
        //BLE��ġ�� ����Ǵ����� Ȯ���Ѵ�.
        if (!getPackageManager().hasSystemFeature(PackageManager.FEATURE_BLUETOOTH_LE)) {
            Toast.makeText(this, "BLE를 지원하지 않습니다.",Toast.LENGTH_SHORT).show();

//            Toast.makeText(this, R.string.ble_not_supported, Toast.LENGTH_SHORT).show();
            finish();
        }

        // Initializes a Bluetooth adapter.  For API level 18 and above, get a reference to
        // BluetoothAdapter through BluetoothManager.
        //�ȵ���̵� �� ��������� �����ϴ� OS���� Ȯ���Ѵ�.(�ȵ���̵� 4.3�̻��� ���������� Ȯ���Ѵ�.)
        final BluetoothManager bluetoothManager =
                (BluetoothManager) getSystemService(Context.BLUETOOTH_SERVICE);
        mBluetoothAdapter = bluetoothManager.getAdapter();

        // Checks if Bluetooth is supported on the device.
        //��������� ������ �Ǿ���� üũ�Ѵ�.
        if (mBluetoothAdapter == null) {
            Toast.makeText(this, "블루투수가 제공되지 않는 OS버전입니다.", Toast.LENGTH_SHORT).show();
            //Toast.makeText(this, R.string.error_bluetooth_not_supported, Toast.LENGTH_SHORT).show();
            finish();
            return;
        }
    }

    private void Display_loop() { // TCP ��ư

        //gf1 = (GifView)findViewById(R.id.gif1);
        //gf1.setGifImage(R.drawable.beacon);
        //gf1.showCover();


        button = (Button) findViewById(R.id.button_Scan);
        //imageView_search=(ImageView)findViewById(R.id.imageView_search);
        //imageView_search_infinity=(ImageView)findViewById(R.id.imageView_search_infinity);


        //imageView_search.setOnClickListener(new View.OnClickListener() { // search
        button.setOnClickListener(new View.OnClickListener() { // search

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                if (Search_state) {
                    // 버튼 누르면 여기로 진입.
                    Search_state = false;
                    mBLE_Device.setclear();
                    mLeDeviceListAdapter.clear();
                    scanLeDevice(true);
                    //imageView_search.setImageResource(R.drawable.search_b_ing1);
                    button.setText("Scanning");
                    mLeDeviceListAdapter.notifyDataSetChanged();
                } else {
                    // 버튼이 누른 상태에서 다시 누르면 여기로 진입.
                    Search_state = true;
                    scanLeDevice(false);
                    mLeDeviceListAdapter.clear();

                    //imageView_search.setImageResource(R.drawable.search_b);
                    button.setText("Start_Scan");

                }

            }
        });



        //���ƽ� �˻�




/*
        imageView_search_infinity.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                if(!mScanning_run){

                    scanLeDevice(false);
                    gf1.showAnimation();
                    mScanning_run=true;
                    mscaneHandler.sendEmptyMessageDelayed(0, 100);
                    imageView_search_infinity.setImageResource(R.drawable.search_l_ing);
                }else{

                    gf1.showCover();

                    mScanning_run=false;
                    mscaneHandler.sendEmptyMessageDelayed(2, 100);
                    imageView_search_infinity.setImageResource(R.drawable.search_l);
                }
            }
        });
        */

        // Adapter for holding devices found through scanning.

    }

    //���ƽ� �˻�
    private void scanLeDevice(final boolean enable) {
        user_data_count=0;
        if (enable) {
            // Stops scanning after a pre-defined scan period.
            mHandler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    mScanning = false;
                    mBluetoothAdapter.stopLeScan(mLeScanCallback);
                    //gf1.showCover();
                    //imageView_search.setImageResource(R.drawable.search_b);
                    button.setText("scanning111");
                    Search_state=true;
                }
            }, SCAN_PERIOD); // 10초 이후에 진입됨.
            //gf1.showAnimation();
            mScanning = true;
            mBluetoothAdapter.startLeScan(mLeScanCallback);
        } else {
            //gf1.showCover();
            //imageView_search.setImageResource(R.drawable.search_b);
            button.setText("scanning222");
            Search_state=true;
            mScanning = false;
            mBluetoothAdapter.stopLeScan(mLeScanCallback);
        }


    }


    private class LeDeviceListAdapter extends BaseAdapter {
        private ArrayList<BluetoothDevice> mLeDevices;
        private LayoutInflater mInflator;

        public LeDeviceListAdapter() {
            super();
            mLeDevices = new ArrayList<BluetoothDevice>();
            //mInflator = DeviceScanActivity.this.getLayoutInflater();
            mInflator = BleActivity.this.getLayoutInflater();
        }

        public void addDevice(BluetoothDevice device) {
            if(!mLeDevices.contains(device)) {
                mLeDevices.add(device);
            }
        }

        public BluetoothDevice getDevice(int position) {
            return mLeDevices.get(position);
        }

        public void clear() {
            mLeDevices.clear();
        }

        @Override
        public int getCount() {
            return mLeDevices.size();
        }

        @Override
        public Object getItem(int i) {
            return mLeDevices.get(i);
        }

        @Override
        public long getItemId(int i) {
            return i;
        }

        // ����̽� user data �� ǥ��
        @SuppressLint("InflateParams")
        @Override
        public View getView(int i, View view, ViewGroup viewGroup) {
            ViewHolder viewHolder;
            try{
                // General ListView optimization code.
                if (view == null) {

                    view = mInflator.inflate(R.layout.activity_device_scan, null);

                    viewHolder = new ViewHolder();
                    viewHolder.deviceAddress = (TextView) view.findViewById(R.id.device_address);
                    viewHolder.deviceName = (TextView) view.findViewById(R.id.device_name);
                    viewHolder.textView_user_data = (TextView) view.findViewById(R.id.textView_user_data);
                    viewHolder.textView_user_value = (TextView) view.findViewById(R.id.textView_user_value);
                    viewHolder.textView_rssi = (TextView) view.findViewById(R.id.textView_rssi);
                    viewHolder.device_tx_level= (TextView) view.findViewById(R.id.device_tx_level);
                    viewHolder.progressBar1 =(ProgressBar) view.findViewById(R.id.progressBar1);
                    //viewHolder.progressBar_user_value =(ProgressBar) view.findViewById(R.id.progressBar_user_value);
                    //viewHolder.imageView_pio10 =(ImageView) view.findViewById(R.id.imageView_pio10);
                    //viewHolder.imageView_pio11 =(ImageView) view.findViewById(R.id.imageView_pio11);
                    viewHolder.linearLayout_user_value =(LinearLayout) view.findViewById(R.id.linearLayout_user_value);


                    view.setTag(viewHolder);
                } else {
                    viewHolder = (ViewHolder) view.getTag();
                }

                BluetoothDevice device = mLeDevices.get(i);

                final String deviceName = device.getName();
                if (deviceName != null && deviceName.length() > 0){
                    viewHolder.deviceName.setText(deviceName);				//����̽� �̸�
                }
                else{
                    //viewHolder.deviceName.setText(R.string.unknown_device);
                    viewHolder.deviceName.setText("알 수 없는 장치");

                }
                Log.d(TAG, "Dbg__ DeviceScan =1 ");
                viewHolder.deviceAddress.setText(device.getAddress());	//����̽� �ּ�
                byte[] array_byte=mBLE_Device.user_alldata[i].getBytes();

                String user_TX_level="";
                switch(array_byte[5]){
                    case (byte)0x08:
                        user_TX_level="8";

                        break;
                    case (byte)0x06:
                        user_TX_level="6";
                        break;
                    case (byte)0x02:
                        user_TX_level="2";
                        break;
                    case (byte) 0xef:
                        switch(array_byte[7]){
                            case (byte) 0xbe:
                                user_TX_level="-2";
                                break;
                            case (byte) 0xba:
                                user_TX_level="-6";
                                break;
                            case (byte) 0xb6:
                                user_TX_level="-10";
                                break;
                            case (byte) 0xb2:
                                user_TX_level="-14";
                                break;
                            case (byte) 0xae:
                                user_TX_level="-18";
                                break;

                        }

                        break;
                }

                Log.d(TAG, "Dbg__ DeviceScan =2 ");
                byte[] inputBytes1 = mBLE_Device.user_alldata[i].getBytes(); //����� ������ ��ġ ã��
                int start_0=0;
                int start_count=0;
                for (byte b : inputBytes1) {
                    if(inputBytes1[start_0]==(byte)0xBF){

                        start_count++;

                        if(start_count == 2) {
                            break;
                        }
                    }
                    else
                        start_count = 0;

                    start_0++;
                }


                Log.d(TAG, "Dbg__ DeviceScan =3 ");
                //mBLE_Device.user_data[i] = new String(Arrays.copyOfRange(array_byte, start_0+1,start_0+12 )); //����� ������

                if(start_0 == array_byte.length)
                    start_0 = array_byte.length - 2;

                mBLE_Device.user_data[i] = new String(Arrays.copyOfRange(array_byte,start_0 + 1, array_byte.length - 1)); //����� ������
                mBLE_Device.user_TX_level[i]=user_TX_level;

                Log.d(TAG, "Dbg__ DeviceScan =4 ");

                //int rssi_buff = mBLE_Device.rssi[i]+99;
                viewHolder.textView_rssi.setText( "Rssi : "+Integer.toString(mBLE_Device.rssi[i])+"dBm"); //RSSI ȭ�鿡 ǥ��
                viewHolder.device_tx_level.setText("Tx Level : "+user_TX_level+"dBm");
                viewHolder.progressBar1.setProgress( mBLE_Device.rssi[i]+99);
                viewHolder.textView_user_data.setText("DATA: "+mBLE_Device.user_data[i]);
                viewHolder.textView_user_value.setText("");
                //viewHolder.imageView_pio10.setImageResource(R.drawable.pio10_off);
                //viewHolder.imageView_pio11.setImageResource(R.drawable.pio11_off);
                viewHolder.linearLayout_user_value.setVisibility(view.GONE);
                try{
                    String data = mBLE_Device.user_data[i];

                    String set_data="";
                    Log.d(TAG, "Dbg__ DeviceScan =72 ="+data);
                    if(data.length()>=5){
    // user data�� �ִ� �� �о� ���� �з� �ϱ�
                        if((data.substring(0, 1).equals("0"))|(data.substring(0, 1).equals("1"))|(data.substring(0, 1).equals("2"))|(data.substring(0, 1).equals("N"))){
                            viewHolder.linearLayout_user_value.setVisibility(view.VISIBLE);
                            if((data.substring(0, 1).equals("0"))|(data.substring(0, 1).equals("1"))|(data.substring(0, 1).equals("2"))){ //AIO �з�
                                if(!(data.substring(1, 5).equals("0000"))){
                                    byte[] bytes = new java.math.BigInteger(data.substring(1, 5), 16).toByteArray();

                                    String aio_vl=String.format("%.3f" , byte2Int(bytes)*0.001);


                                    set_data= "AIO "+data.substring(0, 1)+": "+aio_vl+"V";

                                    //viewHolder.progressBar_user_value.setProgress((byte2Int(bytes)));
                                }else{
                                    set_data= "AIO "+data.substring(0, 1)+": 0.000V";
                                }
                            }


                            /*
                            if((data.substring(5, 6).equals("H"))|(data.substring(5, 6).equals("L"))){ // PIO�з�
                                if(data.substring(5, 6).equals("H")){
                                    viewHolder.imageView_pio10.setImageResource(R.drawable.pio10_on);
                                }else{
                                    viewHolder.imageView_pio10.setImageResource(R.drawable.pio10_off);
                                }
                                if(data.substring(6, 7).equals("H")){
                                    viewHolder.imageView_pio11.setImageResource(R.drawable.pio11_on);
                                }else{
                                    viewHolder.imageView_pio11.setImageResource(R.drawable.pio11_off);
                                }

                            }
                            */
                            viewHolder.textView_user_value.setText(set_data);
                        }

                    }
                }catch(Exception e){

                }

            }catch(Exception e){
                Log.d(TAG, "Dbg__DeviceScan err =2 ");
            }

            return view;
        }

        public int byte2Int(byte[] src) { //byte -> int ������ ����
            int s1 = src[0] & 0xFF;
            int s2 = 0;

            if(src.length>=2){
                s2 = src[1] & 0xFF;
                return ((s1 << 8) + (s2 << 0));
            }else{
                return (s1 << 0);
            }
        }
    }


    @Override
    protected void onResume() {
        super.onResume();

        // Ensures Bluetooth is enabled on the device.  If Bluetooth is not currently enabled,
        // fire an intent to display a dialog asking the user to grant permission to enable it.
        if (!mBluetoothAdapter.isEnabled()) {
            if (!mBluetoothAdapter.isEnabled()) {
                Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
                startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }
        }

        // Initializes list view adapter.
        mLeDeviceListAdapter = new LeDeviceListAdapter();
        setListAdapter(mLeDeviceListAdapter);


    }

    @Override
    protected void onPause() {
        super.onPause();
        mScanning_run=false;
        scanLeDevice(false);
        mHandler.removeMessages(0);
        mLeDeviceListAdapter.clear();
        mscaneHandler.removeMessages(0);
    }



    @SuppressLint("HandlerLeak")
    private final Handler mscaneHandler = new Handler() { //���� �����ϱ����� �ڵ鷯
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case 0:
                    user_data_count=0;
                    mBLE_Device.setclear();
                    mLeDeviceListAdapter.clear();
                    mScanning = true;
                    mBluetoothAdapter.startLeScan(mLeScanCallback);
                    mLeDeviceListAdapter.notifyDataSetChanged();
                    if(mScanning_run){
                        mscaneHandler.sendEmptyMessageDelayed(1, 8000);
                    }
                    break;
                case 1:
                    mScanning = false;
                    mBluetoothAdapter.stopLeScan(mLeScanCallback);
                    if(mScanning_run) {mscaneHandler.sendEmptyMessageDelayed(0, 200);}

                    break;
                case 2:

                    mScanning = false;
                    mBluetoothAdapter.stopLeScan(mLeScanCallback);
                    mscaneHandler.removeMessages(0);

                    break;
            }

        }
    };

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // User chose not to enable Bluetooth.
        if (requestCode == REQUEST_ENABLE_BT && resultCode == Activity.RESULT_CANCELED) {
            finish();
            return;
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    protected void onListItemClick(ListView l, View v, int position, long id) {

        final BluetoothDevice device = mLeDeviceListAdapter.getDevice(position);
        if (device == null) return;

        Intent resultIntent = new Intent();
        resultIntent.putExtra(MainActivity.EXTRAS_DEVICE_NAME, device.getName());
        resultIntent.putExtra(MainActivity.EXTRAS_DEVICE_ADDRESS,device.getAddress());
        setResult(Activity.RESULT_OK, resultIntent);

        finish();


    }

    // Device scan callback.
    //����̽� ��ĵ
    private BluetoothAdapter.LeScanCallback mLeScanCallback =
            new BluetoothAdapter.LeScanCallback() {

                @Override
                public void onLeScan(final BluetoothDevice device, final int rssi,  final byte[] scanRecord) {
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            Log.d(TAG, "Dbg__DeviceScan st =00 ");
                            mLeDeviceListAdapter.addDevice(device);
                            Log.d(TAG, "Dbg__DeviceScan st =01 "+user_data_count);
                            mLeDeviceListAdapter.notifyDataSetChanged();
                            Log.d(TAG, "Dbg__DeviceScan st =02 "+rssi);
                            mBLE_Device.rssi[user_data_count]=rssi;
                            Log.d(TAG, "Dbg__DeviceScan st =03 ");
                            parseAdvertisementPacket(scanRecord);
                            //Log.w(ble_terminal.LOG_TAG,String.format("Dbg__hex=")+rssi);
                            Log.d(TAG, "bgb__hex=" + rssi);
                        }
/*    	Advertising (Discoverable) Data format �м�
BoT    / TX -2  /0123456789 ,       |  020106020a | efbfbe | 04    |  09   | 426f54    | 0eefbfbf0000 | 3031323334353637383930 |
test   / TX  2  /0123456789 ,       |  020106020a |    02  | 05    |  09   | 74657374  | 0eefbfbf0000 | 3031323334353637383900 |
�̸�         /Txlevel  /����� ������                        |             |Txlevel | �̸� ���� |�̸� TYPE|   �̸�            |              |          ����� ������              |
  */

                        void parseAdvertisementPacket(final byte[] scanRecord) {//Advertising ������
                            byte[] advertisedData = Arrays.copyOfRange(scanRecord,0, scanRecord.length);
                            final StringBuilder stringBuilder = new StringBuilder(advertisedData.length);
                            Log.d(TAG, "Dbg__DeviceScan st =1 ");
                            for(byte byteChar : advertisedData){
                                stringBuilder.append((char)  byteChar);
                            }

                            byte[] inputBytes = stringBuilder.toString().getBytes();
                            String hexString = "";

                            for (byte b : inputBytes) {
                                hexString += Integer.toString((b & 0xF0) >> 4, 16);
                                hexString += Integer.toString(b & 0x0F, 16);
                            }
                            Log.w(TAG,String.format("Dbg__hex=")+hexString);
                            mBLE_Device.user_alldata[user_data_count]=stringBuilder.toString();
                            user_data_count++;

                        }
                    });


                }
            };

    static class ViewHolder {
        TextView deviceName;
        TextView deviceAddress;
        TextView textView_user_data;
        TextView textView_user_value;
        TextView textView_rssi;
        TextView device_tx_level;
        ProgressBar progressBar1;
        //ProgressBar progressBar_user_value;
        //ImageView imageView_pio10;
        //ImageView imageView_pio11;
        LinearLayout linearLayout_user_value;


    }
//BLE ��� �� �� ���� CLASS
    static class BLE_Device {
        String[] user_alldata=new String[500];
        String[] user_data=new String[500];
        String[] user_TX_level=new String[500];
        int[] rssi=new int[500];


        public void setclear() {
            for (int i = 0; i < 500; i++) {
                user_alldata[i]="";
                user_data[i]="";
                user_TX_level[i]="";
                rssi[i]=0;
            }

        }
    }
}

