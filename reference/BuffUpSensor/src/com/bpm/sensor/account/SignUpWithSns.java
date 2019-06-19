package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.MemberObj;

@WebServlet("/SignUpWithSns")
public class SignUpWithSns extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public SignUpWithSns() {
        super();
    }


    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	
    	ApiObj<MemberObj> result = new ApiObj<>();
    	
    	MemberObj member = fromJson(req, MemberObj.class);
    	
    	// 회원 확인.
    	if (isEmpty(member)) {
    		sendResponse(resp, result.fail("Empty_Member"));
    		return;
    	}
    	// SNS 확인.
    	if (!member.hasSnsInfo()) {
    		sendResponse(resp, result.fail("Invalid_SNS"));
    		return;
    	}
    	// 회원 정보 확인.
    	if (!member.hasPersonalInfo()) {
    		sendResponse(resp, result.fail("Invalid_Info"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 이메일 정보 생성하기.
    		if (!createSnsAuth(conn, member)) {
    			sendResponse(resp, result.fail("Error_Create_SNS_Auth"));
    			return;
    		}
    		// 생성된 이메일 아이디 가져오기.
    		if (!getSnsAuthId(conn, member)) {
    			sendResponse(resp, result.fail("Error_Get_SNS_Auth_ID"));
				return;
    		}
    		// 회원 아이디 생성하기.
    		if (!createMember(conn, member)) {
    			sendResponse(resp, result.fail("Error_Create_Member"));
    			return;
    		}
    		// 생성된 회원 아이디 가져오기.
    		if (!getMember(conn, member)) {
    			sendResponse(resp, result.fail("Error_Get_Member"));
				return;
    		}
    		// 회원 정보 생성하기.
    		if (!createMemberInfo(conn, member)) {
    			sendResponse(resp, result.fail("Error_Create_Member_Info"));
    			return;
    		}
    	} catch (SQLException e) {
    		showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	setToken(resp, member.id);
    	sendResponse(resp, result.ok(member));
    }
    
    
    /**
     * SNS 정보 생성하기.
     * @param conn
     * @param member
     * @return
     * @throws SQLException
     */
    private boolean createSnsAuth(Connection conn, MemberObj member) throws SQLException {
    	String sql = "INSERT INTO sns_auth (sns, token) VALUES (?, ?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, member.snsInfo.sns);
    		pstmt.setString(2, member.snsInfo.token);
    		if (pstmt.executeUpdate() < 1)
    			return false;
    	}   	
    	return true;
    }
    
    
    /**
     * 생성된 SNS 아이디 가져오기.
     * @param conn
     * @param member
     * @return
     * @throws SQLException
     */
    private boolean getSnsAuthId(Connection conn, MemberObj member) throws SQLException {
    	String sql = "SELECT id FROM sns_auth WHERE sns = ? AND token = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, member.snsInfo.sns);
    		pstmt.setString(2, member.snsInfo.token);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (!rs.next())
    				return false;
    			member.snsInfo.id = rs.getLong("id");
    		}
    	}
    	return true;
    }

    
    /**
     * 회원 아이디 생성하기.
     * @param conn
     * @param member
     * @return
     * @throws SQLException
     */
    private boolean createMember(Connection conn, MemberObj member) throws SQLException {
    	String sql = "INSERT INTO member (id, sns_auth_id) VALUES (UNHEX(REPLACE(UUID(), '-', '')), ?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setLong(1, member.snsInfo.id);
    		if (pstmt.executeUpdate() < 1)
    			return false;
    	}
    	return true;
    }

    
    /**
     * 생성된 회원 아이디 가져오기.
     * @param conn
     * @param member
     * @return
     * @throws SQLException
     */
    private boolean getMember(Connection conn, MemberObj member) throws SQLException {
    	String sql = "SELECT HEX(id) as id FROM member WHERE sns_auth_id = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setLong(1, member.snsInfo.id);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (!rs.next())
    				return false;
    			member.id = rs.getString("id");
    		}
    	}
    	return true;
    }
 
    
    /**
     * 회원 정보 생성하기.
     * @param conn
     * @param member
     * @return
     * @throws SQLException
     */
    private boolean createMemberInfo(Connection conn, MemberObj member) throws SQLException {
    	String sql = "INSERT INTO member_info " +
    			"(member_id, photo, nickname, height, weight, age, gender, region_id) VALUES (UNHEX(?), ?, ?, ?, ?, ?, ?, ?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, member.id);
    		if (isEmpty(member.info.photo))
    			pstmt.setNull(2, Types.VARCHAR);
    		else
    			pstmt.setString(2, member.info.photo);
    		pstmt.setString(3, member.info.nickname);
    		pstmt.setInt(4, member.info.height);
    		pstmt.setInt(5, member.info.weight);
    		pstmt.setInt(6, member.info.age);
    		pstmt.setString(7, member.info.gender);
    		pstmt.setInt(8, member.info.region);
    		if (pstmt.executeUpdate() < 1)
    			return false;
    	}
    	return true;
    }
    
}
