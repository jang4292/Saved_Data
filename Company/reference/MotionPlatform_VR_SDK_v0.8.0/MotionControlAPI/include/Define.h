#ifndef _IM_DEFINE_H
#define _IM_DEFINE_H

#ifdef __cplusplus
extern "C"{
#endif


// For Set
#define OFF			0
#define ON			1
#define AXIS_NUM	3
	
#ifndef BOOL
typedef int		BOOL;
#endif

#ifndef HANDLE
typedef void*	HANDLE;
#endif

typedef struct NMC_Read_Motion_Info
{
	BOOL bBusy[AXIS_NUM];
	BOOL bHome[AXIS_NUM];
	BOOL bAlarm[AXIS_NUM];
	BOOL bInpos[AXIS_NUM];
	BOOL bEmer;
	double dCmd[AXIS_NUM];
	double dEnc[AXIS_NUM];
}NMCREADMOTIONINFO;

typedef struct NMC_Read_Motion_out
{
	BOOL bCurrentOn[AXIS_NUM];
	BOOL bServoOn[AXIS_NUM];
	BOOL bDCCOn[AXIS_NUM];
	BOOL bAlarmResetOn[AXIS_NUM];
}NMCREADMOTIONOUT;


#ifdef __cplusplus
}
#endif

#endif // _IM_DEFINE_H