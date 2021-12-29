using UnityEngine;

namespace TinaX.UIKit.UGUI.Animation
{
    /// <summary>
    /// 委托，播放UGUI简单动画
    /// </summary>
    /// <param name="rootTransform"></param>
    /// <returns>返回这个动画的持续时间</returns>
    public delegate float PlaySimpleAnimation(Transform rootTransform);
}
