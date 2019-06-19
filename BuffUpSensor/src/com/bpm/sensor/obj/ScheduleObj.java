package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;
import com.bpm.sensor.obj.ExObj.Type;

public class ScheduleObj extends JsonObj {

	public long id; // 아이디
	public String member; // 회원
	public Type type; // 운동
	public DayOfWeek day; // 요일
	public int count; // 횟수
	public int weight; // 무게
	public int setCnt; // 세트
	public int pos; // 순서
	public int rest; // 휴식
	
	public boolean isValid() {
		return type != null && day != null && count > 0 && setCnt > 0;
	}
	
	
	public enum DayOfWeek {
		SUNDAY(1),
		MONDAY(2),
		TUESDAY(3),
		WEDNESDAY(4),
		THURSDAY(5),
		FRIDAY(6),
		SATURDAY(7)
		;
		
		private int code;
		
		DayOfWeek(int code) {
			this.code = code;
		}
		
		public int getCode() {
			return this.code;
		}
		
		public static DayOfWeek findByCode(int code) {
			for (DayOfWeek day : values()) {
				if (day.getCode() == code) {
					return day;
				}
			}
			return null;
		}
	}
}
