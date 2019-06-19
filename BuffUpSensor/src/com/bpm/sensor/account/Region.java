package com.bpm.sensor.account;

import java.io.IOException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;
import com.bpm.sensor.obj.RegionObj;

@WebServlet("/Region")
public class Region extends BaseServlet {
	private static final long serialVersionUID = 1L;
       
    public Region() {
        super();
    }

    
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	
    	ApiObj<ArrayList<RegionObj>> result = new ApiObj<>();
    	
    	try (Connection conn = getConnection()) {
    		// 행정구역 가져오기.
    		result.obj = getRegion(conn);
    	} catch (SQLException e) {
			showSqlException(e);
			sendResponse(resp, result.fail(e.getMessage()));
			return;
		}
    	   	
    	sendResponse(resp, result.ok());
    }

    
    /**
     * 행정구역 가져오기.
     * @param conn
     * @return
     * @throws SQLException
     */
    private ArrayList<RegionObj> getRegion(Connection conn) throws SQLException {
    	ArrayList<RegionObj> list = new ArrayList<>();
    	String sql = "SELECT id, main, sub FROM region_list";
    	try (PreparedStatement pstmt = conn.prepareStatement(sql)) {
    		try (ResultSet rs = pstmt.executeQuery()) {
    			while (rs.next()) {
    				RegionObj region = new RegionObj();
    				region.id = rs.getInt("id");
    				region.main = rs.getString("main");
    				region.sub = rs.getString("sub");
    				list.add(region);
    			}
    		}
    	}
    	return list;
    }
    
}
