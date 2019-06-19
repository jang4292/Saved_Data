package com.bpm.sensor.schedule;

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
import com.bpm.sensor.obj.ExObj.Type;
import com.bpm.sensor.obj.ScheduleObj;
import com.bpm.sensor.obj.ScheduleObj.DayOfWeek;

@WebServlet("/Schedule")
public class Schedule extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public Schedule() {
        super();
    }
    
    
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<ArrayList<ScheduleObj>> result = new ApiObj<>();
    	
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
    		// 일정 가져오기.
    		sendResponse(resp, result.ok(getSchedule(conn, memberId)));
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    }
    
    
    /**
     * 일정 가져오기.
     * @param conn
     * @param memberId
     * @return
     * @throws SQLException
     */
    private ArrayList<ScheduleObj> getSchedule(Connection conn, String memberId) throws SQLException {
    	ArrayList<ScheduleObj> list = new ArrayList<>();
    	String sql =
    			"SELECT id, type, day, count, weight, set_cnt, rest " +
    			"FROM schedule " +
    			"WHERE member_id = UNHEX(?) " +
    			"ORDER BY day ASC, pos ASC";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			while (rs.next()) {
    				ScheduleObj schedule = new ScheduleObj();
    				schedule.id = rs.getLong("id");
    				schedule.type = Type.findById(rs.getInt("type"));
    				schedule.day = DayOfWeek.findByCode(rs.getInt("day"));
    				schedule.count = rs.getInt("count");
    				schedule.weight = rs.getInt("weight");
    				schedule.setCnt = rs.getInt("set_cnt");
    				schedule.rest = rs.getInt("rest");
    				list.add(schedule);
    			}
    		}
    	}
    	return list;
    }

}
