
#ifndef _IMOTION_TYPES_H_
#define _IMOTION_TYPES_H_

#ifdef __cplusplus
extern "C"{
#endif
	
#ifndef IM_DRIVER_STATIC_LIB
#	ifdef IM_DRIVER_EXPORTS
#		define IM_DRIVER_DLL_API __declspec(dllexport)
#	else
#		define IM_DRIVER_DLL_API __declspec(dllimport)
#	endif
#else
#  define IM_DRIVER_DLL_API
#endif
	
#ifdef __cplusplus
#	define IMDEFAULT(x) =x
#else
#	define IMDEFAULT(x)
#endif
	
/**
 *  \name common types
 *
 *  공통 자료형을 선언 합니다 
 */
typedef unsigned char	uint8;
typedef char			int8;
typedef unsigned short	uint16;
typedef short int		int16;
typedef unsigned int	uint32;
typedef int				int32;
typedef unsigned __int64 uint64;
typedef __int64			int64;

/**
 *  \name common macro
 *
 *  자주 사용되는 공용 매크로를 선언 합니다.
 */
#define IM_STRING_MAX	256
#define IM_PI			3.14159265358979323846
#define IM_MAXSHORT		0x7FFF
#define IM_MAXINT		0x7FFFFFFF
#define IM_MAX_ENUM		0x7FFFFFFF

#define MOTION_SAMPLE_BIT(fmt)	(fmt & 0xff)
#define MOTION_SAMPLE_BYTE(fmt)	(MOTION_SAMPLE_BIT(fmt) >> 3)

#define MOTION_MAX_VAL(bits)	((1 << (bits - 1)) - 1)
#define MOTION_MAX_16			MOTION_MAX_VAL(16)

#define MOTION_MIN_VAL(bits)	-(1 << (bits - 1))
#define MOTION_MIN_16			MOTION_MIN_VAL(16)

/**
 *  \name IM_CFG_*
 *
 *  IMotion 드라이버 시동 옵션을 선언 합니다. (조합 가능)
 *  (Used in IMotion_Startup flags)
 */
#define IM_CFG_DEBUG_MODE	0x1
#define IM_CFG_FILE_LOG		0x2		// check "IMotion.log"
#define IM_CFG_EMUL_MODE	0x4

/**
 *  \name IM_DEVICE_*
 *
 *  IMotion 디바이스 연결 및 해제 옵션을 선언 합니다.
 *  (Used in IMotion_Create and IMotion_Destroy flags)
 */
#define IM_DEVICE_ID_DEFAULT		11	/**< default device IP */
#define IM_DEVICE_ID_MAX			255	/**< 1 ~ 255 */
#define IM_DEVICE_MOVE_NEUTRAL		0x1 /**< Used in IMotion_Stop flags */
#define IM_DEVICE_MOTOR_POWER_OFF	0x1 /**< Used in IMotion_Destroy flags */

/**
 *  \name IM_FORMAT_*
 *
 *  IMotion 포맷 매크로를 선언 합니다. - 타입, 샘플레이트, 데이터 포맷, 채널 등...
 *  (Used in IMotion_CreateSource)
 */
#define IM_FORMAT_TYPE_DOF				0	/**< Degree of freedom */
#define IM_FORMAT_TYPE_AXIS				1	/**< Reserved */
#define IM_FORMAT_TYPE_MATRIX			2	/**< Reserved */

#define IM_FORMAT_SAMPLE_RATE_MAX		200	/**< 5 ms */
#define IM_FORMAT_CHANNELS_MAX			8	/**< 8-DOF (heave, sway, surge, roll, pitch, yaw) */

#define IM_FORMAT_DATA_S16				0x8010	/**< Signed 16-bit samples */
#define IM_FORMAT_DATA_S32				0x8020  /**< Reserved */
#define IM_FORMAT_DATA_F32				0x8120  /**< Reserved */

#define IM_FORMAT_TYPE_DEFAULT			IM_FORMAT_TYPE_DOF	/**< DOF type */
#define IM_FORMAT_SAMPLE_RATE_DEFAULT	50	/**< 20 ms */
#define IM_FORMAT_CHANNELS_DEFAULT		3	/**< 3-DOF (heave, roll, pitch) */
#define IM_FORMAT_DATA_DEFAULT			IM_FORMAT_DATA_S16	/**< (-32768 ~ 32767) */

/**
 *  \name IM_LOOP_*
 *
 *  IMotion 소스 반복 재생 매크로를 선언 합니다.
 *  (Used in IM_BUFFER.nLoopCount)
 */
#define IM_LOOP_MAX			254
#define IM_LOOP_INFINITE	255		

/**
 *  \name IM_END_*
 *
 *  IMotion 소스 재생 매크로를 선언 합니다.
 *  (Used in IM_BUFFER.Flags)
 */
#define IM_ERROR_STATE		0x1
#define IM_START_OF_BUFFER	0x2
#define IM_END_OF_LOOP		0x4		
#define IM_END_OF_BUFFER	0x8		// current buffer
#define IM_END_OF_STREAM	0x10	

/**
 * @brief 모션 포맷 구조체
 * @details 모션 소스는 모션 포맷 정의에 따라 모션 버퍼를 실행 합니다. 
 * 이 구조체는 IMotion_CreateSource() 함수로 소스를 생성할때 사용되고 소스를 제거할때 까지 변경이 불가능 합니다.  
 * nBlockAlign을 0으로 설정 하면 자동 계산되므로, 사용전에 메모리를 초기화(i.e. memset()) 하는것이 좋습니다.  
 */
typedef struct {
    uint32		nType;			/**< motion format type */
    uint32		nSampleRate;	/**< samples per second */
    uint32		nChannels;		/**< number of channels (i.e. heave, roll, pitch, etc.) */
    uint32		nDataFormat;	/**< always IM_FORMAT_DATA_S16 */
    uint32		nBlockAlign;	/**< block size of data (i.e. nBlockAlign = nChannels * MOTION_SAMPLE_BYTE(nDataFormat)) */
} IM_FORMAT;

/**
 * @brief 모션 버퍼 구조체
 * @details 모션 버퍼는 모션 소스에 의해 실행 되는 실제 모션 데이터를 스트림으로 정의해야 합니다. 
 * 이 구조체는 IMotionSource_SubmitBuffer() 함수를 통해 소스에 큐잉 됩니다. 반복 재생 설정과 콜백 함수(IMotionCallback()에 전달할 사용자 context를 지정 할수 있습니다.
 * 모션 버퍼 스트림을 위해 복수의 버퍼들이 큐잉될 수 있는데, 스트림의 마지막 버퍼의 nFlags에 IM_END_OF_STREAM를 지정 하여 스트림 재생 완료 상태를 판단 합니다.
 */
typedef struct {
    uint32		nFlags;			/**< Either 0 or IM_END_OF_STREAM. */
    uint32		nMotionBytes;	/**< Size of the motion data buffer in bytes. */
    const uint8* pMotionData;	/**< Pointer to the motion data buffer. */
    uint32		nPlayBegin;		/**< First sample in this buffer to be played. */
    uint32		nPlayLength;	/**< Length of the region to be played in samples, or 0 to play the whole buffer. */
    uint32		nLoopBegin;		/**< First sample of the region to be looped. */
    uint32		nLoopLength;	/**< Length of the desired loop region in samples, or 0 to loop the entire buffer. */
    uint32		nLoopCount;		/**< Number of times to repeat the loop region, or IM_LOOP_INFINITE to loop forever. */
    void*		pContext;		/**< Context value to be passed back in callbacks. */
} IM_BUFFER;

/**
 * @brief 모션 디바이스 설명 구조체 
 * @details 모션 디바이스의 이름 및 상세 설명을 얻기 위해 이 구조체를 사용 합니다. IMotion는 디바이스 종류별로 지원 가능한 장치의 특징과 설명이 필요 할 수 있습니다.
 * 이 구조체는 IMotion_GetDeviceDescription() 함수를 통해 디바이스의 상세 정보를 얻을 수 있습니다. 
 * IMotion_GetDeviceCount() 함수와 함께 사용 하면 모든 지원 디바이스의 정보를 얻을 수 있습니다. (main_csv.cpp 예제 참고)
 */
typedef struct {
	uint32		nId;					/**< Motion Device Index. */
	uint32		nType;					/**< Either Out device or Capture device. */
	uint32		nFormat;				/**< Reserved Channel info */
	char		szName[IM_STRING_MAX];	/**< Motion Device Name. */
	char		szDetail[IM_STRING_MAX];/**< Detailed description of motion device. */
} IM_DEVICE_DESC;

/**
 * @brief 모션 디바이스 상태 진단 구조체
 * @details 모션 디바이스(H/W) 상태를 조회하기 위해 이 구조체를 사용 합니다. 디바이스에 전달된 제어는 비동기 명령이기 때문에 상태 조회와 에러 정보가 필요할 수 있습니다.
 * 이 구조체는 IMotion_GetInfo() 함수를 통해 디바이스의 상태 정보를 얻을 수 있습니다. (주의, 빈번한 상태 조회는 성능 부하를 줄수 있습니다)
 */
typedef struct {
	int32	bBusy;		/**< Device Running Status. */
	int32	bHome;		/**< Reserved Home Info. */
	int32	bAlarm;		/**< Alarm Error Status. */
	int32	bInpos;		/**< Reserved Home Info. */
	int32	bEmer;		/**< Emergency Error Status. */
	double	dCmd;		/**< Mean of Axis Commands. */
	double	dEnc;		/**< Mean of Axis Encoder. */
} IM_DIAGNOSTIC_INFO;

/**
 * @brief 모션 소스 콜백 함수의 프로토타입을 선언 합니다.
 * @param data : 모션 버퍼에서 지정된 User Context
 * @param state : 모션 버퍼의 재생 상태 값 (IM_END_* 재생 상태 매크로 참고)
 * @return	없음
 * @remarks 모션 소스는 스레드에서 실행 되기 때문에 이 함수의 상태 정보를 통해 정확한 재생 상태를 알 수 있습니다.
 */
typedef void (*IMotionCallback)(void* data, uint32 state);

#ifdef __cplusplus
}
#endif

#endif // _IMOTION_TYPES_H_