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
import com.bpm.sensor.obj.MemberObj;
import com.bpm.sensor.obj.PersonalInfoObj;
import com.bpm.sensor.obj.SnsInfoObj;

@WebServlet("/SignInWithSns")
public class SignInWithSns extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public SignInWithSns() {
        super();
    }


    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	
    	ApiObj<MemberObj> result = new ApiObj<>();
    	MemberObj member = new MemberObj();
    	SnsInfoObj snsInfo = fromJson(req, SnsInfoObj.class);
    	
    	// SNS 확인.
    	if (isEmpty(snsInfo)) {
    		sendResponse(resp, result.fail("Empty_SNS"));
    		return;
    	}
    	// SNS 확인.
    	if (!snsInfo.isvalid()) {
    		sendResponse(resp, result.fail("Invalid_SNS"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// SNS 로그인.
    		member = signIn(conn, snsInfo);
    		// 회원 아이디 확인.
    		if (isEmpty(member.id)) {
    			sendResponse(resp, result.fail("Error_Member_ID"));
    			return;
    		}
    		// 회원 정보 확인.
    		if (!member.hasPersonalInfo()) {
    			sendResponse(resp, result.fail("Error_Personal_Info"));
    			return;
    		}
    	} catch (SQLException e) {
    		showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	setToken(resp, member.id);
    	member.id = null;
    	sendResponse(resp, result.ok(member));
    }
    
    
    /**
     * SNS 로그인.
     * @param conn
     * @param snsInfo
     * @return
     * @throws SQLException
     */
    private MemberObj signIn(Connection conn, SnsInfoObj snsInfo) throws SQLException {
    	MemberObj member = new MemberObj();
    	String sql = 
    			"SELECT HEX(D.member_id) AS member_id, D.photo, D.nickname, D.height, D.weight, D.age, D.gender, D.region_id " +
    			"FROM (" +
    			"	SELECT B.id" +
    			"	FROM (" +
    			"		SELECT id" +
    			"		FROM sns_auth" +
    			"		WHERE sns = ?" +
    			"			AND token = ?" +
    			"	) AS A" +
    			"	LEFT JOIN member AS B" +
    			"	ON A.id = B.sns_auth_id " +
    			") AS C " +
    			"LEFT JOIN member_info AS D " +
    			"ON C.id = D.member_id";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, snsInfo.sns);
    		pstmt.setString(2, snsInfo.token);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (rs.next()) {
    				member.id = rs.getString("member_id");
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
