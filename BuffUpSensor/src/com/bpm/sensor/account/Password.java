package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Map;
import java.util.Properties;

import javax.activation.CommandMap;
import javax.activation.MailcapCommandMap;
import javax.mail.Authenticator;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Multipart;
import javax.mail.PasswordAuthentication;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.EmailInfoObj;

@WebServlet("/Password")
public class Password extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public Password() {
        super();
    }


    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<Boolean> result = new ApiObj<>();
    	
    	String email = req.getParameter("email");
    	if (isEmpty(email)) {
    		sendResponse(resp, result.fail("Empty_Email"));
    		return;
    	}
    	String password = null;
    	
    	try (Connection conn = getConnection()) {
    		// 비밀번호 찾기.
    		password = findPassword(conn, email);
    	} catch (SQLException e) {
    		showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	if (isEmpty(password)) {
    		sendResponse(resp, result.fail("Invalid_Email"));
    		return;
    	}
    	
    	try {
    		// 비밀번호 이메일 발송.
			sendEmail(email, password);
		} catch (MessagingException e) {
			e.printStackTrace();
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	sendResponse(resp, result.ok(true));
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
    	System.out.println(memberId);
    	
    	// 이메일 정보 확인.
    	EmailInfoObj emailInfo = fromJson(req, EmailInfoObj.class);
    	if (isEmpty(emailInfo)) {
    		sendResponse(resp, result.fail("Empty_Email_Info"));
    		return;
    	}
    	if (isEmpty(emailInfo.email, emailInfo.password)) {
    		sendResponse(resp, result.fail("Invalid_Email_Info"));
    		return;
    	}
    	
    	try (Connection conn = getConnection()) {
    		// 사용자 확인.
    		if (!isValidUser(conn, memberId, emailInfo.email)) {
    			sendResponse(resp, result.fail("Invalid_Email_User"));
    			return;
    		}
    		// 비밀번호 변경.
    		if (!changeEmailPassword(conn, emailInfo)) {
    			sendResponse(resp, result.fail("Error_Update_Email_Password"));
    			return;
    		}
    	} catch (SQLException e) {
			e.printStackTrace();
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	
    	sendResponse(resp, result.ok(true));
    }
    
    
    /**
     * 비밀번호 찾기.
     * @param conn
     * @param email
     * @return
     * @throws SQLException
     */
    private String findPassword(Connection conn, String email) throws SQLException {
    	String password = null;
    	String sql = "SELECT password FROM email_auth WHERE email = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, email);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (rs.next()) {
    				password = rs.getString("password");
    			}
    		}
    	}
    	return password;
    }
    
    
    /**
     * 비밀번호 이메일 발송.
     * @param email
     * @param password
     * @throws MessagingException
     */
    private void sendEmail(String email, String password) throws MessagingException {
    	Properties properties = new Properties();
    	properties.setProperty("mail.transport.protocol", "smtp");
    	properties.setProperty("mail.host", "smtp.gmail.com");
    	properties.put("mail.smtp.auth", "true");
    	properties.put("mail.smtp.port", "465");
    	properties.put("mail.smtp.socketFactory.port", "465");
    	properties.put("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
    	properties.put("mail.smtp.socketFactory.fallback", "false");
    	properties.setProperty("mail.smtp.quitwait", "false");
    	
    	Authenticator authenticator = new Authenticator(){
            protected PasswordAuthentication getPasswordAuthentication() {
                return new PasswordAuthentication("202bpm.com@gmail.com", "kimkim44$");
            }
        };
        
        Session session = Session.getDefaultInstance(properties, authenticator);
        
        MimeMessage message = new MimeMessage(session);
        message.setSender(new InternetAddress("202bpm.com@gmail.com"));
		message.setSubject("BuffUpSensor 비밀번호 찾기");
		message.setRecipient(Message.RecipientType.TO, new InternetAddress(email));
		
		Multipart mp = new MimeMultipart();
        MimeBodyPart mbp1 = new MimeBodyPart();
        mbp1.setText("BuffUpSensor 비벌번호는 [" + password + "] 입니다.");
        mp.addBodyPart(mbp1);
        
        MailcapCommandMap mailcapCommandMap = (MailcapCommandMap) CommandMap.getDefaultCommandMap();
        mailcapCommandMap.addMailcap("text/html;; x-java-content-handler=com.sun.mail.handlers.text_html");
        mailcapCommandMap.addMailcap("text/xml;; x-java-content-handler=com.sun.mail.handlers.text_xml");
        mailcapCommandMap.addMailcap("text/plain;; x-java-content-handler=com.sun.mail.handlers.text_plain");
        mailcapCommandMap.addMailcap("multipart/*;; x-java-content-handler=com.sun.mail.handlers.multipart_mixed");
        mailcapCommandMap.addMailcap("message/rfc822;; x-java-content-handler=com.sun.mail.handlers.message_rfc822");
        CommandMap.setDefaultCommandMap(mailcapCommandMap);
        
        message.setContent(mp);
        
        Transport.send(message);
    }
    
    
    /**
     * 비밀번호 변경 전 사용자 확인.
     * @param conn
     * @param member
     * @param email
     * @return
     * @throws SQLException
     */
    private boolean isValidUser(Connection conn, String member, String email) throws SQLException {
    	String sql = "SELECT email FROM email_auth WHERE id = (SELECT email_auth_id FROM member WHERE id = UNHEX(?))";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, member);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			if (rs.next()) {
    				if (rs.getString("email").equals(email)) {
    					return true;
    				}
    			}
    		}
    	}
    	return false;
    }
    
    
    /**
     * 비밀번호 변경.
     * @param conn
     * @param emailInfo
     * @return
     * @throws SQLException
     */
    private boolean changeEmailPassword(Connection conn, EmailInfoObj emailInfo) throws SQLException {
    	String sql = "UPDATE email_auth SET password = ? WHERE email = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, emailInfo.password);
    		pstmt.setString(2, emailInfo.email);
    		return pstmt.executeUpdate() > 0;
    	}
    }
}
