using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KinectBodyMoveManager : MonoBehaviour {

    // 키넥트 매니저 스크립트
    private KinectManager manager = null;
    // 손으로 움직일 마우스 오브젝트
    private GameObject handMouse = null;
    // Kinect depthSize 고정 값
    private const int depthSize = 15;

    /*ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ*/
    /* 제스처를 만들기 위한 depth 값 */

    // Kinect Push depth - 밀기
    private const float pushDepth = 7f;
    // Kinect Pluck depth - 당기기
    private const float pluckDepth = 5f;
    // Kinect Warning Left - 몸 왼쪽
    private const float warning_Left = -0.7f;
    // Kinect Warning Right - 몸 오른쪽
    private const float warning_Right = 0.6f;

    /*ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ*/

    // 경고 사이렌이 울리면서 빨간색, 흰색으로 바뀌어질 오브젝트
    private GameObject wall = null;
    // wall 오브젝트의 스프라이트렌더러
    private SpriteRenderer wall_spriteRenderer = null;
    // wall_window를 바뀌어줄 스프라이트
    public Sprite wall_sprite = null;
    // 사이렌 카메라, 토끼, 벽의 상위 오브젝트 
    public GameObject siren = null;
    // 키넥트 사용 할지
    public bool withKinect = true;
    // 키넥트 움직임을 나타낼 오브젝트
    public GameObject kinect_moving_obj = null;
    // 경고창이 울리면서 흔들릴 카메라 스크립트
    private CameraSource camer;
    // 다음 Scene으로 넘어가는지 체크
    private string sceneCheck = "";
    // scene이름
    private string sceneName = "";
    // 경고 체크
    private bool warning_Check = false;

    // 생성되어있는지
    private static bool check = false;

    void Awake()
    {
        if(check == false)
        {
            DontDestroyOnLoad(siren);
            check = true;
        }
        else
        {
            Destroy(siren);
        }
    }

    void Update() {

        if(manager == null) manager = KinectManager.Instance;

        sceneName = SceneManager.GetActiveScene().name;

        if (!sceneCheck.Equals(sceneName))
        {
            sceneCheck = sceneName;

            // 현재 씬이 인트로,맵이 아니면
            if (!sceneName.Equals("Intro") && !sceneName.Equals("Map"))
            {
                kinect_moving_obj.SetActive(true);
            }
            else
            {
                kinect_moving_obj.SetActive(false);
            }
        }

        // Kinect Body Move
        if (handMouse == null)
        {
            handMouse = GameObject.Find("HandMouse");
        }
        else if (handMouse != null)
        {
            if (withKinect)
            {
                RefreshBodyObject(handMouse);
            }
        }

        // Warning Check
        if (wall != null)
        {
            if (warning_Check)
            {
                if (!wall.activeInHierarchy)
                {
                    handMouse.SetActive(true);
                    siren.SetActive(false);
                    warning_Check = false;
                    camer.shake = 0f;
                    SoundManager.sirenStopSound();
                }
            }
            else if (warning_Check == false)
            {
                if (wall.activeInHierarchy)
                {
                    if (handMouse == null)
                    {
                        handMouse = GameObject.Find("HandMouse");
                    }
                    SoundManager.sirenSound();
                    StartCoroutine(ChangeQuad(handMouse));
                    warning_Check = true;
                }
            }
        }
    }

    // Kinect joint 값들을 수정
    private void RefreshBodyObject(GameObject handObject)
    {
        Vector3 shoulder = Vector3.zero;
        Vector3 mid = Vector3.zero;

        for (KinectInterop.JointType jt = KinectInterop.JointType.SpineBase; jt <= KinectInterop.JointType.ThumbRight; jt++)
        {
            if (manager && manager.IsInitialized())
            {
                if (manager.IsUserDetected())
                {
                    long userId = manager.GetPrimaryUserID();

                    if (manager.IsJointTracked(userId, (int)jt))
                    {
                        Vector3 jointPos = manager.GetJointPosition(userId, (int)jt);

                        if (PlayerPrefs.GetString("control").Equals("왼손"))
                        {
                            switch (jt)
                            {
                                case KinectInterop.JointType.ShoulderLeft:
                                    shoulder = GetVector3FromJoint(jointPos);
                                    break;
                                case KinectInterop.JointType.HandLeft:

                                    Vector3 hand = GetVector3FromJoint(jointPos);

                                    if (shoulder != Vector3.zero)
                                    {
                                        ClickGestures(shoulder, hand, handObject);
                                    }

                                    hand.z = 0;

                                    if(manager.GetLeftHandState(userId) == KinectInterop.HandState.Closed)
                                    {
                                        break;
                                    }

                                    handObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                                    handObject.transform.position = hand;
                                    break;
                            }
                        }
                        else
                        {
                            switch (jt)
                            {
                                case KinectInterop.JointType.ShoulderRight:
                                    shoulder = GetVector3FromJoint(jointPos);
                                    break;
                                case KinectInterop.JointType.HandRight:

                                    Vector3 hand = GetVector3FromJoint(jointPos);

                                    if (shoulder != Vector3.zero)
                                    {
                                        ClickGestures(shoulder, hand, handObject);
                                    }

                                    hand.z = 0;

                                    if (manager.GetRightHandState(userId) == KinectInterop.HandState.Closed)
                                    {
                                        break;
                                    }

                                    handObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    handObject.transform.position = hand;

                                    break;
                            }
                        }

                        switch (jt)
                        {
                            case KinectInterop.JointType.SpineShoulder:
                                Vector3 spine = GetVector3FromJoint(jointPos);

                                // 몸이 비틀어졌을때 경고
                                if (mid != Vector3.zero)
                                {
                                    float warningCheck = mid.x - spine.x;
                                    if (warningCheck < warning_Left || warningCheck > warning_Right)
                                    {
                                        if (!AutoFade.Fading)
                                        {
                                            if (wall == null) DrawQuad();
                                            else wall.SetActive(true);
                                        }
                                    }
                                    else if (wall != null)
                                    {
                                        wall.SetActive(false);
                                    }
                                }
                                
                                break;
                            case KinectInterop.JointType.SpineMid:
                                mid = GetVector3FromJoint(jointPos);
                                break;
                        }
                    }
                }
            }
        }

        shoulder = Vector3.zero;
        mid = Vector3.zero;
    }

    // Click 제스처
    private void ClickGestures(Vector3 shoulder, Vector3 hand, GameObject handObject)
    {
        float shoulderToHandX = (shoulder.x - hand.x) / 4;
        float shoulderToHandY = (shoulder.y - hand.y) / 11;
        float shoulderToHand = shoulder.z - hand.z + Mathf.Abs(shoulderToHandX) + Mathf.Abs(shoulderToHandY);

        HandMouseSource source = handObject.GetComponent<HandMouseSource>();

        if (shoulderToHand > pushDepth)
        {
            //click
            source.OnStatusChanged(HandMouseSource.HandMouseStatus.CLICK);
        }
        else if (shoulderToHand < pluckDepth)
        {
            //none
            source.OnStatusChanged(HandMouseSource.HandMouseStatus.IDLE);
        }
    }

    // Kinect depthSize 사이즈 조절
    private Vector3 GetVector3FromJoint(Vector3 joint)
    {
        return new Vector3(joint.x * depthSize, joint.y * depthSize, joint.z * depthSize);
    }

    // 경고 사이렌이 울릴 시 만들어질 벽
    private void DrawQuad()
    {
        wall = new GameObject("Wall");
        wall_spriteRenderer = wall.AddComponent<SpriteRenderer>();
        wall_spriteRenderer.sprite = wall_sprite;

        DontDestroyOnLoad(wall);
        wall.transform.position = new Vector3(0, 0, -10);
        wall.transform.localScale = new Vector3(20, 14, 1);
        wall.layer = 8; // 사이렌
        wall_spriteRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
        wall_spriteRenderer.sortingOrder = 50;
        wall_spriteRenderer.color = Color.red;
    }

    // 경고창 변경 애니메이션처럼 보이기 위한 코루틴
    private IEnumerator ChangeQuad(GameObject hand)
    {
        camer = GameObject.Find("Main Camera").GetComponent<CameraSource>();
        camer.shake = 100f;
        hand.SetActive(false);
        siren.SetActive(true);

        while (wall.activeInHierarchy)
        {
            wall_spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(0.1f);
            wall_spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
