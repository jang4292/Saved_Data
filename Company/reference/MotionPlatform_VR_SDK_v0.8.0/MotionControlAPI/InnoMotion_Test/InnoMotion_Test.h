
// InnoMotion_Test.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CInnoMotion_TestApp:
// �� Ŭ������ ������ ���ؼ��� InnoMotion_Test.cpp�� �����Ͻʽÿ�.
//

class CInnoMotion_TestApp : public CWinApp
{
public:
	CInnoMotion_TestApp();

// �������Դϴ�.
public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CInnoMotion_TestApp theApp;