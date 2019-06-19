using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class KinectFbxRecorder : MonoBehaviour 
{
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;
	
	// Whether the character is facing the player or not. Default false.
	//private bool mirroredAvatar = false;

	// path to the file to load
	[Tooltip("Path to the fbx file, used to load the model.")]
	public string loadFilePath;
	
	[Tooltip("Path to the fbx file, used to save the model and animation.")]
	public string saveFilePath = string.Empty;

	[Tooltip("Name of the mocap animation.")]
	public string animationName = string.Empty;

	[Tooltip("Unity scale factor of the loaded fbx model.")]
	public float importScaleFactor = 1f;
	
	[Tooltip("Output file format. Leave empty for the default format.")]
	public string outputFileFormat = string.Empty;
	
	[Tooltip("GUI-Text to display information messages.")]
	public GUIText infoText;
	
	[Tooltip("Whether to start recording, right after the scene starts.")]
	public bool recordAtStart = false;


	// the avatar object
	private GameObject avatarObject;
	
	// whether it is recording at the moment
	private bool isRecording = false;

	// reference to KinectManager
	private KinectManager manager = null;

	// reference to AvatarController
	private AvatarController avatarCtrl = null;

	// initial avatar position
	private Vector3 initialPos = Vector3.zero;

	// local rotations of the bone transform
	private Quaternion[] alJointPreRots = null;
	private Quaternion[] alJointPostRots = null;

	// time variables used for recording
	private long liRelTime = 0;
	private float fStartTime = 0f;
	private float fCurrentTime = 0f;
	private int iSavedFrame = 0;

	// unity scale factor
	private float fUnityScale = 0f;

	// fbx global time mode as FPS
	private float fGlobalFps = 0f;

	// fbx global scale factor
	private float fFbxScale = 0f;

	// if the fbx wrapper is available
	private bool bFbxAvailable = false;

	// if the fbx-system is successfully initialized
	private bool bFbxInited = false;
	
	// if the fbx-file was successfully loaded
	private bool bFbxLoaded = false;
	
	// if the fbx file is modified and needs to be saved
	private bool bFbxDirty = false;

    // starts recording
    public bool StartRecording()
	{
		if(avatarObject == null)
			return false;

		if(isRecording)
			return false;

		isRecording = true;

		// stop recording if there is no file name specified
		if(saveFilePath == string.Empty)
		{
			isRecording = false;

			Debug.LogError("No file to save.");
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = "No file to save.";
			}
		}
		
		// create the animation stack, if animation name is specified
		if(animationName != string.Empty && !MocapFbxWrapper.CreateAnimStack(animationName))
		{
			isRecording = false;

			//throw new Exception("Could not create animation: " + animationName);
			Debug.LogError("Could not create animation: " + animationName);
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = "Could not create animation: " + animationName;
			}
		}
		
		if(isRecording && animationName != string.Empty)
		{
			MocapFbxWrapper.SetCurrentAnimStack(animationName);
		}
		
		if(isRecording)
		{
			Debug.Log("Recording started.");
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = "Recording... Say 'Stop' to stop the recorder.";
			}

			// get initial avatar position
			if(avatarCtrl != null)
			{
				int hipsIndex = avatarCtrl.GetBoneIndexByJoint(KinectInterop.JointType.SpineBase, false);
				Transform hipsTransform = avatarCtrl.GetBoneTransform(hipsIndex);
				initialPos = hipsTransform != null ? hipsTransform.position : Vector3.zero;
			}

			// initialize times
			fStartTime = fCurrentTime = Time.time;
			iSavedFrame = 0;

			// get global time mode as fps
			fGlobalFps = MocapFbxWrapper.GetGlobalFps();

			// get global scale factor
			fFbxScale = 100f / MocapFbxWrapper.GetGlobalScaleFactor();

			// Unity scale factor
			fUnityScale = importScaleFactor != 0f ? 1f / importScaleFactor : 1f;
		}

		return isRecording;
	}


	// stops recording
	public void StopRecording()
	{
		if(isRecording)
		{
			SaveFileIfNeeded();
			isRecording = false;

			Debug.Log("Recording stopped.");
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = "Recording stopped.";
			}
		}

		if(infoText != null)
		{
			infoText.GetComponent<GUIText>().text = "Say: 'Record' to start the recorder.";
		}
	}


	// returns if file recording is in progress at the moment
	public bool IsRecording()
	{
		return isRecording;
	}


	// ----- end of public functions -----
	

	// loads the specified fbx file
	private bool LoadFile()
	{
		if(!bFbxInited)
			return false;
		
		if(!SaveFileIfNeeded())
			throw new Exception("Could not save the modified fbx to: " + saveFilePath);
		
		if(loadFilePath != string.Empty)
			bFbxLoaded = MocapFbxWrapper.LoadFbxFile(loadFilePath, false);
		else
			bFbxLoaded = false;
		
		return bFbxLoaded;
	}


	// saves the specified fbx file
	private bool SaveFile()
	{
		if(saveFilePath == string.Empty)
		{
			saveFilePath = loadFilePath;
		}
		
		// delete the old csv file
		if(saveFilePath != string.Empty && File.Exists(saveFilePath))
		{
			File.Delete(saveFilePath);
		}
		
		if(saveFilePath != string.Empty && MocapFbxWrapper.SaveFbxFile(saveFilePath, outputFileFormat))
		{
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh(); //.ImportAsset(saveFilePath);
#endif
			
			bFbxDirty = false;
			return true;
		}
		
		return false;
	}


	// saves the fbx file, if it was modified
	private bool SaveFileIfNeeded()
	{
		if(bFbxDirty)
		{
			SaveFile();
		}
		
		return true;
	}


	// loads the avatar object from the input fbx file
	private bool LoadAvatarObject()
	{
		if(loadFilePath == string.Empty)
			return false;

#if UNITY_EDITOR
        UnityEngine.Object avatarModel = UnityEditor.AssetDatabase.LoadAssetAtPath(loadFilePath, typeof(UnityEngine.Object));
        if (avatarModel == null)
            return false;

        if (avatarObject != null)
        {
            GameObject.Destroy(avatarObject);
        }

        avatarObject = (GameObject)GameObject.Instantiate(avatarModel);
        if (avatarObject != null)
        {
            avatarObject.transform.position = new Vector3(-1f, 0f, -1f); // Vector3.zero;
            avatarObject.transform.rotation = Quaternion.identity;

            AvatarController ac = avatarObject.AddComponent<AvatarController>();
            ac.playerIndex = playerIndex;
            ac.mirroredMovement = false;
            ac.verticalMovement = true;
            ac.smoothFactor = 10f;
            ac.Awake();

            KinectManager km = KinectManager.Instance;

            if (km)
            {
                if (km.IsUserDetected())
                {
                    ac.SuccessfulCalibration(km.GetPrimaryUserID());
                }

                km.avatarControllers.Clear();
                km.avatarControllers.Add(ac);
            }
        }

        return true;
#endif

        return false;
    }


	// returns pre-rotation of a joint (fbx node)
	private Quaternion GetJointPreRot(Transform jointTransform)
	{
		if(jointTransform != null)
		{
			string sJointName = jointTransform.name;
			Vector3 vPreRot = Vector3.zero;
			MocapFbxWrapper.GetNodePreRot(sJointName, ref vPreRot);
			
			Quaternion qPreRot = Quaternion.identity;
			MocapFbxWrapper.Rot2Quat(ref vPreRot, ref qPreRot);
			
			qPreRot.y = -qPreRot.y;
			qPreRot.z = -qPreRot.z;
			
			return qPreRot;
		}

		return Quaternion.identity;
	}


	// returns post-rotation of a joint (fbx node)
	private Quaternion GetJointPostRot(Transform jointTransform)
	{
		if(jointTransform != null)
		{
			string sJointName = jointTransform.name;
			Vector3 vPostRot = Vector3.zero;
			MocapFbxWrapper.GetNodePostRot(sJointName, ref vPostRot);
			
			Quaternion qPostRot = Quaternion.identity;
			MocapFbxWrapper.Rot2Quat(ref vPostRot, ref qPostRot);
			
			qPostRot.y = -qPostRot.y;
			qPostRot.z = -qPostRot.z;
			
			return qPostRot;
		}
		
		return Quaternion.identity;
	}
	
	
	// saves needed animation frames at the current save-time
	private bool SaveAnimFrame(int jointIndex, Transform jointTransform)
	{
		if(jointTransform == null)
			return false;
		
		string sJointName = jointTransform.name;
		bool bSuccess = true;
		
		float fSaveTime = (float)iSavedFrame / fGlobalFps;
		
		if(jointIndex == 0)
		{
			// save avatar position
			Vector3 currentPos = jointTransform.position - initialPos;
			currentPos.x = -currentPos.x;
			currentPos.y += initialPos.y;
			
			Vector3 vJointPos = currentPos * fUnityScale * fFbxScale;
			bSuccess &= MocapFbxWrapper.SetAnimCurveTrans(sJointName, fSaveTime, ref vJointPos);
		}
		
		Quaternion qJointRot = jointTransform.localRotation;
		
		if(alJointPreRots != null && alJointPostRots != null)
		{
			Quaternion qJointPreRot =  alJointPreRots[jointIndex];
			Quaternion qJointPostRot = alJointPostRots[jointIndex];

			qJointRot = Quaternion.Inverse(qJointPreRot) * qJointRot * Quaternion.Inverse(qJointPostRot);
		}
		
		qJointRot.x = -qJointRot.x;
		qJointRot.w = -qJointRot.w;
		
		Vector3 vJointRot = Vector3.zero;
		MocapFbxWrapper.Quat2Rot(ref qJointRot, ref vJointRot);
		
		bSuccess &= MocapFbxWrapper.SetAnimCurveRot(sJointName, fSaveTime, ref vJointRot);
		
		if(iSavedFrame == 0)
		{
			// save the node scale in the first frame only
			Vector3 vJointScale = jointTransform.localScale;
			MocapFbxWrapper.SetAnimCurveScale(sJointName, 0f, ref vJointScale);
		}
		
		return bSuccess;
	}


	void Awake()
	{
		try 
		{
			// ensure the fbx wrapper is available
			bool bNeedRestart = false;
			if(MocapFbxWrapper.EnsureFbxWrapperAvailability(ref bNeedRestart))
			{
				bFbxAvailable = true;
				
				if(bNeedRestart)
				{
					//KinectInterop.RestartLevel(gameObject, "MF");
					return;
				}
			}
			else
			{
				throw new Exception("Fbx-Unity-Wrapper is not available!");
			}
		} 
		catch (Exception ex) 
		{
			Debug.LogError(ex.Message);
			Debug.LogException(ex);
			
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = ex.Message;
			}
		}
	}
	

	void Start()
	{
		// check if fbx-wrapper is available
		if(!bFbxAvailable)
			return;

		try 
		{
			// instantiate the avatar object from the fbx file
			if(LoadAvatarObject() && avatarObject)
			{
				avatarCtrl = avatarObject.gameObject.GetComponent<AvatarController>();
			}
			else
			{
				if(loadFilePath == string.Empty)
					throw new Exception("Please specify fbx-file to load.");
				else
					throw new Exception("Cannot instantiate avatar from file: " + loadFilePath);
			}

			// check the KinectManager availability
			if(!manager)
			{
				manager = KinectManager.Instance;
			}

			if(!manager)
			{
				throw new Exception("KinectManager not found, probably not initialized. See the log for details");
			}

			// initialize fbx wrapper
			if(!bFbxInited)
			{
				bFbxInited = MocapFbxWrapper.InitFbxWrapper();
				
				if(!bFbxInited)
				{
					throw new Exception("Fbx wrapper could not be initialized.");
				}
			}
			
			// load the fbx file
			if(!LoadFile())
			{
				throw new Exception("File could not be loaded: " + loadFilePath);
			}

			if(avatarCtrl && manager && manager.IsInitialized())
			{
				// get the pre- and post-rotations
				int jointCount = manager.GetJointCount();
				alJointPreRots = new Quaternion[jointCount + 2];
				alJointPostRots = new Quaternion[jointCount + 2];
				
				for(int j = 0; j < jointCount; j++)
				{
					int boneIndex = avatarCtrl.GetBoneIndexByJoint((KinectInterop.JointType)j, false);
					Transform jointTransform = avatarCtrl.GetBoneTransform(boneIndex);

					alJointPreRots[j] = GetJointPreRot(jointTransform);
					alJointPostRots[j] = GetJointPostRot(jointTransform);
				}

				// get special transforms
				int specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder, false);
				Transform specTransform = avatarCtrl.GetBoneTransform(specIndex);
				alJointPreRots[jointCount] = GetJointPreRot(specTransform);
				alJointPostRots[jointCount] = GetJointPostRot(specTransform);

				specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder, false);
				specTransform = avatarCtrl.GetBoneTransform(specIndex);
				alJointPreRots[jointCount + 1] = GetJointPreRot(specTransform);
				alJointPostRots[jointCount + 1] = GetJointPostRot(specTransform);
			}

//			// create the animation stack, if animation name is specified
//			if(animationName != string.Empty && !MocapFbxWrapper.CreateAnimStack(animationName))
//			{
//				throw new Exception("Could not create animation: " + animationName);
//			}
//
//			if(animationName != string.Empty)
//			{
//				MocapFbxWrapper.SetCurrentAnimStack(animationName);
//			}

			bFbxDirty = false;

			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = "Say: 'Record' to start the recorder.";
			}

			if(recordAtStart)
			{
				StartRecording();
			}
		} 
		catch (Exception ex) 
		{
			Debug.LogError(ex.Message);
			Debug.LogException(ex);
			
			if(infoText != null)
			{
				infoText.GetComponent<GUIText>().text = ex.Message;
			}
		}
	}


	void Update () 
	{
		if(isRecording && animationName != string.Empty)
		{
			// save the body frame, if any
			if(manager && manager.IsInitialized())
			{
				long userId = manager.GetUserIdByIndex(playerIndex);
				long liFrameTime = manager.GetBodyFrameTimestamp();

				if(userId != 0 && liFrameTime != liRelTime && avatarCtrl)
				{
					liRelTime = liFrameTime;
					fCurrentTime = Time.time;

					float fRelTime = fCurrentTime - fStartTime;
					int iCurrentFrame = Mathf.FloorToInt(fRelTime * fGlobalFps);

					if(infoText)
					{
						infoText.GetComponent<GUIText>().text = string.Format("Recording @ {0:F3}... Say 'Stop' to stop the recorder.", fRelTime);
					}
					
					int jointCount = manager.GetJointCount();
					//bool bSavedOnce = false;

					StringBuilder sbBuf = new StringBuilder();
					const char delimiter = ',';

					//Quaternion qRootRotation = Quaternion.identity;
					//int iSavedFrame1 = iSavedFrame;

					while(iSavedFrame <= iCurrentFrame)
					{
						// save properties of all joints
						for(int j = 0; j < jointCount; j++)
						{
							int boneIndex = avatarCtrl.GetBoneIndexByJoint((KinectInterop.JointType)j, false);
							Transform jointTransform = avatarCtrl.GetBoneTransform(boneIndex);
							
							if(jointTransform != null)
							{
								bool bSuccess = SaveAnimFrame(j, jointTransform);

								if(!bSuccess)
								{
									string sMessage = string.Format("Could not save animation frame at time: {0:F3}", fRelTime);
									
									infoText.GetComponent<GUIText>().text = sMessage;
									Debug.LogError(sMessage);
								}
								
								bFbxDirty = bSuccess;
							}
						}

						// save properties of the special joints
						int specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder, false);
						Transform specTransform = avatarCtrl.GetBoneTransform(specIndex);
						bFbxDirty |= SaveAnimFrame(jointCount, specTransform);

						specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder, false);
						specTransform = avatarCtrl.GetBoneTransform(specIndex);
						bFbxDirty |= SaveAnimFrame(jointCount + 1, specTransform);
						
						//bSavedOnce = true;
						iSavedFrame++;
					}

					// remove the last delimiter
					if(sbBuf.Length > 0 && sbBuf[sbBuf.Length - 1] == delimiter)
					{
						sbBuf.Remove(sbBuf.Length - 1, 1);
					}

				}
			}
		}

	}

	void OnDestroy()
	{
		// check if fbx-wrapper is available
		if(!bFbxAvailable)
			return;

		// finish recording, if needed
		isRecording = false;

		// save the fbx file, if needed
		if(!SaveFileIfNeeded())
		{
			Debug.LogError("Could not save the modified fbx to: " + saveFilePath);
		}
		
		// terminate the fbx wrapper
		if(bFbxInited)
		{
			MocapFbxWrapper.TermFbxFrapper();
			bFbxInited = false;
		}
	}


}
