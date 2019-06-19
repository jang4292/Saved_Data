
// InnoMotion_TestDlg.h : 헤더 파일
//

#pragma once

#include "Define.h"
#include "InnoMotion_API.h"
#include "afxwin.h"

// CInnoMotion_TestDlg 대화 상자
class CInnoMotion_TestDlg : public CDialogEx
{
// 생성입니다.
public:
	CInnoMotion_TestDlg(CWnd* pParent = NULL);	// 표준 생성자입니다.

	CStatic	m_staticBusy3;
	CStatic	m_staticBusy2;
	CStatic	m_staticBusy1;

	CStatic	m_staticEmer;

	CStatic	m_staticAlarm3;
	CStatic	m_staticAlarm2;
	CStatic	m_staticAlarm1;

	CStatic	m_staticInpos1;
	CStatic	m_staticInpos2;
	CStatic	m_staticInpos3;

	CStatic	m_staticCmd1;
	CStatic	m_staticCmd2;
	CStatic	m_staticCmd3;

	CStatic	m_staticEnc1;
	CStatic	m_staticEnc2;
	CStatic	m_staticEnc3;

// 대화 상자 데이터입니다.
	enum { IDD = IDD_INNOMOTION_TEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 지원입니다.


// 구현입니다.
private:
	CInnoMotion_API InnoMotion;
	BOOL m_bConnected;
	BOOL m_bTimerOperation, m_bTimerMotionInfo, m_bTimerMotionOut;

	double m_dAmpHeave, m_dAmpRoll, m_dAmpPitch;
	double m_dFreqHeave, m_dFreqRoll, m_dFreqPitch;

	CString m_strEquipNo;

	void SetMotionInfo();

protected:
	HICON m_hIcon;

	// 생성된 메시지 맵 함수
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonOpen();
	afx_msg void OnBnClickedButtonServo1();
	afx_msg void OnBnClickedButtonServo2();
	afx_msg void OnBnClickedButtonServo3();
	afx_msg void OnBnClickedButtonAlarm1();
	afx_msg void OnBnClickedButtonAlarm2();
	afx_msg void OnBnClickedButtonAlarm3();
	afx_msg void OnBnClickedButtonEncoderReset();
	afx_msg void OnBnClickedButtonSettle();
	afx_msg void OnBnClickedButtonNeutral();
	afx_msg void OnBnClickedButtonOperation();
	afx_msg void OnBnClickedOk();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnBnClickedButtonStop();

	int nRetSettle;
	int nRetNeutral;
	CStatic m_staticStatus;
};
