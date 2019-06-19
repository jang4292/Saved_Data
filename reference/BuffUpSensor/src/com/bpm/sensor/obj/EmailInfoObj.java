package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class EmailInfoObj extends JsonObj {

	public long id; // 아이디
	public String email; // 이메일 주소
	public String password; // 이메일 로그인 패스워드
	
	
	public boolean isvalid() {
		return !isEmpty(email) && !isEmpty(password);
	}
}
