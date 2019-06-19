using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System;

namespace BodySource
{
    public enum KinectMotion
    {
        PULL,
        PUSH,
    }

    public delegate void OnMotionChanged(KinectMotion motion);
    public delegate void OnPositionChanged(Vector3 position);

    public interface KinectCallback
    {
        void OnMotionChanged(KinectMotion motion);
        void OnPositionChanged(Vector3 position);
    }

    public class BodySourceView : MonoBehaviour
    {
        private static BodySourceView _Instance;
        public static BodySourceView Instance
        {
            get
            {
                if (!_Instance) _Instance = (BodySourceView) FindObjectOfType(typeof(BodySourceView));
                return _Instance;
            }
        }

        public Material BoneMaterial;
        public GameObject BodySourceManager;

        private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

        private BodySourceManager _BodyManager;

        private event OnMotionChanged _OnMotionChanged = delegate { };
        private event OnPositionChanged _OnPositionChanged = delegate { };

        private float pushDepth = .5f;
        private float pullDepth = .35f;

        private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

        KinectJointFilter filter = new KinectJointFilter();

        void Awake()
        {
            _Instance = this;
            filter.Init();
        }

        void OnDestroy()
        {
            _Instance = null;
        }

        void Update()
        {
            if (BodySourceManager == null)
            {
                return;
            }

            _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
            if (_BodyManager == null)
            {
                return;
            }

            Kinect.Body[] data = _BodyManager.GetData();

            if (data == null)
            {
                return;
            }

            List<ulong> trackedIds = new List<ulong>();
            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {
                    trackedIds.Add(body.TrackingId);
                }
            }

            List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

            // First delete untracked bodies
            foreach (ulong trackingId in knownIds)
            {
                if (!trackedIds.Contains(trackingId))
                {
                    Destroy(_Bodies[trackingId]);
                    _Bodies.Remove(trackingId);
                }
            }

            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {
                    filter.UpdateFilter(body);

                    if (!_Bodies.ContainsKey(body.TrackingId))
                    {
                        _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    }

                    RefreshBodyObject(body, _Bodies[body.TrackingId]);

                    break;
                }
            }
        }

        private GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject("Body:" + id);
            return body;
        }

        private static float vectorScale = 15;

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            Kinect.CameraSpacePoint? shoulder = null;

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                Kinect.Joint? targetJoint = null;

                if (_BoneMap.ContainsKey(jt))
                {
                    targetJoint = body.Joints[_BoneMap[jt]];
                }

                if (targetJoint.HasValue)
                {
                    if (jt == Kinect.JointType.ShoulderRight)
                    {
                        shoulder = filter.GetFilteredJoint((int)Kinect.JointType.ShoulderRight);

                    }
                    else if (jt == Kinect.JointType.HandRight)
                    {
                        Kinect.CameraSpacePoint point = filter.GetFilteredJoint((int)Kinect.JointType.HandRight);

                        if (shoulder != null /*&& elbow != null*/)
                        {
                            float shoulderToHandX = (shoulder.Value.X - point.X) / 3;
                            float shoulderToHandY = (shoulder.Value.Y - point.Y) / 3;

                            float shoulderToHand = shoulder.Value.Z - point.Z + Mathf.Abs(shoulderToHandX) + Mathf.Abs(shoulderToHandY);

                            if (shoulderToHand > pushDepth)
                            {
                                _OnMotionChanged(KinectMotion.PUSH);
                            }
                            else if (shoulderToHand < pullDepth)
                            {
                                _OnMotionChanged(KinectMotion.PULL);
                            }

                            var position = Vector3.zero;
                            position.x = point.X * vectorScale;
                            position.y = point.Y * vectorScale;

                            _OnPositionChanged(position);
                        }
                    }

                }
            }

            shoulder = null;
        }

        public void AddKinectCallback(KinectCallback callback)
        {
            _OnMotionChanged += callback.OnMotionChanged;
            _OnPositionChanged += callback.OnPositionChanged;
        }

        public void RemoveKinectCallback(KinectCallback callback)
        {
            _OnMotionChanged -= callback.OnMotionChanged;
            _OnPositionChanged -= callback.OnPositionChanged;
        }
    }
}
