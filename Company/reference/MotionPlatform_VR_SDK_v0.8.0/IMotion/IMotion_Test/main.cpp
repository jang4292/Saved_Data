#include <stdio.h>
#include <math.h>
#include <windows.h>

#include "IMotion.h"

#define SAMPLE_CHANNELS	IM_FORMAT_CHANNELS_DEFAULT
#define SAMPLE_RATE		IM_FORMAT_SAMPLE_RATE_DEFAULT

float motion_frequency = 1; /* motion frequency in cycles per sample */
float motion_amplitude = 1; /* motion scale, -32768 ~ 32767 */

int	main(int argc, char *argv[])
{
    /* Start up */	
	IMotion_Startup(IM_CFG_DEBUG_MODE);

	// Create device (default driver)
	IMotion* motion = IMotion_Create();
	motion->Start();
	
	IM_DEVICE_DESC desc;
	motion->GetDescription(&desc);

	/**** Motion Stream test (1st wave) ****/
	double wave;
	int16 buf[SAMPLE_RATE][SAMPLE_CHANNELS] = {0,};
	memset(buf, 0, sizeof(int16)*SAMPLE_RATE*SAMPLE_CHANNELS);
	for(int i=0; i<SAMPLE_RATE; i++) {
		wave = motion_amplitude * sin(2 * IM_PI * (float)i/SAMPLE_RATE * motion_frequency);
		buf[i][0] = (int16)(wave*MOTION_MAX_16);
	#if 1 // manually motion operation test
		motion->SendStream((uint8*)&buf[i], sizeof(int16)*SAMPLE_CHANNELS);
		Sleep(1000/SAMPLE_RATE);
	#endif
	}
	// diagnostics
	int error = motion->GetInfo();
	if(error) {
		fprintf(stderr, "Driver Error (code : %d) !\n", error);
		exit(0);
	}
	
	/**** Motion Buffer test (2nd wave) ****/
	IM_FORMAT format;
	memset(&format, 0, sizeof(IM_FORMAT));
	format.nSampleRate = SAMPLE_RATE;
	format.nChannels = SAMPLE_CHANNELS;
	format.nDataFormat = IM_FORMAT_DATA_DEFAULT;
	IMotionSource* source = motion->CreateSource(&format);

	IM_BUFFER buffer;	
	memset(&buffer, 0, sizeof(IM_BUFFER));
    buffer.nMotionBytes = SAMPLE_RATE*SAMPLE_CHANNELS*sizeof(int16);
    buffer.pMotionData = (uint8*)buf;
#if 0 // buffer loop test
	buffer.nLoopCount = IM_LOOP_INFINITE; 
#endif
	source->SubmitBuffer(&buffer);

	source->Start();	
    // Wait Queued Buffers 
	while(source->GetQueuedBufferCount() > 0)
		Sleep(10);
	source->Stop();

    /* Clean up */	
	motion->DestroySource(source);	
	IMotion_Destroy(motion);
	IMotion_Shutdown();
	return (0);
}