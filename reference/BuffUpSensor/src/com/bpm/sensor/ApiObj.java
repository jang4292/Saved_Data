package com.bpm.sensor;

import java.text.SimpleDateFormat;
import java.util.Date;

public class ApiObj<J> extends JsonObj {

	public J obj;
	public String message; // 오류 메시지
	public String status; // ok 또는 fail
	public String date; // 날짜
	
	
	public ApiObj<J> ok(J obj) {
		this.obj = obj;
		return this.ok();
	}
	
	
	public ApiObj<J> ok() {
		this.status = "ok";
		this.date = now();
		return this;
	}
	
	
	public ApiObj<J> fail(String message) {
		this.status = "fail";
		this.message = message;
		this.date = now();
		return this;
	}
	
	
	private String now() {
		return new SimpleDateFormat("yyyy.MM.dd HH:mm:ss").format(new Date());
	}
}
