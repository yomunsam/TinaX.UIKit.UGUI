using TinaX.UIKit.Page.Controller;
using TinaX.XComponent.Warpper.ReflectionProvider;

namespace TinaX.UIKit.UGUI.Options
{
#nullable enable
    public class GetUGUIPageOptions  //其实这地方用Record更好，但Unity目前不支持
    {
        public GetUGUIPageOptions(string pageUri)
        {
            PageUri = pageUri;
        }

        public string PageUri { get; set; }
        public PageControllerBase? PageController { get; set; }

        /// <summary>
        /// 在GetUIPage阶段加载Prefab
        /// </summary>
        public bool LoadPrefab { get; set; } = true;

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider { get; set; }
    }

#nullable restore
}
