
#ifndef _IMOTION_CSV_H_
#define _IMOTION_CSV_H_

#include "IMotion.h"

#ifdef __cplusplus
extern "C"{
#endif	
	
/**
 * @brief 모션 파일을 로드 합니다.
 * @param filename : 모션 파일 이름 전체 경로
 * @param format : 모션 파일로 얻을 모션 포맷 정보 구조체 포인터
 * @param motion_buf : 얻어올 모션 데이터 스트림 포인터
 * @param motion_len : 얻어올 모션 데이터 스트림 바이트 크기
 * @param key : 암호화된 파일을 로드 할때 사용될 키 값 (이 값이 null 인 경우 암호화 파일은 로드 되지 않습니다)
 * @return	모션 샘플 블럭의 바이트 크기 (i.e. samplesize = 3-DOF x IM_FORMAT_DATA_S16)
 * @remarks 모션 파일의 경로는 실행 파일이 위치로 부터의 상대 경로이거나, 전체 경로가 사용 됩니다. 
 *         암호화된 파일의 경우 암호 키가 필요하고 최대 512 byte 까지 유효 합니다.
 */
IM_DRIVER_DLL_API int IMotion_LoadCSV(const char* filename, IM_FORMAT* format, uint8 ** motion_buf, uint32 * motion_len, const char* key);

/**
 * @brief 로딩된 모션 파일 메모리를 해제 합니다.
 * @param motion_buf : 해제할 모션 데이터 스트림
 * @return	라이브러리 상태(0: 실패, 1: 성공)
 * @remarks 파일을 로드할 메모리는 라이브러리 내부에서 할당 하므로, 로드 후 메모리 사용이 끝나면 이 함수로 해제 하셔야 합니다. 
 */
IM_DRIVER_DLL_API int IMotion_FreeCSV(uint8 * motion_buf);

/**
 * @brief 모션 파일을 저장 합니다.
 * @param filename : 모션 파일 이름 전체 경로
 * @param format : 모션 파일로 저장할 모션 포맷 정보 구조체 포인터
 * @param motion_buf : 저장할 모션 데이터 스트림 포인터
 * @param motion_len : 저장할 모션 데이터 스트림 바이트 크기
 * @param key : 암호화된 파일로 저장 할때 사용될 키 값 
 * @return	모션 샘플 블럭의 바이트 크기
 * @remarks 모션 파일의 경로는 실행 파일이 위치로 부터의 상대 경로이거나, 전체 경로가 사용 됩니다. 
 *         암호화된 파일의 경우 암호 키가 필요하고 최대 512 byte 까지 유효 합니다.
 */
IM_DRIVER_DLL_API int IMotion_SaveCSV(const char* filename, const IM_FORMAT* format, const uint8 * motion_buf, uint32 motion_len, const char* key);
			
#ifdef __cplusplus
}
#endif

#endif // _IMOTION_CSV_H_