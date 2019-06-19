using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soccer
{
    public class Count : MonoBehaviour
    {
        private Text countText = null;
        [System.NonSerialized]
        public int countNumber = 0;

        public AudioManager audio;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            countText = transform.GetComponent<Text>();
        }

        void Update()
        {
            if(Ball.state == BallState.Count)
            {
                countNumber++;
                audio.StartSuccesSound();
                SetCountChange();
                Ball.state = BallState.Stop;
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        public void SetCountChange()
        {
            countText.text = countNumber + "개";
        }
    }

}
