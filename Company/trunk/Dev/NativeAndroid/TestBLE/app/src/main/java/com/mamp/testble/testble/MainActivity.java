package com.mamp.testble.testble;

import android.app.Activity;
import android.app.Fragment;
import android.app.FragmentManager;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattService;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.chipsen.bleservice.BluetoothLeService;

import java.util.List;

/**
 * Created by jangyunhwan on 2017-11-07.
 */

public class MainActivity extends Activity {

    private final static String TAG = "Test_MainActivity";
    private static final String LOG_TAG = "Test_Log";
    //private Button button;
    //private TextView textView;

    // Member fields
    //private BluetoothAdapter mBtAdapter;

    //private BluetoothAdapter mBluetoothAdapter;

    private final int REQUEST_ENABLE_BT = 2020;

    private BLEManager bleManager;

    private static boolean isConnected = false;

    static final int PICK_DEVICE_REQUEST = 0;

    private BluetoothLeService mBluetoothLeService;

    public static Context Mcontext;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main_activity);

        // Use this check to determine whether BLE is supported on the device. Then
        // you can selectively disable BLE-related features.

        Mcontext=this;
        if(bleManager == null){
            bleManager = new BLEManager(this);
        }

        if(!bleManager.hasBluetooth(this)) {
            Toast.makeText(this, "BLE를 지원하지 않습니다.",Toast.LENGTH_SHORT).show();
            finish();
        }

        if (!bleManager.isCheckedOnBLE()) {
            Log.d("Test", "Adapter is null or mBluetoothAdapter.isEnabled() is false");
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
        }

        ((Button) findViewById(R.id.button_terminal)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Create a new fragment and specify the planet to show based on position

                //Fragment fragment = Fragment.instantiate(MainActivity.this, fragments[position]);
                Fragment fragment = Fragment.instantiate(MainActivity.this, "com.mamp.testble.testble.ble_terminal");

                // Insert the fragment by replacing any existing fragment
                FragmentManager fragmentManager = getFragmentManager();
                fragmentManager.beginTransaction()
                        .replace(R.id.content_frame, fragment)
                        .commit();
            }
        });

        ((Button) findViewById(R.id.button)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(final View v) {

                if(v instanceof Button){

                    if (isConnected) {
                        mBluetoothLeService.disconnect();
                    }

                    //Intent intent = new Intent(this, DeviceScanActivity.class);
                    Intent intent = new Intent(MainActivity.this, BleActivity.class);
                    startActivityForResult(intent,PICK_DEVICE_REQUEST);


                    /*
                    if(isConnected) {

                    }
                    ((Button) v).setText("Stop_Button");
                    bleManager.scanLeDevice(true, new BLEManager.ScanLeDeviceCallBack() {
                        @Override
                        public void finished() {
                            ((Button) v).setText("Scan_Button");
                        }
                    });
                    */
                }
                // Register for broadcasts when a device is discovered
                //IntentFilter filter = new IntentFilter(BluetoothDevice.ACTION_FOUND);
                //registerReceiver(mReceiver, filter);


                /*
                Log.d("Test", "Clicked scan Button");
                if(mBtAdapter != null) {
                    Log.d("Test", "devices : " + mBtAdapter.getBondedDevices().toString());
                }
                if(mBluetoothAdapter!= null) {
                    //mBluetoothAdapter.getRemoteDevice()
                }
                */
            }
        });

        Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++2_2");
        // Start service
        //블루투스 서비스가 시작된다.(폰에서 자동으로 블루투스를 켜고 시작한다.)
        //없으면 앱이 실행되지 않는다.
        Intent gattServiceIntent = new Intent(this, BluetoothLeService.class);
        Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++3");
        bindService(gattServiceIntent, mServiceConnection, BIND_AUTO_CREATE);
        Log.e(LOG_TAG, "parkNavigationActivity =+++ ON CREATE +++4");
        /*
        final Set<BluetoothDevice> devices = mBluetoothAdapter.getBondedDevices();
        mBluetoothAdapter.startLeScan(new BluetoothAdapter.LeScanCallback() {
            @Override
            public void onLeScan(BluetoothDevice device, int rssi, byte[] scanRecord) {
                //if(device.getName().equals("TX_BLE2")) {
                //if(device.getName().equals("RX_BLE2")) {
                    Log.d("Test", "Start");
                    Log.d("Test", "1device.getName() : " + device.getName());
                    Log.d("Test", "1device.getUuids() : " + device.getUuids());
                    Log.d("Test", "1device.getUuids().length : " + device.getUuids().length);
                    Log.d("Test", "1device.getUuids().toString() : " + device.getUuids().toString());
                    Log.d("Test", "1device.getAddress() : " + device.getAddress());


                    Log.d("Test", "1rssi : " + rssi);
                    //for (byte b :scanRecord) {
                    //    Log.d("Test", "scanRecord : " + b);
                    //}

                    //BluetoothDevice device = btAdapter.getRemoteDevice(address);
                    BluetoothDevice device_ = mBluetoothAdapter.getRemoteDevice(device.getAddress());
                    Log.d("Test", "1device.getName() : " + device.getName());
                    Log.d("Test", "device_.getUuids(); : " + device_.getUuids());
                    Log.d("Test", "end");

                //connect(device);



              //  }
            }
        });


        if(mBluetoothAdapter.isDiscovering()) {
            mBluetoothAdapter.cancelDiscovery();
        }

        //for(BluetoothDevice device : devices) {
        //    Log.d("Test", "device.getName() : " + device.getName());
        //    Log.d("Test", "device.toString() : " + device.toString());
        //    Log.d("Test", "device.getAddress() : " + device.getAddress());
        //}



        // Register for broadcasts when a device is discovered
        IntentFilter filter = new IntentFilter(BluetoothDevice.ACTION_FOUND);
        this.registerReceiver(mReceiver, filter);

        mBtAdapter = BluetoothAdapter.getDefaultAdapter();
        */

    }
    private String mDeviceName;
    private String mDeviceAddress;

    public static final String EXTRAS_DEVICE_NAME = "DEVICE_NAME";
    public static final String EXTRAS_DEVICE_ADDRESS = "DEVICE_ADDRESS";

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == PICK_DEVICE_REQUEST) {
            if (resultCode == RESULT_OK) {
                // A contact was picked.  Here we will just display it
                // to the user.
                mDeviceName = data.getStringExtra(EXTRAS_DEVICE_NAME);
                mDeviceAddress = data.getStringExtra(EXTRAS_DEVICE_ADDRESS);
                Toast.makeText(this, "디바이스 이름" + ": "+mDeviceName +" - "+ mDeviceAddress + "\n연결되었습니다.", Toast.LENGTH_LONG).show();
            }
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.d("Test", "onResume / registerReceiver");
        registerReceiver(mGattUpdateReceiver, makeGattUpdateIntentFilter());
        Log.d("Test", "onResume / registerReceiver mGattUpdateReceiver is : " + mGattUpdateReceiver);
        Log.d("Test", "onResume / registerReceiver mGattUpdateReceiver.getResultData() is : " + mGattUpdateReceiver.getResultData());
        if (mBluetoothLeService != null) {
            final boolean result = mBluetoothLeService.connect(mDeviceAddress);
            Log.d(TAG, "Connect request result=" + result);
        }



    }

    @Override
    protected void onPause() {
        super.onPause();
        unregisterReceiver(mGattUpdateReceiver);

    }

    private static IntentFilter makeGattUpdateIntentFilter() {
        final IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_CONNECTED);
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_DISCONNECTED);
        intentFilter.addAction(BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED);
        return intentFilter;
    }

    /*
    private final BroadcastReceiver mReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            Log.d("Test", "intent : " + intent);

            String action = intent.getAction();

            // When discovery finds a device
            if (BluetoothDevice.ACTION_FOUND.equals(action)) {
                // Get the BluetoothDevice object from the Intent
                BluetoothDevice device = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);

                Log.d("Test", "device1 : " + device.toString());
                Log.d("Test", "device2 : " + device.getAddress());
                // If it's already paired, skip it, because it's been listed already

                //if (device.getBondState() != BluetoothDevice.BOND_BONDED) {
                //    mNewDevicesArrayAdapter.add(device.getName() + "\n" + device.getAddress());
                //}

                // When discovery is finished, change the Activity title
            }
        }
    };
    */

    @Override
    protected void onDestroy() {
        super.onDestroy();
        unbindService(mServiceConnection);
        mBluetoothLeService = null;

        android.os.Process.killProcess(android.os.Process.myPid());
    }

    // Code to manage Service lifecycle.
    private final ServiceConnection mServiceConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName componentName, IBinder service) {
            Log.d(TAG, "Navig_Service Connected");



            mBluetoothLeService = ((BluetoothLeService.LocalBinder) service).getService();
            if (!mBluetoothLeService.initialize()) {
                Log.e(TAG, "Navig_Unable to initialize Bluetooth");
               finish();
            }
            // Automatically connects to the device upon successful start-up initialization.
            mBluetoothLeService.connect(mDeviceAddress);
        }

        @Override
        public void onServiceDisconnected(ComponentName componentName) {
            mBluetoothLeService = null;
        }
    };

    private final BroadcastReceiver mGattUpdateReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            Log.d("Test", "Dbg_Navig14");
            Log.e(TAG, "Dbg_Navig14");
            final String action = intent.getAction();
            Log.e(TAG, "Dbg_Navig_mGattUpdateReceiver:"+action);


            if (BluetoothLeService.ACTION_GATT_CONNECTED.equals(action)) {
                isConnected = true;
                //imageView_search.setImageResource(R.drawable.search_f);
                Log.d("Test", "onReceive _ Search_F");

            } else if (BluetoothLeService.ACTION_GATT_DISCONNECTED.equals(action)) {
                isConnected = false;
                //imageView_search.setImageResource(R.drawable.search);
                Log.d("Test", "onReceive _ Search");

            }
            else if (BluetoothLeService.ACTION_GATT_SERVICES_DISCOVERED.equals(action)) {
                // Show all the supported services and characteristics on the user interface.
                //서비스를 제공하고 있는지를 보여준다.
                discoveredGattServices(mBluetoothLeService.getSupportedGattServices());
                discoveredGattServices2(mBluetoothLeService.getSupportedGattServices());
            }


        }
    };


    // Demonstrates how to iterate through the supported GATT Services/Characteristics.
    // In this sample, we populate the data structure that is bound to the ExpandableListView
    // on the UI.
    private void discoveredGattServices(List<BluetoothGattService> gattServices) {
        if (gattServices == null) return;
        String uuid = null;

        // Loops through available GATT Services.
        for (BluetoothGattService gattService : gattServices)
        {
            uuid = gattService.getUuid().toString();
            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();

            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics)
            {
                uuid = gattCharacteristic.getUuid().toString();
            }
        }
    }

    private void discoveredGattServices2(List<BluetoothGattService> gattServices) {
        if (gattServices == null) return;
        String uuid = null;

        // Loops through available GATT Services.
        for (BluetoothGattService gattService : gattServices)
        {
            uuid = gattService.getUuid().toString();
            List<BluetoothGattCharacteristic> gattCharacteristics = gattService.getCharacteristics();

            // Loops through available Characteristics.
            for (BluetoothGattCharacteristic gattCharacteristic : gattCharacteristics)
            {
                uuid = gattCharacteristic.getUuid().toString();
            }
        }
    }

}
