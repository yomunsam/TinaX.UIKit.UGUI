using UnityEngine;

namespace TinaX.UIKit.UGUI.ConfigAssets
{
    [CreateAssetMenu(fileName = "UICameraConfig", menuName = "TinaX/UIKit uGUI/UI Camera Config", order = 122)]
    public class UICameraConfigAsset : ScriptableObject
    {
        public CameraClearFlags clearFlags = CameraClearFlags.Depth;
        public Color backgroundColor = new Color(49 / 255, 77 / 255, 121 / 255, 1);
        public int cullingMask = 1 << 5;

        public bool orthographic = true;
        public float orthographicSize = 5;

        public float nearClipPlane = 0.3f;
        public float farClipPlane = 1000f;

        public float depth = 999;

        public RenderingPath renderingPath = RenderingPath.UsePlayerSettings;
        public RenderTexture targetTexture;
        public bool useOcclusionCulling = true;
        public bool allowHDR = false;
        public bool allowMSAA = false;
    }
}
