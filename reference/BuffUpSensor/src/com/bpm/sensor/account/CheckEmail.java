package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Properties;
import java.util.Random;

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

@WebServlet("/CheckEmail")
public class CheckEmail extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public CheckEmail() {
        super();
    }

    
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	ApiObj<Boolean> result = new ApiObj<>();
    	
    	// 이메일 확인.
    	String email = req.getParameter("email");
    	if (isEmpty(email)) {
    		sendResponse(resp, result.fail("Empty_Email"));
    		return;
    	}
    	
    	// 중복확인.
    	try (Connection conn = getConnection()) {
    		result.obj = checkEmail(conn, email);
    	} catch (SQLException e) {
			showSqlException(e);
    		sendResponse(resp, result.fail(e.getMessage()));
    		return;
		}
    	
    	// 중복이 아니면 보안코드 발송.
    	if (!result.obj) {
    		// 보안코드.
        	int code = createCode();
        	try {
        		// 보안코드 발송.
    			sendEmail(email, code);
    			resp.setHeader("EmailCode", String.valueOf(code));
    		} catch (MessagingException e) {
    			sendResponse(resp, result.fail(e.getMessage()));
    			return;
    		}
    	}
    	
    	sendResponse(resp, result.ok());
    }
    
    
    /**
     * 이메일 중복 확인.
     * @param conn
     * @param email
     * @return
     * @throws SQLException
     */
    private boolean checkEmail(Connection conn, String email) throws SQLException {
    	String sql = "SELECT id FROM email_auth WHERE email = ?";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		pstmt.setString(1, email);
    		try (ResultSet rs = pstmt.executeQuery()) {
    			return rs.next();
    		}
    	}
    }
    
    
    /**
     * 보안코드 생성.
     * @return
     */
    private int createCode() {
    	int code = new Random().nextInt(1000000) + 100000;
    	if(code > 999999) code -= 100000;
    	return code;
    }

    
    /**
     * 보안코드 발송.
     * @param email
     * @param code
     * @throws MessagingException
     */
    private void sendEmail(String email, int code) throws MessagingException {
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
		message.setSubject("BuffUpSensor 이메일 본인확인");
		message.setRecipient(Message.RecipientType.TO, new InternetAddress(email));
		
		Multipart mp = new MimeMultipart();
        MimeBodyPart mbp1 = new MimeBodyPart();
        mbp1.setText("어플리케이션에 이메일 본인확인 코드 [" + code + "] 를 입력해주세요.");
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
    
}
