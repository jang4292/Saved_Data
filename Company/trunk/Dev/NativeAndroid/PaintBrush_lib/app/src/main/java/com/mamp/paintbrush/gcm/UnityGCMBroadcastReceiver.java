package com.mamp.paintbrush.gcm;

import android.content.Context;
import android.util.Log;

import com.google.android.gcm.GCMBroadcastReceiver;

/**
 * Overrides GCMBroadcastReceiver to fix the class name of GCMIntentService
 * @author Keisuke Kobayashi
 *
 */
public class UnityGCMBroadcastReceiver extends GCMBroadcastReceiver {

	private static final String TAG = com.mamp.paintbrush.gcm.UnityGCMBroadcastReceiver.class.getSimpleName();
	
	private static final String SERVICE_NAME = "com.mamp.paintbrush.gcm.UnityGCMIntentService";
	
	/**
	 * Get the fixed name of subclass of GCMBaseIntentService
	 */
	@Override
	protected String getGCMIntentServiceClassName(Context context) {
		Log.v(TAG, "getGCMIntentServcieClassName");
		return SERVICE_NAME;
	}
}
