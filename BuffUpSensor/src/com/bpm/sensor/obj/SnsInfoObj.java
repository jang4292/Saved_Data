package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class SnsInfoObj extends JsonObj {

	public long id; // 아이디
	public String sns; // SNS명
	public String token; // 토큰 등의 식별 가능한 키
	
	
	public boolean isvalid() {
		return !isEmpty(sns) && !isEmpty(token);
	}
}
