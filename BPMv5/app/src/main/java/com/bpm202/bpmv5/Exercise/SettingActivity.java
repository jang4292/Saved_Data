package com.bpm202.bpmv5.Exercise;

import android.app.AlertDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import com.bpm202.bpmv5.API.Api;
import com.bpm202.bpmv5.API.WithDrawAPI;
import com.bpm202.bpmv5.BaseActivity;
import com.bpm202.bpmv5.Common.AppPreferences;
import com.bpm202.bpmv5.Data.SignInRepository;
import com.bpm202.bpmv5.R;
import com.bpm202.bpmv5.SplashActivity;
import com.bpm202.bpmv5.Util.QToast;
import com.bpm202.bpmv5.ValueObject.ApiObj;

public class SettingActivity extends BaseActivity {

    public static final String TAG = SignInRepository.class.getSimpleName();

    private Button btn_password;
    private Button btn_my_infomation;
    private Button btn_rule;
    private Button btn_log_out;
    private Button btn_withdraw;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        initView();
        initListener();
    }

    private void initView() {
        setContentView(R.layout.activity_setting);

        btn_password = findViewById(R.id.btn_password);
        btn_my_infomation = findViewById(R.id.btn_my_infomation);
        btn_rule = findViewById(R.id.btn_rule);
        btn_log_out = findViewById(R.id.btn_log_out);
        btn_withdraw = findViewById(R.id.btn_withdraw);
    }

    private void initListener() {
        btn_password.setOnClickListener(null);
        btn_my_infomation.setOnClickListener(null);
        btn_rule.setOnClickListener(null);
        btn_log_out.setOnClickListener(onClickLogoutListener);
        btn_withdraw.setOnClickListener(onClickWithDrawListener);
    }

    private View.OnClickListener onClickLogoutListener = v -> {
        new AppPreferences(getApplicationContext()).setStringPref(AppPreferences.KEY_TOKEN, "");
        activityFinishToSplash();
/*        Intent intent = new Intent(mContext, LoginActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        mContext.startActivity(intent);*/

    };

    private View.OnClickListener onClickWithDrawListener = v -> {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(R.string.popup_title_secession);
        builder.setMessage(R.string.popup_contents_secession);
        builder.setPositiveButton(android.R.string.ok, (dialog, which) -> {
            String token = new AppPreferences(getApplicationContext()).getStringPref(AppPreferences.KEY_TOKEN);
            Log.d(TAG, "TEST TOKEN : " + token);
            //SignInApi.secession(token,
            WithDrawAPI.secession(token,
                    callback -> {
                        ApiObj apiObj = (ApiObj) callback;
                        if (apiObj.status.equals(Api.STATUS_OK)) {
                            QToast.showToast(getApplicationContext(), R.string.menu_secession);
                            //showToast(R.string.menu_secession);
                            dialog.dismiss();
                            activityFinishToSplash();
                        }
                    });
        });
        builder.setNegativeButton(android.R.string.no, (dialog, which) -> {
            dialog.dismiss();
        });

        AlertDialog dialog = builder.create();
        dialog.show();
    };

    private void activityFinishToSplash() {
        Intent intent = new Intent(this, SplashActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK | Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivity(intent);
    }
}
