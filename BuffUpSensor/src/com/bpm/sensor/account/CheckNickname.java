package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;

@WebServlet("/CheckNickname")
public class CheckNickname extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public CheckNickname() {
        super();
    }
    
    
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<Boolean> result = new ApiObj<>();
    	
    	// 닉네임 확인.
    	String nickname = req.getParameter("nickname");
    	if (isEmpty(nickname)) {
    		sendResponse(resp, result.fail("Empty_Email"));
    		return;
    	}
    	
    	// 중복확인.
    	String sql = "SELECT HEX(member_id) AS member_id FROM member_info WHERE nickname = ?";
    	try (Connection conn = getConnection(); PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, nickname);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			result.obj = rs.next();
    		}
    	} catch (SQLException e) {
			showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	sendResponse(resp, result.ok());
    }

}
