using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DataSave : MonoBehaviour {

    // 이렇게 클래스를 만들고
    [Serializable]           // 직렬
    public class PlayerEntry
    {
        // 환자 이름
        public string name;
        // 환자 나이
        public string age;
        // 환자 번호
        public string number;
        // 시간
        public string time;

        /* 임시로 5개 날짜 데이터*/

        // 날짜
        public string day_one;
        // 점수
        public int score_one;
        public string day_two;
        public int score_two;
        public string day_three;
        public int score_three;
        public string day_four;
        public int score_four;
        public string day_five;
        public int score_five;
    }

    // 임시 저장 데이터 길이
    private int saveDataSize = 4;
    // 임시 저장 데이터 이름
    private string[] savePrefsName = { "one", "two", "three", "four" };
    // 임시 저장 환자 이름
    private string[] playerName = { "강우리 님", "강철수 님", "고길동 님", "김영희 님" };
    // 임시 저장 환자 나이
    private string[] playerAge = { "53", "41", "24", "50" };
    // 임시 저장 환자 번호
    private string[] playerNumber = { "003040", "003160", "005040", "003170" };

    // 만든 클래스를 리스트에 담아서 관리하면 마치 테이블처럼 사용할 수 있습니다. 
    public List<PlayerEntry> playerData = new List<PlayerEntry>();

    void Start()
    {
        // 임시 데이터 5개 저장
        for (int i = 0; i < saveDataSize; i++)
        {
            name = savePrefsName[i];

            if (PlayerPrefs.GetString(name).Equals(""))
            {
                AddData(i);
                SaveScores();
            }

            var data = PlayerPrefs.GetString(name);

            if (!string.IsNullOrEmpty(data))
            {
                var binaryFormatter = new BinaryFormatter();
                // Convert = 기본데이터를 다른 데이터로 변환
                // base-64 숫자의 이진 데이터를 해당하는 8비트 부호 없는 정수 배열로 인코딩하는 방법으로 지정된 문자열을 변환합니다.
                // 지정된 바이트 배열을 기반으로 하는 MemoryStream 클래스의 크기 조정이 불가능한 새 인스턴스를 초기화합니다. Byte[]
                var memoryStream = new MemoryStream(Convert.FromBase64String(data));

                // 가져온 데이터를 바이트 배열로 변환하고
                // 사용하기 위해 다시 리스트로 캐스팅해줍니다.
                playerData = (List<PlayerEntry>)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }

    void SaveScores()
    {
        // 개체나 연결된 개체의 전체 그래프를 이진 형식으로 serialize(직렬화) 및 deserialize(역직렬화)합니다.
        var binaryFormatter = new BinaryFormatter();
        // MemoryStream은 스트림 데이터를 파일이나 소켓대신 메모리에 직접 출력합니다.
        var memoryStream = new MemoryStream();

        // PlayerData를 바이트 배열로 변환해서 저장합니다.
        binaryFormatter.Serialize(memoryStream, playerData);

        // 그것을 다시 한번 문자열 값으로 변환해서 
        // Convert = 기본데이터를 다른 데이터로 변환
        PlayerPrefs.SetString(name, Convert.ToBase64String(memoryStream.GetBuffer()));
    }

    void AddData(int number)
    {
        // 이렇게 새로운 데이터를 추가해주고
        // 이름, 날짜, 점수, 시간
        playerData.Add(new PlayerEntry {
            name = playerName[number],
            age = playerAge[number],
            number = playerNumber[number],
            time = "01:12",
            day_one = "2013/04/22 12:45",
            score_one = 12,
            day_two = "2013/08/28 14:35",
            score_two = 23,
            day_three = "2014/12/31 01:31",
            score_three = 8,
            day_four = "2015/03/21 10:45",
            score_four = 30,
            day_five = "2016/01/02 07:31",
            score_five = 41
        });
    }

    // 날짜, 스코어, 시간 저장
    public void saveData(int number, string day, int score, string timeText)
    {
        playerData[number].time = timeText;

        List<string> dayList = new List<string>();

        dayList.Add(playerData[number].day_one + "1");
        dayList.Add(playerData[number].day_two + "2");
        dayList.Add(playerData[number].day_three + "3");
        dayList.Add(playerData[number].day_four + "4");
        dayList.Add(playerData[number].day_five + "5");

        dayList.Sort();
        dayList.Reverse();

        // 최근꺼랑 비교 분석
        if (dayList[0].Substring(0, dayList[0].Length - 1).Equals(day))
        {
            switch(dayList[0].Substring(dayList[0].Length - 1, 1)){
                case "1":
                    playerData[number].score_one = score;
                    break;
                case "2":
                    playerData[number].score_two = score;
                    break;
                case "3":
                    playerData[number].score_three = score;
                    break;
                case "4":
                    playerData[number].score_four = score;
                    break;
                case "5":
                    playerData[number].score_five = score;
                    break;
            }
        }
        else
        {
            for (int i = 0; i < dayList.Count; i++)
            {
                if (i == dayList.Count - 1)
                {
                    switch (dayList[i].Substring(dayList[i].Length - 1, 1))
                    {
                        case "1":
                            playerData[number].day_one = day;
                            playerData[number].score_one = score;
                            break;
                        case "2":
                            playerData[number].day_two = day;
                            playerData[number].score_two = score;
                            break;
                        case "3":
                            playerData[number].day_three = day;
                            playerData[number].score_three = score;
                            break;
                        case "4":
                            playerData[number].day_four = day;
                            playerData[number].score_four = score;
                            break;
                        case "5":
                            playerData[number].day_five = day;
                            playerData[number].score_five = score;
                            break;
                    }
                }
            }
        }

        SaveScores();
    }

    // 스코어 비교
    private bool EqualsCheck(int data, int score)
    {
        // 체크가 트루이면 최근 스코어 점수가 지금 스코어 점수보다 높음
        return data > score ? true : false;
    }

    // 내림차순 정렬
    public List<string> sortDayList(int number)
    {
        List<string> dayList = new List<string>();

        dayList.Add(playerData[number].day_one + "1");
        dayList.Add(playerData[number].day_two + "2");
        dayList.Add(playerData[number].day_three + "3");
        dayList.Add(playerData[number].day_four + "4");
        dayList.Add(playerData[number].day_five + "5");

        dayList.Sort();
        dayList.Reverse();

        return dayList;
    }

    // 내림차순 정렬 숫자들
    public List<string> sortNumberList(int number)
    {
        List<string> scoreList = new List<string>();

        for (int i = 0; i < sortDayList(number).Count; i++)
        {
            scoreList.Add(sortDayList(number)[i].Substring(sortDayList(number)[i].Length - 1, 1));
        }

        return scoreList;
    }
}
