using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.Consts;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers
{
#nullable enable

    /// <summary>
    /// 处理 View资产加载路径
    /// </summary>
    public class ProcessViewAssetLoadPathAsyncHandler : IGetUGuiPageAsyncHandler
    {
        private readonly string _uiKit_UGUI_Scheme;
        private readonly int _uiKit_UGUI_Scheme_Length;
        
        public ProcessViewAssetLoadPathAsyncHandler()
        {
            _uiKit_UGUI_Scheme = $"{UIKitUGUIConsts.SchemeName.ToLower()}://";
            _uiKit_UGUI_Scheme_Length = _uiKit_UGUI_Scheme.Length;
        }

        public string HandlerName => HandlerNameConsts.ProcessViewAssetLoadPath;

        public UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            //Todo: UIName Mapper, 根据UIName简写Page Uri的功能
            string loadPath = payload.PageUriLower.StartsWith(_uiKit_UGUI_Scheme) 
                ? payload.Options.PageUri.Substring(_uiKit_UGUI_Scheme_Length, payload.Options.PageUri.Length - _uiKit_UGUI_Scheme_Length) 
                : payload.Options.PageUri;
            payload.ViewAssetLoadPath = payload.PageUriLower.EndsWith(".prefab") ? loadPath : loadPath + ".prefab";
            return UniTask.CompletedTask;
        }
    }
#nullable restore
}
