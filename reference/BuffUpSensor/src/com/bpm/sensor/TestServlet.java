package com.bpm.sensor;

import java.io.IOException;
import java.sql.Connection;
import java.sql.SQLException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.obj.ExObj.Type;

@WebServlet("/TestServlet")
public class TestServlet extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public TestServlet() {
        super();
    }

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		super.doGet(request, response);
		ApiObj<Type> result = new ApiObj<>();
		int id = toInt(request.getParameter("id"), 0);
		try (Connection conn = getConnection()) {
			sendResponse(response, result.ok(Type.findById(id)));			
		} catch (SQLException e) {
			showSqlException(e);
		}
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		super.doPost(request, response);
		try (Connection conn = getConnection()) {
			response.getWriter().append("Served at: ").append(request.getContextPath());			
		} catch (SQLException e) {
			showSqlException(e);
		}
	}

	
}
