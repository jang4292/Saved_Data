using UnityEngine;
using UnityEditor;

namespace CustomEditor
{
    public class BuildAssetBundles : MonoBehaviour
    {

        [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
        static void ExportResource()
        {
            /// 패널 저장
            string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
            if (path.Length != 0)
            {
                Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

#pragma warning disable CS0618 // Type or member is obsolete
                BuildPipeline.BuildAssetBundle(Selection.activeObject,
                    selection,
                    path,
                    BuildAssetBundleOptions.CollectDependencies |
                    BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
#pragma warning restore CS0618 // Type or member is obsolete
                Selection.objects = selection;
            }
        }

    }
}
