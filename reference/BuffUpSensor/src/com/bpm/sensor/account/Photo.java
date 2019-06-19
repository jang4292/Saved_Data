package com.bpm.sensor.account;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Random;

import javax.servlet.ServletException;
import javax.servlet.annotation.MultipartConfig;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.Part;

import com.bpm.sensor.ApiObj;
import com.bpm.sensor.BaseServlet;

import net.coobird.thumbnailator.Thumbnails;
import net.coobird.thumbnailator.geometry.Positions;

@MultipartConfig(maxFileSize = 1024 * 1024 * 10, fileSizeThreshold = 1024 * 1024, maxRequestSize = 1024 * 1024 * 20)
@WebServlet("/PersonalInfo/Photo")
public class Photo extends BaseServlet {
	private static final long serialVersionUID = 1L;
	
	
	private final String IMAGE_DIR = "images";
	private final String PROFILE_PHOTO_DIR = "profile";
	
       
    public Photo() {
        super();
    }
    
    
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doGet(req, resp);
    	System.out.println(req.getServletContext().getRealPath(""));
    }

    
    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
    	super.doPost(req, resp);
    	ApiObj<String> result = new ApiObj<>();
    	
    	Part photoPart = req.getPart("photo");
    	if (photoPart == null) {
    		sendResponse(resp, result.fail("Empty_Photo"));
    		return;
    	}
    	
    	String applicationPath = req.getServletContext().getRealPath("");
    	String imagePath = applicationPath + File.separator + IMAGE_DIR;
    	File imageDir = new File(imagePath);
    	if (!imageDir.exists()) {
    		imageDir.mkdirs();
    	}
    	
    	String photoPath = imagePath + File.separator + PROFILE_PHOTO_DIR;
    	File photoDir = new File(photoPath);
    	if (!photoDir.exists()) {
    		photoDir.mkdirs();
    	}
    	
    	String photoName = makeFileName();
    	File photo = new File(photoDir, photoName);
    	Thumbnails.of(photoPart.getInputStream())
//    				.scale(1)
    				.crop(Positions.CENTER)
    				.size(250, 250)
    				.toFile(photo);
    	
    	String prevName = req.getHeader("PreviousContents");
    	if (!isEmpty(prevName)) {
    		File prev = new File(photoDir, prevName);
    		if (prev.exists()) {
    			prev.delete();
    		}
    	}
    	
    	sendResponse(resp, result.ok(photoName));
    }
    
    
    protected String makeFileName() {
		String prefix = new SimpleDateFormat("yyyyMMddHHmmssSSS").format(new Date());
		int postfix = new Random().nextInt(1000) + 100;
    	if(postfix > 999) postfix -= 100;
		return String.format(Locale.getDefault(), "%s%03d.%s", prefix, postfix, "jpg");
	}
    
    
    protected String getFileExtension(String file) {
		int position = file.lastIndexOf(".");
		return file.substring(position + 1);
	}

}
