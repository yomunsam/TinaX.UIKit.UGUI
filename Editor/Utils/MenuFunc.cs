using UnityEditor;
using TinaX;
using System.IO;

namespace TinaXEditor.UIKit.UGUI.Utils
{
    //Todo: 这个代码是旧版搬过来的，以后记得重构一下
    public class MenuFunc 
    {
        [MenuItem("Assets/Create/TinaX/UIKit uGUI/UI Page", priority = 10)]
        static void CreateUI()
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
            UIKitUGUIUtility.CreateUIPage(path);
        }
    }
}
