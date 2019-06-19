
#ifndef _IM_API_H
#define _IM_API_H

#include "Define.h"

#ifdef __cplusplus
extern "C"{
#endif

#ifdef MOTION_DLL_EXPORTS
#define InnoMotion_DLL_API __declspec(dllexport)
#else
#define InnoMotion_DLL_API __declspec(dllimport)
#endif

class InnoMotion_DLL_API CInnoMotion_API
{
public:
	CInnoMotion_API(void);
	~CInnoMotion_API(void);

	void SetEquipNumber(int nEquipNo);

	int OpenDevice();
	void CloseDevice();

	// Set Function
	// Command

	int SetServoOnOff( int nAxisNo, BOOL bOnOff );
	int SetAlarmOnOff( int nAxisNo, BOOL bOnOff );

	// Get Function
	// Read
	int ReadAllData(NMCREADMOTIONINFO *ReadMotionInfoData, NMCREADMOTIONOUT *ReadMotionOutData);

	int ReadMotionInfo(NMCREADMOTIONINFO *ReadMotionInfoData);
	int ReadMotionOut(NMCREADMOTIONOUT *ReadMotionOutData);

	int ReadBusyData(int nAxisNo);
	int ReadInpoData(int nAxisNo);
	int ReadAlarmData(int nAxisNo);

	double ReadCmdData(int nAxisNo);
	double ReadEncData(int nAxisNo);

	int ReadEmerData();

	// Reset
	void ResetEncoder();

	// 
	int SetSettle();
	int SetNeutral();
	int SetOperation(double dAmpHeave, double dAmpRoll, double dAmpPitch,
					double dFreHeave, double dFreRoll, double dFrePitch);
	
	int SetType(int type);
};

	
InnoMotion_DLL_API void SetEquipNumber(int nEquipNo);

InnoMotion_DLL_API int OpenDevice();
InnoMotion_DLL_API void CloseDevice();

// Set Function
// Command

InnoMotion_DLL_API int SetServoOnOff( int nAxisNo, BOOL bOnOff );
InnoMotion_DLL_API int SetAlarmOnOff( int nAxisNo, BOOL bOnOff );

// Get Function
// Read
InnoMotion_DLL_API int ReadAllData(NMCREADMOTIONINFO *ReadMotionInfoData, NMCREADMOTIONOUT *ReadMotionOutData);

InnoMotion_DLL_API int ReadMotionInfo(NMCREADMOTIONINFO *ReadMotionInfoData);
InnoMotion_DLL_API int ReadMotionOut(NMCREADMOTIONOUT *ReadMotionOutData);

InnoMotion_DLL_API int ReadBusyData(int nAxisNo);
InnoMotion_DLL_API int ReadInpoData(int nAxisNo);
InnoMotion_DLL_API int ReadAlarmData(int nAxisNo);

InnoMotion_DLL_API double ReadCmdData(int nAxisNo);
InnoMotion_DLL_API double ReadEncData(int nAxisNo);

InnoMotion_DLL_API int ReadEmerData();

// Reset
InnoMotion_DLL_API void ResetEncoder();

// 
InnoMotion_DLL_API int SetSettle();
InnoMotion_DLL_API int SetNeutral();
InnoMotion_DLL_API int SetOperation(double dAmpHeave, double dAmpRoll, double dAmpPitch,
		double dFreHeave, double dFreRoll, double dFrePitch);

#ifdef __cplusplus
}
#endif

#endif