/*
  Innosimulation Motion Library
  Copyright (C) 2016 by Innosimulation <http://www.innosim.com/>.
*/

#ifndef _IMOTION_HPP_
#define _IMOTION_HPP_

#include "IMotion_types.h"
class IMotionSource;


/**
 * @brief 모션 디바이스 C++ 인터페이스 
 * @details 모션 디바이스의 추상 클래스를 정의 합니다. 
 * 이 클래스 객체는 IMotion_Create() 함수로 생성해야 합니다. 
 * 인터페이스 상세 설명은 IMotion.h 파일의 IMotion_* 함수 들을 참고 하세요. \n
 * 
 * (IMotion 시동, 모션 디바이스 객체 생성과 모션 직접 제어의 예)
 * @code
 * IMotion_Startup(0, 0);
 * IMotion* motion = IMotion_Create(0, 0);
 * //...
 * motion->SendStream(_sample, sizeof(int16)*SAMPLE_CHANNELS);
 * //...
 * IMotion_Destroy(motion);
 * IMotion_Shutdown();
 * @endcode
 */
class IMotion 
{
public:
	IMotion(uint32 id) { m_id = id; }	/**< @brief 상세 설명 IMotion_Create() 참고. */
	virtual ~IMotion() {}				/**< @brief 상세 설명 IMotion_Destroy() 참고. */		

	virtual int32 Start(uint8* data=0, uint32 size=0) { return 0; }	/**< @brief 상세 설명 IMotion_Start() 참고. */	
	virtual int32 Stop(uint32 flags=0) { return 0; }				/**< @brief 상세 설명 IMotion_Stop() 참고. */	
	virtual int32 SendStream(uint8* data, uint32 size, 
		uint32 count=1) = 0;										/**< @brief 상세 설명 IMotion_SendStream() 참고. */	
	virtual int32 GetInfo(IM_DIAGNOSTIC_INFO* info=0) = 0;			/**< @brief 상세 설명 IMotion_GetInfo() 참고. */	
	virtual int32 GetDescription(IM_DEVICE_DESC* desc=0);			/**< @brief 상세 설명 IMotion_GetDescription() 참고. */	
	virtual uint32 GetID() { return m_id; }							/**< @brief 상세 설명 IMotion_GetID() 참고. */	
	virtual IMotionSource* CreateSource(const IM_FORMAT* format,
		IMotionCallback user_func=0, uint32 cb_flags=0) = 0;		/**< @brief 상세 설명 IMotion_CreateSource() 참고. */	
	virtual int32 DestroySource(IMotionSource* source) = 0;			/**< @brief 상세 설명 IMotion_DestroySource() 참고. */

private:
	uint32 m_id;
};

/**
 * @brief 모션 소스 C++ 인터페이스 
 * @details 모션 소스의 추상 클래스를 정의 합니다. 
 * 이 클래스 객체는 IMotion 객체의 CreateSource() 함수 또는 IMotion_CreateSource() 함수로 생성해야 합니다. 
 * 인터페이스 상세 설명은 IMotion.h 파일의 IMotionSource_* 함수 들을 참고 하세요. \n
 * 
 * (모션 소스 생성 예)
 * @code
 * IM_FORMAT format;
 * memset(&format, 0, sizeof(IM_FORMAT));
 * format.nSampleRate = 50;
 * // fill format ...
 * IMotionSource* source = motion->CreateSource(&format, 0, 0);
 * //...
 * motion->DestroySource(source);	
 * @endcode
 * 
 * (스트림 버퍼의 소스 큐잉과 스레드 모션 자동 제어의 예)
 * @code
 * IM_BUFFER buffer;
 * memset(&buffer, 0, sizeof(IM_BUFFER));
 * buffer.pMotionData = (uint8*)_buf;
 * // fill buffer ...
 * source->SubmitBuffer(&buffer);
 * source->Start();
 * //...
 * source->Stop();	
 * @endcode 
 */
class IMotionSource 
{
public:
	IMotionSource(IMotion* device, const IM_FORMAT* format);/**< @brief 상세 설명 IMotion_CreateSource() 참고. */
	virtual ~IMotionSource() {}								/**< @brief 상세 설명 IMotion_DestroySource() 참고. */

	virtual int32 Start(uint32 flags=0) = 0;				/**< @brief 상세 설명 IMotionSource_Start() 참고. */
	virtual int32 Stop(uint32 flags=0) = 0;					/**< @brief 상세 설명 IMotionSource_Stop() 참고. */
	virtual int32 SubmitBuffer(const IM_BUFFER* buffer) = 0;/**< @brief 상세 설명 IMotionSource_SubmitBuffer() 참고. */
	virtual uint32 GetQueuedBufferCount() = 0;				/**< @brief 상세 설명 IMotionSource_GetQueuedBufferCount() 참고. */
	virtual IMotion* GetDevice() { return m_device; }		/**< @brief 상세 설명 IMotionSource_GetDevice() 참고. */
	virtual int GetFormat(IM_FORMAT* format);				/**< @brief 상세 설명 IMotionSource_GetFormat() 참고. */
	
private:
	IMotion*	m_device;
	IM_FORMAT	m_format;
};

#endif // _IMOTION_HPP_