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

@WebServlet("/Exercise/Month")
public class ExerciseMonth extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public ExerciseMonth() {
        super();
    }

    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<ArrayList<String>> result = new ApiObj<>();
    	
    	// 년/월 확인.
    	String year = req.getParameter("y");
    	String month = req.getParameter("m");
    	if (isEmpty(year, month)) {
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
    		// 해당 년월의 운동 일자 조회.
    		result.obj = queryMonthData(conn, memberId, year, month);
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	sendResponse(resp, result.ok());
    }
    
    /**
     * 해당 년월의 운동 일자 조회.
     */
    private ArrayList<String> queryMonthData(Connection conn, String memberId, String year, String month) throws SQLException {
    	ArrayList<String> list = new ArrayList<>();
    	String sql =
    			"SELECT DAY(created) AS d " + 
    			"FROM exercise " + 
    			"WHERE member_id = UNHEX(?) " +
    			"AND YEAR(created) = ? " + 
    			"AND MONTH(created) = ? " + 
    			"GROUP BY d";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		pstmt.setString(2, year);
    		pstmt.setString(3, month);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			while (rs.next()) {
    				list.add(rs.getString("d"));
    			}
    		}
    	}
    	return list;
    }
}
