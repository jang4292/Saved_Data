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

@WebServlet("/ScheduleDelete")
public class ScheduleDelete extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ScheduleDelete() {
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
    		// 일정 삭제.
    		sendResponse(resp, result.ok(deleteSchedule(conn, memberId, schedule)));
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    }
    
    
    /**
     * 일정 삭제.
     * @param conn
     * @param memberId
     * @param schedule
     * @return
     * @throws SQLException
     */
    private boolean deleteSchedule(Connection conn, String memberId, ScheduleObj schedule) throws SQLException {
    	String sql =
    			"DELETE FROM schedule " +
    			"WHERE id = ? " +
    			"AND member_id = UNHEX(?) " +
    			"AND type = ? " +
    			"AND day = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setLong(1, schedule.id);
    		pstmt.setString(2, memberId);
    		pstmt.setInt(3, schedule.type.getId());
    		pstmt.setInt(4, schedule.day.getCode());
    		return pstmt.executeUpdate() > 0;
    	}
    }

}
