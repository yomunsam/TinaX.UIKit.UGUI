namespace TinaX.UIKit.UGUI.Page.BackgroundMask
{
    /// <summary>
    /// 背景遮罩接口
    /// </summary>
    public interface IBackgroundMask
    {
        /// <summary>
        /// 使用了背景遮罩了吗
        /// </summary>
        bool IsUseBackgroundMask { get; }
    }

    public interface IBackgroundMaskInfo : IBackgroundMask
    {
        /// <summary>
        /// 点击背景遮罩关闭UI（点击空白处关闭的功能）
        /// </summary>
        bool CloseByBackgroundMask { get; }

        /// <summary>
        /// 指定遮罩的颜色
        /// </summary>
        UnityEngine.Color? BackgroundMaskColor { get; }
    }
}
