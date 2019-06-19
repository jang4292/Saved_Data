package com.mamp.testble.testble;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothManager;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Handler;
import android.util.Log;

/**
 * Created by jangyunhwan on 2017-11-08.
 */

public class BLEManager {


    private Activity mActivity;
    private BluetoothAdapter bluetoothAdapter;

    private String bluetoothDeviceAddress;


    public BLEManager(Activity activity) {
        //mContext = context;
        mActivity = activity;
    }

    private boolean mScaning = false;
    public interface ScanLeDeviceCallBack{
        public void finished();
    };

    private static final long SCAN_PERIOD = 10000;
    public void scanLeDevice(final boolean enable, final ScanLeDeviceCallBack callBack) {
        if (enable) {
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    StartLeScan(false);
                    if(callBack!= null)  {
                        callBack.finished();
                    }
                }
            }, SCAN_PERIOD);

            StartLeScan(true);
        } else {
            StartLeScan(false);
        }
    }

    private void StartLeScan(boolean isStarted) {
        if(isStarted) {
            mScaning = true;
            Log.d("Test", "StartLeScan");
            getBluetoothAdapter().startLeScan(mLeScanCallback);
            //mBluetoothAdapter.startLeScan(mLeScanCallback);
        } else {
            mScaning = false;
            Log.d("Test", "StopLeScan");
            getBluetoothAdapter().stopLeScan(mLeScanCallback);
            //mBluetoothAdapter.stopLeScan(mLeScanCallback);
        }
    }

    //private LeDeviceListAdapter mLeDeviceListAdapter;

    // Device scan callback.
    private BluetoothAdapter.LeScanCallback mLeScanCallback =
            new BluetoothAdapter.LeScanCallback() {
                @Override
                public void onLeScan(final BluetoothDevice device, int rssi,
                                     byte[] scanRecord) {
                    mActivity.runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            if(device != null) {
                            //Log.d("Test", "RunOnUIThread // device.device.getName() : " + device.getName());
                            //Log.d("Test", "RunOnUIThread // device.getUuids().length" + device.getUuids().length);
                            //Log.d("Test", "RunOnUIThread // device.getUuids().toString() : " + device.getUuids().toString());
                            //Log.d("Test", "RunOnUIThread // device.device.getAddress() : " + device.getAddress());
//                            mLeDeviceListAdapter.addDevice(device);
//                            mLeDeviceListAdapter.notifyDataSetChanged();
                                        if(bluetoothDeviceAddress == null) {
                                            if(device.getName() != null && device.getName().equals("TX_BLE2")) {
                                                bluetoothDeviceAddress = device.getAddress();
                                                BluetoothDevice device = bluetoothAdapter.getRemoteDevice(bluetoothDeviceAddress);
                                                device.connectGatt(mActivity, true, mBluetoothGattCallback);
                                    }
                                }
                            } else {
                                Log.d("Test", "Device is null");
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
    protected boolean hasBluetooth(Context context) {
        return context.getPackageManager().hasSystemFeature(PackageManager.FEATURE_BLUETOOTH_LE);
    }

    public BluetoothAdapter getBluetoothAdapter() {

        if(bluetoothAdapter == null) {
            final BluetoothManager bluetoothManager = (BluetoothManager) mActivity.getSystemService(Context.BLUETOOTH_SERVICE);
            bluetoothAdapter = bluetoothManager.getAdapter();
        }
        return bluetoothAdapter;
    }

    protected boolean isCheckedOnBLE(){
        return  bluetoothAdapter == null || !bluetoothAdapter.isEnabled();
        //Log.d("Test", "Adapter is null : " + (mBluetoothAdapter == null) + " or mBluetoothAdapter.isEnabled() : " + mBluetoothAdapter.isEnabled());
        // Ensures Bluetooth is available on the device and it is enabled. If not,
        // displays a dialog requesting user permission to enable Bluetooth.
        /*
        if (mBluetoothAdapter == null || !mBluetoothAdapter.isEnabled()) {
            Log.d("Test", "Adapter is null or mBluetoothAdapter.isEnabled() : " + mBluetoothAdapter.isEnabled());
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
        }
        */
    }

/*    private void makeBluetoothAdapter(Context context) {
        //final BluetoothManager bluetoothManager = (BluetoothManager) context.getSystemService(Context.BLUETOOTH_SERVICE);
        //mBluetoothAdapter = bluetoothManager.getAdapter();
    }

    // Initializes Bluetooth adapter.
    final BluetoothManager bluetoothManager = (BluetoothManager) getSystemService(Context.BLUETOOTH_SERVICE);
    mBluetoothAdapter = bluetoothManager.getAdapter();

        Log.d("Test", "Adapter is null : " + (mBluetoothAdapter == null) + " or mBluetoothAdapter.isEnabled() : " + mBluetoothAdapter.isEnabled());
    // Ensures Bluetooth is available on the device and it is enabled. If not,
    // displays a dialog requesting user permission to enable Bluetooth.
        if (mBluetoothAdapter == null || !mBluetoothAdapter.isEnabled()) {
        Log.d("Test", "Adapter is null or mBluetoothAdapter.isEnabled() : " + mBluetoothAdapter.isEnabled());
        Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
        startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
    }
    */


}
