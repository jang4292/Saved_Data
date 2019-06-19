package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class MemberObj extends JsonObj {

	public String id; // 아이디
	
	public EmailInfoObj emailInfo; // 이메일 정보
	public SnsInfoObj snsInfo; // sns 정보
	
	public PersonalInfoObj info; // 개인 정보
	
	
	public boolean hasSignUpInfo() {
		if (!isEmpty(emailInfo)) 
			return emailInfo.isvalid();
		if (isEmpty(snsInfo))
			return snsInfo.isvalid();
		return  false;
	}
	
	
	public boolean hasEmailInfo() {
		return !isEmpty(emailInfo) && emailInfo.isvalid();
	}
	
	
	public boolean hasSnsInfo() {
		return !isEmpty(snsInfo) && snsInfo.isvalid();
	}
	
	public boolean hasPersonalInfo() {
		return info != null && info.isValid();
	}
}
