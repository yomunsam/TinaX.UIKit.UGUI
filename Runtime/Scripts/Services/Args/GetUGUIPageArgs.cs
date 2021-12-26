﻿using TinaX.UIKit.Page.Controller;
using TinaX.XComponent.Warpper.ReflectionProvider;

namespace TinaX.UIKit.UGUI
{
#nullable enable
    public class GetUGUIPageArgs
    {
        public GetUGUIPageArgs(string pageUri)
        {
            PageUri = pageUri;
        }
        public string PageUri { get; set; }

        public UGUIPageController? PageController { get; set; }

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider { get; set; }

        public IControllerReflectionProvider? ControllerReflectionProvider { get; set; }

        /// <summary>
        /// 在GetUIPage阶段加载Prefab
        /// </summary>
        public bool LoadPrefab { get; set; } = true;


        public GetUIPageArgs GetGetUIPageArgs()
        {
            return new GetUIPageArgs(PageUri)
            {
                PageController = PageController,
            };
        }
    }
#nullable restore
}
