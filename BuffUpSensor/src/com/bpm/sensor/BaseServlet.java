package com.bpm.sensor;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Type;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.Part;

import com.google.gson.stream.JsonReader;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.CompressionCodecs;
import io.jsonwebtoken.ExpiredJwtException;
import io.jsonwebtoken.Jws;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.MalformedJwtException;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.SignatureException;
import io.jsonwebtoken.UnsupportedJwtException;

public abstract class BaseServlet extends HttpServlet {

	private static final long serialVersionUID = 1L;
	
	private static final String MY_SQL_URL = "jdbc:mysql://localhost:3306/buffupsensor?autoReconnect=true&useSSL=false";
	private static final String MY_SQL_USER = "root";
	private static final String MY_SQL_PASSWORD = "202dlrhddl";
	
	
	@Override
	public void init() throws ServletException {
		super.init();
		
		// DB
		loadDriver();
	}
	
	
	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doPut(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doDelete(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doHead(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doOptions(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	@Override
	protected void doTrace(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		setCharacterEncoding(req, resp);
	}
	
	
	/**
	 * 응답, 요청 설정.
	 * @param req
	 * @param resp
	 * @throws IOException
	 */
	private void setCharacterEncoding(HttpServletRequest req, HttpServletResponse resp) throws IOException {
		req.setCharacterEncoding("UTF-8");
		resp.setContentType("text/plain");
		resp.setCharacterEncoding("UTF-8");
	}
	
	
	/**
	 * 토큰 발행.
	 * @param resp
	 * @param member
	 * @throws IOException
	 */
	protected void setToken(HttpServletResponse resp, String member) throws IOException {
		String token = Jwts.builder()
    			.setSubject("token")
    			.claim("member", member)
    			.compressWith(CompressionCodecs.DEFLATE)
    			.signWith(SignatureAlgorithm.HS512, "key_sensor".getBytes("UTF-8"))
    			.compact();
    	resp.setHeader("Authorization", token);
	}
	
	
	/**
	 * 토큰 검증.
	 * @param req
	 * @return "member": "아이디", "error": "오류"</br>
	 * ClaimJwtException: JWT 권한claim 검사가 실패했을 때</br>
	 * ExpiredJwtException: 유효 기간이 지난 JWT를 수신한 경우</br>
	 * MalformedJwtException: 구조적인 문제가 있는 JWT인 경우</br>
	 * PrematureJwtException: 접근이 허용되기 전인 JWT가 수신된 경우</br>
	 * SignatureException: 시그너처 연산이 실패하였거나, JWT의 시그너처 검증이 실패한 경우</br>
	 * UnsupportedJwtException: 수신한 JWT의 형식이 애플리케이션에서 원하는 형식과 맞지 않는 경우.
	 * 예를 들어, 암호화된 JWT를 사용하는 애프리케이션에 암호화되지 않은 JWT가 전달되는 경우에 이 예외가 발생합니다.
	 */
	protected Map<String, String> getMemberFromToken(HttpServletRequest req) {
		Map<String, String> map = new HashMap<>();
		String token = req.getHeader("Authorization");
		if (isEmpty(token)) {
			map.put("error", "Empty_Token");
		} else {
			try {
				Jws<Claims> claims = Jwts.parser().setSigningKey("key_sensor".getBytes("UTF-8")).parseClaimsJws(token);
				map.put("member", (String) claims.getBody().get("member"));
			} catch (ExpiredJwtException | UnsupportedJwtException | MalformedJwtException | SignatureException
					| IllegalArgumentException | UnsupportedEncodingException e) {
				e.printStackTrace();
				map.put("error", e.getMessage());
			}
		}
		return map;
	}
	
	
	/**
	 * 응답.
	 * @param resp
	 * @param result
	 * @throws IOException
	 */
	protected void sendResponse(HttpServletResponse resp, ApiObj<?> result) throws IOException {
		try (PrintWriter writer = resp.getWriter()) {
			writer.println(result.toJson());
			writer.flush();
		}
	}
	
	
	/**
	 * DB.
	 */
	private void loadDriver() {
		try {
			Class.forName("com.mysql.jdbc.Driver").newInstance();			
		} catch (Exception e) {
			
		}
	}
	
	
	/**
	 * DB.
	 * @return
	 * @throws SQLException
	 */
	protected Connection getConnection() throws SQLException {
		return DriverManager.getConnection(MY_SQL_URL, MY_SQL_USER, MY_SQL_PASSWORD);
	}
	
	
	/**
	 * 요청에서 JSON 객체 가져오기.
	 * @param req
	 * @param classOfT
	 * @return
	 * @throws IOException
	 */
	protected <J extends JsonObj> J fromJson(HttpServletRequest req, Class<?> classOfT) throws IOException {
		try (BufferedReader br = req.getReader(); JsonReader jr = new JsonReader(br)) {
			if (jr.hasNext()) {
				return JsonObj.fromJson(jr, classOfT);
			}
		}
		return null;
	}
	
	
	/**
	 * 요청에서 JSON 객체 가져오기.
	 * @param req
	 * @param typeOfT
	 * @return
	 * @throws IOException
	 */
	@SuppressWarnings("hiding")
	protected <Object> Object fromJson(HttpServletRequest req, Type typeOfT) throws IOException {
		try (BufferedReader br = req.getReader(); JsonReader jr = new JsonReader(br)) {
			if (jr.hasNext()) {
				return JsonObj.fromJson(jr, typeOfT);
			}
		}
		return null;
	}
	
	
	/**
	 * 요청에서 JSON 객체 가져오기.
	 * @param part
	 * @param classOfT
	 * @return
	 * @throws IOException
	 */
	protected <J extends JsonObj> J fromJson(Part part, Class<?> classOfT) throws IOException {
		try (InputStreamReader isr = new InputStreamReader(part.getInputStream(), "UTF-8");
				BufferedReader br = new BufferedReader(isr);
				JsonReader jr = new JsonReader(br)) {
			if (jr.hasNext()) {
				return JsonObj.fromJson(jr, classOfT);
			}
		}
		return null;
	}
	
	
	/**
	 * 요청에서 JSON 객체 가져오기.
	 * @param part
	 * @param typeOfT
	 * @return
	 * @throws IOException
	 */
	@SuppressWarnings("hiding")
	protected <Object> Object fromJson(Part part, Type typeOfT) throws IOException {
		try (InputStreamReader isr = new InputStreamReader(part.getInputStream(), "UTF-8");
				BufferedReader br = new BufferedReader(isr);
				JsonReader jr = new JsonReader(br)) {
			if (jr.hasNext()) {
				return JsonObj.fromJson(jr, typeOfT);
			}
		}
		return null;
	}
	
	
	/**
	 * 빈 객체 체크.
	 * @param obj
	 * @return
	 */
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
	
	
	/**
	 * 빈 객체 체크.
	 * @param objs
	 * @return
	 */
	protected boolean isEmpty(Object... objs) {
		for (Object obj : objs) {
			if (isEmpty(obj)) {
				return true;
			}
		}
		return false;
	}
	
	
	/**
	 * 문자열을 숫자로 변환.
	 * @param str
	 * @param defaultValue
	 * @return
	 */
	protected int toInt(String str, int defaultValue) {
		try {
			return Integer.parseInt(str);
		} catch (Exception e) {
			return defaultValue;
		}
	}
	
	
	/**
	 * 문자열을 숫자로 변환.
	 * @param str
	 * @param defaultValue
	 * @return
	 */
	protected long toLong(String str, long defaultValue) {
		try {
			return Long.parseLong(str);
		} catch (Exception e) {
			return defaultValue;
		}
	}
	
	
	/**
	 * 문자열을 숫자로 변환.
	 * @param str
	 * @param defaultValue
	 * @return
	 */
	protected float toFloat(String str, float defaultValue) {
		try {
			return Float.parseFloat(str);
		} catch (Exception e) {
			return defaultValue;
		}
	}
	
	
	/**
	 * 메시지 출력.
	 * @param message
	 */
	protected void println(String message) {
		System.out.println(message);
	}
	
	
	/**
	 * SQL Exception 출력.
	 * @param e
	 */
	protected void showSqlException(SQLException e) {
		println("SQLException: " + e.getMessage());
	    println("SQLState: " + e.getSQLState());
	    println("VendorError: " + e.getErrorCode());	
	}
}
