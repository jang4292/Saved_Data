package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Types;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.PersonalInfoObj;

@WebServlet("/PersonalInfo")
public class PersonalInfo extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public PersonalInfo() {
        super();
    }


    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	
    	ApiObj<PersonalInfoObj> result = new ApiObj<>();
    	
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
    	// 수정될 정보 확인.
    	PersonalInfoObj personalInfo = fromJson(req, PersonalInfoObj.class);
    	if (isEmpty(personalInfo)) {
    		sendResponse(resp, result.fail("Empty_Personal_Information"));
    		return;
    	}
    	// 정보 유효성 확인.
    	if (!personalInfo.isValid()) {
    		sendResponse(resp, result.fail("Invalid_Personal_Information"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
			// 회원 정보 업데이트.
    		if (!updatePersonalInfo(conn, memberId, personalInfo)) {
    			sendResponse(resp, result.fail("Error_Update_Personal_Information"));
    			return;
    		}
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	sendResponse(resp, result.ok(personalInfo));
    }
    
    
    /**
     * 회원 정보 업데이트
     * @param conn
     * @param memberId
     * @param personalInfo
     * @return
     * @throws SQLException
     */
    private boolean updatePersonalInfo(Connection conn, String memberId, PersonalInfoObj personalInfo) throws SQLException {
    	String sql =
    			"UPDATE member_info " +
    			"SET photo = ?, nickname = ?, height = ?, weight = ?, age = ?, gender = ?, region_id = ? " +
    			"WHERE member_id = UNHEX(?)";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		if (isEmpty(personalInfo.photo))
    			pstmt.setNull(1, Types.VARCHAR);
    		else
    			pstmt.setString(1, personalInfo.photo);
    		pstmt.setString(2, personalInfo.nickname);
    		pstmt.setInt(3, personalInfo.height);
    		pstmt.setInt(4, personalInfo.weight);
    		pstmt.setInt(5, personalInfo.age);
    		pstmt.setString(6, personalInfo.gender);
    		pstmt.setInt(7, personalInfo.region);
    		pstmt.setString(8, memberId);
    		if (pstmt.executeUpdate() < 1) {
    			return false;
    		}
    	}
    	return true;
    }
    
}
