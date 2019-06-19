package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class PersonalInfoObj extends JsonObj {

	public String photo; // 사진
	public String nickname; // 별명
	public int height; // 키
	public int weight; // 몸무게
	public int age; // 나이
	public String gender; // 성별 (남: M / 여: F)
	public int region; // 지역
	
	
	public boolean isValid() {
		if (isEmpty(nickname))
			return false;
		if (height < 100)
			return false;
		if (weight < 10)
			return false;
		if (age < 1) {
			return false;
		}
		if (isEmpty(gender))
			return false;
		if (region < 1)
			return false;
		return true;
	}
}
