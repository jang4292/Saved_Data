using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///Developed by Indie Games Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiegstudio.com
///freelance.art2014@gmail.com

namespace Drawing
{
    [DisallowMultipleComponent]
    public class DrawingContents : MonoBehaviour
    {

        /// <summary>
        /// The parts colors of the character.
        /// </summary>
        public Hashtable characterPartsColors = new Hashtable();

        /// <summary>
        /// The parts sorting order of the character.
        /// </summary>
        public Hashtable characterPartsSortingOrder = new Hashtable();

        /// <summary>
        /// The current sorting order of Drawing Content.
        /// </summary>
        public int currentSortingOrder;

        /// <summary>
        /// The sorting order of the last filled/painted part.
        /// </summary>
        public int lastPartSortingOrder;

        void Start()
        {
            currentSortingOrder = 0;
        }
    }
}
