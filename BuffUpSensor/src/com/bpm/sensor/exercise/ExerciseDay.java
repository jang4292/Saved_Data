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

@WebServlet("/Exercise/Day")
public class ExerciseDay extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ExerciseDay() {
        super();
    }

    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<ArrayList<ExObj>> result = new ApiObj<>();
    	
    	// 인덱스 확인.
    	String date = req.getParameter("d");
    	if (isEmpty(date)) {
    		sendResponse(resp, result.fail("Invalid_Parameters"));
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
    		// 일별 운동 결과 조회.
    		result.obj = queryDayData(conn, memberId, date);
    	} catch (SQLException e) {
			showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	sendResponse(resp, result.ok());
    }
    
    /**
     * 일별 운동 결과 조회.
     */
    private ArrayList<ExObj> queryDayData(Connection conn, String memberId, String date) throws SQLException {
    	ArrayList<ExObj> list = new ArrayList<>();
    	String sql =
    			"SELECT id, type, count, count_max, duration, distance, angle, balance, set_cnt, set_max, rest, created " + 
    			"FROM exercise " +
    			"WHERE member_id = UNHEX(?) " +
    			"AND DATE(created) = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		pstmt.setString(2, date);
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
}
