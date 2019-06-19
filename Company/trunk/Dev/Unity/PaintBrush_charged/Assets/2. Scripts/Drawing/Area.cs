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
    public class Area : MonoBehaviour
    {

        /// <summary>
        /// The characters drawing contents.
        /// </summary>
        public static List<DrawingContents> charactersDrawingContents = new List<DrawingContents>();
    }
}
