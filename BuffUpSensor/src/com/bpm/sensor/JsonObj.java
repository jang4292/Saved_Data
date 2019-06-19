package com.bpm.sensor;

import java.lang.reflect.Type;
import java.util.List;
import java.util.Map;

import com.google.gson.GsonBuilder;
import com.google.gson.stream.JsonReader;

public class JsonObj {

	
	public String toJson() {
		return new GsonBuilder().setPrettyPrinting().create().toJson(this);
	}
	
	
	@SuppressWarnings("unchecked")
	public static <J extends JsonObj> J fromJson(JsonReader jr, Class<?> classOfT) {
		return (J) new GsonBuilder().create().fromJson(jr, classOfT);
	}
	
	
	@SuppressWarnings("unchecked")
	public static <J extends JsonObj> J fromJson(String json, Class<?> classOfT) {
		return (J) new GsonBuilder().create().fromJson(json, classOfT);
	}
	
	
	@SuppressWarnings("hiding")
	public static <Object> Object fromJson(JsonReader jr, Type typeOfT) {
		return new GsonBuilder().create().fromJson(jr, typeOfT);
	}
	
	
	@SuppressWarnings("hiding")
	public static <Object> Object fromJson(String json, Type typeOfT) {
		return new GsonBuilder().create().fromJson(json, typeOfT);
	}
	
	
	private boolean isEmpty(Object obj) {
		if (obj == null) {
			return true;
		}
		if ((obj instanceof String) && ((String) obj).trim().length() == 0) {
			return true;
		}
		if (obj instanceof Map) {
			return ((Map<?, ?>) obj).isEmpty();
		}
		if (obj instanceof List) {
			return ((List<?>) obj).isEmpty();
		}
		if (obj instanceof Object[]) {
			return ((Object[]) obj).length == 0;
		}
		return false;
	}
	
	
	protected boolean isEmpty(Object... objs) {
		for (Object obj : objs) {
			if (isEmpty(obj)) {
				return true;
			}
		}
		return false;
	}
}
