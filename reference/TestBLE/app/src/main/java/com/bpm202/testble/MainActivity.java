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

public class MainActivity extends AppCompatActivity {

    private final static String TAG = "Test_MainActivity";
    private static final String LOG_TAG = "Test_Log";

    private Button btn_button;
    private BluetoothAdapter mBluetoothAdapter;
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
            if (ACTION_GATT_CONNECTED.equals(action)) {
                isConnected = true;
            } else if (ACTION_GATT_DISCONNECTED.equals(action)) {
                isConnected = false;
            } else if (ACTION_GATT_SERVICES_DISCOVERED.equals(action)) {
                Log.d(TAG, "TEST onReceive _ ACTION_GATT_SERVICES_DISCOVERED");
                // Show all the supported services and characteristics on the user interface.
                //서비스를 제공하고 있는지를 보여준다.
                discoveredGattServices(gatt.getServices());
            } else if (ACTION_DATA_AVAILABLE.equals(action)) {
                byte[] data = intent.getByteArrayExtra(EXTRA_DATA_RAW);        //�����Ͱ� ������ �б�
                String data_string = intent.getStringExtra(EXTRA_DATA_STRING);    //�����Ͱ� ��Ʈ������ ��ȯ�ؼ� �б�
                String uudi_data = intent.getStringExtra(UUID_STRING);    //UUID�� ��Ʈ������ ��ȯ�ؼ� �б�
                dataReceived(uudi_data, data_string, data);
            }
        }
    };

    public void dataReceived(String uudi_data, String data_string, byte[] row_data) {
        int stx = row_data[0];
        int seq = row_data[1];
        int Gyro_X = row_data[3] + row_data[2];
        int Gyro_Y = row_data[5] + row_data[4];
        int Gyro_Z = row_data[7] + row_data[6];
        int Range = row_data[15] + row_data[14];
        int batt = row_data[16];
        int etx = row_data[17];


        Log.d(TAG, "[TEST_Data111] stx : " + stx + " seq : " + seq + " etx : " + etx);
        Log.d(TAG, "[TEST_Data111] Gyro_X : " + Gyro_X + " Gyro_Y : " + Gyro_Y + " GyroZ : " + Gyro_Z + " Range : " + Range);
        Log.d(TAG, "[TEST_Data111] Batt : " + batt + " length : " + row_data.length);
    }


    // Demonstrates how to iterate through the supported GATT Services/Characteristics.
    // In this sample, we populate the data structure that is bound to the ExpandableListView
    // on the UI.
    private void discoveredGattServices(List<BluetoothGattService> gattServices) {
        if (gattServices == null) {
            return;
        }
        String uuid = null;

        // Loops through available GATT Services.
        for (BluetoothGattService gattService : gattServices) {
            uuid = gattService.getUuid().toString();

            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();

            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics) {
                uuid = gattCharacteristic.getUuid().toString();
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
                            Toast.makeText(MainActivity.this, "[DeviceName] : " + mDeviceName + "\n[DeviceAddress] : " + mDeviceAddress, Toast.LENGTH_SHORT).show();
                            mBLEScanner.stopScan(mScanCallback);
                            gatt = mDevice.connectGatt(MainActivity.this, true, mGattCallback);
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
        intent.putExtra(UUID_INTENT, characteristic.getValue());
        intent.putExtra(UUID_STRING, characteristic.getUuid().toString());
        byte[] data = characteristic.getValue();
        if (data != null && data.length > 0) {
            StringBuilder stringBuilder = new StringBuilder(data.length);
            byte[] var9 = data;
            int var8 = data.length;
            for (int var7 = 0; var7 < var8; ++var7) {
                byte byteChar = var9[var7];
                stringBuilder.append(String.format("%02X ", byteChar));
            }
            intent.putExtra(EXTRA_DATA_STRING, stringBuilder.toString());
            intent.putExtra(EXTRA_DATA_RAW, data);
        }
        this.sendBroadcast(intent);
    }

    BluetoothGattCallback mGattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            Log.d(TAG, "TEST, BluetoothGatt [onConnectionStateChange]: gatt " + gatt + " status : " + status + " newState : " + newState);
            String intentAction;
            if (newState == 2) {
                intentAction = ACTION_GATT_CONNECTED;
                broadcastUpdate(intentAction);
                gatt.discoverServices();
            } else if (newState == 0) {
                intentAction = ACTION_GATT_DISCONNECTED;
                broadcastUpdate(intentAction);
            }
        }


        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            BluetoothGattCharacteristic characteristic = gatt.getService(serviceUUID).getCharacteristic(characteristicUUID);
            gatt.setCharacteristicNotification(characteristic, true);
            BluetoothGattDescriptor descriptor = characteristic.getDescriptor(convertFromInteger(0x2902));
            descriptor.setValue(BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE);
            gatt.writeDescriptor(descriptor);
            if (status == BluetoothGatt.GATT_SUCCESS) {
                broadcastUpdate(ACTION_GATT_SERVICES_DISCOVERED);
            }
        }

        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                broadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
            }
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            broadcastUpdate(ACTION_DATA_AVAILABLE, characteristic);
        }

        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            BluetoothGattCharacteristic characteristic = gatt.getService(serviceUUID).getCharacteristic(characteristicUUID);
            characteristic.setValue(new byte[]{1, 1});
            gatt.writeCharacteristic(characteristic);
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
