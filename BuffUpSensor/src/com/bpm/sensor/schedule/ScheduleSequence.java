package com.bpm.sensor.schedule;

import java.io.IOException;
import java.lang.reflect.Type;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.ScheduleObj;
import com.google.gson.reflect.TypeToken;

@WebServlet("/ScheduleSequence")
public class ScheduleSequence extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ScheduleSequence() {
        super();
    }


    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	ApiObj<Boolean> result = new ApiObj<>();
    	
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
    	
    	// 일정 확인.
    	Type type = new TypeToken<ArrayList<ScheduleObj>>() {}.getType();
    	ArrayList<ScheduleObj> schedules = fromJson(req, type);
    	if (isEmpty(schedules) || schedules.isEmpty()) {
    		sendResponse(resp, result.fail("Empty_Schedules"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 일정 순서 업데이트.
    		for (ScheduleObj schedule : schedules) {
    			updateSequence(conn, memberId, schedule);
    		}
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	sendResponse(resp, result.ok(true));
    }
    
    
    /**
     * 일정 순서 업데이트.
     * @param conn
     * @param memberId
     * @param schedule
     * @throws SQLException
     */
    private void updateSequence(Connection conn, String memberId, ScheduleObj schedule) throws SQLException {
    	String sql = "UPDATE schedule SET pos = ? WHERE id = ? AND member_id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setInt(1, schedule.pos);
    		pstmt.setLong(2, schedule.id);
    		pstmt.setString(3, memberId);
    		pstmt.executeUpdate();
    	}
    }
    
}
