package com.bpm.sensor.exercise;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.ExObj;
import com.bpm.sensor.obj.ExObj.Type;

@WebServlet("/Exercise")
public class Exercise extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public Exercise() {
        super();
    }
    

    /**
     * 운동 기록 조회.
     */
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<ArrayList<ExObj>> result = new ApiObj<>();
    	
    	// 인덱스 확인.
    	long index = toLong(req.getParameter("index"), 0);
    	if (index < 0) {
    		sendResponse(resp, result.fail("Invalid_Index"));
    		return;
    	}
    	
    	// Token 확인.
    	Map<String, String> map = getMemberFromToken(req);
    	String err = map.get("error");
    	if (!isEmpty(err)) {
    		sendResponse(resp, result.fail(err));
    		return;
    	}
    	// 아이디 확인.
    	String memberId = map.get("member");
    	if (isEmpty(memberId)) {
    		sendResponse(resp, result.fail("Empty_Member"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 운동 결과 조회.
    		result.obj = getExInfo(conn, memberId, index);
    	} catch (SQLException e) {
			showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	sendResponse(resp, result.ok());
    }
    
    
    /**
     * 운동 결과 조회.
     * @param conn
     * @param memberId
     * @param index
     * @return
     * @throws SQLException
     */
    private ArrayList<ExObj> getExInfo(Connection conn, String memberId, long index) throws SQLException {
    	ArrayList<ExObj> list = new ArrayList<>();
    	String sql =
    			"SELECT id, type, count, count_max, duration, distance, angle, balance, set_cnt, set_max, rest, created " +
    			"FROM exercise " +
    			"WHERE member_id = UNHEX(?) " +
    			"AND id < ? " +
    			"ORDER BY id DESC " +
    			"LIMIT 20";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		pstmt.setLong(2, index < 1 ? Long.MAX_VALUE : index);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			while (rs.next()) {
    				ExObj obj = new ExObj();
    				obj.id = rs.getLong("id");
    				obj.type = Type.findById(rs.getInt("type"));
    				obj.count = rs.getInt("count");
    				obj.countMax = rs.getInt("count_max");
    				obj.duration = rs.getInt("duration");
    				obj.distance = rs.getInt("distance");
    				obj.angle = rs.getInt("angle");
    				obj.balance = rs.getInt("balance");
    				obj.setCnt = rs.getInt("set_cnt");
    				obj.setMax = rs.getInt("set_max");
    				obj.rest = rs.getInt("rest");
    				obj.created = rs.getString("created");
    				list.add(obj);
    			}
    		}
    	}
    	return list;
    }

    
    /**
     * 운동 결과 등록.
     */
    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	ApiObj<Boolean> result = new ApiObj<>();
    	
    	// 운동 정보.
    	ExObj ex = fromJson(req, ExObj.class);
    	// 운동 정보 확인.
    	if (ex.type == null) {
    		sendResponse(resp, result.fail("Empty_Type"));
    		return;
    	}
    	if (!ex.isValid()) {
    		sendResponse(resp, result.fail("Invalid_Ex"));
    		return;
    	}
    	
    	// Token 확인.
    	Map<String, String> map = getMemberFromToken(req);
    	String err = map.get("error");
    	if (!isEmpty(err)) {
    		sendResponse(resp, result.fail(err));
    		return;
    	}
    	// 아이디 확인.
    	String memberId = map.get("member");
    	if (isEmpty(memberId)) {
    		sendResponse(resp, result.fail("Empty_Member"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		if (!updateExercise(conn, memberId, ex)) {
    			sendResponse(resp, result.fail("Fail_Update"));
        		return;
    		}
    	} catch (SQLException e) {
    		showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	sendResponse(resp, result.ok(true));
    }

    
    /**
     * 운동결과 등록.
     * @param conn
     * @param memberId
     * @param ex
     * @return
     * @throws SQLException
     */
    private boolean updateExercise(Connection conn, String memberId, ExObj ex) throws SQLException {
    	String sql =
    			"INSERT INTO exercise " +
    			"(member_id, type, count, count_max, weight, duration, distance, angle, balance, set_cnt, set_max, rest) " +
    			"VALUES (UNHEX(?), ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		pstmt.setInt(2, ex.type.getId());
    		pstmt.setInt(3, ex.count);
    		pstmt.setInt(4, ex.countMax);
    		pstmt.setInt(5, ex.weight);
    		pstmt.setInt(6, ex.duration);
    		pstmt.setInt(7, ex.distance);
    		pstmt.setInt(8, ex.angle);
    		pstmt.setInt(9, ex.balance);
    		pstmt.setInt(10, ex.setCnt);
    		pstmt.setInt(11, ex.setMax);
    		pstmt.setInt(12, ex.rest);
    		return pstmt.executeUpdate() > 0;
    	}
    }
    
}
