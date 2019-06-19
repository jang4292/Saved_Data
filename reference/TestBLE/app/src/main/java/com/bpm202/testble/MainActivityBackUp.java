package com.bpm202.testble;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
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
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.chipsen.bleservice.BluetoothLeService;
import com.chipsen.bleservice.SampleGattAttributes;

import java.util.List;

public class MainActivityBackUp extends AppCompatActivity {

    private final static String TAG = "Test_MainActivity";
    private static final String LOG_TAG = "Test_Log";

    private BluetoothLeService mBluetoothLeService;

    private Button btn_button;
    private BluetoothAdapter mBluetoothAdapter;
    private boolean mScanning;
    private Handler mHandler = new Handler();

    // Stops scanning after 10 seconds.
    private static final long SCAN_PERIOD = 10000;
    private BluetoothLeScanner mBLEScanner;
    private final int MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE = 10;
    private TextView tv_contents;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        // 블루투스 사용가능 스마트폰인지 확인
        if (!getPackageManager().hasSystemFeature(PackageManager.FEATURE_BLUETOOTH_LE)) {
            Toast.makeText(this, "BLE 지원 안함", Toast.LENGTH_SHORT).show();
            finish();
        } else {

            // 블루투스 매니저에서 어뎁터를 가져오기위해 시스템에서 매니저를 얻음
            final BluetoothManager bluetoothManager =
                    (BluetoothManager) getSystemService(Context.BLUETOOTH_SERVICE);
            mBluetoothAdapter = bluetoothManager.getAdapter(); // 블루투스 어뎁터를 얻음

            // 블루투스 어뎁터가 없으면 종료
            if (mBluetoothAdapter == null) {
                Toast.makeText(this, "블루투스 지원 안함", Toast.LENGTH_SHORT).show();
                finish();
                return;
            }

            mBLEScanner = mBluetoothAdapter.getBluetoothLeScanner();
            // Checks if Bluetooth LE Scanner is available.
            if (mBLEScanner == null) {
                Toast.makeText(this, "Can not find BLE Scanner", Toast.LENGTH_SHORT).show();
                finish();
                return;
            }


            btn_button = findViewById(R.id.btn_button);

            tv_contents = findViewById(R.id.tv_contents);
            check();
            btn_button.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    //if (isConnected) {
                    if(mBluetoothLeService!= null) {

                        mBluetoothLeService.disconnect();
                    }
                    //}
                    scanLeDevice(true);
                }
            });
            /*Intent gattServiceIntent = new Intent(this, BluetoothLeService.class);
            Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++3");
            bindService(gattServiceIntent, mServiceConnection, BIND_AUTO_CREATE);
            Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++4");*/

            /*Intent gattServiceIntent = new Intent(getActivity(), BluetoothLeService.class);
            getActivity().bindService(gattServiceIntent, mServiceConnection, Context.BIND_AUTO_CREATE);*/

            Intent gattServiceIntent = new Intent(this, BluetoothLeService.class);
            Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++3");
            //bindService(gattServiceIntent, mServiceConnectionMain, Context.BIND_AUTO_CREATE);
            bindService(gattServiceIntent, mServiceConnection, Context.BIND_AUTO_CREATE);
            Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++4");

            registerReceiver(mGattUpdateReceiver, makeGattUpdateIntentFilter());
        }

    }

    private final ServiceConnection mServiceConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName componentName, IBinder service) {
            Log.d("Test", "onServiceConnected ");
            Log.d(TAG, "BleFragment_Service Connected");
            mBluetoothLeService = ((BluetoothLeService.LocalBinder) service).getService();
            if (!mBluetoothLeService.initialize()) {
                //getActivity().finish();
                finish();
            }

            findCharacteristic();
            //setBLEstate();
        }

        @Override
        public void onServiceDisconnected(ComponentName componentName) {
            mBluetoothLeService = null;
        }
    };


    private void check() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION},
                    MY_PERMISSIONS_REQUEST_READ_EXT_STORAGE);
        }
    }

    @Override
    public void onDestroy() {
        super.onDestroy();

        unregisterReceiver(mGattUpdateReceiver);
        unbindService(mServiceConnection);
        mBluetoothLeService = null;

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

    private void scanLeDevice(final boolean enable) {
        if (enable) {
            // Stops scanning after a pre-defined scan period.
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

                    mDeviceName = result.getDevice().getName();
                    mDeviceAddress = result.getDevice().getAddress();
                    if (mDeviceName != null) {


                        if (result != null && result.getDevice() != null && result.getDevice().getName() != null && !result.getDevice().getName().isEmpty() && result.getDevice().getName().equals("sensor")) {
                            mBLEScanner.stopScan(mScanCallback);

                            if (mBluetoothLeService != null) {

                                final boolean result = mBluetoothLeService.connect(mDeviceAddress);
                                if(result) {
                                    //mBluetoothLeService.
                                    //mBluetoothLeService.getSupportedGattServices().
                                }


                                //this.mBluetoothGatt = device.connectGatt(this, false, this.mGattCallback);

                                //bluetoothDeviceAddress = device.getAddress();
                                //BluetoothDevice device = bluetoothAdapter.getRemoteDevice(bluetoothDeviceAddress);
//                                BluetoothDevice device = mBluetoothAdapter.getRemoteDevice(mDeviceAddress);
//
//                                device.connectGatt(MainActivity.this, true, mBluetoothGattCallback);
                            }

                        }
                    }


                }
            });
        }
    };
    private BluetoothGattCallback mBluetoothGattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            super.onConnectionStateChange(gatt, status, newState);
            Log.d("Test", "gatt : " + gatt);
            Log.d("Test", "status : " + status);
            Log.d("Test", "newState : " + newState);
            Log.d("Test","onConnectionStateChange");
        }

        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            super.onServicesDiscovered(gatt, status);
            Log.d("Test","onServicesDiscovered");
        }

        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            super.onCharacteristicRead(gatt, characteristic, status);
            Log.d("Test","onCharacteristicRead");
        }

        @Override
        public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            super.onCharacteristicWrite(gatt, characteristic, status);
            Log.d("Test","onCharacteristicWrite");
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            super.onCharacteristicChanged(gatt, characteristic);
            Log.d("Test","onCharacteristicChanged");
        }

        @Override
        public void onDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            super.onDescriptorRead(gatt, descriptor, status);
            Log.d("Test","onDescriptorRead");
        }

        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            super.onDescriptorWrite(gatt, descriptor, status);
            Log.d("Test","onDescriptorWrite");
        }

        @Override
        public void onReliableWriteCompleted(BluetoothGatt gatt, int status) {
            super.onReliableWriteCompleted(gatt, status);
            Log.d("Test","onReliableWriteCompleted");
        }

        @Override
        public void onReadRemoteRssi(BluetoothGatt gatt, int rssi, int status) {
            super.onReadRemoteRssi(gatt, rssi, status);
            Log.d("Test","onReadRemoteRssi");
        }

        @Override
        public void onMtuChanged(BluetoothGatt gatt, int mtu, int status) {
            super.onMtuChanged(gatt, mtu, status);
            Log.d("Test","onMtuChanged");
        }
    };

    /*private BluetoothAdapter bluetoothAdapter;
    public BluetoothAdapter getBluetoothAdapter() {

        if(bluetoothAdapter == null) {
            final BluetoothManager bluetoothManager = (BluetoothManager) mActivity.getSystemService(Context.BLUETOOTH_SERVICE);
            bluetoothAdapter = bluetoothManager.getAdapter();
        }
        return bluetoothAdapter;
    }*/

    private String mDeviceName;
    private String mDeviceAddress;


    private final BroadcastReceiver mGattUpdateReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            final String action = intent.getAction();
            Log.d(TAG, "TEST mGattUpdateReceiver Action : " + action);
            if (BluetoothLeService.ACTION_DATA_AVAILABLE.equals(action)) {
                byte[] data = intent.getByteArrayExtra(BluetoothLeService.EXTRA_DATA_RAW);        //�����Ͱ� ������ �б�
                String data_string = intent.getStringExtra(BluetoothLeService.EXTRA_DATA_STRING);    //�����Ͱ� ��Ʈ������ ��ȯ�ؼ� �б�
                String uudi_data = intent.getStringExtra(BluetoothLeService.UUID_STRING);    //UUID�� ��Ʈ������ ��ȯ�ؼ� �б�

                dataReceived(uudi_data, data_string, data);


            } else if (BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED.equals(action)) {
                // Show all the supported services and characteristics on the user interface.
                //���񽺰� ������ �������̽��� �����Ѵ�.
                findCharacteristic();
                //setBLEstate();

                // Show all the supported services and characteristics on the user interface.
                //서비스를 제공하고 있는지를 보여준다.
                //discoveredGattServices(mBluetoothLeService.getSupportedGattServices());
                //discoveredGattServices2(mBluetoothLeService.getSupportedGattServices());

            } else if (BluetoothLeService.ACTION_GATT_CONNECTED.equals(action)) {
                //isConnected = true;
                //imageView_search.setImageResource(R.drawable.search_f);
                Log.d("Test", "onReceive _ Search_F");

            } else if (BluetoothLeService.ACTION_GATT_DISCONNECTED.equals(action)) {
                //isConnected = false;
                //imageView_search.setImageResource(R.drawable.search);
                Log.d("Test", "onReceive _ Search");

            } else if (BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED.equals(action)) {
                /*// Show all the supported services and characteristics on the user interface.
                //서비스를 제공하고 있는지를 보여준다.
                discoveredGattServices(mBluetoothLeService.getSupportedGattServices());
                discoveredGattServices2(mBluetoothLeService.getSupportedGattServices());*/
            }

        }
    };

    public void dataReceived(String uudi_data, String data_string, byte[] row_data) {

        Log.d(TAG, "TEST , [uudi_data] : " + uudi_data);
        Log.d(TAG, "TEST , [data_string] : " + data_string);
        Log.d(TAG, "TEST, [row_Data] : " + row_data);

    }

    public void findCharacteristic() {
        // Find BLE112 service for writing to
        List<BluetoothGattService> gattServices = mBluetoothLeService.getSupportedGattServices();

        if (gattServices == null) return;
        String uuid = null;

        // Loops through available GATT Services.
        //������ GATT ���񽺸� ���� UUID�� ã�´�.
        for (BluetoothGattService gattService : gattServices) {
            //Log.d("Test", "gattService");
            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();
            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics) {
                uuid = gattCharacteristic.getUuid().toString();
                if (SampleGattAttributes.UART_READ_UUID.equals(uuid)) {
                    UART_Read = gattCharacteristic;
                    mBluetoothLeService.setCharacteristicNotification(UART_Read, true);// Notification on ����
                }
                if (SampleGattAttributes.UART_WRITE_UUID.equals(uuid))
                    UART_Write = gattCharacteristic;
                if (SampleGattAttributes.PIO_READ_WRITE_UUID.equals(uuid))
                    PIO_Read_Write = gattCharacteristic;
                if (SampleGattAttributes.PWM_READ_WRITE_UUID.equals(uuid))
                    PWM_Read_Write = gattCharacteristic;
                if (SampleGattAttributes.PIO_DIRECTION_UUID.equals(uuid))
                    PIO_Direction = gattCharacteristic;
                if (SampleGattAttributes.PIO_STATE_UUID.equals(uuid))
                    PIO_State = gattCharacteristic;
                if (SampleGattAttributes.AIO_READ_UUID.equals(uuid)) AIO_Read = gattCharacteristic;
            }
        }
    }

    BluetoothGattCharacteristic UART_Read;
    BluetoothGattCharacteristic UART_Write;
    BluetoothGattCharacteristic PWM_Read_Write;
    BluetoothGattCharacteristic PIO_Read_Write;
    BluetoothGattCharacteristic PIO_State;
    BluetoothGattCharacteristic PIO_Direction;
    BluetoothGattCharacteristic AIO_Read;

    private static IntentFilter makeGattUpdateIntentFilter() {
        final IntentFilter intentFilter = new IntentFilter();

        intentFilter.addAction(BluetoothLeService.ACTION_DATA_AVAILABLE);
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED);

        intentFilter.addAction(BluetoothLeService.ACTION_GATT_CONNECTED);
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_DISCONNECTED);
        //intentFilter.addAction(BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED);

        return intentFilter;
    }


    // Demonstrates how to iterate through the supported GATT Services/Characteristics.
    // In this sample, we populate the data structure that is bound to the ExpandableListView
    // on the UI.
    private void discoveredGattServices(List<BluetoothGattService> gattServices) {
        if (gattServices == null) {
            Log.d(TAG, "TEST discoveredGattServices : gattServices is null");
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

    /*private void discoveredGattServices2(List<BluetoothGattService> gattServices) {
        if (gattServices == null) return;
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
    }*/
    //private static boolean isConnected = false;


}
