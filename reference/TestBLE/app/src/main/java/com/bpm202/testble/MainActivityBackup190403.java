package com.bpm202.testble;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothManager;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanResult;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.util.List;
import java.util.UUID;

public class MainActivityBackup190403 extends AppCompatActivity {

    private final static String TAG = "Test_MainActivity";
    private static final String LOG_TAG = "Test_Log";

    private Button btn_button;
    private BluetoothAdapter mBluetoothAdapter;
    private boolean mScanning;
    private Handler mHandler = new Handler();

    // Stops scanning after 10 seconds.
    private static final long SCAN_PERIOD = 10000;
    private BluetoothLeScanner mBLEScanner;
    private final int MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE = 10;
    private TextView tv_contents;
    private boolean isConnected;
    private BluetoothGatt gatt;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        if (initBLE()) {
            finish();
        } else {
            checkPermissions();
            initView();
        }
        registerReceiver(mGattUpdateReceiver, makeGattUpdateIntentFilter());
    }

    private final BroadcastReceiver mGattUpdateReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            final String action = intent.getAction();

            Log.e(TAG, "TEST Dbg_Navig_mGattUpdateReceiver:" + action);

            if (ACTION_GATT_CONNECTED.equals(action)) {
                isConnected = true;
                Log.d(TAG, "TEST onReceive _ Search_F");

            } else if (ACTION_GATT_DISCONNECTED.equals(action)) {
                isConnected = false;
                Log.d(TAG, "TEST onReceive _ Search");

            } else if (ACTION_GATT_SERVICES_DISCOVERED.equals(action)) {
                Log.d(TAG, "TEST onReceive _ ACTION_GATT_SERVICES_DISCOVERED");
                // Show all the supported services and characteristics on the user interface.
                //서비스를 제공하고 있는지를 보여준다.

                discoveredGattServices(gatt.getServices());
                //discoveredGattServices(mBluetoothLeService.getSupportedGattServices());
            } else if (ACTION_DATA_AVAILABLE.equals(action)) {
                byte[] data = intent.getByteArrayExtra(EXTRA_DATA_RAW);        //�����Ͱ� ������ �б�
                String data_string = intent.getStringExtra(EXTRA_DATA_STRING);    //�����Ͱ� ��Ʈ������ ��ȯ�ؼ� �б�
                String uudi_data = intent.getStringExtra(UUID_STRING);    //UUID�� ��Ʈ������ ��ȯ�ؼ� �б�

                dataReceived(uudi_data, data_string, data);
            }
        }
    };

    //UART ?��?��?�� ?��?��
    public void dataReceived(String uudi_data, String data_string, byte[] row_data) {

        Log.d(TAG, "[TEST_Data] uudi_data : " + uudi_data);
        Log.d(TAG, "[TEST_Data] data_string : " + data_string);
        Log.d(TAG, "[TEST_Data] row_data : " + row_data);
        Log.d(TAG, "[TEST_Data] row_data.toString() : " + row_data.toString());

        StringBuilder sb = new StringBuilder();
        for (byte b : row_data) {
            sb.append(b + "/");
        }

        Log.d(TAG, "[TEST_Data11] byte : " + sb.toString());


        int stx = row_data[0];
        int seq = row_data[1];
//        int Gyro_X1 = row_data[2];
//        int Gyro_X2 = row_data[3];
//
//        int Gyro_Y1 = row_data[4];
//        int Gyro_Y2 = row_data[5];
//
//        int Gyro_Z1 = row_data[6];
//        int Gyro_Z2 = row_data[7];
//
//        int Range1 = row_data[14];
//        int Range2 = row_data[15];

        int Gyro_X = row_data[3] + row_data[2];
        int Gyro_Y = row_data[5] + row_data[4];
        int Gyro_Z = row_data[7] + row_data[6];
        int Range = row_data[15] + row_data[14];
        int batt = row_data[16];


        Log.d(TAG, "[TEST_Data111] stx : " + stx + " seq : " + seq);
        Log.d(TAG, "[TEST_Data111] Gyro_X : " + Gyro_X + " Gyro_Y : " + Gyro_Y + " GyroZ : " + Gyro_Z + " Range : " + Range);
        Log.d(TAG, "[TEST_Data111] Batt : " + batt + " length : " + row_data.length);
//        Log.d(TAG, "[TEST_Data111] Gyro_X1 : " + Gyro_X1 + " Gyro_X2 : " + Gyro_X2 + " Gyro : " + (Gyro_X1 + Gyro_X2));
//        Log.d(TAG, "[TEST_Data111] Gyro_Y1 : " + Gyro_Y1 + " Gyro_Y2 : " + Gyro_Y2 + " Gyro : " + (Gyro_Y1 + Gyro_Y2));
//        Log.d(TAG, "[TEST_Data111] Gyro_Z1 : " + Gyro_Z1 + " Gyro_Z2 : " + Gyro_Z2 + " Gyro : " + (Gyro_Z1 + Gyro_Z2));
//        Log.d(TAG, "[TEST_Data111] Range1 : " + Range1 + " Range2 : " + Range2 + " Range : " + (Range1 + Range2));
    }


    // Demonstrates how to iterate through the supported GATT Services/Characteristics.
    // In this sample, we populate the data structure that is bound to the ExpandableListView
    // on the UI.
    private void discoveredGattServices(List<BluetoothGattService> gattServices) {
        if (gattServices == null) {
            Log.d(TAG, "TEST, [gattService] : null");

            return;
        }
        String uuid = null;

        // Loops through available GATT Services.
        for (BluetoothGattService gattService : gattServices) {
            uuid = gattService.getUuid().toString();
            Log.d(TAG, "TEST, [gattService / uuid] : " + uuid);
            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();

            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics) {
                uuid = gattCharacteristic.getUuid().toString();
                Log.d(TAG, "TEST, [gattCharacteristic / uuid] : " + uuid);
            }
        }
    }


    private void initView() {
        btn_button = findViewById(R.id.btn_button);
        tv_contents = findViewById(R.id.tv_contents);
        btn_button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                scanLeDevice(true);
            }
        });
    }

    private boolean initBLE() {
        // 블루투스 사용가능 스마트폰인지 확인
        if (!getPackageManager().hasSystemFeature(PackageManager.FEATURE_BLUETOOTH_LE)) {
            Toast.makeText(this, "BLE 지원 안함", Toast.LENGTH_SHORT).show();
            return true;
        } else {
            // 블루투스 매니저에서 어뎁터를 가져오기위해 시스템에서 매니저를 얻음
            final BluetoothManager bluetoothManager = (BluetoothManager) getSystemService(Context.BLUETOOTH_SERVICE);
            mBluetoothAdapter = bluetoothManager.getAdapter(); // 블루투스 어뎁터를 얻음
            if (mBluetoothAdapter == null) { // 블루투스 어뎁터가 없으면 종료
                Toast.makeText(this, "블루투스 지원 안함", Toast.LENGTH_SHORT).show();
                return true;
            }

            mBLEScanner = mBluetoothAdapter.getBluetoothLeScanner();
            if (mBLEScanner == null) { // Checks if Bluetooth LE Scanner is available.
                Toast.makeText(this, "Can not find BLE Scanner", Toast.LENGTH_SHORT).show();
                return true;
            }
        }
        return false;
    }


    @Override
    public void onDestroy() {
        super.onDestroy();
        android.os.Process.killProcess(android.os.Process.myPid());
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {

        switch (requestCode) {

            case MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    Log.d("TEST", "MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE :  OK ");
                } else {
                    Log.d("TEST", "MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE :  NOPE ");
                }
                break;

        }

        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    private void checkPermissions() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION}, MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE);
        }
    }

    private void scanLeDevice(final boolean enable) {
        if (enable) { // Stops scanning after a pre-defined scan period.
            mHandler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    mBLEScanner.stopScan(mScanCallback);
                }
            }, SCAN_PERIOD);
            mBLEScanner.startScan(mScanCallback);
        } else {
            mBLEScanner.stopScan(mScanCallback);
        }
    }

    private String mDeviceName;
    private String mDeviceAddress;
    private ScanCallback mScanCallback = new ScanCallback() {
        @Override
        public void onScanResult(int callbackType, ScanResult result) {
            processResult(result);
        }

        @Override
        public void onBatchScanResults(List<ScanResult> results) {
            for (ScanResult result : results) {
                processResult(result);
            }
        }

        @Override
        public void onScanFailed(int errorCode) {
            Log.d("TEST", "onScanFailed :" + errorCode);
        }

        private void processResult(final ScanResult result) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {


                    BluetoothDevice mDevice = result.getDevice();
                    mDeviceName = result.getDevice().getName();
                    mDeviceAddress = result.getDevice().getAddress();

                    if (mDeviceName != null) {
                        if (result != null && result.getDevice() != null && result.getDevice().getName() != null && !result.getDevice().getName().isEmpty() && result.getDevice().getName().equals("sensor")) {
                            Toast.makeText(MainActivityBackup190403.this, "[DeviceName] : " + mDeviceName + "\n[DeviceAddress] : " + mDeviceAddress, Toast.LENGTH_SHORT).show();
                            mBLEScanner.stopScan(mScanCallback);
                            gatt = mDevice.connectGatt(MainActivityBackup190403.this, true, mGattCallback);
                        }
                    }
                }
            });
        }
    };

    UUID serviceUUID = UUID.fromString("4880c12c-fdcb-4077-8920-a450d7f9b907");
    UUID characteristicUUID = UUID.fromString("fec26ec4-6d71-4442-9f81-55bc21d658d6");

    public static final String ACTION_GATT_CONNECTED = "com.example.bluetooth.le.ACTION_GATT_CONNECTED";
    public static final String ACTION_GATT_DISCONNECTED = "com.example.bluetooth.le.ACTION_GATT_DISCONNECTED";
    public static final String ACTION_GATT_SERVICES_DISCOVERED = "com.example.bluetooth.le.ACTION_GATT_SERVICES_DISCOVERED";
    public static final String ACTION_DATA_AVAILABLE = "com.example.bluetooth.le.ACTION_DATA_AVAILABLE";
    public static final String EXTRA_DATA_STRING = "com.example.bluetooth.le.EXTRA_DATA_STRING";
    public static final String EXTRA_DATA_RAW = "com.example.bluetooth.le.EXTRA_DATA_RAW";
    public static final String UUID_STRING = "com.example.bluetooth.le.UUID_STRING";
    public static final String UUID_INTENT = "com.example.bluetooth.le.UUID_INTENT";


    private void broadcastUpdate(String action) {
        Log.i(TAG, "park_Blue broadcastUpdate()0 : ");
        Intent intent = new Intent(action);
        this.sendBroadcast(intent);
    }

    private void broadcastUpdate(String action, BluetoothGattCharacteristic characteristic) {
        Intent intent = new Intent(action);
        //intent.putExtra("com.example.bluetooth.le.UUID_INTENT", characteristic.getValue());
        intent.putExtra(UUID_INTENT, characteristic.getValue());
        Log.d(TAG, "park_Blue_Service broadcastUpdate_0 :" + characteristic.getUuid().toString());
        Log.d(TAG, "TEST park_Blue_Service broadcastUpdate_0 :" + characteristic.getUuid().toString());
        //intent.putExtra("com.example.bluetooth.le.UUID_STRING", characteristic.getUuid().toString());
        intent.putExtra(UUID_STRING, characteristic.getUuid().toString());
        byte[] data = characteristic.getValue();
        Log.d(TAG, "park_Blue_Service broadcastUpdate_1 :" + data.length);
        Log.d(TAG, "TEST park_Blue_Service broadcastUpdate_1 :" + data.length);
        if (data != null && data.length > 0) {
            StringBuilder stringBuilder = new StringBuilder(data.length);
            byte[] var9 = data;
            int var8 = data.length;

            for (int var7 = 0; var7 < var8; ++var7) {
                byte byteChar = var9[var7];
                stringBuilder.append(String.format("%02X ", byteChar));
            }

            //Log.d(TAG, "park_Blue_Service broadcastUpdate_2 :" + stringBuilder.toString());
            Log.d(TAG, "TEST park_Blue_Service broadcastUpdate_2 :" + stringBuilder.toString());
            //intent.putExtra("com.example.bluetooth.le.EXTRA_DATA_STRING", stringBuilder.toString());
            intent.putExtra(EXTRA_DATA_STRING, stringBuilder.toString());
            //intent.putExtra("com.example.bluetooth.le.EXTRA_DATA_RAW", data);
            intent.putExtra(EXTRA_DATA_RAW, data);
        }

        this.sendBroadcast(intent);
    }


    BluetoothGattCallback mGattCallback = new BluetoothGattCallback() {
        @Override
        public void onPhyUpdate(BluetoothGatt gatt, int txPhy, int rxPhy, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onPhyUpdate]: ");
            super.onPhyUpdate(gatt, txPhy, rxPhy, status);
        }

        @Override
        public void onPhyRead(BluetoothGatt gatt, int txPhy, int rxPhy, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onPhyRead]: ");
            super.onPhyRead(gatt, txPhy, rxPhy, status);
        }

        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            Log.d(TAG, "TEST, BluetoothGatt [onConnectionStateChange]: gatt " + gatt + " status : " + status + " newState : " + newState);
            //super.onConnectionStateChange(gatt, status, newState);

            //Log.i(BluetoothLeService.TAG, "park_Blue_Connected to GATT server.");
            String intentAction;
            if (newState == 2) {
                //intentAction = "com.example.bluetooth.le.ACTION_GATT_CONNECTED";
                intentAction = ACTION_GATT_CONNECTED;
                //BluetoothLeService.this.mConnectionState = 2;
                //BluetoothLeService.this.broadcastUpdate(intentAction);
                broadcastUpdate(intentAction);
                //  Log.i(BluetoothLeService.TAG, "park_Blue_Attempting to start service discovery:");
                //BluetoothLeService.this.mBluetoothGatt.discoverServices();
                gatt.discoverServices();
            } else if (newState == 0) {
                //intentAction = "com.example.bluetooth.le.ACTION_GATT_DISCONNECTED";
                intentAction = ACTION_GATT_DISCONNECTED;
                //BluetoothLeService.this.mConnectionState = 0;
                //Log.i(BluetoothLeService.TAG, "Disconnected from GATT server.");
                //BluetoothLeService.this.broadcastUpdate(intentAction);
                broadcastUpdate(intentAction);
            }
        }


        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onServicesDiscovered]: ");
            //super.onServicesDiscovered(gatt, status);
            //Log.w(BluetoothLeService.TAG, "park_Blue_onServicesDiscovered received1: ");
            Log.w(TAG, "TEST park_Blue_onServicesDiscovered received1: ");


            //BluetoothGattCharacteristic characteristic = gatt.getService(HEART_RATE_SERVICE_UUID).getCharacteristic(HEART_RATE_MEASUREMENT_CHAR_UUID);
            //gatt.setCharacteristicNotification(characteristic, enabled);
            BluetoothGattCharacteristic characteristic = gatt.getService(serviceUUID).getCharacteristic(characteristicUUID);
            //List<BluetoothGattCharacteristic> characteristics = gatt.getService(serviceUUID).getCharacteristics();
            gatt.setCharacteristicNotification(characteristic, true);

            //BluetoothGattDescriptor descriptor = characteristic.getDescriptor(CLIENT_CHARACTERISTIC_CONFIG_UUID);
            BluetoothGattDescriptor descriptor = characteristic.getDescriptor(convertFromInteger(0x2902));


            descriptor.setValue(BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE);
            gatt.writeDescriptor(descriptor);

//            for (BluetoothGattCharacteristic characteristic :characteristics) {
//                broadcastUpdate(ACTION_GATT_SERVICES_DISCOVERED, characteristic);
//            }


            if (status == 0) {
                //BluetoothLeService.this.broadcastUpdate("com.example.bluetooth.le.ACTION_GATT_SERVICES_DISCOVERED");
                //broadcastUpdate(ACTION_GATT_SERVICES_DISCOVERED);
            } else {
                //Log.w(BluetoothLeService.TAG, "park_Blue_onServicesDiscovered received: " + status);
                Log.w(TAG, "TEST park_Blue_onServicesDiscovered received: " + status);
            }
        }

        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onCharacteristicRead]: ");
            //super.onCharacteristicRead(gatt, characteristic, status);
            //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicRead()00 : ");
            Log.i(TAG, "TEST park_Blue_onCharacteristicRead()00 : ");
            if (status == 0) {
                //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicRead()01 : ");
                Log.i(TAG, "TEST park_Blue_onCharacteristicRead()01 : ");
                //BluetoothLeService.this.broadcastUpdate("com.example.bluetooth.le.ACTION_DATA_AVAILABLE", characteristic);
                broadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
            }
        }

        @Override
        public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            Log.d(TAG, "TEST TEST, BluetoothGatt [onCharacteristicWrite]: ");
            //super.onCharacteristicWrite(gatt, characteristic, status);
            //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicWrite_length()0 : ");
            Log.i(TAG, "TEST TEST park_Blue_onCharacteristicWrite_length()0 : ");
            if (status == 0) {
                byte[] data = characteristic.getValue();
                //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicWrite_length() : " + data.length);
                Log.i(TAG, "TEST park_Blue_onCharacteristicWrite_length() : " + data.length);
                //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicWrite() : " + new String(data));
                Log.i(TAG, "TEST park_Blue_onCharacteristicWrite() : " + new String(data));
            }


        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            Log.d(TAG, "TEST, BluetoothGatt [onCharacteristicChanged]: ");
            //super.onCharacteristicChanged(gatt, characteristic);
            //Log.i(BluetoothLeService.TAG, "park_Blue_onCharacteristicChanged()11 : ");
            Log.i(TAG, "TEST park_Blue_onCharacteristicChanged()11 : ");
            //BluetoothLeService.this.broadcastUpdate("com.example.bluetooth.le.ACTION_DATA_AVAILABLE", characteristic);
            broadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
        }

        @Override
        public void onDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onDescriptorRead]: ");
            super.onDescriptorRead(gatt, descriptor, status);
        }

        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onDescriptorWrite]: ");
            //super.onDescriptorWrite(gatt, descriptor, status);
            //BluetoothGattCharacteristic characteristic = gatt.getService(HEART_RATE_SERVICE_UUID).getCharacteristic(HEART_RATE_CONTROL_POINT_CHAR_UUID);
            BluetoothGattCharacteristic characteristic = gatt.getService(serviceUUID).getCharacteristic(characteristicUUID);
            characteristic.setValue(new byte[]{1, 1});
            gatt.writeCharacteristic(characteristic);
        }

        @Override
        public void onReliableWriteCompleted(BluetoothGatt gatt, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onReliableWriteCompleted]: ");
            super.onReliableWriteCompleted(gatt, status);
        }

        @Override
        public void onReadRemoteRssi(BluetoothGatt gatt, int rssi, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onReadRemoteRssi]: ");
            super.onReadRemoteRssi(gatt, rssi, status);
        }

        @Override
        public void onMtuChanged(BluetoothGatt gatt, int mtu, int status) {
            Log.d(TAG, "TEST, BluetoothGatt [onMtuChanged]: ");
            super.onMtuChanged(gatt, mtu, status);
        }
    };

    private static IntentFilter makeGattUpdateIntentFilter() {
        final IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(ACTION_DATA_AVAILABLE);
        intentFilter.addAction(ACTION_GATT_CONNECTED);
        intentFilter.addAction(ACTION_GATT_DISCONNECTED);
        intentFilter.addAction(ACTION_GATT_SERVICES_DISCOVERED);
        return intentFilter;
    }

    public UUID convertFromInteger(int i) {
        final long MSB = 0x0000000000001000L;
        final long LSB = 0x800000805f9b34fbL;
        long value = i & 0xFFFFFFFF;
        return new UUID(MSB | (value << 32), LSB);
    }
}
