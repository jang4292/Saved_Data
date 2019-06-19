package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.MemberObj;
import com.bpm.sensor.obj.PersonalInfoObj;

@WebServlet("/SignInToken")
public class SignInToken extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public SignInToken() {
        super();
    }


    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	
    	ApiObj<MemberObj> result = new ApiObj<>();
    	MemberObj member = null;
    	
    	// Token 확인
    	Map<String, String> map = getMemberFromToken(req);
    	if (!isEmpty(map.get("error"))) {
    		sendResponse(resp, result.fail(map.get("error")));
    		return;
    	}
    	String memberId = map.get("member");
    	if (isEmpty(memberId)) {
    		sendResponse(resp, result.fail("Empty_Member"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// Token 로그인
    		member = signIn(conn, memberId);
    	} catch (SQLException e) {
    		showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	if (isEmpty(member) || !member.hasPersonalInfo()) {
    		sendResponse(resp, result.fail("Error_Member_Info"));
    		return;
    	}
    	
    	sendResponse(resp, result.ok(member));
    }
    
    
    /**
     * Token 로그인
     * @param conn
     * @param memberId
     * @return
     * @throws SQLException
     */
    private MemberObj signIn(Connection conn, String memberId) throws SQLException {
    	MemberObj member = new MemberObj();
    	String sql = 
    			"SELECT photo, nickname, height, weight, age, gender, region_id " +
    			"FROM member_info " +
    			"WHERE member_id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (rs.next()) {
    				member.info = new PersonalInfoObj();
    				member.info.photo = rs.getString("photo");
    				member.info.nickname = rs.getString("nickname");
    				member.info.height = rs.getInt("height");
    				member.info.weight = rs.getInt("weight");
    				member.info.age = rs.getInt("age");
    				member.info.gender = rs.getString("gender");
    				member.info.region = rs.getInt("region_id");
    			}
    		}
    	}
    	return member;
    }
    
}
