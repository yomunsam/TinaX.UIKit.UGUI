using System.IO;
using TinaX;
using TinaXEditor.UIKit.UGUI.Helper;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;

namespace TinaXEditor.UIKit.UGUI.Menus
{
    public static class UIKitUGUIMenu
    {
        [MenuItem("Assets/Create/TinaX/UIKit uGUI/Page View", priority = 10)]
        static void CreateViewPrefab()
        {
            string path = "Assets/";
            foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!path.IsNullOrEmpty() && File.Exists(path) && AssetDatabase.IsValidFolder(path))
                {
                    break;
                }
            }
            var prefab = EditorUIPageHelper.CreateUIPageViewPrefab(path);
            EditorGUIUtility.PingObject(prefab);

            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if(stage == null)
            {
                AssetDatabase.OpenAsset(prefab);
            }
        }
    }
}
