using System;
using UnityEngine;

namespace TinaX.UIKit.UGUI.MultipleDisplay
{
    [Serializable]
    public enum DisplayIndex : int
    {
        [Header("Display 1")]
        Display1 = 0,
        Display2 = 1,
        Display3 = 2,
        Display4 = 3,
        Display5 = 4,
        Display6 = 5,
        Display7 = 6,
        Display8 = 7,
    }

    public static class DisplayIndexRange
    {
        public const int MinimumIndex = 0;
        public const int MaximumIndex = 7;

        //截至2021年年末，目前Unity没有.NET的 Index 和 Range 特性，所以只能写得比较蠢
    }

}
