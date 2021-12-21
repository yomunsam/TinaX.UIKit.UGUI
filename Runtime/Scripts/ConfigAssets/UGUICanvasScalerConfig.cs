using System;
using UnityEngine;
using UnityEngine.UI;

namespace TinaX.UIKit.UGUI.ConfigAssets
{
    [Serializable]
    public class UGUICanvasScalerConfig
    {
        public CanvasScaler.ScaleMode UICanvasScalerMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        public float UIScaleFactor = 1;

        [Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
        public float ReferencePixelsPerUnit = 100;

        public Vector2 ReferenceResolution = new Vector2(1600, 1200);
        public CanvasScaler.ScreenMatchMode ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        [Range(0f, 1f)]
        public float CanvasScalerMatchWidthOrHeight = 0;

        public CanvasScaler.Unit PhySicalUnit = CanvasScaler.Unit.Points;
        public float FallbackScreenDPI = 96;
        public float DefaultSpriteDPI = 96;
    }
}
