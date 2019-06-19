package com.mamp.paintbrush.gcm;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.google.android.gcm.GCMBaseIntentService;
import com.mamp.paintbrush.vo.PushVo;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Set;

/**
 * GCMIntentService.<br>
 * For each callback, this class sends message to GameObject via UnitySendMessage.
 * @author Keisuke Kobayashi
 *
 */
public class UnityGCMIntentService extends GCMBaseIntentService {

	private static final String TAG = UnityGCMIntentService.class.getSimpleName();

	private static final String ON_ERROR = "OnError";
	private static final String ON_MESSAGE = "OnMessage";
	private static final String ON_REGISTERED = "OnRegistered";
	private static final String ON_UNREGISTERED = "OnUnregistered";
	
	private static final String ON_DELETE_MESSAGES = "OnDeleteMessages";

	@Override
	protected void onError(Context context, String errorId) {
		Log.v(TAG, "onError");
		com.mamp.paintbrush.gcm.Util.sendMessage(ON_ERROR, errorId);
	}

	@Override
	protected void onMessage(Context context, Intent intent) {
		Log.v(TAG, "onMessage");
		// Notify to C# layer
		Bundle bundle = intent.getExtras();
		Set<String> keys = bundle.keySet();
		JSONObject json = new JSONObject();
		try {
			for (String key : keys) {
				Log.v(TAG, key + ": " + bundle.get(key));
				json.put(key, bundle.get(key));
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}

		if (!com.mamp.paintbrush.gcm.Util.notificationsEnabled) {
			return;
		}

		// Show native notification view in status bar if defined fields are put.

		try {
			String msg = json.getString("msg");
			PushVo vo = (PushVo)new PushVo().fromJson(msg);
			UnityGCMNotificationManager.showNotification(this, vo);
		} catch (JSONException e) {
			// Title is mandatory, do not display in status bar
			Log.v(TAG, "No content_title specified, not showing anything in Android status bar");
		}
	}

	@Override
	protected void onRegistered(Context context, String registrationId) {
		Log.v(TAG, "onRegistered");
		com.mamp.paintbrush.gcm.Util.sendMessage(ON_REGISTERED, registrationId);
	}

	@Override
	protected void onUnregistered(Context context, String registrationId) {
		Log.v(TAG, "onUnregistered");
		com.mamp.paintbrush.gcm.Util.sendMessage(ON_UNREGISTERED, registrationId);
	}

	@Override
	protected void onDeletedMessages (Context context, int total) {
		Log.v(TAG, "onDeleteMessages");
		Util.sendMessage(ON_DELETE_MESSAGES, Integer.toString(total));
	}
}
