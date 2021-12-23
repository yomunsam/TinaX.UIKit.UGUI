using UnityEngine;

namespace TinaX.UIKit.UGUI.Page.Group
{
#nullable enable
    public class PushUGUIPageArgs
    {
        /// <summary>
        /// 使用背景遮罩
        /// </summary>
        public bool UseBackgroundMask { get; set; }

        /// <summary>
        /// 指定背景遮罩颜色，留空则使用默认色
        /// </summary>
        public Color? BackgroundMaskColor { get; set; }

        /// <summary>
        /// 点击背景遮罩可关闭页面
        /// </summary>
        public bool CloseByMask { get; set; } = true;

        /// <summary>
        /// 显示UI消息中附带的参数
        /// </summary>
        public object?[]? DisplayMessageArgs { get; set; } = null;
    }
#nullable restore
}
