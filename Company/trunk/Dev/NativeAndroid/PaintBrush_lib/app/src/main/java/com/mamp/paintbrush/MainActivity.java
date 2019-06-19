package com.mamp.paintbrush;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.app.AlertDialog;
import android.content.ComponentName;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.ServiceConnection;
import android.net.Uri;
import android.net.http.SslError;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.speech.tts.TextToSpeech;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.SslErrorHandler;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Toast;

import com.android.vending.billing.IInAppBillingService;
import com.mamp.paintbrush.inapp.IabHelper;
import com.mamp.paintbrush.inapp.IabResult;
import com.mamp.paintbrush.inapp.Inventory;
import com.mamp.paintbrush.inapp.Purchase;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.LinkedList;
import java.util.List;
import java.util.Locale;


/**
 *  notice
 *
 * DEV Google Analytics : https://developers.google.com/analytics/devguides/collection/android/v4/campaigns
 * Google Analytics : https://analytics.google.com/analytics/web/?hl=ko&pli=1#report/defaultid/a78413908w117413511p122811211/
 *
 */
public class MainActivity extends UnityPlayerActivity implements TextToSpeech.OnInitListener {

  // 텍스트 읽어주기
  private TextToSpeech tts;

  private String toastMessage;
  private String readText;
  private String imagePath;

  private IInAppBillingService mService;
  private IabHelper mHelper;

  private final String base64EncodiedPushedkey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMI" +
      "IBCgKCAQEAnWGJ/vQUD85RlsnHmC5zuAkMc0ghgvJoSZaEc3D4piQ3VKcSqqvbyB9OcoeGG+D" +
      "0aAFq729bm7rGWTsll67sNGfEsASAnvaSLj3A3JfEZ1ugUix965J97ggM/5HVbtdCUzFbqpLs" +
      "HHoSO0iQEHHQn5I65NY/hZTGZBZDgFpCgjGXzUX4ODvqQO68PhSBFZGHhugZBWeXQK74glb/5" +
      "QRnlPy4ELJLxgUtEetLNnH+oTGpRr9EwKdnrVAZ2TQPTualdJb6qotdbHGL5vYAStTjNJhUjK" +
      "0DE40dgL9GC2Ok3eUBTO5UqZSZKxfXHUbhkJf2EvMQ3NfXMNzsy2BcEjWJcQIDAQAB";

  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);

    inAppInit();
  }

  @Override
  protected void onResume() {
    super.onResume();
    tts = new TextToSpeech(getApplicationContext(), this);
  }

  @Override
  protected void onPause() {
    super.onPause();
    try {
      if (tts != null) {
        tts.stop();
      }
    } catch (Exception e) {

    }
  }

  @Override
  protected void onDestroy() {
    super.onDestroy();

    try {
      if (tts != null) {
        tts.shutdown();
        tts = null;
      }
    } catch (Exception e) {
      Log.d("TTS ERROR : ", e.getMessage());
    }

    if (mServiceConn != null) {
      unbindService(mServiceConn);
    }
  }

  @Override
  public void onInit(int status) {
  }

  private void inAppInit() {
    Intent intent = new Intent("com.android.vending.billing.InAppBillingService.BIND");
    intent.setPackage("com.android.vending.billing");

    bindService(intent, mServiceConn, Context.BIND_AUTO_CREATE);

    InAppInit_U(base64EncodiedPushedkey, true);
  }

  ServiceConnection mServiceConn = new ServiceConnection() {

    @Override
    public void onServiceConnected(ComponentName name, IBinder service) {
      mService = IInAppBillingService.Stub.asInterface(service);

  }

    @Override
    public void onServiceDisconnected(ComponentName name) {
      mService = null;
    }

  };

  private void InAppInit_U(String strPublicKey, boolean bDebug) {
    Log.d("myLog", "Creating IAB helper." + bDebug);
    mHelper = new IabHelper(this, strPublicKey);

    if (bDebug == true) {
      mHelper.enableDebugLogging(true, "IAB");
    }

    mHelper.startSetup(new IabHelper.OnIabSetupFinishedListener() {

      @Override
      public void onIabSetupFinished(IabResult result) {
        // TODO Auto-generated method stub
        boolean bInit = result.isSuccess();
        Log.d("myLog", "IAB Init " + bInit + result.getMessage());

        if (bInit == true) {
          Log.d("myLog", "Querying inventory.");

          mHelper.queryInventoryAsync(mGotInventoryListener);
        }
      }
    });
  }

  IabHelper.QueryInventoryFinishedListener mGotInventoryListener = new IabHelper.QueryInventoryFinishedListener() {
    public void onQueryInventoryFinished(IabResult result,
                                         Inventory inventory) {
      if (result.isFailure()) {
        Log.d("myLog", "Failed to query inventory: " + result);
        SendConsumeResult(null, result);
        return;
      }

            /*
             * Check for items we own. Notice that for each purchase, we check
             * the developer payload to see if it's correct! See
             * verifyDeveloperPayload().
             */

      List<String> inappList = inventory
          .getAllOwnedSkus(IabHelper.ITEM_TYPE_INAPP);

      for (String inappSku : inappList) {
        Purchase purchase = inventory.getPurchase(inappSku);
        Log.d("myLog", "Consumeing ... " + inappSku);
        mHelper.consumeAsync(purchase, mConsumeFinishedListener);
      }

      Log.d("myLog", "Query inventory was successful.");
    }
  };

  // InApp - 아이템 구매 (팝업)
  public void InAppBuyItem_U(final String strItemId, final String gamilName) {
    runOnUiThread(new Runnable() {

      @Override
      public void run() {
        // TODO Auto-generated method stub

                /*
                 * TODO: for security, generate your payload here for
                 * verification. See the comments on verifyDeveloperPayload()
                 * for more info. Since this is a SAMPLE, we just use an empty
                 * string, but on a production app you should carefully generate
                 * this.
                 */
        String payload = gamilName;

        mHelper.launchPurchaseFlow(MainActivity.this, strItemId,
            1001, mPurchaseFinishedListener, payload);

        Log.d("myLog", "InAppBuyItem_U " + strItemId);
      }
    });
  }

  // InApp - 구매 완료 시
  IabHelper.OnIabPurchaseFinishedListener mPurchaseFinishedListener = new IabHelper.OnIabPurchaseFinishedListener() {
    public void onIabPurchaseFinished(IabResult result, Purchase purchase) {
      Log.d("myLog", "Purchase finished: " + result + ", purchase: "
          + purchase);

      if (purchase != null) {
        // 결제 성공 시
        if (!verifyDeveloperPayload(purchase)) {
          Log.d("myLog",
              "Error purchasing. Authenticity verification failed.");
        }

        UnityPlayer.UnitySendMessage("StoreManager", "BuyUserMoney", purchase.getSku().substring(13));

        mHelper.consumeAsync(purchase, mConsumeFinishedListener);
      } else {
        // 결제 취소시
        Log.d("myLog", String.valueOf(result.getResponse()));

        Toast.makeText(MainActivity.this, "결제를 취소 하셨습니다.", Toast.LENGTH_SHORT).show();

        UnityPlayer.UnitySendMessage("StoreManager", "InAppPurchaseCancel", "cancel");
      }
    }
  };

  // InApp - 페이로드 확인
  boolean verifyDeveloperPayload(Purchase p) {
    String payload = p.getDeveloperPayload();
    Log.d("myLog", payload);

        /*
         * TODO: verify that the developer payload of the purchase is correct.
         * It will be the same one that you sent when initiating the purchase.
         *
         * WARNING: Locally generating a random string when starting a purchase
         * and verifying it here might seem like a good approach, but this will
         * fail in the case where the user purchases an item on one device and
         * then uses your app on a different device, because on the other device
         * you will not have access to the random string you originally
         * generated.
         *
         * So a good developer payload has these characteristics:
         *
         * 1. If two different users purchase an item, the payload is different
         * between them, so that one user's purchase can't be replayed to
         * another user.
         *
         * 2. The payload must be such that you can verify it even when the app
         * wasn't the one who initiated the purchase flow (so that items
         * purchased by the user on one device work on other devices owned by
         * the user).
         *
         * Using your own server to store and verify developer payloads across
         * app installations is recommended.
         */

    return true;
  }

  // InApp - 소비가 완료 될 때 통지
  IabHelper.OnConsumeFinishedListener mConsumeFinishedListener = new IabHelper.OnConsumeFinishedListener() {
    public void onConsumeFinished(Purchase purchase, IabResult result) {
      Log.d("myLog", "Consumption finished. Purchase: " + purchase
          + ", result: " + result);
      SendConsumeResult(purchase, result);
    }
  };

  // InApp - 소비 후 데이터 (영수증)
  protected void SendConsumeResult(Purchase purchase, IabResult result) {
    JSONObject jsonObj = new JSONObject();

    try {
      jsonObj.put("Result", result.getResponse());
      if (purchase != null) {
        jsonObj.put("OrderId", purchase.getOrderId());
        jsonObj.put("Sku", purchase.getSku());
        jsonObj.put("purchaseData", purchase.getOriginalJson());
        jsonObj.put("signature", purchase.getSignature());

        Log.d("myLog", "OrderId" + purchase.getOrderId());
        Log.d("myLog", "Sku" + purchase.getSku());
        Log.d("myLog", "purchaseData" + purchase.getOriginalJson());
        Log.d("myLog", "signature" + purchase.getSignature());
      }
    } catch (JSONException e) {
      // TODO Auto-generated catch block
      e.printStackTrace();
    }
  }

  public void onActivityResult(int requestCode, int resultCode, Intent data) {
    Log.d("myLog", "onActivityResult(" + requestCode + "," + resultCode
        + "," + data);
    if (requestCode == 1001) {
      // Pass on the activity result to the helper for handling
      if (!mHelper.handleActivityResult(requestCode, resultCode, data)) {
        // not handled, so handle it ourselves (here's where you'd
        // perform any handling of activity results not related to
        // in-app
        // billing...
        super.onActivityResult(requestCode, resultCode, data);
      } else {
        Log.d("myLog", "onActivityResult handled by IABUtil.");
      }
    }
  }

  // 유니티 - InApp 아이템 구매
  public void PutInAppBuyItem(String item, String gmailName) {
    InAppBuyItem_U(item, gmailName);
  }

  // 유니티 - InApp 결제 후 서버 동기화 실패 시 지메일로 문의
  public void PostQuestionGmail() {
    Intent it = new Intent(Intent.ACTION_SEND);
    it.setType("plain/text");
    it.putExtra(Intent.EXTRA_EMAIL, new String[]{"mamp.corp@gmail.com"});
    startActivity(it);
  }

  // 유니티 - Toast 메세지
  public void ToastMessage(String message) {
    this.toastMessage = message;
    mHandler.sendEmptyMessage(0);
  }

  // 유니티 - TTS 읽어주기 기능
  public void ReadingText(String readText) {
    this.readText = readText;
    mHandler.sendEmptyMessage(1);
  }

  // 유니티 - SNS 공유
  public void GetDrawingShareSNS(String imagePath) {
    this.imagePath = imagePath;
    mHandler.sendEmptyMessage(2);
  }

  // 유니티 - Gmail 아이디 불러오기
  public String GetUserGmailName() {
    AccountManager manager = AccountManager.get(MainActivity.this);
    Account[] accounts = manager.getAccountsByType("com.google");
    List<String> username = new LinkedList<String>();

    for (Account account : accounts) {
      username.add(account.name);
    }

    return username.get(0);
  }

  // 유니티 - 갤러리 사진 업데이트
  public void GetFileGalleryRefresh(String path) {
    sendBroadcast(new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, Uri.parse("file://" + path)));
  }

  public Handler mHandler = new Handler(new Handler.Callback() {
    @Override
    public boolean handleMessage(Message msg) {

      if (msg == null) {
        return false;
      } else {
        switch (msg.what) {
          case 0:
            Toast.makeText(getApplicationContext(), toastMessage, Toast.LENGTH_SHORT).show();
            break;
          case 1:
            try {
              tts.setLanguage(Locale.ENGLISH);
              tts.speak(readText, TextToSpeech.QUEUE_FLUSH, null);
              tts.speak(readText, TextToSpeech.QUEUE_ADD, null);
            } catch (Exception e) {
              ToastMessage(e.getMessage());
            }
            break;
          case 2:
            try {
              Intent intent = new Intent(Intent.ACTION_SEND);
              intent.setType("image/png");
              intent.putExtra(Intent.EXTRA_STREAM, Uri.parse(imagePath));

              startActivity(Intent.createChooser(intent, "공유"));
            } catch (Exception e) {
              ToastMessage(e.getMessage());
            }
            break;
          default:
            break;
        }
        return true;
      }
    }
  });

  private WebView webView;
  public void OpenNativeWebView(final String url) {
    runOnUiThread(new Runnable() {
      @Override
      public void run() {
        if(webView == null) {
          webView = new WebView(getApplicationContext());
          webView.getSettings().setJavaScriptEnabled(true);
          ViewGroup.LayoutParams layoutPrams = new ViewGroup.LayoutParams
                  (ViewGroup.LayoutParams.MATCH_PARENT,
                          ViewGroup.LayoutParams.MATCH_PARENT);
          getWindow().addContentView(webView, layoutPrams);
          webView.setWebViewClient(new MyWebViewClient());
        } else {
          webView.setVisibility(View.VISIBLE);
        }
        webView.loadUrl(url);
      }
    });
  }

  private String SERTIFICATE_ERROR = "인증서 오류";
  private String SSL_UNTRUSTED = "인증서를 신뢰할 수 없습니다.";
  private String EXPIRED = "인증서가 만료되었습니다";
  private String SSL_IDMISMATCH = "인증서 ID가 일치하지 않습니다.";
  private String SSL_NOTYETVALID = "인증서가 아직 유효하지 않습니다.";
  private String SSL_CONTINUE_MESSAGE = "계속 하시겠습니까?";
  private String SSL_CERTIFICATE_ERROR = "SSL 인증서 오류";

  private class MyWebViewClient extends WebViewClient {
    // 새로운 URL이 webview에 로드되려 할 경우 컨트롤을 대신할 기회를 줌
    @Override
    public boolean shouldOverrideUrlLoading(WebView view, String url) {
      view.loadUrl(url);
      return true;
    }

    @Override
    public void onReceivedSslError(WebView view, final SslErrorHandler handler, SslError error) {

      // 메시지 생성.
      StringBuilder message = new StringBuilder();
      message.append(SERTIFICATE_ERROR);

      message.append("\n");
      switch (error.getPrimaryError()) {
        case SslError.SSL_UNTRUSTED:
          message.append(SSL_UNTRUSTED);
          break;
        case SslError.SSL_EXPIRED:
          message.append(EXPIRED);
          break;
        case SslError.SSL_IDMISMATCH:
          message.append(SSL_IDMISMATCH);
          break;
        case SslError.SSL_NOTYETVALID:
          message.append(SSL_NOTYETVALID);
          break;
      }
      message.append("\n");
      message.append(SSL_CONTINUE_MESSAGE);

      new AlertDialog.Builder(MainActivity.this)
              .setTitle(SSL_CERTIFICATE_ERROR)
              .setMessage(message)
              .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                  handler.proceed();
                }
              })
              .setNegativeButton(android.R.string.cancel, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                  handler.cancel();
                }
              })
              .create().show();
    }
  }

  @Override
  public boolean onKeyDown(int keyCode, KeyEvent event) {
    if(keyCode == KeyEvent.KEYCODE_BACK) {
      if(webView != null && webView.getVisibility() == View.VISIBLE)  {
        webView.loadUrl("");
        webView.setVisibility(View.GONE);
        return true;
      }
    }
    return super.onKeyDown(keyCode,event);
  }
}
