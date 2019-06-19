﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class MocapFbxWrapper
{
	[DllImport("FbxUnityWrapper")]
	public static extern bool InitFbxWrapper();
	
	[DllImport("FbxUnityWrapper")]
	public static extern void TermFbxFrapper();
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool LoadFbxFile([MarshalAs(UnmanagedType.LPStr)]string sFileName, bool bPrintInfo);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool SaveFbxFile([MarshalAs(UnmanagedType.LPStr)]string sFileName, [MarshalAs(UnmanagedType.LPStr)]string sFormatName);

	[DllImport("FbxUnityWrapper")]
	public static extern float GetGlobalFps();

	[DllImport("FbxUnityWrapper")]
	public static extern float GetGlobalTimeMode();
	
	[DllImport("FbxUnityWrapper")]
	public static extern float GetGlobalScaleFactor();

	[DllImport("FbxUnityWrapper")]
	public static extern bool GetNodePreRot([MarshalAs(UnmanagedType.LPStr)]string sNodeName, ref Vector3 vfValue);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool GetNodePostRot([MarshalAs(UnmanagedType.LPStr)]string sNodeName, ref Vector3 vfValue);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool CreateAnimStack([MarshalAs(UnmanagedType.LPStr)]string sStackName);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool SetCurrentAnimStack([MarshalAs(UnmanagedType.LPStr)]string sStackName);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool SetAnimCurveRot([MarshalAs(UnmanagedType.LPStr)]string sNodeName, float fTime, ref Vector3 vfValue);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool SetAnimCurveTrans([MarshalAs(UnmanagedType.LPStr)]string sNodeName, float fTime, ref Vector3 vfValue);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool SetAnimCurveScale([MarshalAs(UnmanagedType.LPStr)]string pNodeName, float fTime, ref Vector3 vfValue);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool Rot2Quat(ref Vector3 vRot, ref Quaternion vQuat);
	
	[DllImport("FbxUnityWrapper")]
	public static extern bool Quat2Rot(ref Quaternion vQuat, ref Vector3 vRot);


	// unzips the needed native libraries, if needed
	public static bool EnsureFbxWrapperAvailability(ref bool bNeedRestart)
	{
		bool bOneCopied = false, bAllCopied = true;
		string sTargetPath = KinectInterop.GetTargetDllPath(".", KinectInterop.Is64bitArchitecture()) + "/";
		
		if(!KinectInterop.Is64bitArchitecture())
		{
			//Debug.Log("x32-architecture detected.");
			
			Dictionary<string, string> dictFilesToUnzip = new Dictionary<string, string>();
			dictFilesToUnzip["FbxUnityWrapper.dll"] = sTargetPath + "FbxUnityWrapper.dll";
			dictFilesToUnzip["fbxsdk_20113_1.dll"] = sTargetPath + "fbxsdk_20113_1.dll";
			dictFilesToUnzip["msvcp100.dll"] = sTargetPath + "msvcp100.dll";
			dictFilesToUnzip["msvcr100.dll"] = sTargetPath + "msvcr100.dll";
			
			KinectInterop.UnzipResourceFiles(dictFilesToUnzip, "FbxUnityWrapper.x86.zip", ref bOneCopied, ref bAllCopied);
		}
		else
		{
			//Debug.Log("x64-architecture detected.");
			
			Dictionary<string, string> dictFilesToUnzip = new Dictionary<string, string>();
			dictFilesToUnzip["FbxUnityWrapper.dll"] = sTargetPath + "FbxUnityWrapper.dll";
			dictFilesToUnzip["fbxsdk_20113_1_amd64.dll"] = sTargetPath + "fbxsdk_20113_1_amd64.dll";
			dictFilesToUnzip["msvcp100.dll"] = sTargetPath + "msvcp100.dll";
			dictFilesToUnzip["msvcr100.dll"] = sTargetPath + "msvcr100.dll";
			
			KinectInterop.UnzipResourceFiles(dictFilesToUnzip, "FbxUnityWrapper.x64.zip", ref bOneCopied, ref bAllCopied);
		}
		
		bNeedRestart = (bOneCopied && bAllCopied);
		
		return true;
	}
	
}
