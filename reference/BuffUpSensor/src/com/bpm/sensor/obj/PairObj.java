package com.bpm.sensor.obj;

import com.bpm.sensor.JsonObj;

public class PairObj<F, S> extends JsonObj {

	public F first;
	public S second;
	
	public String message;

	
	public PairObj(F first, S second) {
		this.first = first;
		this.second = second;
	}

	
	public PairObj() {
		
	}
}
