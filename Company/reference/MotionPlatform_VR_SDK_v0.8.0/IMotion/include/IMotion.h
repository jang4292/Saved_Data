
#ifndef _IMOTION_H_
#define _IMOTION_H_

#include "IMotion_types.h"

#ifdef __cplusplus
#include "IMotion.hpp"
#else
typedef void IMotion;
typedef void IMotionSource;
#endif

#ifdef __cplusplus
extern "C"{
#endif	
	
/************************************
 * @section 모션 드라이버 인터페이스 
 ************************************/

/**
 * @brief 모션 라이브러리 시동 합니다.
 * @param flags : 라이브러리 시동 옵션
 * @param param : flags 설정에 따른 파라미터 값(항상 0)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 라이브러리의 실행 모드를 정의 할 수 있습니다. 0을 입력하면 기본값으로 설정됩니다.
 *         IM_CFG_DEBUG_MODE는 디버깅 로그 출력, IM_CFG_FILE_LOG는 파일 출력, IM_CFG_EMUL_MODE는 에뮬레이션 모드입니다. (참고, IMotion_Shutdown)
 */
IM_DRIVER_DLL_API int32 IMotion_Startup(uint32 flags IMDEFAULT(0), uint32 param IMDEFAULT(0));


/**
 * @brief 모션 라이브러리 종료 합니다.
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 라이브러리 실행 중인 모든 메모리를 해제 합니다. 
 *         라이브러리의 시동과 종료의 반복 수행은 프로그램 실행 성능에 영향을 줍니다. 
 */
IM_DRIVER_DLL_API int32 IMotion_Shutdown();

/**
 * @brief 모션 라이브러리의 지원 디바이스 수를 얻습니다.
 * @return	지원 디바이스 드라이버의 갯수
 * @remarks 모션 라이브러리가 지원하는 지원 디바이스의 수를 얻습니다. HW 디바이스 및 SW 에뮬레이션 드라이버를 포함 합니다.
 *         디바이스의 갯수는 연결되어 있는 장치의 수가 아닌 지원 가능한 장치 드라이버의 수 입니다. 
 * 			IMotion_GetDeviceDescription 함수와 함께 사용하여 생성할 디바이스 이름을 얻을 수 있습니다.
 */
IM_DRIVER_DLL_API int32 IMotion_GetDeviceCount();

/**
 * @brief 모션 디바이스 상세 정보를 얻습니다.
 * @param index : 조회를 원하는 지원 디바이스 인덱스 (0 ~ IMotion_GetDeviceCount()-1)
 * @param desc : 상세 정보를 얻기 원하는 구조체 포인터 (IM_DEVICE_DESC 참고)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스 상세 정보를 통해 디바이스 생성 정보를 얻을 수 있으며, 디바이스의 지원 범위를 알 수 있습니다. 
 *         IMotion_Create 함수의 devname 인자에 조회된 디바이스 이름으로 디바이스를 생성 할 수 있습니다.
 */
IM_DRIVER_DLL_API int32 IMotion_GetDeviceDescription(int32 index, IM_DEVICE_DESC* desc IMDEFAULT(0));	

/**
 * @brief 모션 디바이스 객체를 생성합니다.
 * @param devname : 생성할 디바이스 이름 (기본은 모션 시트 입니다)
 * @param devid : 생성할 디바이스 식별자 (연결할 IP 번호, 기본은 11번 입니다)
 * @return	생성된 디바이스 객체 포인터 
 * @remarks 모션 디바이스 객체 생성에서 디바이스 이름 중복 사용이 가능 하지만. ID의 중복 사용은 불가능 합니다.
 *         모션 디바이스 객체는 고유 식별 ID를 동시 지원 최대수는 256 입니다 (1~31 ID 권장). 객체의 생성은 장치 IP 접속과 모션 테이블을 초기 상태로 변경 합니다.
 */
IM_DRIVER_DLL_API IMotion* IMotion_Create(const char* devname IMDEFAULT(0), uint32 devid IMDEFAULT(IM_DEVICE_ID_DEFAULT));

/**
 * @brief 모션 디바이스 객체를 제거합니다.
 * @param device : 디바이스 객체 포인터 
 * @param flags : 디바이스 객체 제거 옵션
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스 객체 제거에서 모션 테이블를 초기화 하고 디바이스 연결을 해제 합니다.
 *         IM_DEVICE_MOTOR_POWER_OFF 옵션 사용은 모터의 전원을 OFF 후 연결을 해제 합니다 . 
 */
IM_DRIVER_DLL_API int32 IMotion_Destroy(IMotion* device, uint32 flags IMDEFAULT(0));

/************************************
 * @section 모션 디바이스 인터페이스 
 ************************************/

/**
 * @brief 모션 디바이스 제어를 시작 합니다.
 * @param device : 디바이스 객체 포인터
 * @param data : 모션 테이블의 초기 위치 데이터 (미사용, 항상 Identity Matrix)
 * @param size : 모션 테이블의 초기 위치 데이터 크기 (미사용, 항상 Identity Matrix)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스의 테이블 초기 위치로 이동하여 모션 제어를 준비 합니다. 초기 위치로 이동에 약간의 시간이 소요 되므로, 사용자 준비 완료 후 호출을 권장합니다.
 *         대부분의 모션 테이블의 초기 변환 행렬은 Identity Matrix를 사용 하지만, 임의의 자세를 원하는 경우 이 함수 사용을 생략할 수 있습니다.
 */
IM_DRIVER_DLL_API int32 IMotion_Start(IMotion* device, uint8* data IMDEFAULT(0), uint32 size IMDEFAULT(0)); 

/**
 * @brief 모션 디바이스 제어를 종료 합니다.
 * @param device : 디바이스 객체 포인터
 * @param flags : 모션 디바이스 종료 옵션 (미사용)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스를 초기 상태로 변경 합니다. IMotion_Destroy 에서 이 함수를 호출 하지만 명시적인 호출이 가능 합니다.
 *         모션 초기 상태 변경이 필요한 경우 이 함수와 함께 IMotion_Start 함수의 중복 사용이 가능 합니다.
 */
IM_DRIVER_DLL_API int32 IMotion_Stop(IMotion* device, uint32 flags IMDEFAULT(0)); 

/**
 * @brief 모션 디바이스에 자세 데이터를 전송 종료 합니다.
 * @param device : 디바이스 객체 포인터
 * @param data : 모션 테이블의 자세 데이터 포인터 (short 형 데이터 스트림 포인터)
 * @param size : 모션 테이블의 자세 데이터 크기 (channels * 2 bytes)
 * @param count : 모션 테이블의 자세의 갯수 (미사용, 항상 1)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스에 데이터 스트림을 전달하여, 모션 디바이스의 테이블 자세를 직접 변경 합니다. 
 *         이 함수는 비동기 함수로 성능 범위를 초과한 사용은 HW 장애를 유발 할 수 있습니다 (IM_FORMAT_SAMPLE_RATE_MAX 참고).
 */
IM_DRIVER_DLL_API int32 IMotion_SendStream(IMotion* device, uint8* data, uint32 size, uint32 count IMDEFAULT(1)); 

/**
 * @brief 모션 디바이스 상태 정보를 조회 합니다.
 * @param device : 디바이스 객체 포인터
 * @param info : 모션 디바이스 상태 정보 구조체 포인터 (이 값인 null 인 경우 에러 상태만 얻습니다.) 
 * @return	에러 코드(0: 성공, 2: emergency, 3: alarm)
 * @remarks 모션 디바이스로 부터 장치 상태를 얻습니다. HW 장애 상태 및 전송 명령 상태를 조회할 수 있습니다. 
 *         디바이스 상태 구조체는 디바이스 종류에 따라 변경 될 수 있습니다.
 */
IM_DRIVER_DLL_API int32 IMotion_GetInfo(IMotion* device, IM_DIAGNOSTIC_INFO* info IMDEFAULT(0));

/**
 * @brief 모션 디바이스 상세 정보를 조회 합니다.
 * @param device : 디바이스 객체 포인터
 * @param desc : 조회할 모션 디바이스 상태 정보 구조체 포인터
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 현재 모션 디바이스의 상세 정보를 얻습니다. (IMotion_GetDeviceDescription 참고) 
 */
IM_DRIVER_DLL_API int32 IMotion_GetDescription(IMotion* device, IM_DEVICE_DESC* desc IMDEFAULT(0)); 

/**
 * @brief 모션 디바이스 ID를 얻습니다.
 * @param device : 디바이스 객체 포인터
 * @return	모션 디바이스 ID(연결된 장치 IP)
 * @remarks 현재 모션 디바이스의 식별 ID를 얻습니다. (IMotion_Create 참고) 
 */
IM_DRIVER_DLL_API int32 IMotion_GetID(IMotion* device); 

/**
 * @brief 모션 소스 객체를 생성 합니다.
 * @param device : 디바이스 객체 포인터
 * @param format : 모션 소스 객체의 포맷 구조체 포인터 (이 값인 null 인 경우 기본값이 사용 됩니다.) 
 * @param user_func : 모션 소스 객체가 호출 할 사용자 콜백 함수 포인터 (콜백 호출이 불필요한 경우 null 사용) 
 * @param cb_flags : 모션 소스 객체의 생성 옵션 (미사용, 항상 0 인 마스터 소스 생성) 
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 디바이스의 소스 객체를 생성 합니다. 마스터 소스는 모션 디바이스 객체에 하나만 지원 합니다. 
 *         모션 소스 객체는 IM_FORMAT 정보에 따라 스레드에서 자동으로 모션 디바이스에 데이터 스트림을 전송 합니다. (IMotion_SendStream 참고)
 */
IM_DRIVER_DLL_API IMotionSource* IMotion_CreateSource(IMotion* device, const IM_FORMAT* format, IMotionCallback user_func IMDEFAULT(0), uint32 cb_flags IMDEFAULT(0)); 

/**
 * @brief 모션 소스 객체를 제거 합니다.
 * @param device : 디바이스 객체 포인터
 * @param source : 모션 소스 객체 포인터 
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 소스 객체를 제거 합니다. 마스터 소스인 경우 내부 스레드를 종료 합니다. 
 */
IM_DRIVER_DLL_API int32 IMotion_DestroySource(IMotion* device, IMotionSource* source); 
	
/************************************
 * @section 모션 소스 인터페이스 
 ************************************/

/**
 * @brief 모션 소스 객체를 시작 합니다.
 * @param source : 소스 객체 포인터
 * @param flags : 모션 소스 시작 옵션 (미사용)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 소스 객체에서 모션 버퍼 실행을 시작 합니다. 마스터 소스인 경우 내부 스레드를 시작 합니다. 
 */
IM_DRIVER_DLL_API int32 IMotionSource_Start(IMotionSource* source, uint32 flags IMDEFAULT(0));

/**
 * @brief 모션 소스 객체를 종료 합니다.
 * @param source : 소스 객체 포인터
 * @param flags : 모션 소스 종료 옵션 (미사용)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 소스 객체에서 모션 버퍼 실행을 정지 합니다. 마스터 소스인 경우 내부 스레드를 종료 합니다. 
 *         모션 소스 객체에 큐잉된 모션 버퍼들을 모두 비웁니다. 
 */
IM_DRIVER_DLL_API int32 IMotionSource_Stop(IMotionSource* source, uint32 flags IMDEFAULT(0));

/**
 * @brief 모션 소스 객체에 모션 버퍼를 제출 합니다.
 * @param source : 소스 객체 포인터
 * @param buffer : 모션 버퍼 구조체 포인터 (재생 구간 및 반복 설정 가능)
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 소스 객체의 큐에 모션 버퍼를 추가 합니다. 큐잉된 버퍼가 있고 Start 상태이면 스레드는 바로 모션 플랫폼에 스트림을 전달 합니다. 
 *         IMotion_CreateSource 함수의 콜백 함수를 통해 재생 정보를 조회할 수 있고, IMotionSource_GetQueuedBufferCount 함수로 큐잉된 버퍼의 수를 얻을 수 있습니다. 
 */
IM_DRIVER_DLL_API int32 IMotionSource_SubmitBuffer(IMotionSource* source, const IM_BUFFER* buffer);

/**
 * @brief 모션 소스 객체 큐에 추가된 버퍼의 수를 조회 합니다.
 * @param source : 소스 객체 포인터
 * @return	소스 객체에 큐잉된 버퍼의 수
 * @remarks 현재 모션 소스 객체에 큐잉된 모션 버퍼의 갯수를 얻습니다. 
 *         이 값이 0 이면 모션 소스 객체는 다시 큐잉 될때 까지 스레드에서 블럭킹 되며, 제출된 모든 작업의 완료 상태를 알 수 있습니다.
 */
IM_DRIVER_DLL_API uint32 IMotionSource_GetQueuedBufferCount(IMotionSource* source);

/**
 * @brief 모션 소스 객체가 생성된 디바이스를 조회 합니다.
 * @param source : 소스 객체 포인터
 * @return	모션 디바이스 객체 포인터
 * @remarks 모션 소스는 디바이스로 부터 생성 되고, 디바이스 마다 다른 소스가 생성 됩니다. 
 */
IM_DRIVER_DLL_API IMotion* IMotionSource_GetDevice(IMotionSource* source);

/**
 * @brief 모션 소스 객체의 포맷 정보를 조회 합니다.
 * @param source : 소스 객체 포인터
 * @param format : 조회할 모션 포맷 구조체의 포인터
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 모션 소스의 모션 포맷은 생성 시점에 결정 후 변경이 불가능 하기 때문에, 샘플 레이트의 변경과 같은 작업을 위해 모션 소스를 재생성 해야 합니다.
 */
IM_DRIVER_DLL_API int32 IMotionSource_GetFormat(IMotionSource* source, IM_FORMAT* format);

#ifdef __cplusplus
}
#endif

#endif // _IMOTION_H_