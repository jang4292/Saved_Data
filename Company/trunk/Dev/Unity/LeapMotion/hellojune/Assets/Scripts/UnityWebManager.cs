using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using LitJson;

public class UnityWebManager : MonoBehaviour
{
    public InputField uuidObj; //인증키(uuid)
    public Text uuidButtonObj; //확인 버튼
    public AbstractGameController gameController;

    private static string SERVER_URL = "http://localhost:8080/unity";
    private const string BNMTEC_URL   = "http://www.bnmtec.com:8080/unity";
    private const string REQUEST_CODE = "request_code";
    private const string RESPONSE_CODE = "response_code";

    private string sceneName = "";

    void Start()
    {
        bool isMainMenuScene = (this.gameObject.name == "UUIDInputButton");

        if (isMainMenuScene)
        {
            autoLogin();
        }
    }

    //자동 로그인
    private void autoLogin()
    {
        if (PlayerPrefs.HasKey("uuid"))
        {
            uuidObj.text = PlayerPrefs.GetString("uuid");
            requestLogin();
        }
    }

    //로그인 기록 저장
    public void requestLogin()
    {
        if(string.IsNullOrEmpty(uuidObj.text))
        {
            return;
        }

        Dictionary<string,string> param = new Dictionary<string, string>();
        param.Add(REQUEST_CODE, "login");
        param.Add("data", uuidObj.text);

        StartCoroutine(checkServer(param));
    }

    //놀이 기록 저장
    public void requestPlayLog(string sceneName)
    {
        if (PlayerPrefs.HasKey("uuid"))
        {
            this.sceneName = sceneName;
            string uuid = PlayerPrefs.GetString("uuid");
            string gameName = gameController.getGameName();
            int duration = gameController.getTime();
            int points = gameController.getPoints();
            

            string tmp = "{\"uuid\":\"" + uuid + "\",\"game_name\":\"" + gameName + "\",\"duration\":" + duration + ",\"points\":" + points + "}";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add(REQUEST_CODE, "playlog");
            param.Add("data", tmp);

            StartCoroutine(checkServer(param));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    //웹서버 상태 확인: 로컬서버 다운 => 본서버로 대체
    IEnumerator checkServer(Dictionary<string, string> param)
    {
        if(SERVER_URL != BNMTEC_URL)
        {
            WWW www = new WWW(SERVER_URL);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                SERVER_URL = BNMTEC_URL;
            }
        }

        StartCoroutine(requestWWW(param));
    }

    //웹서버 통신 모듈
    IEnumerator requestWWW(Dictionary<string,string> param)
    {
        string jsonString = JsonMapper.ToJson(param);

        WWWForm form = new WWWForm();
        form.AddField("model", jsonString);

        WWW www = new WWW(SERVER_URL, form);
        yield return www;

        responseWWW(www);
    }

    //응답 처리
    void responseWWW(WWW response)
    {
        if (string.IsNullOrEmpty(response.error))
        {
            JsonData jsonObj = JsonMapper.ToObject(response.text);

            if(jsonObj[RESPONSE_CODE].ToString() == "success")
            {
                doSuccessResponse(jsonObj);
            }
            else
            {
                doFailResponse(jsonObj);
            }
        }
        else
        {
            if(uuidButtonObj != null)
            {
                uuidButtonObj.text = "오프라인";
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    //성공 응답처리
    void doSuccessResponse(JsonData jsonObj)
    {
        if (jsonObj[REQUEST_CODE].ToString() == "login")
        {
            PlayerPrefs.SetString("uuid", uuidObj.text);
            uuidButtonObj.text = "인증 성공";
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    //실패 응답처리
    void doFailResponse(JsonData jsonObj)
    {
        if (jsonObj[REQUEST_CODE].ToString() == "login")
        {
            uuidButtonObj.text = "인증 실패";
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    //인증키 삭제
    public void deleteUUID()
    {
        PlayerPrefs.DeleteKey("uuid");
        uuidObj.text = "";
        uuidButtonObj.text = "확인";
    }
    
}






