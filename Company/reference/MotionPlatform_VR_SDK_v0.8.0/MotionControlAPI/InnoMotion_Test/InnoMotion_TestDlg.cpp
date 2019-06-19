
// InnoMotion_TestDlg.cpp : 구현 파일
//

#include "stdafx.h"
#include "InnoMotion_Test.h"
#include "InnoMotion_TestDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


#define TIMER_OPERTATION				1001
#define TIMER_OPERTATION_INTERVAL		10

#define TIMER_MOTIONINFO				1002
#define TIMER_MOTIONINFO_INTERVAL		10

// 응용 프로그램 정보에 사용되는 CAboutDlg 대화 상자입니다.

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

// 구현입니다.
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CInnoMotion_TestDlg 대화 상자




CInnoMotion_TestDlg::CInnoMotion_TestDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CInnoMotion_TestDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_strEquipNo = _T("11");
}

void CInnoMotion_TestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);

	DDX_Control(pDX, IDC_STATIC_BUSY3, m_staticBusy3);
	DDX_Control(pDX, IDC_STATIC_BUSY2, m_staticBusy2);
	DDX_Control(pDX, IDC_STATIC_BUSY1, m_staticBusy1);

	DDX_Control(pDX, IDC_STATIC_ALARM3,m_staticAlarm3);
	DDX_Control(pDX, IDC_STATIC_ALARM2,m_staticAlarm2);
	DDX_Control(pDX, IDC_STATIC_ALARM1,m_staticAlarm1);

	DDX_Control(pDX, IDC_STATIC_INPOS1,m_staticInpos1);
	DDX_Control(pDX, IDC_STATIC_INPOS2,m_staticInpos2);
	DDX_Control(pDX, IDC_STATIC_INPOS3,m_staticInpos3);

	DDX_Control(pDX, IDC_STATIC_CMD1,m_staticCmd1);
	DDX_Control(pDX, IDC_STATIC_CMD2,m_staticCmd2);
	DDX_Control(pDX, IDC_STATIC_CMD3,m_staticCmd3);

	DDX_Control(pDX, IDC_STATIC_ENC1, m_staticEnc1);
	DDX_Control(pDX, IDC_STATIC_ENC2, m_staticEnc2);
	DDX_Control(pDX, IDC_STATIC_ENC3, m_staticEnc3);

	DDX_Control(pDX, IDC_STATIC_EMERGENCY, m_staticEmer);

	DDX_Control(pDX, IDC_STATIC_STATUS, m_staticStatus);

	DDX_Text(pDX, IDC_EDIT_EQUIP, m_strEquipNo);
	
}

BEGIN_MESSAGE_MAP(CInnoMotion_TestDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_OPEN, &CInnoMotion_TestDlg::OnBnClickedButtonOpen)
	ON_BN_CLICKED(IDC_BUTTON_SERVO1, &CInnoMotion_TestDlg::OnBnClickedButtonServo1)
	ON_BN_CLICKED(IDC_BUTTON_SERVO2, &CInnoMotion_TestDlg::OnBnClickedButtonServo2)
	ON_BN_CLICKED(IDC_BUTTON_SERVO3, &CInnoMotion_TestDlg::OnBnClickedButtonServo3)
	ON_BN_CLICKED(IDC_BUTTON_ALARM1, &CInnoMotion_TestDlg::OnBnClickedButtonAlarm1)
	ON_BN_CLICKED(IDC_BUTTON_ALARM2, &CInnoMotion_TestDlg::OnBnClickedButtonAlarm2)
	ON_BN_CLICKED(IDC_BUTTON_ALARM3, &CInnoMotion_TestDlg::OnBnClickedButtonAlarm3)
	ON_BN_CLICKED(IDC_BUTTON_ENCODER_RESET, &CInnoMotion_TestDlg::OnBnClickedButtonEncoderReset)
	ON_BN_CLICKED(IDC_BUTTON_SETTLE, &CInnoMotion_TestDlg::OnBnClickedButtonSettle)
	ON_BN_CLICKED(IDC_BUTTON_NEUTRAL, &CInnoMotion_TestDlg::OnBnClickedButtonNeutral)
	ON_BN_CLICKED(IDC_BUTTON_OPERATION, &CInnoMotion_TestDlg::OnBnClickedButtonOperation)
	ON_BN_CLICKED(IDC_BUTTON_STOP, &CInnoMotion_TestDlg::OnBnClickedButtonStop)
	ON_BN_CLICKED(IDOK, &CInnoMotion_TestDlg::OnBnClickedOk)
	ON_WM_TIMER()
END_MESSAGE_MAP()


// CInnoMotion_TestDlg 메시지 처리기

BOOL CInnoMotion_TestDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 시스템 메뉴에 "정보..." 메뉴 항목을 추가합니다.

	// IDM_ABOUTBOX는 시스템 명령 범위에 있어야 합니다.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 이 대화 상자의 아이콘을 설정합니다. 응용 프로그램의 주 창이 대화 상자가 아닐 경우에는
	//  프레임워크가 이 작업을 자동으로 수행합니다.
	SetIcon(m_hIcon, TRUE);			// 큰 아이콘을 설정합니다.
	SetIcon(m_hIcon, FALSE);		// 작은 아이콘을 설정합니다.

	// TODO: 여기에 추가 초기화 작업을 추가합니다.
	
	m_bConnected = FALSE;
	m_bTimerOperation = FALSE;
	m_bTimerMotionInfo = FALSE;
	m_bTimerMotionOut = FALSE;

	SetDlgItemText(IDC_EDIT_AMP_HEAVE, _T("0"));
	SetDlgItemText(IDC_EDIT_AMP_ROLL, _T("0"));
	SetDlgItemText(IDC_EDIT_AMP_PITCH, _T("0"));

	SetDlgItemText(IDC_EDIT_FREQ_HEAVE, _T("0"));
	SetDlgItemText(IDC_EDIT_FREQ_ROLL, _T("0"));
	SetDlgItemText(IDC_EDIT_FREQ_PITCH, _T("0"));
	m_staticStatus.SetWindowTextW(_T("OFF"));

	return TRUE;  // 포커스를 컨트롤에 설정하지 않으면 TRUE를 반환합니다.
}

void CInnoMotion_TestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 대화 상자에 최소화 단추를 추가할 경우 아이콘을 그리려면
//  아래 코드가 필요합니다. 문서/뷰 모델을 사용하는 MFC 응용 프로그램의 경우에는
//  프레임워크에서 이 작업을 자동으로 수행합니다.

void CInnoMotion_TestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 그리기를 위한 디바이스 컨텍스트입니다.

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 클라이언트 사각형에서 아이콘을 가운데에 맞춥니다.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 아이콘을 그립니다.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// 사용자가 최소화된 창을 끄는 동안에 커서가 표시되도록 시스템에서
//  이 함수를 호출합니다.
HCURSOR CInnoMotion_TestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CInnoMotion_TestDlg::OnBnClickedButtonOpen()
{
	CString strConnetion;
	GetDlgItemText(IDC_BUTTON_OPEN, strConnetion);
	
	int nRet;
	if(strConnetion == _T("Open"))
	{
		UpdateData();

		int nEquipNo;
		nEquipNo = _ttoi(m_strEquipNo);

		InnoMotion.SetEquipNumber(nEquipNo);

		nRet = InnoMotion.OpenDevice();

		if(nRet == 0)
		{
			AfxMessageBox(_T("Connection success"));
			
			SetDlgItemText(IDC_BUTTON_SERVO1, _T("OFF"));
			SetDlgItemText(IDC_BUTTON_SERVO2, _T("OFF"));
			SetDlgItemText(IDC_BUTTON_SERVO3, _T("OFF"));

			InnoMotion.SetServoOnOff(0, ON);
			InnoMotion.SetServoOnOff(1, ON);
			InnoMotion.SetServoOnOff(2, ON);


			InnoMotion.SetAlarmOnOff(0, ON);
			InnoMotion.SetAlarmOnOff(1, ON);
			InnoMotion.SetAlarmOnOff(2, ON);

			m_bConnected = TRUE;

			GetDlgItem(IDC_BUTTON_SERVO1)->EnableWindow(TRUE);
			GetDlgItem(IDC_BUTTON_SERVO2)->EnableWindow(TRUE);
			GetDlgItem(IDC_BUTTON_SERVO3)->EnableWindow(TRUE);

			GetDlgItem(IDC_BUTTON_ALARM1)->EnableWindow(TRUE);
			GetDlgItem(IDC_BUTTON_ALARM2)->EnableWindow(TRUE);
			GetDlgItem(IDC_BUTTON_ALARM3)->EnableWindow(TRUE);

			GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(TRUE);

			SetTimer(TIMER_MOTIONINFO, TIMER_MOTIONINFO_INTERVAL, NULL);

			InnoMotion.SetAlarmOnOff(0, OFF);
			InnoMotion.SetAlarmOnOff(1, OFF);
			InnoMotion.SetAlarmOnOff(2, OFF);

			m_staticStatus.SetWindowTextW(_T("READY"));
			
		}
		else
		{
			m_bConnected = FALSE;
			AfxMessageBox(_T("Connection failed"));
		}
	}
	else
	{
		KillTimer(TIMER_MOTIONINFO);

		InnoMotion.SetServoOnOff(0, OFF);
		InnoMotion.SetServoOnOff(1, OFF);
		InnoMotion.SetServoOnOff(2, OFF);

		InnoMotion.CloseDevice();
		m_bConnected = FALSE;

		AfxMessageBox(_T("Connection close"));
	}

	if(m_bConnected == TRUE)
		SetDlgItemText(IDC_BUTTON_OPEN, _T("Close"));
	else
		SetDlgItemText(IDC_BUTTON_OPEN, _T("Open"));

}


void CInnoMotion_TestDlg::OnBnClickedButtonServo1()
{
	if( m_bConnected == FALSE )    
		return;

	CString str;
	int nRet;
	GetDlgItemText(IDC_BUTTON_SERVO1,str);

	if( str == "ON" )
	{
		nRet = InnoMotion.SetServoOnOff(0, ON);
		SetDlgItemText(IDC_BUTTON_SERVO1,_T("OFF"));
	}
	else
	{
		nRet = InnoMotion.SetServoOnOff(0, OFF);
		SetDlgItemText(IDC_BUTTON_SERVO1,_T("ON"));
	}

	if( nRet == -1)
		m_bConnected = FALSE;

	
}


void CInnoMotion_TestDlg::OnBnClickedButtonServo2()
{
	if( m_bConnected == FALSE )    
		return;

	CString str;
	int nRet;
	GetDlgItemText(IDC_BUTTON_SERVO2,str);

	if( str == "ON" )
	{
		nRet = InnoMotion.SetServoOnOff(1, ON);
		SetDlgItemText(IDC_BUTTON_SERVO2,_T("OFF"));
	}
	else
	{
		nRet = InnoMotion.SetServoOnOff(1, OFF);
		SetDlgItemText(IDC_BUTTON_SERVO2,_T("ON"));
	}

	if( nRet == -1)
		m_bConnected = FALSE;

}


void CInnoMotion_TestDlg::OnBnClickedButtonServo3()
{
	if( m_bConnected == FALSE )    
		return;

	CString str;
	int nRet;
	GetDlgItemText(IDC_BUTTON_SERVO3,str);

	if( str == "ON" )
	{
		nRet = InnoMotion.SetServoOnOff(2, ON);
		SetDlgItemText(IDC_BUTTON_SERVO3,_T("OFF"));
	}
	else
	{
		nRet = InnoMotion.SetServoOnOff(2, OFF);
		SetDlgItemText(IDC_BUTTON_SERVO3,_T("ON"));
	}

	if( nRet == -1)
		m_bConnected = FALSE;

}


void CInnoMotion_TestDlg::OnBnClickedButtonAlarm1()
{
	if( m_bConnected == FALSE )    
		return;
	int nRet;

	nRet = InnoMotion.SetAlarmOnOff(0, ON);

	if( nRet == -1)
		m_bConnected = FALSE;

}


void CInnoMotion_TestDlg::OnBnClickedButtonAlarm2()
{
	if( m_bConnected == FALSE )    
		return;
	int nRet;

	nRet = InnoMotion.SetAlarmOnOff(1, ON);

	if( nRet == -1)
		m_bConnected = FALSE;

}


void CInnoMotion_TestDlg::OnBnClickedButtonAlarm3()
{
	if( m_bConnected == FALSE )    
		return;
	int nRet;

	nRet = InnoMotion.SetAlarmOnOff(2, ON);

	if( nRet == -1)
		m_bConnected = FALSE;

}


void CInnoMotion_TestDlg::OnBnClickedButtonEncoderReset()
{
	if( m_bConnected == FALSE )    
		return;

	InnoMotion.ResetEncoder();
}


void CInnoMotion_TestDlg::OnBnClickedButtonSettle()
{
	if( m_bConnected == FALSE )    
		return;

	nRetSettle = InnoMotion.SetSettle();
	
	if( nRetSettle == -1)
		m_bConnected = FALSE;
	
}


void CInnoMotion_TestDlg::OnBnClickedButtonNeutral()
{
	if( m_bConnected == FALSE )    
		return;
	
	nRetNeutral = InnoMotion.SetNeutral();

	if( nRetNeutral == -1)
		m_bConnected = FALSE;


}


void CInnoMotion_TestDlg::OnBnClickedButtonOperation()
{
	if( m_bConnected == FALSE )    
		return;
	
	CString strAmpHeave, strAmpRoll, strAmpPitch;
	CString strFreqHeave, strFreqRoll, strFreqPitch;


	GetDlgItemText(IDC_EDIT_AMP_HEAVE, strAmpHeave);
	GetDlgItemText(IDC_EDIT_AMP_ROLL, strAmpRoll);
	GetDlgItemText(IDC_EDIT_AMP_PITCH, strAmpPitch);

	GetDlgItemText(IDC_EDIT_FREQ_HEAVE, strFreqHeave);
	GetDlgItemText(IDC_EDIT_FREQ_ROLL, strFreqRoll);
	GetDlgItemText(IDC_EDIT_FREQ_PITCH, strFreqPitch);
		
	m_dAmpHeave		= _ttof(strAmpHeave);
	m_dAmpRoll		= _ttof(strAmpRoll);
	m_dAmpPitch		= _ttof(strAmpPitch);

	m_dFreqHeave	= _ttof(strFreqHeave);
	m_dFreqRoll		= _ttof(strFreqRoll);
	m_dFreqPitch	= _ttof(strFreqPitch);

	if(m_dAmpHeave > 30)
	{
		m_dAmpHeave = 30;
		SetDlgItemText(IDC_EDIT_AMP_HEAVE, _T("30"));
	}
	else if(m_dAmpHeave < -30)
	{
		m_dAmpHeave = -30;
		SetDlgItemText(IDC_EDIT_AMP_HEAVE, _T("-30"));
	}

	if(m_dAmpRoll > 10)
	{
		m_dAmpRoll = 10;
		SetDlgItemText(IDC_EDIT_AMP_ROLL, _T("10"));
	}
	else if(m_dAmpRoll < -10)
	{
		m_dAmpRoll = -10;
		SetDlgItemText(IDC_EDIT_AMP_ROLL, _T("-10"));
	}

	if(m_dAmpPitch > 10)
	{
		m_dAmpPitch = 10;
		SetDlgItemText(IDC_EDIT_AMP_PITCH, _T("10"));
	}
	else if(m_dAmpPitch < -10)
	{
		m_dAmpPitch = -10;
		SetDlgItemText(IDC_EDIT_AMP_PITCH, _T("-10"));
	}
	


	GetDlgItem(IDC_EDIT_AMP_HEAVE)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_AMP_ROLL)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_AMP_PITCH)->EnableWindow(FALSE);

	GetDlgItem(IDC_EDIT_FREQ_HEAVE)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FREQ_ROLL)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FREQ_PITCH)->EnableWindow(FALSE);

	GetDlgItem(IDC_BUTTON_OPERATION)->EnableWindow(FALSE);
	GetDlgItem(IDC_BUTTON_STOP)->EnableWindow(TRUE);

	GetDlgItem(IDC_BUTTON_ENCODER_RESET)->EnableWindow(FALSE);
	GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(FALSE);
	GetDlgItem(IDC_BUTTON_NEUTRAL)->EnableWindow(FALSE);


	//InnoMotion.SetOperation();

	m_bTimerOperation = TRUE;
	SetTimer(TIMER_OPERTATION, TIMER_OPERTATION_INTERVAL, NULL);


}


void CInnoMotion_TestDlg::OnBnClickedButtonStop()
{
	KillTimer(TIMER_OPERTATION);

	m_bTimerOperation = FALSE;

	GetDlgItem(IDC_EDIT_AMP_HEAVE)->EnableWindow(TRUE);
	GetDlgItem(IDC_EDIT_AMP_ROLL)->EnableWindow(TRUE);
	GetDlgItem(IDC_EDIT_AMP_PITCH)->EnableWindow(TRUE);

	GetDlgItem(IDC_EDIT_FREQ_HEAVE)->EnableWindow(TRUE);
	GetDlgItem(IDC_EDIT_FREQ_ROLL)->EnableWindow(TRUE);
	GetDlgItem(IDC_EDIT_FREQ_PITCH)->EnableWindow(TRUE);

	GetDlgItem(IDC_BUTTON_OPERATION)->EnableWindow(FALSE);
	GetDlgItem(IDC_BUTTON_STOP)->EnableWindow(FALSE);

	GetDlgItem(IDC_BUTTON_ENCODER_RESET)->EnableWindow(FALSE);
	GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(TRUE);
	GetDlgItem(IDC_BUTTON_NEUTRAL)->EnableWindow(TRUE);

	m_staticStatus.SetWindowTextW(_T("STOP"));
}


void CInnoMotion_TestDlg::OnBnClickedOk()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	if(m_bTimerOperation == TRUE)
		KillTimer(TIMER_OPERTATION);

	if(m_bTimerMotionInfo == TRUE)
		KillTimer(TIMER_MOTIONINFO);

	CDialogEx::OnOK();
}


void CInnoMotion_TestDlg::SetMotionInfo()
{
	CString strBusy1, strBusy2, strBusy3;
	CString strHome1, strHome2, strHome3;
	CString strAlarm1, strAlarm2, strAlarm3;
	CString strInpos1, strInpos2, strInpos3;

	CString strCmd1, strCmd2, strCmd3;
	CString strEnc1, strEnc2, strEnc3;

	CString strEmer;

	CString strCmd, strEnc;
	
	int iBusy0, iBusy1, iBusy2; 

	iBusy0 = InnoMotion.ReadBusyData(0);
	iBusy1 = InnoMotion.ReadBusyData(1);
	iBusy2 = InnoMotion.ReadBusyData(2);

	m_staticBusy1.SetWindowTextW(InnoMotion.ReadBusyData(0)?_T("1"):_T("0"));
	m_staticBusy2.SetWindowTextW(InnoMotion.ReadBusyData(1)?_T("1"):_T("0"));
	m_staticBusy3.SetWindowTextW(InnoMotion.ReadBusyData(2)?_T("1"):_T("0"));

	m_staticEmer.SetWindowTextW(InnoMotion.ReadEmerData()?_T("1"):_T("0"));

	m_staticAlarm1.SetWindowTextW(InnoMotion.ReadAlarmData(0)?_T("1"):_T("0"));
	m_staticAlarm2.SetWindowTextW(InnoMotion.ReadAlarmData(1)?_T("1"):_T("0"));
	m_staticAlarm3.SetWindowTextW(InnoMotion.ReadAlarmData(2)?_T("1"):_T("0"));

	m_staticInpos1.SetWindowTextW(InnoMotion.ReadInpoData(0)?_T("1"):_T("0"));
	m_staticInpos2.SetWindowTextW(InnoMotion.ReadInpoData(1)?_T("1"):_T("0"));
	m_staticInpos3.SetWindowTextW(InnoMotion.ReadInpoData(2)?_T("1"):_T("0"));

	strCmd.Format(_T("%.3f"), InnoMotion.ReadCmdData(0));	m_staticCmd1.SetWindowTextW(strCmd);
	strCmd.Format(_T("%.3f"), InnoMotion.ReadCmdData(1));	m_staticCmd2.SetWindowTextW(strCmd);
	strCmd.Format(_T("%.3f"), InnoMotion.ReadCmdData(2));	m_staticCmd3.SetWindowTextW(strCmd);

	strEnc.Format(_T("%.3f"), InnoMotion.ReadEncData(0));	m_staticEnc1.SetWindowTextW(strEnc);
	strEnc.Format(_T("%.3f"), InnoMotion.ReadEncData(1));	m_staticEnc2.SetWindowTextW(strEnc);
	strEnc.Format(_T("%.3f"), InnoMotion.ReadEncData(2));	m_staticEnc3.SetWindowTextW(strEnc);

	if (nRetSettle == 0)
	{
		GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_NEUTRAL)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_OPERATION)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_STOP)->EnableWindow(FALSE);
		m_staticStatus.SetWindowTextW(_T("MOVING..."));
		if (iBusy0 == 0 && iBusy1 == 0 && iBusy2 == 0) 
		{
			GetDlgItem(IDC_BUTTON_NEUTRAL)->EnableWindow(TRUE);
			m_staticStatus.SetWindowTextW(_T("SETTELED"));
			nRetSettle = 1;
		}
	}
	
	if (nRetNeutral== 0)
	{
		GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_NEUTRAL)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_OPERATION)->EnableWindow(FALSE);
		GetDlgItem(IDC_BUTTON_STOP)->EnableWindow(FALSE);
		m_staticStatus.SetWindowTextW(_T("MOVING..."));
		if (iBusy0 == 0 && iBusy1 == 0 && iBusy2 == 0) 
		{
			GetDlgItem(IDC_BUTTON_OPERATION)->EnableWindow(TRUE);
			GetDlgItem(IDC_BUTTON_SETTLE)->EnableWindow(TRUE);
			nRetNeutral = 1;
			m_staticStatus.SetWindowTextW(_T("NEUTRAL"));
		}
	}

}

void CInnoMotion_TestDlg::OnTimer(UINT_PTR nIDEvent)
{
	switch(nIDEvent)
	{
	case TIMER_OPERTATION:
		InnoMotion.SetOperation(m_dAmpHeave, m_dAmpRoll, m_dAmpPitch, m_dFreqHeave, m_dFreqRoll, m_dFreqPitch);
		m_staticStatus.SetWindowTextW(_T("OPERATING..."));
		break;
	case TIMER_MOTIONINFO:
		SetMotionInfo();
		break;
	}

	CDialogEx::OnTimer(nIDEvent);
}


