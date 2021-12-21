using System;
using System.IO;
using TinaX;
using TinaX.UIKit.UGUI.Page.View;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.UIKit.UGUI.Helper
{
#nullable enable
    public static class EditorUIPageHelper
    {
        public static GameObject CreateUIPageViewPrefab(string targetFolder, string? viewName = null)
        {
            if(targetFolder.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(targetFolder));

            string GetDefaultUIName()
            {
                int t = 1;
                string curDir = Directory.GetCurrentDirectory();
                string resultUIName = $"UIView{t}";
                while(File.Exists(Path.Combine(curDir, targetFolder, $"{resultUIName}.prefab")))
                {
                    t++;
                    resultUIName = $"UIView{t}";
                }
                return resultUIName;
            }


            string prefabSavePath = Path.Combine(targetFolder, $"{viewName ?? GetDefaultUIName()}.prefab");

            GameObject viewGameObject = new GameObject("UIView");
            MakeUIPageView(ref viewGameObject);
            var prefab = PrefabUtility.SaveAsPrefabAsset(viewGameObject, prefabSavePath);
            UnityEngine.Object.DestroyImmediate(viewGameObject);
            return prefab;
        }

        /// <summary>
        /// 把给定的GameObject变成一个UIView
        /// </summary>
        /// <param name="go"></param>
        public static void MakeUIPageView(ref GameObject gameObject, int layer = 5)
        {
            gameObject.SetLayerRecursive(layer);

            var uGUICanvas = gameObject.GetComponentOrAdd<UnityEngine.Canvas>();
            uGUICanvas.overrideSorting = true;

            var rectTrans = gameObject.GetComponentOrAdd<RectTransform>();
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            rectTrans.sizeDelta = Vector2.zero;

            var graphicRaycaster = gameObject.GetComponentOrAdd<UnityEngine.UI.GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = UnityEngine.UI.GraphicRaycaster.BlockingObjects.None;

            _ = gameObject.GetComponentOrAdd<UGUIPageViewComponent>();
        }

    }

#nullable restore
}
