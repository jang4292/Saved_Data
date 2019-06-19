package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class ExObj extends JsonObj {

	public long id; // 아이디
	public String member; // 회원
	public int count; // 횟수
	public int countMax; // 목표 횟수
	public int weight; // 무게
	public int duration; // 지속 시간(초)
	public int setCnt; // 현재 세트
	public int setMax; // 목표 세트
	public int rest; // 휴식
	public String created; // 시간
	
	public int distance; // 거리
	public int angle; // 각도
	public int balance; // 균형
	
	public Type type; // 운동 종류
	
	
	public boolean isValid() {
		return count > 0 && duration > 0;
	}


	public enum Type {
		PUSH_UP(1, "push_up", true, false, false), // distance
		PULL_UP(2, "pull_up", true, false, false), // distance
		CYCLE(3, "cycle", false, false, false),
		BARBELL_CURL(4, "barbell_curl", false, true, true), // angle, balance
		DUMBBELL_CURL(5, "dumbbell_curl", false, true, false), // angle
		SIT_UP(6, "sit_up", false, true, false), // angle
		SQUAT(7, "squat", false, true, false), // angle
		BARBELL_DEADLIFT(8, "barbell_deadlift", true, false, true), // distance, balance
		DUMBBELL_DEADLIFT(9, "dumbbell_deadlift", true, false, false), // distance
		BARBELL_ROW(10, "barbell_row", true, false, true), // distance, balance
		DUMBBELL_ROW(11, "dumbbell_row", true, false, false), // distance
		BARBELL_BENCH_PRESS(12, "barbell_bench_press", true, false, true), // distance, balance
		DUMBBELL_BENCH_PRESS(13, "dumbbell_bench_press", true, false, false), // distance
		BARBELL_SHOULDER_PRESS(14, "barbell_shoulder_press", true, false, true), // distance, balance
		DUMBBELL_SHOULDER_PRESS(15, "dumbbell_shoulder_press", true, false, false), // distance
		FRONT_LATERAL_RAISE(16, "front_lateral_raise", false, true, false), // angle
		SIDE_LATERAL_RAISE(17, "side_lateral_raise", false, true, false), // angle
		OVER_LATERAL_RAISE(18, "over_lateral_raise", false, true, false), // angle
		LAT_PULL_DOWN(19, "lat_pull_down", true, false, true), // distance, balance
		AB_SLIDE(20, "ab_slide", true, false, false) // distance
		;
		
		private int id;
		private String name;
		
		private boolean distance;
		private boolean angle;
		private boolean balance;
		
		Type(int id, String name, boolean distance, boolean angle, boolean balance) {
			this.id = id;
			this.name = name;
			this.distance = distance;
			this.angle = angle;
			this.balance = balance;
		}
		
		public int getId() {
			return this.id;
		}
		
		public String getName() {
			return this.name;
		}
		
		public boolean useDistance() {
			return this.distance;
		}
		
		public boolean useAngle() {
			return this.angle;
		}
		
		public boolean useBalance() {
			return this.balance;
		}
		
		public static Type findById(int id) {
			for (Type type : values()) {
				if (type.getId() == id) {
					return type;
				}
			}
			return null;
		}
		
		public static Type findByName(String name) {
			for (Type type : values()) {
				if (type.getName().equals(name)) {
					return type;
				}
			}
			return null;
		}
	}
}
