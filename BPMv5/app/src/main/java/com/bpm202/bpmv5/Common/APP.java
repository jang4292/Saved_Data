package com.bpm202.bpmv5.Common;

import android.app.Application;

public class APP extends Application {

    private static String token;
    public static String getToken() {
        return token;
    }
    public static void setToken(String tokenStr) {
        token = tokenStr;
    }

}
