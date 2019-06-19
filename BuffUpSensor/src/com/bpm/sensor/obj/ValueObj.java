package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class ValueObj<V> extends JsonObj {

	public V value;
	public String reason;

	
	public boolean isOk() {
		return value != null;
	}
	
	
	public ValueObj<V> setReason(String reason) {
		this.reason = reason;
		return this;
	}
}
