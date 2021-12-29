using TinaX.UIKit.UGUI.MultipleDisplay;
using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit.UGUI
{
#nullable enable
    public class OpenUGUIArgs : GetUGUIPageArgs
    {

        public OpenUGUIArgs(string pageUri) : base(pageUri) { }


        public PushUGUIPageArgs? PushToGroupArgs { get; set; }


        /// <summary>
        /// UI显示屏幕序号（从导航器打开UI时，此选项不可用，因为导航器强制只能加载到当前栈或子栈）（也所以这个设置没有放在 PushUGUIPageArgs 里）
        /// </summary>
        public DisplayIndex? DisplayIndex { get; set; } = null;

        ///// <summary>
        ///// 显示UI参数（启动参数）
        ///// </summary>
        //public object[]? UIDisplayArgs
        //{
        //    get => PushToGroupArgs?.DisplayMessageArgs ?? null;
        //    set
        //    {
        //        if (PushToGroupArgs == null)
        //            PushToGroupArgs = new PushUGUIPageArgs();
        //        PushToGroupArgs.DisplayMessageArgs = value;
        //    }
        //}

        //public bool UseBackgroundMask
        //{
        //    get => PushToGroupArgs?.UseBackgroundMask ?? false;
        //    set
        //    {
        //        if (PushToGroupArgs == null)
        //            PushToGroupArgs = new PushUGUIPageArgs();
        //        PushToGroupArgs.UseBackgroundMask = value;
        //    }
        //}
    }
#nullable restore
}
