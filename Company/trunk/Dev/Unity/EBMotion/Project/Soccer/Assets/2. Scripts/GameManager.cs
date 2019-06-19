using UnityEngine;
using System.Collections;

namespace Soccer
{
    public class GameManager : MonoBehaviour
    {
        public enum GameMotionState
        {
            Right,
            Left
        }

        public GameMotionState state = GameMotionState.Right;

        public Timer timer = null;
        public Count count = null;

        void Update()
        {
            if(timer.state == TimerState.Stop)
            {
                count.countNumber = 0;
                count.SetCountChange();
                timer.state = TimerState.Start;
            }
        }
    }
}
