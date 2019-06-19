using UnityEngine;
using System.Collections;

public abstract class AbstractGameController : MonoBehaviour
{
    public abstract int getTime();
    public abstract int getPoints();
    public abstract string getGameName();
}
