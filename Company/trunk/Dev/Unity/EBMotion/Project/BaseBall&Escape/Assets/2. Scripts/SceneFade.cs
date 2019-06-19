using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour {

    // 인스턴스화
    private static SceneFade m_Instance = null;
    private Material m_Material = null;
    // 다음 씬으로 넘어갈 이름
    private string m_LevelName = "";
    // 다음 씬으로 넘어갈 번호
    private int m_LevelIndex = 0;
    // 효과가 진행되고 있는지
    private bool m_Fading = false;

    // 다른 클래스에서 인스턴스화를 하지 않고 불러도 되게
    private static SceneFade Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (new GameObject("AutoFade")).AddComponent<SceneFade>();
            }
            return m_Instance;
        }
    }

    // 효과가 진행되고 있는지 체크
    public static bool Fading
    {
        get { return Instance.m_Fading; }
    }

    // 생성자
    private void Awake()
    {
        // 새로운 레벨이 로드 될 때 이전 레벨의 오브젝트가 삭제되지 않게 DontDestroyOnLoad()
        DontDestroyOnLoad(this);
        m_Instance = this;
        m_Material = new Material(Shader.Find("Hidden/Internal-Colored"));
    }

    // 다음 씬으로 넘어갈때 나오는 화면
    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        m_Material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(aColor);   
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, t);
        }
        if (m_LevelName != "")
            SceneManager.LoadScene(m_LevelName);
        else
            SceneManager.LoadScene(m_LevelIndex);
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            DrawQuad(aColor, t);
        }
        m_Fading = false;
    }

    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        m_Fading = true;
        StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
    }

    public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance.m_LevelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
    public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance.m_LevelName = "";
        Instance.m_LevelIndex = aLevelIndex;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
}
