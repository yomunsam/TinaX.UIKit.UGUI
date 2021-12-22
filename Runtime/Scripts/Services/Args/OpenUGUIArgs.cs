namespace TinaX.UIKit.UGUI
{
#nullable enable
    public class OpenUGUIArgs : GetUGUIPageArgs
    {
        public OpenUGUIArgs(string pageUri) : base(pageUri) { }

        /// <summary>
        /// 显示UI参数（启动参数）
        /// </summary>
        public object[]? UIDisplayArgs { get; set; }
    }
#nullable restore
}
