package com.mamp.paintbrush.gcm;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.drawable.BitmapDrawable;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.mamp.paintbrush.R;
import com.mamp.paintbrush.vo.PushVo;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerProxyActivity;

/**
 * Notification manager class.
 * @author Keisuke Kobayashi
 *
 */
public class UnityGCMNotificationManager {
	
	private static final String TAG = com.mamp.paintbrush.gcm.UnityGCMNotificationManager.class.getSimpleName();
	
	// Request code for launching unity activity
	private static final int REQUEST_CODE_UNITY_ACTIVITY = 1001;
	// ID of notification
	private static final int ID_NOTIFICATION = 1;
	
	/**
	 * Show notification view in status bar
	 */
	public static void showNotification(Context context, PushVo data) {
		Log.v(TAG, "showNotification");

		String contentTitle = data.title;
		String contentText = data.msg;

		// Intent 
		Intent intent = new Intent(context, UnityPlayerProxyActivity.class);
		PendingIntent contentIntent = PendingIntent.getActivity(context, REQUEST_CODE_UNITY_ACTIVITY, intent, PendingIntent.FLAG_UPDATE_CURRENT);
		
		//??Show notification in status bar
		NotificationCompat.Builder builder = new NotificationCompat.Builder(context.getApplicationContext());
		builder.setContentIntent(contentIntent);
		builder.setContentText(contentText);
		builder.setContentTitle(contentTitle);
		builder.setWhen(System.currentTimeMillis());
		builder.setAutoCancel(true);

		// LargeIcon Bitmap만들기
		Resources res = context.getResources();
		BitmapDrawable contactPicDrawable = (BitmapDrawable) res.getDrawable(R.drawable.app_icon); // 자신의 게임 아이콘명
		Bitmap contactPic = contactPicDrawable.getBitmap();

		int height = (int) res.getDimension(android.R.dimen.notification_large_icon_height);
		int width = (int) res.getDimension(android.R.dimen.notification_large_icon_width);
		contactPic = Bitmap.createScaledBitmap(contactPic, width, height, false);

		builder.setSmallIcon(res.getIdentifier("ic_stat_nagoyang", "drawable", context.getPackageName()));
		builder.setLargeIcon(contactPic);

		builder.setDefaults(Notification.DEFAULT_SOUND | Notification.DEFAULT_VIBRATE | Notification.DEFAULT_LIGHTS);
		
		NotificationManager nm = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);
		nm.notify(ID_NOTIFICATION, builder.build());
	}
	
	/**
	 * Show notification view in status bar
	 */
	public static void clearAllNotifications() {
		Log.v(TAG, "clearAllNotifications");
		
		NotificationManager nm = (NotificationManager) UnityPlayer.currentActivity.getSystemService(Context.NOTIFICATION_SERVICE);
		nm.cancelAll();
	}
	
}
