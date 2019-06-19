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
import com.bpm.sensor.obj.PairObj;

@WebServlet("/DeleteMe")
public class DeleteMe extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public DeleteMe() {
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
    	
    	try (Connection conn = getConnection()) {
    		// 가입시 생성된 인증정보 가져오기.
    		PairObj<Long, Long> authId = getAuthId(conn, memberId);
    		if (!isEmpty(authId.message)) {
    			sendResponse(resp, result.fail(authId.message));
    			return;
    		}
    		// 회원 삭제.
    		if (!deleteMember(conn, memberId)) {
    			sendResponse(resp, result.fail("Can_Not_Delete_Member"));
    			return;
    		}
    		if (!isEmpty(authId.first))
    			// 이메일 인증 정보 삭제.
    			deleteEmailAuth(conn, authId.first);
    		else if (!isEmpty(authId.second))
    			// SNS 인증 정보 삭제.
    			deleteSnsAuth(conn, authId.second);
    	} catch (SQLException e) {
    		showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	sendResponse(resp, result.ok(true));
    }
    
    
    /**
     * 가입시 생성된 인증정보 가져오기.
     * @param conn
     * @param memberId
     * @return
     * @throws SQLException
     */
    private PairObj<Long, Long> getAuthId(Connection conn, String memberId) throws SQLException {
    	PairObj<Long, Long> authId = new PairObj<>();
    	String sql =
    			"SELECT email_auth_id, sns_auth_id " +
    			"FROM member " +
    			"WHERE id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (rs.next()) {
    				authId.first = rs.getLong("email_auth_id");
    				authId.second = rs.getLong("sns_auth_id");
    			} else {
    				authId.message = "Can_Not_Found_Auth_ID";
    			}
    		}
    	}
    	return authId;
    }
    
    
    /**
     * 회원 삭제.
     * @param conn
     * @param memberId
     * @return
     * @throws SQLException
     */
    private boolean deleteMember(Connection conn, String memberId) throws SQLException {
    	String sql = "DELETE FROM member WHERE id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, memberId);
    		if (pstmt.executeUpdate() < 1)
    			return false;
    	}
    	return true;
    }
    
    
    /**
     * 이메일 인증 정보 삭제.
     * @param conn
     * @param id
     * @throws SQLException
     */
    private void deleteEmailAuth(Connection conn, long id) throws SQLException {
    	String sql = "DELETE FROM email_auth WHERE id = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setLong(1, id);
    		pstmt.executeUpdate();
    	}
    }
    
    
    /**
     * SNS 인증 정보 삭제.
     * @param conn
     * @param id
     * @throws SQLException
     */
    private void deleteSnsAuth(Connection conn, long id) throws SQLException {
    	String sql = "DELETE FROM sns_auth WHERE id = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setLong(1, id);
    		pstmt.executeUpdate();
    	}
    }
    
}
