using UnityEngine;
using System.Collections;

///Developed by Indie Games Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiegstudio.com
///freelance.art2014@gmail.com

namespace Drawing
{
    [DisallowMultipleComponent]
    public class Character : MonoBehaviour
    {

        /// <summary>
        /// The index of the current character.
        /// </summary>
        public static int currentCharacterIndex = 0;

        /// <summary>
        /// The index of the character.
        /// </summary>
        public int index;
    }
}
