package com.bpm.sensor.schedule;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.ScheduleObj;

@WebServlet("/ScheduleAdd")
public class ScheduleAdd extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ScheduleAdd() {
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
    	ScheduleObj schedule = fromJson(req, ScheduleObj.class);
    	if (schedule == null || !schedule.isValid()) {
    		sendResponse(resp, result.fail("Invalid_Schedule"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 일정 등록.
    		sendResponse(resp, result.ok(addSchedule(conn, memberId, schedule)));
    	} catch (SQLException e) {
			showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    }

    
    /**
     * 일정 등록.
     * @param conn
     * @param memberId
     * @param schedule
     * @return
     * @throws SQLException
     */
    private boolean addSchedule(Connection conn, String memberId, ScheduleObj schedule) throws SQLException {
    	String sql =
    			"INSERT INTO schedule " +
    			"(member_id, type, day, count, weight, set_cnt, rest) " +
    			"VALUES (UNHEX(?), ?, ?, ?, ?, ?, ?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		pstmt.setInt(2, schedule.type.getId());
    		pstmt.setInt(3, schedule.day.getCode());
    		pstmt.setInt(4, schedule.count);
    		pstmt.setInt(5, schedule.weight);
    		pstmt.setInt(6, schedule.setCnt);
    		pstmt.setInt(7, schedule.rest);
    		return pstmt.executeUpdate() > 0;
    	}
    }

}
