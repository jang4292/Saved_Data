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

@WebServlet("/ScheduleEdit")
public class ScheduleEdit extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ScheduleEdit() {
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
    	
    	// 수정될 일정 확인.
    	ScheduleObj schedule = fromJson(req, ScheduleObj.class);
    	if (isEmpty(schedule)) {
    		sendResponse(resp, result.fail("Empty_Schedule"));
    		return;
    	}
    	
    	// 일정 유효성 검사.
    	if (!schedule.isValid()) {
    		sendResponse(resp, result.fail("Invalid_Schedule"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 일정 업데이트.
    		if (!updateSchedule(conn, memberId, schedule)) {
    			sendResponse(resp, result.fail("Error_Update_Schedule"));
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
     * 일정 업데이트.
     * @param conn
     * @param memberId
     * @param schedule
     * @return
     * @throws SQLException
     */
    private boolean updateSchedule(Connection conn, String memberId, ScheduleObj schedule) throws SQLException {
    	String sql = "UPDATE schedule SET count = ?, weight = ?, set_cnt = ?, rest = ? WHERE id = ? AND member_id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setInt(1, schedule.count);
    		pstmt.setInt(2, schedule.weight);
    		pstmt.setInt(3, schedule.setCnt);
    		pstmt.setInt(4, schedule.rest);
    		pstmt.setLong(5, schedule.id);
    		pstmt.setString(6, memberId);
    		return pstmt.executeUpdate() > 0;
    	}
    }
    
}
