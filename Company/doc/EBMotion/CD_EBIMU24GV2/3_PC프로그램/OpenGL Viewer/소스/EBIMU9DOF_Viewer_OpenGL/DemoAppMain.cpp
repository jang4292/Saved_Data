
// Include files
//#include "stdafx.h"
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "glfw.h"	// GLFW library header file.

// Import libraries
#ifdef _DEBUG
	#pragma comment ( lib, "glfwVC6d.lib" )	
#else
	#pragma comment ( lib, "glfwVC6r.lib" )	
#endif	


/*---------------------------------------------------------------------------------------*/
#include <commctrl.h>
#include "resource.h"
#include "serial.cpp"
BOOL CALLBACK MainDlgProc(HWND hDlg, UINT iMessage, WPARAM wParam, LPARAM lParam);
HINSTANCE g_hInst;
HWND hDlgMain;



#define SERIAL_BAUDRATE  115200

BOOL ComPortOpen=FALSE;
int  OpenedPort=1;

char *ItemComPort[10]={ "COM1" , "COM2", "COM3" , "COM4" , "COM5" , "COM6" , "COM7" , "COM8" , "COM9" , "COM10"  };
HWND hComport;
HWND hConnect;
HWND hProgPitch;
HWND hProgRoll;
HWND hProgYaw;

HWND hStaticPitch;
HWND hStaticRoll;
HWND hStaticYaw;

float DegPitch=0.0f;
float DegRoll=0.0f;
float DegYaw=0.0f;
 

#define TIMER0 1
#define TIMER1 2
#define TIMER2 3
#define TIMER3 4
#define TIMER4 5

/*---------------------------------------------------------------------------------------*/

// ////////////////////////////////////////////////////////////////////////


// Constants definition
#define NEAR_PLANE					1.0
#define FAR_PLANE					200.0
#define FIELD_OF_VIEW				60.0



// ////////////////////////////////////////////////////////////////////////



//
// Used when window size is changed.
//
static void ChangeWindowSize ( GLsizei iWidth, GLsizei iHeight )
{
	GLfloat	fAspectRatio;

	if ( iHeight == 0 )
		iHeight = 1;

	// Set the new view port:
	glViewport( 0, 0, iWidth, iHeight );

	// Setup the projection matrix:
	glMatrixMode( GL_PROJECTION );
	glLoadIdentity();
	fAspectRatio = ( (GLfloat)iWidth ) / ( (GLfloat)iHeight );
	gluPerspective( FIELD_OF_VIEW, fAspectRatio, NEAR_PLANE, FAR_PLANE );

	glMatrixMode( GL_MODELVIEW );
	glLoadIdentity();
	
	// Place the camera:
	gluLookAt( 0.0f, 0.0f, 80.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0 );

} // ChangeWindowSize()



// ////////////////////////////////////////////////////////////////////////


// Cube Vertex
/*static GLfloat g_VertexArray[][4] =
{
	{ -10.0f, -10.0f,  10.0f, 1.0f },	// V0
	{  10.0f, -10.0f,  10.0f, 1.0f },	// V1
	{  10.0f,  10.0f,  10.0f, 1.0f },	// V2
	{ -10.0f,  10.0f,  10.0f, 1.0f },	// V3
	{ -10.0f, -10.0f, -10.0f, 1.0f },	// V4
	{  10.0f, -10.0f, -10.0f, 1.0f },	// V5
	{  10.0f,  10.0f, -10.0f, 1.0f },	// V6
	{ -10.0f,  10.0f, -10.0f, 1.0f },	// V7
};
*/
/*
static GLfloat g_VertexArray[][4] =
{
	{ -20.0f, -3.0f,  15.0f, 1.0f },	// V0
	{  20.0f, -3.0f,  15.0f, 1.0f },	// V1
	{  20.0f,  3.0f,  15.0f, 1.0f },	// V2
	{ -20.0f,  3.0f,  15.0f, 1.0f },	// V3
	{ -20.0f, -3.0f, -15.0f, 1.0f },	// V4
	{  20.0f, -3.0f, -15.0f, 1.0f },	// V5
	{  20.0f,  3.0f, -15.0f, 1.0f },	// V6
	{ -20.0f,  3.0f, -15.0f, 1.0f },	// V7
};
*/
static GLfloat g_VertexArray[][4] =
{
	{ -15.0f, -3.0f,  20.0f, 1.0f },	// V0
	{  15.0f, -3.0f,  20.0f, 1.0f },	// V1
	{  15.0f,  3.0f,  20.0f, 1.0f },	// V2
	{ -15.0f,  3.0f,  20.0f, 1.0f },	// V3
	{ -15.0f, -3.0f, -20.0f, 1.0f },	// V4
	{  15.0f, -3.0f, -20.0f, 1.0f },	// V5
	{  15.0f,  3.0f, -20.0f, 1.0f },	// V6
	{ -15.0f,  3.0f, -20.0f, 1.0f },	// V7
};


//
// Draw a Cube using OpenGL.
//
static void RenderCube ( void )
{
	// Draw the scene:
	glBegin( GL_QUADS );	
		glNormal3f( 0.0f, 0.0f, 1.0f );		// Front
		glVertex4fv( (const GLfloat *)&( g_VertexArray[0] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[1] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[2] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[3] )  );

		glNormal3f( 1.0f, 0.0f, 0.0f );		// Right
		glVertex4fv( (const GLfloat *)&( g_VertexArray[1] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[5] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[6] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[2] )  );

		glNormal3f( 0.0f, 1.0f, 0.0f );		// Top
		glVertex4fv( (const GLfloat *)&( g_VertexArray[3] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[2] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[6] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[7] )  );

		glNormal3f( -1.0f, 0.0f, 0.0f );	// Left
		glVertex4fv( (const GLfloat *)&( g_VertexArray[4] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[0] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[3] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[7] )  );

		glNormal3f( 0.0f, 0.0f, -1.0f );	// Back
		glVertex4fv( (const GLfloat *)&( g_VertexArray[5] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[4] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[7] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[6] )  );

		glNormal3f( 0.0f, -1.0f, 0.0f );	// Bottom
		glVertex4fv( (const GLfloat *)&( g_VertexArray[4] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[5] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[1] )  );
		glVertex4fv( (const GLfloat *)&( g_VertexArray[0] )  );

	glEnd();
	glFlush();

} // RenderCube()



// ////////////////////////////////////////////////////////////////////////

// Lights settings
static GLfloat g_fAmbientLight[]	= { 0.3f, 0.3f, 0.3f, 1.0f };
static GLfloat g_fLAmbient[]		= { 0.3f, 0.3f, 0.3f, 1.0f };
static GLfloat g_fLDiffuse[]		= { 0.7f, 0.7f, 0.7f, 1.0f };
static GLfloat g_fLSpecular[]		= { 1.0f, 1.0f, 1.0f, 1.0f };
static GLfloat g_fLSpecRef[]		= { 1.0f, 1.0f, 1.0f, 1.0f };

// Lights positions and directions.
static GLfloat g_fLightPos[]		= { 0.0f, 0.0f, 50.0f, 1.0f };
static GLfloat g_fLightDir[]		= { 0.0f, 0.0f, -1.0f };
static GLfloat g_fLightPos1[]		= { 0.0f, 50.0f, 0.0f, 1.0f };
static GLfloat g_fLightDir1[]		= { 0.0f, -1.0f, 0.0f };



//
// Setup the OpenGL Environment.
//
static void SetupOpenGL ( void )
{
	// Set the shade model:
	glShadeModel( GL_SMOOTH );

	// Set the clear screen color:
	glClearColor( 0.0f, 0.0f, 0.0f, 0.0f );

	// Enable the depth Test.
	glEnable( GL_DEPTH_TEST );

	// Enable Back-face culling.
	glEnable( GL_CULL_FACE);

	// Enable light Calculation.
	glEnable( GL_LIGHTING );

	// Polygon winding: counter clockwise.
	glFrontFace( GL_CCW );

	// Setup Lights:
	glLightfv( GL_LIGHT0, GL_DIFFUSE,	g_fAmbientLight );
	glLightfv( GL_LIGHT0, GL_SPECULAR,	g_fLSpecular );
	glLightfv( GL_LIGHT0, GL_POSITION,	g_fLightPos );
	glLightfv( GL_LIGHT0, GL_SPOT_DIRECTION, g_fLightDir );
	glLightf( GL_LIGHT0, GL_SPOT_CUTOFF, 60.0f );
	glEnable( GL_LIGHT0 );

	glLightfv( GL_LIGHT1, GL_DIFFUSE,	g_fAmbientLight );
	glLightfv( GL_LIGHT1, GL_SPECULAR,	g_fLSpecular );
	glLightfv( GL_LIGHT1, GL_POSITION,	g_fLightPos1 );
	glLightfv( GL_LIGHT1, GL_SPOT_DIRECTION, g_fLightDir1 );
	glLightf( GL_LIGHT1, GL_SPOT_CUTOFF, 120.0f );
	glEnable( GL_LIGHT1 );

	// Setup Material
	glEnable( GL_COLOR_MATERIAL );
	glColorMaterial( GL_FRONT, GL_AMBIENT_AND_DIFFUSE );
	glMaterialfv( GL_FRONT, GL_SPECULAR, g_fLSpecRef );
	glMateriali( GL_FRONT, GL_SHININESS, 128 );

} // SetupOpenGL()


void euler2axis_angle(double roll, double pitch, double yaw, double *out_angle, double *out_x, double *out_y, double *out_z) 
{
    double heading, attitude,bank;

    bank = roll * 3.141592/180.;
	attitude = pitch * 3.141592/180.;
	heading = yaw * 3.141592/180.;

	// Assuming the angles are in radians.
	double c1 = cos(heading/2);
	double s1 = sin(heading/2);
	double c2 = cos(attitude/2);
	double s2 = sin(attitude/2);
	double c3 = cos(bank/2);
	double s3 = sin(bank/2);
	double c1c2 = c1*c2;
	double s1s2 = s1*s2;
	double w =c1c2*c3 - s1s2*s3;
	double x =c1c2*s3 + s1s2*c3;
	double y =s1*c2*c3 + c1*s2*s3;
	double z =c1*s2*c3 - s1*c2*s3;
	double angle = 2 * acos(w);
	double norm = x*x+y*y+z*z;
	if (norm < 0.001) { // when all euler angles are zero angle =0 so
		// we can set axis to anything to avoid divide by zero
		x=1;
		y=z=0;
	} else { 
		norm = sqrt(norm);
    	x /= norm;
    	y /= norm;
    	z /= norm;
	}

   *out_angle = angle * 180./3.141592;
   *out_x = x;
   *out_y = y;
   *out_z = z;
}


// ////////////////////////////////////////////////////////////////////////

//
// Window message handler for Window 1.
//
static LRESULT WINAPI MsgProc1 ( HWND hWnd, UINT iMsg, WPARAM wParam, LPARAM lParam )
{
    static float old_pitch=0.0f, old_roll=0.0f, old_yaw=0.0f;
	double angle,x,y,z;

	// Get the context of the window and make it the current one:
	GLFW_WCTXT *pContext;
	pContext = glfwGetWinContext( hWnd );
	glfwSetCurrWinContext( pContext );

	switch( iMsg )
    {
        case WM_DESTROY:	// Destroy window.
			// Destroy the Window Context:
			glfwDestroyContext( pContext );
			KillTimer(hWnd,TIMER1);
			
			// Send the quit message: 
            //PostQuitMessage( 0 );
			break;

		case WM_SIZE:		// Change the window size.
			ChangeWindowSize( LOWORD(lParam), HIWORD(lParam) );
			break;

		case WM_TIMER:
            if( (old_pitch!=DegPitch) || (old_roll!=DegRoll)  || (old_yaw!=DegYaw)) 
		      { 
		        old_pitch=DegPitch; old_roll=DegRoll; old_yaw=DegYaw;  
			    InvalidateRect( hWnd, NULL, FALSE );
		      }
			break;

        case WM_PAINT:		// Repaint window.
			// Clear the screen.
			glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );

			glPushMatrix();

			euler2axis_angle(DegRoll,DegPitch,DegYaw,&angle,&x,&y,&z);

			glRotatef(angle,-x,-y,z);


/*				glRotatef( DegRoll, 0.0f, 0.0f, 1.0f );
				glRotatef( DegPitch, 1.0f, 0.0f, 0.0f );
                glRotatef( DegYaw, 0.0f, 1.0f, 0.0f );
*/


				glColor3f( 1.0f, 1.0f, 1.0f ); 
				RenderCube();

				glfwTextColor2D( 1.0f, 1.0f, 1.0f, 1.0f );
				glfwText2D( pContext, 2, 10, "IMU Tester 3D - Pitch/Roll/Yaw" );

			glPopMatrix();

			SwapBuffers( pContext->m_hDC );
			ValidateRect( pContext->m_hWin, NULL );
			break;
	}

    return DefWindowProc( hWnd, iMsg, wParam, lParam );

} // MsgProc1()



// ////////////////////////////////////////////////////////////////////////

//
// Window message handler for Window 2.
//
static LRESULT WINAPI MsgProc2 ( HWND hWnd, UINT iMsg, WPARAM wParam, LPARAM lParam )
{
    static float old_pitch=0.0f;

	// Get the context of the window and make it the current one:
	GLFW_WCTXT *pContext;
	pContext = glfwGetWinContext( hWnd );
	glfwSetCurrWinContext( pContext );

	switch( iMsg )
    {
        case WM_DESTROY:	// Destroy window.
			// Destroy the Window Context:
			glfwDestroyContext( pContext );
			KillTimer(hWnd,TIMER2);
			
			// No calls to PostQuitMessage() here!
			break;

		case WM_SIZE:		// Change the window size.
			ChangeWindowSize( LOWORD(lParam), HIWORD(lParam) );
			break;

		case WM_TIMER:
            if(old_pitch!=DegPitch)
		      { 
		        old_pitch=DegPitch;
			    InvalidateRect( hWnd, NULL, FALSE );
		      }
			break;

        case WM_PAINT:		// Repaint window.
			glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
		
			glPushMatrix();
				glRotatef( DegPitch, -1.0f, 0.0f, 0.0f );

				glColor3f( 0.0f, 0.0f, 1.0f );
				RenderCube();

				glfwTextColor2D( 0.0f, 0.0f, 1.0f, 1.0f );
				glfwText2D( pContext, 2, 10, "IMU Tester 3D - Pitch" );

			glPopMatrix();

			SwapBuffers( pContext->m_hDC );
			ValidateRect( pContext->m_hWin, NULL );
			break;
	}

    return DefWindowProc( hWnd, iMsg, wParam, lParam );

} // MsgProc2()



// ////////////////////////////////////////////////////////////////////////

//
// Window message handler for Window 3.
//
static LRESULT WINAPI MsgProc3 ( HWND hWnd, UINT iMsg, WPARAM wParam, LPARAM lParam )
{
    static float old_roll=0.0f;

	// Get the context of the window and make it the current one:
	GLFW_WCTXT *pContext;
	pContext = glfwGetWinContext( hWnd );
	glfwSetCurrWinContext( pContext );

	switch( iMsg )
    {
        case WM_DESTROY:	// Destroy window.
			// Destroy the Window Context:
			glfwDestroyContext( pContext );
			KillTimer(hWnd,TIMER3);
			
			// No calls to PostQuitMessage() here!
			break;

		case WM_SIZE:		// Change the window size.
			ChangeWindowSize( LOWORD(lParam), HIWORD(lParam) );
			break;

		case WM_TIMER:
            if(old_roll!=DegRoll)
		      { 
		        old_roll=DegRoll;
			    InvalidateRect( hWnd, NULL, FALSE );
		      }
			break;

        case WM_PAINT:		// Repaint window.
			glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
		
			glPushMatrix();
				glRotatef( DegRoll, 0.0f, 0.0f, -1.0f );

				glColor3f( 0.0f, 1.0f, 0.0f );
				RenderCube();

				glfwTextColor2D( 0.0f, 1.0f, 0.0f, 1.0f );
				glfwText2D( pContext, 2, 10, "IMU Tester 3D - Roll" );

			glPopMatrix();

			SwapBuffers( pContext->m_hDC );
			ValidateRect( pContext->m_hWin, NULL );
			break;
	}

    return DefWindowProc( hWnd, iMsg, wParam, lParam );

} // MsgProc3()



// ////////////////////////////////////////////////////////////////////////

//
// Window message handler for Window 4.
//
static LRESULT WINAPI MsgProc4 ( HWND hWnd, UINT iMsg, WPARAM wParam, LPARAM lParam )
{
    static float old_yaw=0.0f;

	// Get the context of the window and make it the current one:
	GLFW_WCTXT *pContext;
	pContext = glfwGetWinContext( hWnd );
	glfwSetCurrWinContext( pContext );

	switch( iMsg )
    {
        case WM_DESTROY:	// Destroy window.
			// Destroy the Window Context:
			glfwDestroyContext( pContext );
			KillTimer(hWnd,TIMER4);
			
			// No calls to PostQuitMessage() here!
			break;

		case WM_SIZE:		// Change the window size.
			ChangeWindowSize( LOWORD(lParam), HIWORD(lParam) );
			break;

		case WM_TIMER:
            if(old_yaw!=DegYaw) 
		      { 
		        old_yaw=DegYaw;  
			    InvalidateRect( hWnd, NULL, FALSE );
		      }
			break;

        case WM_PAINT:		// Repaint window.
			glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
		
			glPushMatrix();
				glRotatef( DegYaw, 0.0f, -1.0f, 0.0f );

				glColor3f( 1.0f, 0.0f, 0.0f );
				RenderCube();

				glfwTextColor2D( 1.0f, 0.0f, 0.0f, 1.0f );
				glfwText2D( pContext, 2, 10, "IMU Tester 3D - Yaw" );

			glPopMatrix();

			SwapBuffers( pContext->m_hDC );
			ValidateRect( pContext->m_hWin, NULL );
			break;
	}

    return DefWindowProc( hWnd, iMsg, wParam, lParam );

} // MsgProc4()





//
// WinMain routine.
// 
INT APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
			 LPSTR lpszCmdParam, int nCmdShow)
{

    g_hInst=hInstance;

	MSG sMsg; 

	// Initialize the GLFW library.
	glfwInit();

	GLWF_WINPAR	sWinPar;	// Window parameters data structure.
	GLFW_WCTXT	*pWinCntx;	// Window Context pointer.

    // ///////////////////////////////////////////////////////////////////

	// Initialize and fill the parameters data structure for Window 1:
	ZeroMemory( (PVOID)&sWinPar, sizeof( GLWF_WINPAR ) );
	sWinPar.m_iPosX			= 0;
	sWinPar.m_iPosY			= 0;
	sWinPar.m_iWidth		= 500;	
	sWinPar.m_iHeight		= 500;
	sWinPar.m_iBpp			= GLFW_32BPP;
	sWinPar.m_iZDepth		= GLFW_16BPP;
	sWinPar.m_szTitle		= _T( "IMU Tester 3D - Pitch/Roll/Yaw" );
	sWinPar.n_bFullScreen	= FALSE;	
	sWinPar.m_fpWindProc	= MsgProc1;

	// Create the 1st window:
	glfwCreateWindow( &sWinPar, &pWinCntx );

	// Set the Window 1 Context as current:
	glfwSetCurrWinContext( pWinCntx );

	// Setup the OpenGL Environment for Window 1.
	SetupOpenGL();

	// Set the timer for Window 1:
	SetTimer( pWinCntx->m_hWin, TIMER1, 10, NULL );


	// ///////////////////////////////////////////////////////////////////

	// Initialize and fill the parameters data structure for Window 2:
	ZeroMemory( (PVOID)&sWinPar, sizeof( GLWF_WINPAR ) );
	sWinPar.m_iPosX			= 500;
	sWinPar.m_iPosY			= 0;
	sWinPar.m_iWidth		= 250;	
	sWinPar.m_iHeight		= 250;
	sWinPar.m_iBpp			= GLFW_32BPP;
	sWinPar.m_iZDepth		= GLFW_16BPP;
	sWinPar.m_szTitle		= _T( "IMU Tester 3D - Pitch" );
	sWinPar.n_bFullScreen	= FALSE;	
	sWinPar.m_fpWindProc	= MsgProc2;

	// Create the 2nd window:
	glfwCreateWindow( &sWinPar, &pWinCntx );

	// Set the Window 2 Context as current:
	glfwSetCurrWinContext( pWinCntx );

	// Setup the OpenGL Environment for Window 2.
	SetupOpenGL();

	// Set the timer for Window 2:
	SetTimer( pWinCntx->m_hWin, TIMER2, 10, NULL );


	// ///////////////////////////////////////////////////////////////////
	

	// Initialize and fill the parameters data structure for Window 3:
	ZeroMemory( (PVOID)&sWinPar, sizeof( GLWF_WINPAR ) );
	sWinPar.m_iPosX			= 500;
	sWinPar.m_iPosY			= 250;
	sWinPar.m_iWidth		= 250;	
	sWinPar.m_iHeight		= 250;
	sWinPar.m_iBpp			= GLFW_32BPP;
	sWinPar.m_iZDepth		= GLFW_16BPP;
	sWinPar.m_szTitle		= _T( "IMU Tester 3D - Roll" );
	sWinPar.n_bFullScreen	= FALSE;	
	sWinPar.m_fpWindProc	= MsgProc3;

	// Create the 3rd window:
	glfwCreateWindow( &sWinPar, &pWinCntx );

	// Set the Window 3 Context as current:
	glfwSetCurrWinContext( pWinCntx );

	// Setup the OpenGL Environment for Window 3.
	SetupOpenGL();

	// Set the timer for Window 3:
	SetTimer( pWinCntx->m_hWin, TIMER3, 10, NULL );

    // ///////////////////////////////////////////////////////////////////

	// Initialize and fill the parameters data structure for Window 4:
	ZeroMemory( (PVOID)&sWinPar, sizeof( GLWF_WINPAR ) );
	sWinPar.m_iPosX			= 500;
	sWinPar.m_iPosY			= 500;
	sWinPar.m_iWidth		= 250;	
	sWinPar.m_iHeight		= 250;
	sWinPar.m_iBpp			= GLFW_32BPP;
	sWinPar.m_iZDepth		= GLFW_16BPP;
	sWinPar.m_szTitle		= _T( "IMU Tester 3D - Yaw" );
	sWinPar.n_bFullScreen	= FALSE;	
	sWinPar.m_fpWindProc	= MsgProc4;

	// Create the 4th window:
	glfwCreateWindow( &sWinPar, &pWinCntx );

	// Set the Window 4 Context as current:
	glfwSetCurrWinContext( pWinCntx );

	// Setup the OpenGL Environment for Window 4.
	SetupOpenGL();

	// Set the timer for Window 4:
	SetTimer( pWinCntx->m_hWin, TIMER4, 10, NULL );

    // ///////////////////////////////////////////////////////////////////



    g_hInst=hInstance;
    DialogBox(g_hInst, MAKEINTRESOURCE(IDD_DIALOG1), HWND_DESKTOP, MainDlgProc);

    hDlgMain=CreateDialog(g_hInst,MAKEINTRESOURCE(IDD_DIALOG1),NULL,MainDlgProc);
	ShowWindow(hDlgMain,SW_SHOW);

    // ///////////////////////////////////////////////////////////////////


	// Begin the messages pump.
    while ( GetMessage( &sMsg, NULL, 0, 0 ) )
    {
	   if(!IsWindow(hDlgMain) || !IsDialogMessage(hDlgMain,&sMsg))  
	   {
        TranslateMessage( &sMsg );
        DispatchMessage( &sMsg );
	   }
    }

	return 0;

} // WinMain()








int GetComport(void)
{  return SendMessage(hComport,CB_GETCURSEL,NULL,NULL);
}

int FindComma(char * buf)
{
   int n;
   for(n=0;n<100;n++)
    {
	   if(buf[n]==',') break;
    }

   return n;
}

char buf[1024];
char SBuf[1000];
int SCnt=0;


void OnTimer(HWND hDlg,WPARAM wParam, LPARAM lParam)   // 10ms..
{
   SERIALREADDATA srd;
   unsigned int n;
   int value,value2;

   if(ReadSerialPort(OpenedPort,&srd)==ERR_OK)
     {  if(srd.nSize)
	      {  for(n=0;n<srd.nSize;n++) 
		        {  

//////////////////////////////////////////////////////////////////////
                     SBuf[SCnt]=srd.szData[n];
					 if(SBuf[SCnt]==0x0a)
					  {   value=FindComma(SBuf);
					      SBuf[value]='\0';
                          //DegRoll=atof(SBuf)*-1;
						  DegRoll=atof(SBuf);
                          SendMessage(hProgRoll, PBM_SETPOS,DegRoll+180,0);  sprintf(buf,"%.1f",DegRoll);  SetWindowText(hStaticRoll,buf);

                          value++;
                          value2=FindComma(&SBuf[value]);
					      SBuf[value+value2]='\0';
                          //DegPitch=atof(&SBuf[value])*-1;
						  DegPitch=-atof(&SBuf[value]);
						  SendMessage(hProgPitch, PBM_SETPOS,DegPitch+180,0);  sprintf(buf,"%.1f",DegPitch);  SetWindowText(hStaticPitch,buf);

						  value=value+value2+1;
                          value2=FindComma(&SBuf[value]);
					      SBuf[value+value2]='\0';
                          //DegYaw=atof(&SBuf[value])*-1;
						  DegYaw=atof(&SBuf[value]);
						  SendMessage(hProgYaw, PBM_SETPOS,DegYaw+180,0);  sprintf(buf,"%.1f",DegYaw);  SetWindowText(hStaticYaw,buf);
					  }
					 else if(SBuf[SCnt]=='*')
					  {   SCnt=-1;
					  }

					 SCnt++;

///////////////////////////////////////////////////////////////////////

				}
		  }
	 }
}



BOOL CALLBACK MainDlgProc(HWND hDlg, UINT iMessage,WPARAM wParam,LPARAM lParam)
{
   RECT rt;
   int n;

   switch(iMessage)
     {
       case WM_INITDIALOG:
	      GetClientRect(hDlg,&rt);
	      MoveWindow(hDlg,0,500,rt.right-rt.left,rt.bottom-rt.top,TRUE);
	      SetTimer(hDlg,TIMER0,10,NULL);
		  //SendMessage(hDlg,WM_TIMER,0,0);
		  //SetWindowPos(hDlg, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);   // 윈도우 최상위로...

		  hComport=GetDlgItem(hDlg,IDC_COMPORT);
          hConnect=GetDlgItem(hDlg,IDC_CONNECT);
          hProgPitch=GetDlgItem(hDlg,IDC_PROG_PITCH);  SendMessage(hProgPitch,PBM_SETRANGE,0,MAKELPARAM(0,360));
          hProgRoll=GetDlgItem(hDlg,IDC_PROG_ROLL);  SendMessage(hProgRoll,PBM_SETRANGE,0,MAKELPARAM(0,360));
          hProgYaw=GetDlgItem(hDlg,IDC_PROG_YAW);  SendMessage(hProgYaw,PBM_SETRANGE,0,MAKELPARAM(0,360));
          hStaticPitch=GetDlgItem(hDlg,IDC_DEGPITCH);
          hStaticRoll=GetDlgItem(hDlg,IDC_DEGROLL);
          hStaticYaw=GetDlgItem(hDlg,IDC_DEGYAW);

          for(n=0;n<10;n++) SendMessage(hComport,CB_ADDSTRING,0,(LPARAM)ItemComPort[n]);

	   return TRUE;

       //case WM_PAINT:
	   //return TRUE;

       case WM_TIMER:
          OnTimer(hDlg,wParam,lParam);
	   return TRUE;

 case WM_COMMAND:
	  switch(LOWORD (wParam))
	    {
          case IDC_CONNECT:
		    if(ComPortOpen==TRUE) 
			  {  CloseSerialPort(OpenedPort);
			     ComPortOpen=FALSE;
			   //  SetWindowText(hDlg,PROGRAMNAME" - CLOSE");
                 SetDlgItemText(hDlg,IDC_CONNECT,"Connect");
                 EnableWindow(hComport,1); 
			  }
            else
			  {  if(OpenSerialPort(GetComport()+1,SERIAL_BAUDRATE,NOPARITY,8,ONESTOPBIT)==ERR_OK)
			       {  
                      OpenedPort=GetComport()+1;
                      ComPortOpen=TRUE;
				   //   SetWindowText(hDlg,PROGRAMNAME" - OPEN");
                      SetDlgItemText(hDlg,IDC_CONNECT,"Disconnect");
                      EnableWindow(hComport,0); 
				   }
				 else
				   {  ComPortOpen=FALSE;
				      MessageBox(hDlg,"PORT OPEN ERROR","ERROR",MB_OK);
				   }
			  }
          break;


	      case IDOK:
 		  case IDCANCEL:
 		  KillTimer(hDlg,TIMER0);
		  //EndDialog(hDlg,0);
		  PostQuitMessage( 0 );
		  return TRUE;
	    }
	 }
   return FALSE;
}



// ////////////////////////////////////////////////////////////////////////
//  END of Code.
// ////////////////////////////////////////////////////////////////////////