using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit.UGUI
{
#nullable enable
    public class OpenUGUIArgs : GetUGUIPageArgs
    {

        public OpenUGUIArgs(string pageUri) : base(pageUri) { }


        public PushUGUIPageArgs? PushToGroupArgs { get; set; }

        /// <summary>
        /// 显示UI参数（启动参数）
        /// </summary>
        public object[]? UIDisplayArgs
        {
            get => PushToGroupArgs?.DisplayMessageArgs ?? null;
            set
            {
                if (PushToGroupArgs == null)
                    PushToGroupArgs = new PushUGUIPageArgs();
                PushToGroupArgs.DisplayMessageArgs = value;
            }
        }

        public bool UseBackgroundMask
        {
            get => PushToGroupArgs?.UseBackgroundMask ?? false;
            set
            {
                if (PushToGroupArgs == null)
                    PushToGroupArgs = new PushUGUIPageArgs();
                PushToGroupArgs.UseBackgroundMask = value;
            }
        }
    }
#nullable restore
}
