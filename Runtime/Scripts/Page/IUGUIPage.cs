using TinaX.UIKit.Page;
using TinaX.UIKit.UGUI.Page.Group;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page
{
    public interface IUGUIPage : IPage
    {
        new UGUIPageGroup Parent { get; }

        /// <summary>
        /// 其实也就是view gameObject的Transform
        /// </summary>
        Transform Transform { get; }
    }
}
